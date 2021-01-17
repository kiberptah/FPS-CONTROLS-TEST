using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    // Attack
    [Header("Attack")]
    public Transform attackTarget;
    [SerializeField]
    public float attackDamage;

    public float attackCooldown;
    public float attackCurrentCooldown;

    public float attackCharge;
    public float attackChargeNeed = 2f;

    public bool attackisReady = false;

    public float attackDistance = 20f;

    public GameObject attackVisualizer;
    // Movement
    [Header("Movement")]
    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    private float hoveringHeight;

    // Senses 
    [Header("Senses")]
    [SerializeField]
    private float sightFOV;
    [SerializeField]
    private float sightDistance;
    private float spherecastThickness = 1f;

    public bool canSeePlayer = false;
    public Vector3 lastKnownPlayerPosition;
    public Vector3 lastCombatPosition;

    [Header("Other")]
    [SerializeField]
    public string currentStateName;
    public INPCState currentState;


    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();
    public ChaseState chaseState = new ChaseState();

    public Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

    }
    private void Start()
    {
        attackTarget = Player.singleton?.transform;

    }

    void OnEnable()
    {
        currentState = patrolState;
    }

    void Update()
    {
        seeingPlayer();
        HoverAboveGround();

        currentState = currentState.DoState(this);
        currentStateName = currentState.ToString();


    }


    void seeingPlayer()
    {
        var _direction = Player.singleton.transform.position - transform.position;
        RaycastHit _hit;

        //if (Physics.SphereCast(transform.position, spherecastThickness, _direction, out _hit, sightDistance, LayerMask.GetMask("Player", "Walls")))
        if (Physics.Raycast(transform.position, _direction, out _hit, sightDistance, LayerMask.GetMask("Player", "Walls", "Enemy", "LevelGeometry")))
        {
            if (_hit.transform.tag == "Player" && Vector3.Angle(_direction, transform.forward) <= sightFOV)
            {
                canSeePlayer = true;
                lastKnownPlayerPosition = _hit.transform.position;
                lastCombatPosition = transform.position;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }

    void HoverAboveGround()
    {
        RaycastHit _hit;

        // Going up
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, hoveringHeight))
        {
            // If ceiling allows
            if (Physics.Raycast(transform.position, Vector3.up, hoveringHeight * 0.5f) == false)
            {
                transform.position = new Vector3(transform.position.x,
                    Mathf.SmoothStep(transform.position.y, transform.position.y + hoveringHeight, 0.1f),
                    transform.position.z);
            }
        }
        // Going down if reasonable
        else if ((Physics.Raycast(transform.position, Vector3.down, out _hit, hoveringHeight * 1.1f)) == false)
        {
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed * 0.25f, Space.World);

        }
    }

    
}
