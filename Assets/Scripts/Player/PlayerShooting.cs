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


    public static event Action<Transform, Vector3, float, Vector3> playerHitEnemy;

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
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                if (hit.transform.GetComponent<TagSystem>() != null)
                {
                    if (hit.transform.GetComponent<TagSystem>().CheckTag(TagSystem.Tags.damagable))
                    {
                        // // hit.transform.GetComponent<Health>().TakeDamage(damage);

                        playerHitEnemy?.Invoke(hit.transform, hit.point, damage, transform.position);

                        //hit.transform.GetComponent<CharacterEvents>().ReceiveDamage(damage);

                    }

                }
            }
        }
    }
}
