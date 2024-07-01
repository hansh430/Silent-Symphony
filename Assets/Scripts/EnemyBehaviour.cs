using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float sightDistance = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip chasingSound;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private float idleTimer = 0f;
    private Transform player;
    private AudioSource audioSource;
    private EnemyState currentState = EnemyState.Idle;
    private bool isChasingAnimation = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        SetDestinationWaypoint();
    }
    private void Update()
    {
        switch (currentState) 
        { 
            case EnemyState.Idle:
                idleTimer += Time.deltaTime;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false);
                PlaySound(idleSound);
                if (idleTimer >= idleTime)
                {
                    NextWaypoint();
                }
                CheckForPlayerDetection();
                break;

            case EnemyState.Walk:
                idleTimer = 0f;
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsChasing", false);
                PlaySound(walkingSound);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = EnemyState.Idle;
                }
                CheckForPlayerDetection();
                break;

            case EnemyState.Chase:
                idleTimer = 0f;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
                isChasingAnimation = true;
                animator.SetBool("IsChasing", true);
                PlaySound(chasingSound);
                if(Vector3.Distance(transform.position,player.position)>sightDistance)
                {
                    currentState = EnemyState.Walk;
                    agent.speed = walkSpeed;
                }
                break;
        }
    }

    private void CheckForPlayerDetection()
    {
        RaycastHit hit;
        Vector3 playerDirection = player.position - transform.position;
        if(Physics.Raycast(transform.position, playerDirection.normalized,out hit, sightDistance))
        {
            currentState = EnemyState.Chase;
            Debug.Log("Player detected!");
        }
    }

    private void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
        SetDestinationWaypoint();
    }

    private void PlaySound(AudioClip soundClip)
    {
        if (!audioSource.isPlaying || audioSource.clip != soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    private void SetDestinationWaypoint()
    {
        agent.SetDestination(wayPoints[currentWaypointIndex].position);
        currentState = EnemyState.Walk;
        agent.speed= walkSpeed;
        animator.enabled = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = currentState == EnemyState.Chase ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }

}
enum EnemyState { Idle, Walk, Chase }
