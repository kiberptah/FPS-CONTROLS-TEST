///This script is required to assign player as child of the surface object 
///to prevent jitter in elevators 
///and stick to moving platforms

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations;

public class StickToTheSurface : MonoBehaviour
{
    [SerializeField] CheckFeetGround groundCheck;

    [SerializeField] GameObject playerHolder;
    Transform oldParent = null;

    Transform surface;
    Vector3 surfaceGlobalSpeed = Vector3.zero;
    Vector3 lastSurfacePosition;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }
    void FixedUpdate()
    {
        CalculateSurfaceSpeed();

        FindSurfaceBelow();
        Stick();

        //Debug.Log("player velocity: " + rb.velocity.ToString("f3"));
        //Stick2();//   
    }

    private void Update()
    {
        

    }



    void Stick()
    {
        /// CHANGE TO ANOTHER SURFACE
        if (surface != null && surface != oldParent)
        {
            /// DELETE PREVIOUS PARENT
            if (transform.parent != null)
            {
                //я хз как это сделать не через лист надо удалить старого родителя при этом сменив его на null
                List<GameObject> del = new List<GameObject>();
                del.Add(transform.parent.gameObject);
                //Destroy(transform.parent.gameObject);
                transform.SetParent(null);
                oldParent = null;

                foreach (GameObject g in del)
                {
                    Destroy(g);
                }
            }

            oldParent = surface;
            GameObject playerParent = Instantiate(playerHolder, null);
            playerParent.name = "PlayerHolder";
            playerParent.transform.rotation = surface.rotation;

            transform.SetParent(playerParent.transform, true);
            playerParent.transform.SetParent(surface, true);
        }
        /// LEFT FROM ANY SURFACE
        if (surface == null)
        {
            /// DELETE PREVIOUS PARENT
            if (transform.parent != null)
            {
                /// IMITATE INERTIA
                //Debug.Log("rb.velocity: " + rb.velocity + "| surfaceGlobalSpeed: " + surfaceGlobalSpeed.ToString("f3") + "| Time.fixedDeltaTime: " + Time.fixedDeltaTime);
                rb.velocity += surfaceGlobalSpeed / Time.fixedDeltaTime;
                
                //
                //я хз как это сделать не через лист надо удалить старого родителя при этом сменив его на null
                List<GameObject> del = new List<GameObject>();
                del.Add(transform.parent.gameObject);
                //Destroy(transform.parent.gameObject);
                transform.SetParent(null);
                oldParent = null;

                foreach (GameObject g in del)
                {
                    Destroy(g);
                }
            }
        }
    }

    void FindSurfaceBelow()
    {
        /// TAKE SURFACE FROM GROUNDCHECK
        surface = groundCheck.surface;
        if (surface != null)
            lastSurfacePosition = surface.transform.position; // DONT FORGET TO RESET LAST POSITION OVERWISE IT WOULD BE FROM PREVIOUS SURFACE!!!

        /// REASSURE SURFACE BY RAYCASTING DOWN also helps stick to platforms better cause ray is long and there's extra gravity applied in other script
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y / 2;

        Vector3 _rayOrigin = transform.position;
        float _rayLengh = _currentHeight * 1.2f;
        RaycastHit _hit;

        if (Physics.Raycast(_rayOrigin, Vector3.down, out _hit, _rayLengh, ~LayerMask.GetMask("Player")))
        {
            surface = _hit.transform;
            if (surface != null)
                lastSurfacePosition = surface.transform.position; // DONT FORGET TO RESET LAST POSITION OVERWISE IT WOULD BE FROM PREVIOUS SURFACE!!!
        }

        //Debug.Log(surface);
    }

    /// CALCULATE GLOBAL VELOCITY OF THE SURFACE
    void CalculateSurfaceSpeed()
    {
        if (surface != null)
        {
            surfaceGlobalSpeed = surface.transform.position - lastSurfacePosition;
            surfaceGlobalSpeed = surface.transform.TransformDirection(surfaceGlobalSpeed);
            lastSurfacePosition = surface.transform.position;
            //Debug.Log("| surfaceGlobalSpeed: " + surfaceGlobalSpeed.ToString("f3"));
        }

    }
}
