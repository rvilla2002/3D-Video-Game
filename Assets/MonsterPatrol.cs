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

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Patrol();
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
}
