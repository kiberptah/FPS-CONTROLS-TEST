///This script is required to assign player as child of the surface object 
///to prevent jitter in elevators 
///and stick to moving platforms

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations;

public class StickToTheSurface : MonoBehaviour
{

    public float downwardRaycastLengh = 1.5f; //consider altering it for different situations (platforms/elevators)

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


        if (surface == null)
        {
            //Debug.Break();
        }
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

        /// REASSURE SURFACE BY RAYCASTING DOWN also helps stick to platforms better cause ray is long
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y / 2;

        Vector3 _rayOrigin = transform.position;
        float _rayLengh = _currentHeight + downwardRaycastLengh; // if that's too long i suggest using separate checks for old and new surfaces
        RaycastHit _hit;

        if (Physics.Raycast(_rayOrigin, Vector3.down, out _hit, _rayLengh, ~LayerMask.GetMask("Player")))
        {
            surface = _hit.transform;
            if (surface != null)
                lastSurfacePosition = surface.transform.position; // DONT FORGET TO RESET LAST POSITION OVERWISE IT WOULD BE FROM PREVIOUS SURFACE!!!
        }

        //Debug.Log("hit: " + _hit.transform + " and surface " + surface);
        //Debug.Log(_currentHeight);
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

    private void OnDrawGizmos()
    {
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y / 2;

        Vector3 _rayOrigin = transform.position;
        float _rayLengh = _currentHeight + downwardRaycastLengh;
        RaycastHit _hit;



        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rayOrigin, _rayOrigin + Vector3.down * _rayLengh);
    }
}
