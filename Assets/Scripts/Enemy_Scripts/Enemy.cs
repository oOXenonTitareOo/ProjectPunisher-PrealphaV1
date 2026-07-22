using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using Unity.Mathematics;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using JetBrains.Annotations;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject bulletPrefab;
    public GameObject weaponFlash;
    public Transform bulletSpawnPoint;
    public Transform[] patrolPoints;
    private Transform playerTransform;
    public float bloom;
    public float fireRate;
    public float positionThreshold;
    public float idleTime = 5f;
    public float attackDistance = 5f;
    public float maxVisionDistance = 20f;
    public float minChasingHealth = 30f;
    private float idleTimeCounter;
    public float fieldOfView = 120f;
    private float lastShotTime = 0;
    private Rigidbody rb;
    public Material hitMaterial;
    private Renderer rend;
    private Material originalMaterial;
    private NavMeshAgent agent;
    public int currentPointIndex = 0;
    public Vector3 currentTarget;
    private Vector3 lastKnownPlayerPosition;
    private bool canSeePlayer;
    public enum State { Idle, Patrolling, Chasing, Attacking}
    public State state = State.Idle;
    private Quaternion idleStartrotation;
    private WeaponShellEjection weaponShellEjectionScript;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;

        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        GameObject patrolPointParent = GameObject.FindWithTag("PatrolPoint");
        patrolPoints = patrolPointParent.GetComponentsInChildren<Transform>().Where(t => t != patrolPointParent.transform).ToArray();

        if(patrolPoints.Length > 0)
        {
            currentTarget = patrolPoints[0].position;
        }

        idleTimeCounter = idleTime;

        weaponShellEjectionScript = GetComponent<WeaponShellEjection>();
        if (weaponShellEjectionScript != null)
        {
            weaponShellEjectionScript.ChangeShellGroup("GROUP-ENEMY_SHELLS");
        }
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
        Vector3 rayDir = directionToPlayer.normalized;

        // Ai Vision Debug
        DebugVisualizer.DrawArrow(transform.position, transform.forward * 2f, Color.white);
        DebugVisualizer.DrawArrow(transform.position, rayDir * maxVisionDistance, canSeePlayer ? Color.green : Color.red);

        float angleToPlayer = Vector3.Angle(transform.forward, rayDir);

        if(angleToPlayer <= fieldOfView / 2f)
        {
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

        Shoot();

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
        else if(canSeePlayer && Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
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

    private void Shoot()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

        DebugVisualizer.DrawArrow(bulletSpawnPoint.position, directionToPlayer * 5f, Color.magenta);

        if(Time.time > lastShotTime + fireRate)
        {
            Quaternion bulletRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion flashRotation = bulletSpawnPoint.rotation * Quaternion.Euler(0f,90f,0f);

            float maxInaccuracy = 10f;
            float currentInaccuracy = bloom * maxInaccuracy;
            float randomJaw = UnityEngine.Random.Range(-currentInaccuracy, currentInaccuracy);
            float randomPitch = UnityEngine.Random.Range(-currentInaccuracy, currentInaccuracy);

            bulletRotation *= Quaternion.Euler(randomPitch, randomJaw + 90,  0f);

            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
            Instantiate(weaponFlash, bulletSpawnPoint.position, flashRotation);
            Instantiate(weaponFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            lastShotTime = Time.time;

            weaponShellEjectionScript.EjectShell();
        }
    }
}
