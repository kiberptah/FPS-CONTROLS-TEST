using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEyeLevelAligner : MonoBehaviour
{
    public Transform body;
    public float foreheadHeight = 15f;

    private Vector3 defaultPosition;
    void Start()
    {
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        AlignCamera();
    }

    void AlignCamera()
    {
        float _foreheadHeight = foreheadHeight / 9 * 10; //because scale is fucked

        //Debug.Log(body.transform.localScale.y - _foreheadHeight);

        transform.localPosition = new Vector3(transform.localPosition.x, body.transform.localScale.y - _foreheadHeight, transform.localPosition.z);
    }
}
