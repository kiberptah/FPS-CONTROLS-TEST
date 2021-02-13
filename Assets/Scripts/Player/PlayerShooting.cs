using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerShooting : MonoBehaviour
{
    private Camera fpsCam;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float rechargeSeconds = 1;
    private float rechargeTimer = 0;
    private bool isReadyToFire = true;

    private bool isWeaponDrawn;
    private void OnEnable()
    {
        EventDirector.player_isWeaponDrawn += isWeaponDrawnStatus;
    }
    private void OnDisable()
    {
        EventDirector.player_isWeaponDrawn -= isWeaponDrawnStatus;
    }

    private void Awake()
    {
        fpsCam = Camera.main;
    }
    void Update()
    {
        Shoot();
    }
    void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && isReadyToFire && isWeaponDrawn)
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~LayerMask.GetMask("Player", "Trigger")))
            {
                if (hit.transform.GetComponent<IHealth>() != null) //probably unnecessary since using events
                {
                    EventDirector.someAttack?.Invoke(transform, hit.transform, hit.point, damage);
                    //EventDirector.player_firing?.Invoke();
                }
                else
                {
                    //EventDirector.player_firing?.Invoke();
                }
            }


            EventDirector.player_firing?.Invoke();
            StartCoroutine(Recharge());
        }
    }
    IEnumerator Recharge()
    {
        float timestepSeconds = 0.1f;

        isReadyToFire = false;
        rechargeTimer = 0;
        while (rechargeTimer <= rechargeSeconds)
        {
            rechargeTimer += timestepSeconds;
            yield return new WaitForSeconds(timestepSeconds);
        }
        isReadyToFire = true;

        yield return null;
    }

    void isWeaponDrawnStatus(bool _status)
    {
        isWeaponDrawn = _status;
    }

}
