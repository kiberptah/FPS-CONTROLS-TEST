using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [Header("References")]
    public GameObject groundCheck;
    [Header("Features")]
    public bool slowWalkingEnabled = true;
    public bool jumpingEnabled = true;
    public bool crouchingEnabled = true;
    [Header("Tweaking")]
    public float setSpeedZ = 6f;
    public float setSpeedX = 6f;
    public float acceleration = 75f;

    private float maxSpeedZ;
    private float maxSpeedX;

    public float crouchWalkingMultiplier = 0.2f;
    public float slowWalkingMultiplier = 0.3f;

    public float airWalkSpeedMultiplier = 0.05f;
    public float jumpStrengh = 10f;

    [Header("Debug")]
    [SerializeField]
    private bool isGrounded = true;
    [SerializeField]
    private bool isSlowWalking = false;
    [SerializeField]
    private Vector3 localVelocityDebug;
    [SerializeField]
    private bool isCrouching = false;  
    //
    private float targetCrouchSquish = 0.45f;
    private float heightScaleSave;

    IEnumerator Crch;
    Bounds myBounds;
    private float standingHeight;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        heightScaleSave = transform.localScale.y;

        maxSpeedX = setSpeedX;
        maxSpeedZ = setSpeedZ;

        if (isCrouching)
        {
            Debug.Log("FIX THIS! Or don't spawn player crouched");
        }
        myBounds = gameObject.GetComponent<Collider>().bounds;
        standingHeight = myBounds.size.y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        localVelocityDebug = transform.InverseTransformDirection(rb.velocity);

        Movement();


    }

    private void Update()
    {
        Jumping();
        Crouching();
    }

    private void Movement()
    {
        float _xVel = Input.GetAxis("Horizontal");
        float _zVel = Input.GetAxis("Vertical");

        float _xStep = _xVel * acceleration * Time.deltaTime;
        float _zStep = _zVel * acceleration * Time.deltaTime;

        Vector3 _addForce = Vector3.zero;
        float _airWalkSpeedMultiplier;
        float _slowWalkingMultiplier;
        float _crouchWalkingMultiplier;


        // Reduce movespeed in air
        if (isGrounded)
        {
            _airWalkSpeedMultiplier = 1f;
        }
        else
        {
            _airWalkSpeedMultiplier = airWalkSpeedMultiplier;
        }
        // Slow Walking
        if (Input.GetButton("SlowWalk"))
        {
            isSlowWalking = true;
        }
        else
        {
            isSlowWalking = false;
        }
        if (slowWalkingEnabled && isSlowWalking)
        {
            _slowWalkingMultiplier = slowWalkingMultiplier;
        }
        else
        {
            _slowWalkingMultiplier = 1f;
        }
        //Crouch Walkspeed
        if (isCrouching)
        {
            _crouchWalkingMultiplier = crouchWalkingMultiplier;
        }
        else
        {
            _crouchWalkingMultiplier = 1f;
        }

        // Limit speed
        if ((Mathf.Abs(transform.InverseTransformDirection(rb.velocity).x) + Mathf.Abs(_xStep)) > maxSpeedX * _slowWalkingMultiplier * _crouchWalkingMultiplier)
        {
            _xStep = Mathf.Sign(transform.InverseTransformDirection(rb.velocity).x) * maxSpeedX * _slowWalkingMultiplier * _crouchWalkingMultiplier
                - transform.InverseTransformDirection(rb.velocity).x;
        }
        if ((Mathf.Abs(transform.InverseTransformDirection(rb.velocity).z) + Mathf.Abs(_zStep)) > maxSpeedZ * _slowWalkingMultiplier * _crouchWalkingMultiplier)
        {
            _zStep = Mathf.Sign(transform.InverseTransformDirection(rb.velocity).z) * maxSpeedZ * _slowWalkingMultiplier * _crouchWalkingMultiplier
                - transform.InverseTransformDirection(rb.velocity).z;
        }
        // Start Moving
        _addForce += new Vector3(_xStep, 0, _zStep);
        rb.AddRelativeForce(_addForce * _airWalkSpeedMultiplier, ForceMode.VelocityChange);
        

    }

    private void Jumping()
    {
        float _xVel = Input.GetAxis("Horizontal");
        float _zVel = Input.GetAxis("Vertical");

        isGrounded = groundCheck.GetComponent<GroundCheck>().isGrounded;
        if (isGrounded && jumpingEnabled)
        {
            if (Input.GetButtonDown("Jump") && !isCrouching)
            {
                rb.AddForce(Vector3.up * jumpStrengh * rb.mass, ForceMode.Impulse);
                //rb.AddRelativeForce(new Vector3(_xVel * jumpPush, jumpStrengh * rb.mass, _zVel * jumpPush), ForceMode.Impulse);

                groundCheck.GetComponent<GroundCheck>().isGrounded = false;
            }
        }
    }

    private void Crouching()
    {
        if (Input.GetButtonDown("Crouch"))
        {            
            if (Crch != null)
            {
                StopCoroutine(Crch);
            }

            Crch = CrouchSquishing(!isCrouching);
            StartCoroutine(Crch);

            isCrouching = !isCrouching;
        }       
        
    }

    IEnumerator CrouchSquishing(bool _isStanding)
    {
        bool _canStand = true;

        Bounds _currentBounds = gameObject.GetComponent<Collider>().bounds; ;
        float _currentHeight = _currentBounds.size.y;
        
        Vector3 _rayOrigin = new Vector3(transform.position.x, transform.position.y - _currentHeight * 0.5f, transform.position.z);
        RaycastHit _hit;
        
        if (_isStanding == true)
        {
            while (transform.localScale.y > targetCrouchSquish)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.025f, transform.localScale.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else 
        {
            if (Physics.Raycast(_rayOrigin, Vector3.up, out _hit, standingHeight, ~LayerMask.GetMask("Player")))
            {
                Debug.Log(_rayOrigin);
                Debug.Log(_hit.transform.name);
                _canStand = false;
            }
            if (_canStand)
            {
                while (transform.localScale.y < heightScaleSave)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.025f, transform.localScale.z);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
        yield return null;
    }
}
