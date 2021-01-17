using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineToTarget : MonoBehaviour
{
    private LineRenderer line;
    public Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        //
    }
    private void Start()
    {
        if (target == null)
        {
            target = Player.singleton.gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target.position);
    }
}
