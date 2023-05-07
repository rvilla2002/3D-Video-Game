using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class player_master_script : MonoBehaviour
{
    public float player_speed = 500;
    public Camera cam;
    public int maxHealth = 100;
    public int hitPoints = 100;
    public int attackValue = 10;
    public float attackDistance = 100f;


    private Animator animator;
    private bool isAttacking = false;
    private float attackAnimationLength;

    private HealthBarController healthBarController;

    public LayerMask monsterLayer;


    void Start()
    {
        healthBarController = GetComponentInChildren<HealthBarController>();
        animator = GetComponent<Animator>();

        AnimatorClipInfo[] clipInfoArray = animator.GetCurrentAnimatorClipInfo(0);
        foreach (AnimatorClipInfo clipInfo in clipInfoArray)
        {
            if (clipInfo.clip.name == "NormalAttack01_SwordShield")
            {
                attackAnimationLength = clipInfo.clip.length;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(!PauseMenu.isPaused) {
            // Converting the mouse position to a point in 3D-space
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            // Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
            float t = cam.transform.position.y / (cam.transform.position.y - point.y);
            Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, 1, t * (point.z - cam.transform.position.z) + cam.transform.position.z);
            // Rotating the object to that point
            transform.LookAt(finalPoint, Vector3.up);
            // Checks if any key was pressed
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * player_speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * player_speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * player_speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate((Vector3.forward * player_speed * Time.deltaTime) * -1);
                animator.SetBool("isMoving", true);
            }
            else
            {
                // If no key was pressed, person not moving, so isMoving is false, to start idle animation.
                animator.SetBool("isMoving", false);
            }

            if (Input.GetMouseButtonDown(0) && !isAttacking)
            {
                StartCoroutine(TriggerAttack());
            }
        }

    }

    IEnumerator TriggerAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance, monsterLayer))
        {
            MonsterPatrol monster = hit.transform.GetComponent<MonsterPatrol>();
            if (monster != null)
            {
                monster.TakeDamage(attackValue);
            }
        }
        yield return new WaitForSeconds(.3f);
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            animator.SetBool("isDead", true);
            SceneManager.LoadScene("game_over");
        }
        else
        {
            animator.SetBool("isHit", true);
            Invoke("ResetIsHit", 0.5f);
        }
        float percentage = Mathf.Clamp01((float)hitPoints / maxHealth);
        healthBarController.UpdateHealthBar(percentage);
    }



    private void ResetIsHit()
    {
        animator.SetBool("isHit", false);
    }
}