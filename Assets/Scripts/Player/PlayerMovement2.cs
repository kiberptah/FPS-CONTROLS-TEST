using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerMovement2 : MonoBehaviour
{
    /*public static event Action<bool, bool, Vector3> eventPlayerMoving;
    public static event Action<Vector3> globalSpeedEvent;*/

    [Header("Features")]
    public bool sprintEnabled = true;
    public bool jumpingEnabled = true;
    public bool crouchingEnabled = true;
    [Header("Tweaking")]
    //public float speed = 1f;
    public float acceleration = 1f;
    public float speedLimit = 10;

    [Range(0.1f, 0.9f)]
    public float fakeInertiaModifier = 0.1f;

    public float sprintSpeedMultiplier = 0.5f;
    public float crouchSpeedMultiplier = 0.25f;
    public float inAirSpeedMultiplier = 0.25f;

    public float jumpStrengh = 10f;

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

    private bool isSprinting = false;
    private bool isMoving = false;

    Rigidbody rb;
    float rb_defaultDrag;
    Bounds myBounds;

    private float standingHeight;

    IEnumerator Crch;

    private bool wasUnderObstacleWhileCrouching = false; // для автовставания когда вылезаешь из под откуда-то

    // Calculate global velocity
    Vector3 lastPosition;
    public Vector3 globalSpeed = Vector3.zero;


    // RB.MOVEPOSITION has no inertia so there's need to imitate it when jumping and falling
    Vector3 fakeInertia = Vector3.zero;
    bool wasFakeInertiaPerformed = false;

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
        //FakeInertia();
        Movement();
        FakeFriction();
        //DragWhenStopped();
        CheckHeadCollisions();
        CheckGroundBelow();
        //JumpPhysicsAdjust();
        //MoreGravityCloserToTheGround();
        CalculateGlobalSpeed();

        //Debug.Log(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
    }
    void Update()
    {
        Jumping();
        CrouchingReworked();
        Sprinting();

    }



    void Movement()
    {
        float _xVel = Input.GetAxis("Horizontal");
        float _zVel = Input.GetAxis("Vertical");
        Vector3 positionOffset = transform.TransformDirection(new Vector3(_xVel, 0, _zVel));

        float acceleration = this.acceleration;

        float sprintSpeedMultiplier = 1f;
        float crouchWalkMultiplier = 1f;
        float inAirSpeedMultiplier = 1f;
        float speedLimit = this.speedLimit;

        //sprint Walking Speed
        if (isSprinting)
        {
            sprintSpeedMultiplier = this.sprintSpeedMultiplier;
            speedLimit = this.speedLimit * sprintSpeedMultiplier;
        }
        // Crouching Speed
        if (isCrouching)
        {
            crouchWalkMultiplier = this.crouchSpeedMultiplier;
            speedLimit = this.speedLimit * crouchSpeedMultiplier;
        }
        // In-Air Speed
        if (isGrounded == false)
        {
            inAirSpeedMultiplier = this.inAirSpeedMultiplier;

            crouchWalkMultiplier = 1f;
            //speedLimit = this.speedLimit * inAirSpeedMultiplier;
            speedLimit = this.speedLimit * 0.5f; // magic constant for better feel!
            acceleration = this.acceleration * inAirSpeedMultiplier * inAirSpeedMultiplier;

        }

        //Moving
        positionOffset = positionOffset.normalized * acceleration * crouchWalkMultiplier;
        if (_xVel != 0 || _zVel != 0)
        {
            //Debug.Log("move offset :" + positionOffset);
            isMoving = true;


            float accelerationModifier = 1 - Mathf.Clamp(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / speedLimit, 0, 1);

            //rb.MovePosition(transform.position + _positionOffset  * Time.fixedDeltaTime); // NO INERTIA! also doesnt work with moving platfroms but doesnt glitch between colliders
            //rb.velocity = new Vector3(_positionOffset.x, rb.velocity.y, _positionOffset.z);


            rb.AddForce(positionOffset * accelerationModifier, ForceMode.Acceleration);

            EventDirector.eventPlayerMoving?.Invoke(true, isGrounded, new Vector3(_xVel, 0, _zVel));

        }
        else
        {
            EventDirector.eventPlayerMoving?.Invoke(false, isGrounded, Vector3.zero);
            isMoving = false;



            /*Vector3 slowdownForce = rb.velocity;
            slowdownForce.y = 0;
            float accelerationModifier = -Mathf.Clamp(rb.velocity.magnitude / speedLimit, 0, 1);

            rb.AddForce(slowdownForce.normalized * acceleration * accelerationModifier, ForceMode.Acceleration);*/
        }


    }

    void FakeFriction()
    {
        if (isGrounded)
        {
            float sprintSpeedMultiplier = 1f;

            if (isSprinting)
            {
                sprintSpeedMultiplier = this.sprintSpeedMultiplier;
            }



            Vector3 slowdownForce = rb.velocity;
            slowdownForce.y = 0;
            float accelerationModifier = -Mathf.Clamp(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / speedLimit, 0, 1);

            rb.AddForce(slowdownForce.normalized * acceleration / sprintSpeedMultiplier * fakeInertiaModifier * accelerationModifier, ForceMode.Acceleration);
        }
    }

    void FakeInertia()
    {
        if (isGrounded == false && wasFakeInertiaPerformed == false)
        {
            fakeInertia = globalSpeed;
            fakeInertia.y = 0;

            rb.AddForce(fakeInertia, ForceMode.VelocityChange);

            wasFakeInertiaPerformed = true;

            Debug.Log(Time.time + " Faking inertia: " + fakeInertia.ToString("f3"));
        }
        if (isGrounded)
        {
            wasFakeInertiaPerformed = false;
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
    void Sprinting()
    {
        if (Input.GetButton("SlowWalk"))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
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
        globalSpeed = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
        //Debug.Log("velocity: " + globalSpeed.ToString("f3"));

        EventDirector.globalSpeedEvent?.Invoke(globalSpeed);
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
