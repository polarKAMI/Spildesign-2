using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygteaoesound : MonoBehaviour
{

    public AudioSource lygtelyd;
    // Start is called before the first frame update
    void Start()
    {
        Playlygtelyd();
    }

   
    private void Playlygtelyd()
    {
        lygtelyd.Play();
    }

}
