using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFireVisualizer : MonoBehaviour
{
    LineRenderer lr;
    public Transform muzzle;
    Vector3 hitPoint;

    public float duration = 0.5f;

    bool showLine;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void OnEnable()
    {
        EventDirector.player_firing += EventListner;
    }
    private void OnDisable()
    {
        EventDirector.player_firing -= EventListner;
    }
    private void Update()
    {

        lr.SetPosition(0, muzzle.position);
        //lr.SetPosition(1, hitPoint.point);
    }
    void EventListner(Vector3 _hitPoint)
    {
        //Debug.Log("laser viz");
        //showLine = true;
        hitPoint = _hitPoint;

        lr.SetPosition(0, muzzle.position);
        lr.SetPosition(1, hitPoint);

        StartCoroutine(showLaser());
    }

    IEnumerator showLaser()
    {
        lr.enabled = true;

        float timestep = 0.1f;
        float _duration = 0;
        while (_duration <= duration)
        {
            _duration += timestep;

            yield return new WaitForSeconds(timestep);
        }

        lr.enabled = false;

        yield return null;
    }
}
