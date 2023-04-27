using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    public List<Transform> waypoints;
    public float patrolSpeed = 3f;
    public float waypointThreshold = 0.5f;
    public float rotationSpeed = 500f;

    private int currentWaypointIndex = 0;
    private Animator animator;

    //stuff im using, to not mess up ur script
    public float detectionRadius = 500f; // the radius around the monster that triggers detection
    public float moveSpeed = 70f; // the speed at which the monster moves towards the player
    
    private Transform player; // reference to the player's transform component
    private bool playerInRange = false; // flag to track if player is within detection range

    void Start()
    {
        animator = GetComponent<Animator>();
        //new stuff
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //Patrol();
        chase_player();
    }

    private void Patrol()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);

        if (distance > waypointThreshold)
        {
            transform.position += direction * patrolSpeed * Time.deltaTime;

            // Rotate the monster towards the target waypoint
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            animator.SetBool("isWalking", true);
        }
        else
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            animator.SetBool("isWalking", false);
        }
    }

    ///////////////////////////////////////////////////////////////////////
    private void chase_player() {
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius) {
            //print("Player in Range is true");
            playerInRange = true;
            animator.SetBool("has_detected_player", true);
        }
        else {
            playerInRange = false;
            animator.SetBool("has_detected_player", false);
            Patrol();
        }
        
        if (playerInRange) {
            print("Entering player in range script");
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        }
    }
}
