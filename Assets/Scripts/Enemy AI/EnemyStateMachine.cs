using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    // Attack
    [Header("Attack")]
    public float attackDamage;
    public float attackDistance = 20f;

    public float attackCooldown;
    [HideInInspector] public float attackCurrentCooldown;

    [HideInInspector] public float attackCharge;
    public float attackChargeRequired = 2f;

    [HideInInspector] public Transform attackTarget;
    [HideInInspector] public bool isAttackReady = false;

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

    [HideInInspector] public bool doesSeePlayer = false;
    [HideInInspector] public Vector3 lastKnownPlayerPosition;
    [HideInInspector] public Vector3 lastCombatPosition;

    [Header("Other")]
    [HideInInspector] public string currentStateName;
    public INPCState currentState;


    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();
    public ChaseState chaseState = new ChaseState();


    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

    }
    private void Start()
    {
        attackTarget = Player.singleton?.transform; // fine since player is the only possible target... overwise the whole thing has to be reworked anyway
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
                doesSeePlayer = true;
                lastKnownPlayerPosition = _hit.transform.position;
                lastCombatPosition = transform.position;
            }
            else
            {
                doesSeePlayer = false;
            }
        }
        else
        {
            doesSeePlayer = false;
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
