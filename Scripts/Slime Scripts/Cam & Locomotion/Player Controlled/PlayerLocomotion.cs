using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : BaseLocomotion
{
    private SlimeInputMap slimeInputMap;
    private Camera cam;

    public bool DisableBehavior { get; set; }

    [Tooltip("How many fixed speeds to use with linear movement? 0=linear control")]
    private int FixedSpeedSteps = 0;

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        slimeInputMap = GetComponent<SlimeInputMap>();
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if(DisableBehavior)
        {
            Controller.enabled = false;
            return;
        }

        UpdateController();
        SetRotation();
    }
    public override void SetRotation()
    {
        Vector3 lookPos = visualOrientation.position - cam.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        visualOrientation.rotation = Quaternion.Slerp(visualOrientation.rotation, rotation, Time.deltaTime * rotSpeed);
        if (Controller.isGrounded)
        {//fixed looking away on jump(above)
            if (Controller.velocity.magnitude > 0)
            {
                Vector3 newPos = slimeOrientation.position - (transform.position - Controller.velocity);
                newPos.y = 0;
                var slimeRot = Quaternion.LookRotation(newPos);
                slimeOrientation.rotation = Quaternion.Slerp(slimeOrientation.rotation, slimeRot, Time.deltaTime * (rotSpeed / 2f));
            }
            else
            {
                //slimeVisual.rotation = slimeVisual.parent.transform.rotation;
                Quaternion desiredRot = slimeOrientation.parent.transform.rotation;
                slimeOrientation.rotation = Quaternion.Slerp(slimeOrientation.rotation, desiredRot, Time.deltaTime * (rotSpeed / 2.5f));
            }
        }
    }
    public void UpdateController()
    {
        UpdateMovement();
        UpdateImpactData();

        if (slimeInputMap.Jump)
            Jump();

        Vector3 moveDirection = Vector3.zero;
        float motorDamp = (1.0f + (damping * simulationRate * Time.deltaTime));
        moveThrottle.x /= motorDamp;
        moveThrottle.y = (moveThrottle.y > 0.0f) ? (moveThrottle.y / motorDamp) : moveThrottle.y;
        moveThrottle.z /= motorDamp;

        moveDirection += moveThrottle * simulationRate * Time.deltaTime;
        moveDirection.y += gravity * Time.deltaTime;

        if (Controller.isGrounded && moveThrottle.y <= transform.lossyScale.y * .001f)
        {
            float bumpUpOffset = Mathf.Max(Controller.stepOffset, new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
            moveDirection -= bumpUpOffset * Vector3.up;
        }

        Vector3 predictedXZ = Vector3.Scale((Controller.transform.localPosition + moveDirection), new Vector3(1, 0, 1));

        Controller.Move(moveDirection);
        Vector3 actualXZ = Vector3.Scale(Controller.transform.localPosition, new Vector3(1, 0, 1));

        if (predictedXZ != actualXZ)
            moveThrottle += (actualXZ - predictedXZ) / (simulationRate * Time.deltaTime);      
    }

    public override void UpdateMovement()
    {
        if(enableMovement)
        {
            bool moveForward = slimeInputMap.SlimeInput.locomotion.move.triggered;
            bool moveLeft = slimeInputMap.SlimeInput.locomotion.move.triggered;
            bool moveRight = slimeInputMap.SlimeInput.locomotion.move.triggered;
            bool moveBack = slimeInputMap.SlimeInput.locomotion.move.triggered;

            moveScale = 1f;

            if ((moveForward && moveLeft) || (moveForward && moveRight) ||
                    (moveBack && moveLeft) || (moveBack && moveRight))
                moveScale = 0.85f;

            if (!Controller.isGrounded)
                moveScale = .65f;

            moveScale *= simulationRate * Time.deltaTime;

            float moveInfluence = acceleration * .1f * moveScale * moveScaleMultiplier;

            Quaternion ort = visualOrientation.rotation;
            Vector3 ortEuler = ort.eulerAngles;
            ortEuler.z = ortEuler.x = 0f;
            ort = Quaternion.Euler(ortEuler);

            if (moveForward)
                moveThrottle += ort * (transform.lossyScale.z * moveInfluence * Vector3.forward);
            if (moveBack)
                moveThrottle += ort * (transform.lossyScale.z * moveInfluence /* * backAndSideDampen */ * Vector3.back);
            if (moveLeft)
                moveThrottle += ort * (transform.lossyScale.x * moveInfluence /* * backAndSideDampen */ * Vector3.left);
            if (moveRight)
                moveThrottle += ort * (transform.lossyScale.x * moveInfluence /* * backAndSideDampen */ * Vector3.right);

            moveInfluence = acceleration * 0.1f * moveScale * moveScaleMultiplier;

            //analog
            Vector2 primaryAxis = slimeInputMap.MoveData;

            if (FixedSpeedSteps > 0)
            {
                primaryAxis.y = Mathf.Round(primaryAxis.y * FixedSpeedSteps) / FixedSpeedSteps;
                primaryAxis.x = Mathf.Round(primaryAxis.x * FixedSpeedSteps) / FixedSpeedSteps;
            }

            if (primaryAxis.y > 0.0f)
                moveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

            if (primaryAxis.y < 0.0f)
                moveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence *
                                       /* backAndSideDampen * */ Vector3.back);

            if (primaryAxis.x < 0.0f)
                moveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence *
                                       /* backAndSideDampen * */ Vector3.left);

            if (primaryAxis.x > 0.0f)
                moveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence *
                    /* backAndSideDampen * */ Vector3.right);

            //Debug.Log("Velocity -> " + Controller.velocity.magnitude);
        }
    }
}
