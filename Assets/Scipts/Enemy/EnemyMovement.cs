using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] Patrolpoints;
    public float Movespeed;
    public int patroldestination;
  

    // Update is called once per frame
    void Update()
    {
        if (patroldestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[0].position, Movespeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Patrolpoints[0].position) < .2f)
            {
                transform.localScale = new Vector3(1.7f, 1.5f, 1);
                patroldestination = 1;
            }
        }

        if (patroldestination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[1].position, Movespeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Patrolpoints[1].position) < .2f)
            {
                transform.localScale = new Vector3(-1.7f, 1.5f, 1);
                patroldestination = 0;
            }
        }
    }
}
