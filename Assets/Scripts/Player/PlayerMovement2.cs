using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerMovement2 : MonoBehaviour
{
    public static event Action<bool, Vector3> eventPlayerMoving;

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

    //
    //
    [Header("Debug")]
    [SerializeField]
    private bool isGrounded = true;
    [SerializeField]
    private bool isObstacleAbove = false;
    [SerializeField]
    private bool isCrouching = false;

    private bool isSlowWalking = false;
    private bool isMoving = false;

    Rigidbody rb;
    Bounds myBounds;
    [SerializeField]

    private float standingHeight;

    IEnumerator Crch;

    private bool wasUnderObstacleWhileCrouching = false; // для автовставания когда вылезаешь из под откуда-то

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

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
        CheckHeadCollisions();
        CheckGroundBelow();
    }
    void Update()
    {
        Jumping();
        CrouchingReworked();
        WalkingSlowly();

    }

    private void OnDrawGizmos()
    {
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds;
        float _currentHeight = _currentBounds.size.y;

        Vector3 _spherePosition = transform.position;
        _spherePosition.y = transform.position.y + _currentHeight * 0.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_spherePosition, 0.25f);
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
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }
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
                // Add Force Upwards
                rb.AddForce(Vector3.up * jumpStrengh * rb.mass, ForceMode.Impulse);
                // Add Force in walking direction
                //rb.AddForce(horizontalJumpStrengh, ForceMode.Impulse);
                
            }
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


            if (isCrouching || (isCrouching == false && isObstacleAbove == true && transform.localScale.y < _heightScaleCrouching)) // delete last check for autocrouch
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
            else
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
        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds; ;
        float _currentHeight = _currentBounds.size.y;

        Vector3 _rayOrigin = new Vector3(transform.position.x, transform.position.y - _currentHeight * 0.5f, transform.position.z);
        float _rayLengh = standingHeight;
        RaycastHit _hit;

        float _sphereRadius = 0.25f;
        Vector3 _spherePosition = transform.position;
        _spherePosition.y = transform.position.y + _currentHeight * 0.5f;

        if (Physics.Raycast(_rayOrigin, Vector3.up, out _hit, _rayLengh, ~LayerMask.GetMask("Player"))
         || Physics.CheckSphere(_spherePosition, _sphereRadius, ~LayerMask.GetMask("Player")))
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
        _spherePosition.y = transform.position.y - _currentHeight * 0.5f + _sphereRadius * 0.5f;

        if (Physics.CheckSphere(_spherePosition, _sphereRadius, ~LayerMask.GetMask("Player")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
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

}
