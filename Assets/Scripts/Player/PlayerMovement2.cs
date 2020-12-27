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
    public float airWalkSpeedMultiplier = 0.2f;

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
    private bool isObstacleAbove = false;
    [SerializeField]
    private bool isGrounded = true;
    [SerializeField]
    private bool isCrouching = false;
    [SerializeField]
    private bool isSlowWalking = false;

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
        //Crouching();
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
        float _airWalkSpeedMultiplier = 1f;

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
        // in-th-air speed
        if (isGrounded == false)
        {
            _airWalkSpeedMultiplier = airWalkSpeedMultiplier;
        }


        //Moving
        _positionOffset = _positionOffset.normalized * speed * _crouchWalkMultiplier * _slowWalkMultiplier * _airWalkSpeedMultiplier;
        horizontalJumpStrengh = _positionOffset * rb.mass;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            rb.MovePosition(transform.position + _positionOffset  * Time.fixedDeltaTime);
            eventPlayerMoving?.Invoke(true, new Vector3(_xVel, 0, _zVel));

        }
        else
        {
            eventPlayerMoving?.Invoke(false, Vector3.zero);
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

    void Crouching()
    {
        if (crouchingEnabled)
        {
            bool _isStanding = true;
            //if (Input.GetButtonDown("Crouch") || Input.GetButtonUp("Crouch")) enable if crouching only while holding button
            if (Input.GetButtonDown("Crouch"))
            {
                /*enable if crouching only while holding button
                if (Input.GetButtonDown("Crouch"))
                    _isStanding = true;
                else
                    _isStanding = false;
                */
                _isStanding = !isCrouching;
                if (Crch != null)
                {
                    StopCoroutine(Crch);
                }

                Crch = CrouchSquishing(_isStanding);
                StartCoroutine(Crch);

                //isCrouching = !isCrouching;
            }
        }
    }
    IEnumerator CrouchSquishing(bool _isStanding)
    {     
        ChangeHeight:
        if (_isStanding == true)
        {
            isCrouching = true;
            // СЯДЬ
            while (transform.localScale.y > heightScaleCrouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.075f, transform.localScale.z);


                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {           
            if (!isObstacleAbove)
            {
                isCrouching = false;
                //ВЫПРЯМИСЬ
                while (transform.localScale.y < heightScaleStanding)
                {              
                    if (isObstacleAbove)
                    {
                        // СЯДЬ НАЗАД ПОТОЛОК МЕШАЕТ
                        while (transform.localScale.y > heightScaleCrouching)
                        {
                            Debug.Log("Trying to sit back");
                            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.025f, transform.localScale.z);
                            yield return new WaitForSeconds(0.01f);
                        }
                        isCrouching = true;
                        //goto ChangeHeight;
                        break;
                    }
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.025f, transform.localScale.z);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else
            {
                isCrouching = true;
            }
        }
        yield return null;
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
