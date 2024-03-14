using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    

    public int Maxhealth = 10;
    public int currenthealth;

    void Start()
    {
        currenthealth = Maxhealth;
    }

   

    public void Takedamage(int amount)
    {
        currenthealth -= amount;
        Debug.Log(currenthealth);

        if (currenthealth <= 0)
        {
            SceneManager.LoadScene("SampleScene");
            Start();
        }
           



    }
}
