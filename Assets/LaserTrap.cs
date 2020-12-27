using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    private LineRenderer laser;

    [Header("Tweaking")]
    [SerializeField]
    private float maxLaserLengh = 10f;
    [SerializeField]
    private float damagePerSecond = 10f;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject contactPoint;

    // Start is called before the first frame update
    void Awake()
    {
        //contactPoint = new GameObject("contactPoint");
        laser = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CastLaser();
    }

    void CastLaser()
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, transform.forward, out _hit, maxLaserLengh, ~LayerMask.GetMask("Trigger")))
        {
            GameObject _target;
            _target = Instantiate(contactPoint, transform, true);
            _target.transform.position = _hit.point;

            laser.SetPosition(1, new Vector3(0, 0, _target.transform.localPosition.z));

            Destroy(_target.gameObject);

            DealDamage(_hit.transform);
        }
        else
        {
            laser.SetPosition(1, Vector3.forward * maxLaserLengh);

        }
    }

    void DealDamage(Transform _target)
    {
        if (_target.GetComponent<TagSystem>() != null)
        {
            if (_target.GetComponent<TagSystem>().CheckTag(TagSystem.Tags.damagable))
            {
                _target.GetComponent<Health>()?.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
