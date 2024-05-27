using UnityEngine;


public class enemyhealth : MonoBehaviour
{
    public int Maxhealth = 10;
    public int currentenemyhealth;

    public GameObject Nissedudsound;

    public GameObject Nisseavsound;
    void Start()
    {
        currentenemyhealth = Maxhealth;
    }


    public void Takedamage(int amount)
    {
        currentenemyhealth -= amount;

        if (currentenemyhealth < Maxhealth)
        {
            Instantiate(Nissedudsound);
        }
        else
        {
            Instantiate(Nisseavsound);
        }
        

        currentenemyhealth -= amount;
        Debug.Log("Nisse tog squ skade");


        
        
    }
}
