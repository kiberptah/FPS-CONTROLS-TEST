using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSWeaponShaking : MonoBehaviour
{
    bool isPlayerWalking;
    bool isGrounded;
    Vector3 direction;

    Animator animator;

    WeaponShake currentState;

    enum WeaponShake
    {
        weapon_idle,
        weapon_sway

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
        WeaponSway();
    }
    void WeaponSway()
    {
        if (isPlayerWalking && isGrounded)
        {
            // Move straight
            if (Mathf.Abs(direction.z) >= Mathf.Abs(direction.x))
            {
                ChangeAnimationState(WeaponShake.weapon_sway);
            }
            // Strafe
            if (Mathf.Abs(direction.z) < Mathf.Abs(direction.x))
            {
                if (direction.x > 0)
                {
                    ChangeAnimationState(WeaponShake.weapon_idle);
                }
                else
                {
                    ChangeAnimationState(WeaponShake.weapon_idle);
                }
            }
        }
        else
        {
            ChangeAnimationState(WeaponShake.weapon_idle);

        }
    }

    void MovementCheck(bool _isPlayerWalking, bool _isGrounded, Vector3 _direction)
    {
        isPlayerWalking = _isPlayerWalking;
        isGrounded = _isGrounded;

        direction = _direction;
    }

    void ChangeAnimationState(WeaponShake _newState)
    {
        if (currentState == _newState) return;

        //animator.Play(_newState.ToString());
        animator.CrossFade(_newState.ToString(), 0.15f);

        currentState = _newState;
    }

}
