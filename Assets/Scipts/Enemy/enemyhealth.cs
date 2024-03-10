using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemyhealth : MonoBehaviour
{
    public int Maxhealth = 10;
    public int currentenemyhealth;

    void Start()
    {
        currentenemyhealth = Maxhealth;
    }


    public void Takedamage(int amount)
    {
        currentenemyhealth -= amount;
        Debug.Log(currentenemyhealth);

        
        
    }
}
