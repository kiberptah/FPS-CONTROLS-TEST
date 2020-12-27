using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Camera fpsCam;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private float damage = 10f;
    // Start is called before the first frame update
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
                        hit.transform.GetComponent<Health>().TakeDamage(damage);
                    }
                    
                }
            }
        }
    }
}
