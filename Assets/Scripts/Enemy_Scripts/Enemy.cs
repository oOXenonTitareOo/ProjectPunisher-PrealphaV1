using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using Unity.Mathematics;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private Rigidbody rb;
    public Material hitMaterial;
    private Renderer rend;
    private Material originalMaterial;

    // AI Settings
    private NavMeshAgent agent;
    public int currentPointIndex = 0;
    public Vector3 currentTarget;
    private Vector3 lastKnownPlayerPosition;
    public float positionThreshold;
    public float idleTime = 5f;
    public float attackDistance = 5f;
    public float maxVisionDistance = 20f;
    public float minChasingHealth = 30f;
    private float idleTimeCounter;
    public float fieldOfView = 120f;
    public Transform[] patrolPoints;
    private Transform playerTransform;
    private bool canSeePlayer;
    public enum State { Idle, Patrolling, Chasing, Attacking}
    public State state = State.Idle;
    private Quaternion idleStartrotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        GameObject patrolPointParent = GameObject.FindWithTag("PatrolPoint");
        patrolPoints = patrolPointParent.GetComponentsInChildren<Transform>().Where(t => t != patrolPointParent.transform).ToArray();

        Debug.Log("Megtalált pontok száma: " + patrolPoints.Length);
        if(patrolPoints.Length > 0)
        {
            currentTarget = patrolPoints[0].position;
            Debug.Log("Az első pont koordinátája: " + patrolPoints[0].position);
        }

        idleTimeCounter = idleTime;
    }

    void Update()
    {
        LookForPlayer();
        switch(state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrolling:
                Patrolling();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Chasing:
                Chasing();
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            health -= 10;

            if(health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Wound());
            }

            rb.linearVelocity = Vector3.zero;

            lastKnownPlayerPosition = playerTransform.position;

            if(state == State.Idle || state == State.Patrolling)
            {
                state = State.Chasing;
            }
            SetLastKnownPlayerPosition();
        }
    }
    private void LookForPlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        Vector3 rayStart = transform.position + Vector3.up * 1f;
        Vector3 rayDir = directionToPlayer.normalized;

        float angleToPlayer = Vector3.Angle(transform.forward, rayDir);


        if(Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, maxVisionDistance))
        {
            canSeePlayer = hit.transform == playerTransform;
            if(canSeePlayer)
            {   
                lastKnownPlayerPosition = playerTransform.position;

                if(state != State.Attacking)
                {
                    state = State.Chasing;
                }
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
    private void LookAtPlayer()
    {
        if(canSeePlayer)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0f;

            if(direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
    private void SetLastKnownPlayerPosition()
    {
        if(canSeePlayer)
        {
            lastKnownPlayerPosition = playerTransform.position;
        }
    }

    private void Idle()
    {
        agent.ResetPath();

        if(idleTimeCounter >= idleTime)
        {
            idleStartrotation = transform.rotation;
        }

        idleTimeCounter -= Time.deltaTime;

        float elapsedTime = idleTime - idleTimeCounter;
        float speed = (Mathf.PI * 6f) / idleTime;
        float currentAngle = Mathf.Sin(elapsedTime * speed) * 60f;

        transform.rotation = idleStartrotation * Quaternion.Euler(0f, currentAngle, 0f);
        
        if(idleTimeCounter < 0)
        {
            state = State.Patrolling;
            idleTimeCounter = idleTime;
        }
    }

    private void Patrolling()
    {
        if(Vector3.Distance(currentTarget, transform.position) < positionThreshold)
        {
            float chance = UnityEngine.Random.Range(0,100);

            if(chance < 10)
            {
                state = State.Idle;
                return;
            }

            currentPointIndex++;
            currentTarget = patrolPoints[currentPointIndex % patrolPoints.Length].position;
        }
        else
        {
            agent.SetDestination(currentTarget);
        }
    }
    private void Attacking()
    {
        idleTimeCounter = idleTime;
        agent.ResetPath();

        LookAtPlayer();

        if(Vector3.Distance(transform.position, playerTransform.position) > attackDistance || !canSeePlayer)
        {
            if(health < minChasingHealth)
            {
                state = State.Patrolling;
            }
            else
            {
                state = State.Chasing;
            }
        }
    }
    private void Chasing()
    {
        idleTimeCounter = idleTime;
        agent.SetDestination(lastKnownPlayerPosition);

        if(health < minChasingHealth)
        {
            state = State.Patrolling;
        }
        else if(!canSeePlayer && Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
        {
            state = State.Attacking;
        }
        else if(!canSeePlayer && agent.remainingDistance < 1f)
        {
            state = State.Idle;
        }
    }
    void Die()
    {
        if(!this.enabled) return;
        if(agent != null) agent.enabled = false;
        rb.freezeRotation = false;
        rb.AddRelativeTorque(new Vector3(-10f, 0f, 0f), ForceMode.Impulse);
        this.enabled = false;
    }

    IEnumerator Wound()
    {
        rend.material = hitMaterial;
        yield return new WaitForSeconds(0.1f);
        rend.material = originalMaterial;
    }
}
