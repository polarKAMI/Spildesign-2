using UnityEngine;


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
