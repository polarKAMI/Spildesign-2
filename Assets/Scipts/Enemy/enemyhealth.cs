using UnityEngine;


public class enemyhealth : MonoBehaviour
{
    public int Maxhealth = 10;
    public int currentenemyhealth;

    public GameObject Nissedudsound;
    public LogSO log;
    public GameObject Nisseavsound;
    public NotificationManager notificationManager;
    public EnemyMovement enemyMovement;
    public Enemy_Chase enemyChase;
    public GameObject damagescript;
    public Animator animator;

    private bool isDead = false;
    void Start()
    {
        currentenemyhealth = Maxhealth;
    }

    private void FixedUpdate()
    {
        if (currentenemyhealth <= 0)
        {
            Die();
        }
    }
    public void Takedamage(int amount)
    {
        if (!isDead)
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
    public void Die()
    {
        if (!log.Collected)
        {
            // Add the log to the LogManager
            LogManager.AddLog(log);
            // Set the collected flag to true
            log.Collected = true;
            notificationManager.ShowNotification("new log;");
        }
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }

        if (enemyChase != null)
        {
            enemyChase.enabled = false;
        }
        if(damagescript != null)
        {
            damagescript.SetActive(false);
        }
        isDead = true;

        animator.SetBool("IsDead", true);
    }
}
