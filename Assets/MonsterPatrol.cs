using System.Collections;
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

    public int maxHealth = 100;
    public int monsterHitPoints = 100;
    public int attackDamage = 10;
    public float attackDistance = 100f;
    public float attackCooldown = 30f;

    private float lastAttackTime;

    private HealthBarController healthBarController;



    void Start()
    {
        animator = GetComponent<Animator>();
        //new stuff
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBarController = GetComponentInChildren<HealthBarController>();

    }

    void Update()
    {
        if (!playerInRange)
        {
            Patrol();
        }
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
            animator.SetBool("isIdle", false);
        }
        else
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }
    }

    ///////////////////////////////////////////////////////////////////////
    private void chase_player()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            playerInRange = true;
            animator.SetBool("has_detected_player", true);
        }
        else
        {
            playerInRange = false;
            animator.SetBool("has_detected_player", false);
            Patrol();
        }

        if (playerInRange)
        {
            transform.LookAt(player);

            if (distanceToPlayer > attackDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", false);
            }
            else
            {
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    animator.SetBool("isRunning", false);
                    StartCoroutine(AttackAnimation());
                }
                else
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isIdle", true);
                }
            }
        }
    }

    IEnumerator AttackAnimation()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isIdle", false);
        DealDamageToPlayer();
        lastAttackTime = Time.time;
        yield return new WaitForSeconds(attackCooldown);
        animator.SetBool("isAttacking", false);
    }



    private void DealDamageToPlayer()
    {
        player.GetComponent<player_master_script>().TakeDamage(attackDamage);

    }

    public void TakeDamage(int damage)
    {
        monsterHitPoints -= damage;
        if (monsterHitPoints <= 0)
        {
            animator.SetBool("isDead", true);
        }
        else
        {
            animator.SetBool("isHit", true);
            Invoke("ResetIsHit", 0.5f);
        }
        float percentage = Mathf.Clamp01((float)monsterHitPoints / maxHealth);
        healthBarController.UpdateHealthBar(percentage);
    }



    private void ResetIsHit()
    {
        animator.SetBool("isHit", false);
    }

}
