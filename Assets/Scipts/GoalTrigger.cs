using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.tag == "Player")
       Destroy(other.gameObject);
        Debug.Log("The cake Wasn't a lie after all");
    }
    

}
