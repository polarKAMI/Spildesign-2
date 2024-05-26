using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] Patrolpoints;
    public float Movespeed;
    public int patroldestination;

    public Animator animator;
    Enemy_Chase enemymovement;

    private void Start()
    {
        enemymovement = GetComponent<Enemy_Chase>();
        IgnorePlayerCollision();
    }

    private void OnEnable()
    {
        // When the script is enabled, stop the animation
        animator.SetBool("IsTransformed", false);
    }

    private void OnDisable()
    {
        // When the script is disabled, play the animation
        animator.SetBool("IsTransformed", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (patroldestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[0].position, Movespeed * Time.deltaTime);
            enemymovement.FaceTarget(Patrolpoints[0].position);
            if (Vector2.Distance(transform.position, Patrolpoints[0].position) < .2f)
            {
                patroldestination = 1;
            }
        }

        if (patroldestination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[1].position, Movespeed * Time.deltaTime);
            enemymovement.FaceTarget(Patrolpoints[1].position);
            if (Vector2.Distance(transform.position, Patrolpoints[1].position) < .2f)
            {
                patroldestination = 0;
            }
        }
    }

    void IgnorePlayerCollision()
    {
        Collider2D myCollider = GetComponent<Collider2D>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider);
            }
        }
    }
}