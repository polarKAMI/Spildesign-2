using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;


public class ObjectController : MonoBehaviour
{
    public GameObject prefabToInstantiate;

    public Transform playerCharacter;

    public Transform spawnPoint;

    private GameObject instantiatedObject;

    private bool rotationApplied = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (instantiatedObject == null)
            {
                Vector3 spawnPosition = spawnPoint.position;

                instantiatedObject = Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);

                if (!rotationApplied)
                {
                    instantiatedObject.transform.Rotate(Vector3.forward, -90f);
                    rotationApplied = true;
                }
            }
        }
        else
        {
            if (instantiatedObject != null)
            {
                Destroy(instantiatedObject);
                instantiatedObject = null;
                rotationApplied = false;
            }
        }

        if (instantiatedObject != null)
        {
            instantiatedObject.transform.position = spawnPoint.position;
        }
    }
}

