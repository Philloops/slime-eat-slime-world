using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    public Animator anim;
    public Transform focusPoint;

    private BaseLocomotion locomotion;

    private Vector3 prevPos;
    private Vector3 velocity;

    private void Awake()
    {
        locomotion = GetComponent<BaseLocomotion>();
    }
    void FixedUpdate()
    {
        velocity = (focusPoint.position - prevPos) / Time.deltaTime;
        prevPos = focusPoint.position;       
    }
    void Update()
    {
        CheckMoveState();
        CheckLocomotionValues();
    }
    public void CheckMoveState()
    {
        if (locomotion.Controller.velocity.magnitude > 0.1f)
            anim.SetBool("Move", true);
        else
            anim.SetBool("Move", false);
    }
    public void CheckLocomotionValues()
    {
        velocity.y = 0;
        velocity = velocity.normalized;

        float fwdDotProduct = Vector3.Dot(focusPoint.forward, velocity);
        float upDotProduct = Vector3.Dot(focusPoint.up, velocity);
        float rightDotProduct = Vector3.Dot(focusPoint.right, velocity);

        Vector3 velocityVector = new Vector3(rightDotProduct, upDotProduct, fwdDotProduct);

        anim.SetFloat("VelocityX", velocityVector.x);
        anim.SetFloat("VelocityY", velocityVector.z);
    }
    public void PlayAnimEvent(string _name)
    {
        anim.Play(_name);
    }
    public void CrossFadeAnim(string _name, float _fade)
    {
        anim.CrossFade(_name, _fade);
    }
}
