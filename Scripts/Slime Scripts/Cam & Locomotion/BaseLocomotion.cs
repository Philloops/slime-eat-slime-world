using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLocomotion : MonoBehaviour
{
    #region variables and properties
    [Header("Orientation")]
    public Transform visualOrientation;
    public Transform slimeOrientation;
    public float rotSpeed = 10f;

    [Header("Locomotion")]
    [Range(0, 3)]
    public float acceleration = .5f;
    [Range(0, 3)]
    public float damping = .3f;
    [Range(0, 3)]
    public float backAndSideDampen = .75f;
    public bool enableMovement;
    public float gravity = -5.86f;
    public float jumpForce = .3f;

    public CharacterController Controller { get; set; }

    protected float moveScale = 1;
    protected Vector3 moveThrottle = Vector3.zero;
    protected float moveScaleMultiplier = 1.0f;
    protected float simulationRate = 60;
    protected Vector3 impact = Vector3.zero;
    protected float mass = 3f;
    #endregion

    public virtual void SetRotation() { }
    public virtual void UpdateMovement() { }

    public bool Jump()
    {
        if (!Controller.isGrounded)
            return false;

        moveThrottle.y += Mathf.Sqrt(jumpForce);
        return true;
    }
    #region Status effect type methods
    protected void UpdateImpactData()
    {
        if (impact.magnitude > .2f)
            Controller.Move(impact * Time.deltaTime);

        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
    public void AddImpact(Vector3 _dir, float _force)
    {
        _dir.Normalize();

        if (_dir.y < 0)
            _dir.y = -_dir.y;

        Stop();
        impact += _dir.normalized * _force / mass;
        moveThrottle.y += Mathf.Sqrt(jumpForce / 3);
    }
    public void ContinuousForce(Vector3 _dir, float _force)
    {
        _dir.Normalize();
        _dir.y = 0;
        //Stop();
        impact -= _dir.normalized * _force / mass;
        //moveThrottle.y += Mathf.Sqrt(jumpForce / 3);
    }
    public void Stop()
    {
        Controller.Move(Vector3.zero);
        moveThrottle = Vector3.zero;
    }
    #endregion
}
