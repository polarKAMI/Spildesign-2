using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerattack2 : MonoBehaviour
{
    public Transform fireposition;
    public GameObject Projectile;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(Projectile, fireposition.position, fireposition.rotation);

        }
    }
}
