using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygtemandenmovement : MonoBehaviour
{
    public Transform[] Patrolpoints;
    public float Movespeed;
    public int patroldestination;

    // Reference to Lygtemanden script
    private Lygtemanden lygtemandenScript;

    void Start()
    {
        lygtemandenScript = GetComponent<Lygtemanden>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patroldestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[0].position, Movespeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Patrolpoints[0].position) < .2f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                patroldestination = 1;
            }
        }

        if (patroldestination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[1].position, Movespeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Patrolpoints[1].position) < .2f)
            {
                transform.localScale = new Vector3(1, 1, 1);
                patroldestination = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // If the Lygtemanden collides with an obstacle, resume patrolling after a delay
        lygtemandenScript.ResumePatrollingAfterDelay();
    }
}