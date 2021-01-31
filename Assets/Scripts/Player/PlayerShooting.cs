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


    //public static event Action<Transform, Vector3, float, Vector3> playerHitEnemy;

    private void Awake()
    {
        fpsCam = Camera.main;
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~LayerMask.GetMask("Player")))
            {
                if (hit.transform.GetComponent<Health>() != null)
                {
                    EventDirector.someAttack(transform, hit.transform, hit.point, damage);

                }
            }
        }
    }
}
