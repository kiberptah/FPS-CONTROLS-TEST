using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class CameraShaking : MonoBehaviour
{
    bool isPlayerWalking;
    bool isGrounded;

    Vector3 direction;

    Animator animator;

    CamShake currentState;

    enum CamShake
    {
        camera_idle,
        camera_headbobbing,
        camera_strafeLeft,
        camera_strafeRight

    }
    private void OnEnable()
    {
        EventDirector.eventPlayerMoving += MovementCheck;
        animator = transform.GetComponent<Animator>();
    }
    void OnDisable()
    {
        EventDirector.eventPlayerMoving -= MovementCheck;
    }

    private void Update()
    {
        Headbobbing();
    }

    void Headbobbing()
    {
        if (isPlayerWalking && isGrounded)
        {
            // Move straight
            if (Mathf.Abs(direction.z) >= Mathf.Abs(direction.x))
            {
                ChangeAnimationState(CamShake.camera_headbobbing);
            }
            // Strafe
            if (Mathf.Abs(direction.z) < Mathf.Abs(direction.x))
            {
                if (direction.x > 0)
                {
                    ChangeAnimationState(CamShake.camera_strafeRight);
                }
                else
                {
                    ChangeAnimationState(CamShake.camera_strafeLeft);
                }
            }
        }
        else
        {
            ChangeAnimationState(CamShake.camera_idle);

        }
    }


    void MovementCheck(bool _isPlayerWalking, bool _isGrounded, Vector3 _direction)
    {
        isPlayerWalking = _isPlayerWalking;
        isGrounded = _isGrounded;

        direction = _direction;
    }

    void ChangeAnimationState(CamShake _newState)
    {
        if (currentState == _newState) return;

        //animator.Play(_newState.ToString());
        animator.CrossFade(_newState.ToString(), 0.15f);

        currentState = _newState;
    }

    
}
