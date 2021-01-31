using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerMovement2 : MonoBehaviour
{
    public static event Action<bool, Vector3> eventPlayerMoving;
    public static event Action<Vector3> globalSpeedEvent;

    [Header("Features")]
    public bool slowWalkingEnabled = true;
    public bool jumpingEnabled = true;
    public bool crouchingEnabled = true;
    [Header("Tweaking")]
    public float speed = 1f;

    public float slowWalkMultiplier = 0.5f;
    public float crouchWalkMultiplier = 0.3f;

    public float jumpStrengh = 10f;
    private Vector3 horizontalJumpStrengh = Vector3.zero;

    [SerializeField]
    private float heightScaleCrouching = 0.5f;
    [SerializeField]
    private float heightScaleStanding;

    [SerializeField] private float extraGravityCloseToTheGround = 100f;

    //
    //
    [Header("Debug")]
    [SerializeField]
    public bool isGrounded = true;
    [SerializeField] CheckFeetGround feetGroundCheck;
    private bool hasLanded = true;
    [SerializeField]
    private bool isObstacleAbove = false;
    [SerializeField]
    private bool isCrouching = false;

    private bool isSlowWalking = false;
    private bool isMoving = false;

    Rigidbody rb;
    float rb_defaultDrag;
    Bounds myBounds;
    [SerializeField]

    private float standingHeight;

    IEnumerator Crch;

    private bool wasUnderObstacleWhileCrouching = false; // для автовставания когда вылезаешь из под откуда-то

    // Calculate global velocity
    Vector3 lastPosition;
    public Vector3 globalSpeed = Vector3.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb_defaultDrag = rb.drag;
        lastPosition = transform.position;

        if (isCrouching)
        {
            Debug.Log("FIX THIS! Or don't spawn player crouched");
        }
        myBounds = gameObject.GetComponent<Collider>().bounds;
        standingHeight = myBounds.size.y;
        heightScaleStanding = transform.localScale.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        DragWhenStopped();
        CheckHeadCollisions();
        CheckGroundBelow();
        JumpPhysicsAdjust();
        //MoreGravityCloserToTheGround();
        CalculateGlobalSpeed();
    }
    void Update()
    {
        Jumping();
        CrouchingReworked();
        WalkingSlowly();

    }



    void Movement()
    {
        float _xVel = Input.GetAxis("Horizontal");
        float _zVel = Input.GetAxis("Vertical");
        Vector3 _positionOffset = transform.TransformDirection(new Vector3(_xVel, 0, _zVel));
        float _slowWalkMultiplier = 1f;
        float _crouchWalkMultiplier = 1f;

        //Slow Walking Speed
        if (isSlowWalking)
        {
            _slowWalkMultiplier = slowWalkMultiplier;
        }
        // Crouching Speed
        if (isCrouching)
        {
            _crouchWalkMultiplier = crouchWalkMultiplier;
        }

        //Moving
        _positionOffset = _positionOffset.normalized * speed * _crouchWalkMultiplier * _slowWalkMultiplier;
        horizontalJumpStrengh = _positionOffset * rb.mass;
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && isGrounded)
        {
            isMoving = true;
            //rb.MovePosition(transform.position + _positionOffset  * Time.fixedDeltaTime); // no inertia
            rb.velocity = new Vector3(_positionOffset.x, rb.velocity.y, _positionOffset.z);
            eventPlayerMoving?.Invoke(true, new Vector3(_xVel, 0, _zVel));
        }
        else
        {
            eventPlayerMoving?.Invoke(false, Vector3.zero);
            if (isMoving)
            {
                isMoving = false;
                if (isGrounded)
                {
                    //rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }
        }

        
    }

    void DragWhenStopped()
    {
        if (isMoving)
        {
            rb.drag = rb_defaultDrag;
        }
        else
        {
            rb.drag = rb_defaultDrag * 2;
        }
    }

    void Jumping()
    {
        float _xVel = Input.GetAxis("Horizontal");
        float _zVel = Input.GetAxis("Vertical");

        if (jumpingEnabled)
        {                 
            if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                /*if (rb.velocity.y > 0)
                {
                    

                    rb.AddForce(Vector3.up * jumpStrengh + new Vector3(0, rb.velocity.y, 0), ForceMode.VelocityChange);
                    Debug.Log("jump adjust");
                }
                else
                {
                    rb.AddForce(Vector3.up * jumpStrengh, ForceMode.VelocityChange);
                }*/

                rb.AddForce(Vector3.up * jumpStrengh, ForceMode.VelocityChange);

            }
        }
    }
    void JumpPhysicsAdjust()
    {
        if (isGrounded == false)
        {
            rb.drag = rb_defaultDrag * 0.1f;
            hasLanded = false;
        }

        if (hasLanded == false && isGrounded == true)
        {
            //Debug.Log("EAGLE HAS LANDED " + Time.time);
            //rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.drag = rb_defaultDrag;
            hasLanded = true;
        }
    }

    void CrouchingReworked()
    {
        float _shrinkingSpeed = 1.5f * Time.deltaTime;
        float _heightScaleCrouching = heightScaleStanding * heightScaleCrouching; // to make it relative to scaled height not to raw height


        if (crouchingEnabled)
        {
            if (Input.GetButtonDown("CrouchSwitch"))
            {
                isCrouching = !isCrouching;

            }
            if (Input.GetButtonDown("CrouchHold"))
            {
                isCrouching = true;
            }
            if (Input.GetButtonUp("CrouchHold"))
            {
                isCrouching = false;
            }

            if (isObstacleAbove && transform.localScale.y < heightScaleStanding)
            {
                isCrouching = true;
            }

            // Приседаем
            if (isCrouching || (isCrouching == false && isObstacleAbove == true && transform.localScale.y < heightScaleStanding)) // delete last check for autocrouch
            {
                if (transform.localScale.y > _heightScaleCrouching)
                {
                    float _newY = transform.localScale.y - _shrinkingSpeed;
                    _newY = Mathf.Clamp(_newY, _heightScaleCrouching, heightScaleStanding);

                    transform.localScale = new Vector3(transform.localScale.x, _newY, transform.localScale.z);

                    transform.position = new Vector3(transform.position.x, 
                        transform.position.y - _shrinkingSpeed * 0.5f, 
                        transform.position.z);
                    //Debug.Log(standingHeight * 0.5f - standingHeight * 0.5f * transform.localScale.y);
                }
            }
            // Встаем
            if (isCrouching == false && isObstacleAbove == false)
            {
                if (transform.localScale.y < heightScaleStanding)
                {
                    float _newY = transform.localScale.y + _shrinkingSpeed;
                    _newY = Mathf.Clamp(_newY, _heightScaleCrouching, heightScaleStanding);
                    transform.localScale = new Vector3(transform.localScale.x, _newY, transform.localScale.z);

                    transform.position = new Vector3(transform.position.x,
                        transform.position.y + _shrinkingSpeed * 0.5f,
                        transform.position.z);
                }
            }
        }
    }
    void CheckHeadCollisions()
    {
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y;

        Vector3 _rayOrigin = new Vector3(transform.position.x, transform.position.y - _currentHeight * 0.5f, transform.position.z);
        float _rayLengh = standingHeight;
        RaycastHit _hit;

        float _sphereRadius = 0.25f;
        Vector3 _spherePosition = transform.position;
        _spherePosition.y = transform.position.y + _currentHeight * 0.5f - _sphereRadius;

        if (Physics.Raycast(_rayOrigin, Vector3.up, out _hit, _rayLengh, ~LayerMask.GetMask("Player"))
         || Physics.CheckSphere(_spherePosition, _sphereRadius, ~LayerMask.GetMask("Player"))
         )
        {
            isObstacleAbove = true;
        }
        else
        {
            isObstacleAbove = false;
        }
    }
    void CheckGroundBelow()
    {
        //Checking ground
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y;

        float _sphereRadius = 0.25f;
        Vector3 _spherePosition = transform.position;
        _spherePosition.y = transform.position.y - _currentHeight * 0.5f + _sphereRadius * 0.95f;

        if (Physics.CheckSphere(_spherePosition, _sphereRadius, ~LayerMask.GetMask("Player")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Checking Ground via Feet
        isGrounded = feetGroundCheck.isColliding;
    }
    void WalkingSlowly()
    {
        if (Input.GetButton("SlowWalk"))
        {
            isSlowWalking = true;
        }
        else
        {
            isSlowWalking = false;
        }
    }

    void MoreGravityCloserToTheGround()
    {
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y / 2;

        Vector3 _rayOrigin = transform.position;
        float _rayLengh = _currentHeight * 1.1f;
        RaycastHit _hit;

        if (Physics.Raycast(_rayOrigin, Vector3.down, out _hit, _rayLengh, ~LayerMask.GetMask("Player")) && isGrounded == false && rb.velocity.y < 0)
        {
            //rb.AddRelativeForce(Vector3.down * extraGravityCloseToTheGround * Mathf.Abs(rb.velocity.y), ForceMode.VelocityChange);
            //Debug.Log("GOING DOWN " + Time.time);
            //Debug.Log(rb.velocity);
        }
    }

    void CalculateGlobalSpeed()
    {
        globalSpeed = transform.position - lastPosition;
        lastPosition = transform.position;
        //Debug.Log("velocity: " + globalSpeed.ToString("f3"));

        globalSpeedEvent?.Invoke(globalSpeed);
    }


    private void OnDrawGizmos()
    {

        /*// Ground check debug
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y;

        float _sphereRadius = 0.25f;
        Vector3 _spherePosition = transform.position;
        _spherePosition.y = transform.position.y - _currentHeight * 0.5f + _sphereRadius * 0.95f;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_spherePosition, _sphereRadius);*/

        /*// ceiling check debug
        Vector3 _rayOrigin = new Vector3(transform.position.x, transform.position.y - _currentHeight * 0.5f, transform.position.z);
        float _rayLengh = standingHeight;
        RaycastHit _hit;

        float _sphereRadius2 = 0.25f;
        Vector3 _spherePosition2 = transform.position;
        _spherePosition2.y = transform.position.y + _currentHeight * 0.5f;

        Gizmos.color = Color.red;
        //Gizmos.DrawLine(_rayOrigin, _rayOrigin + Vector3.up *_rayLengh);
        Gizmos.DrawLine(_rayOrigin, _rayOrigin + Vector3.up * _rayLengh);*/

        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y / 2;

        Vector3 _rayOrigin = transform.position;
        float _rayLengh = _currentHeight * 1.1f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayOrigin, _rayOrigin + Vector3.down * _rayLengh);
    }


}
