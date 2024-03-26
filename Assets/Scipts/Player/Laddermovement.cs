using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laddermovement : MonoBehaviour
{
    // Start is called before the first frame update

    private float vertical;
    private float speed = 4f;
    public bool isLadder;
    public bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
  

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if( isLadder && Mathf.Abs(vertical) > 0)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if(isClimbing == true) 
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 4f;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isLadder = true;
            
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
