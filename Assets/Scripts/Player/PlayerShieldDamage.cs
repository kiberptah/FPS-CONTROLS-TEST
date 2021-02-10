using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class PlayerShieldDamage : MonoBehaviour
{
    [Header("Tweaking")]
    [Range(0, 1)]
    public float resistAmount;
    public float shieldDuration = 0.5f;
    public float shieldRecharge = 0.5f;
    float durationTimePassed;
    float rechargeTimePassed;

    bool isShieldRecharged;
    bool isShieldActive;

    [Header("References")]
    //public PlayerHealth playerH;
    public GameObject shieldFX;

    private event Action activateShield;
    private event Action deactivateShield;

    private void OnEnable()
    {
        activateShield += enableShield;
        deactivateShield += disableShield;

    }
    private void OnDisable()
    {
        activateShield -= enableShield;
        deactivateShield -= disableShield;
    }

    private void Awake()
    {
        durationTimePassed = 0;
        rechargeTimePassed = 0;

        isShieldRecharged = true;
    }

    private void Update()
    {
        ShieldControl();
    }
    void ShieldControl()
    {
        if (Input.GetButtonDown("Shield"))
        {
            if (isShieldRecharged)
            {
                activateShield?.Invoke();
            }
        }
        if (Input.GetButtonUp("Shield"))
        {
            if (isShieldActive)
            {
                deactivateShield?.Invoke();
            }
        }      
    }

    void enableShield()
    {
        isShieldActive = true;

        StopAllCoroutines();
        StartCoroutine(calculateShieldDuration());

        EventDirector.player_ChangeResistance(resistAmount);
        shieldFX.SetActive(true);
    }   
    void disableShield()
    {
        isShieldActive = false;

        StopAllCoroutines();
        StartCoroutine(calculateShieldRecharge());

        EventDirector.player_ChangeResistance(-resistAmount);
        shieldFX.SetActive(false);
    }

    IEnumerator calculateShieldDuration()
    {
        float timestep = 0.5f;

        durationTimePassed = 0;
        while (durationTimePassed < shieldDuration)
        {
            durationTimePassed += timestep;

            yield return new WaitForSeconds(timestep);
        }

        deactivateShield?.Invoke();

        yield return null;
    }
    IEnumerator calculateShieldRecharge()
    {
        float timestep = 0.5f;

        isShieldRecharged = false;
        rechargeTimePassed = 0;
        while(rechargeTimePassed < shieldRecharge)
        {
            rechargeTimePassed += timestep;

            yield return new WaitForSeconds(timestep);
        }

        isShieldRecharged = true;

        yield return null;
    }
}
