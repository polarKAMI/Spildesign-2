using UnityEngine;

public class UpdateShaderProperties : MonoBehaviour
{
    public Transform playerTransform; // Assign this in the inspector
    public Material objectMaterial;   // Assign the material that uses the shader graph

    private int playerPositionID;
    private int objectPositionID;

    void Start()
    {
        // Cache property IDs for performance
        playerPositionID = Shader.PropertyToID("_PlayerPosition");
        objectPositionID = Shader.PropertyToID("_ObjectPosition");

        if (objectMaterial == null)
        {
            // Automatically get the material from the MeshRenderer if not assigned
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                objectMaterial = renderer.material;
            }
        }
    }

    void Update()
    {
        if (objectMaterial != null && playerTransform != null)
        {
            // Update the player position in the shader
            objectMaterial.SetVector(playerPositionID, playerTransform.position);

            // Update the object position in the shader
            objectMaterial.SetVector(objectPositionID, transform.position);
        }
    }
}