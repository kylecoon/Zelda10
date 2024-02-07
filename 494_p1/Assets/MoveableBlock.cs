using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour
{
    
    public bool reset = true;

    public bool TrueReset; // reset even if in place

    private Vector2 origin;
    // Start is called before the first frame update
    public float gridSize = 0.125f; // Size of the grid
    private Rigidbody rb;

    void Start() //gpt helped
    {
        rb = GetComponent<Rigidbody>();
        // if (rb == null)
        // {
        //     Debug.LogError("Rigidbody component not found on the object.");
        // }
    }

    void Update()
    {
        // Calculate the position on the grid
        Vector3 newPos = SnapToGrid(transform.position);

        // Update the position of the block
        rb.MovePosition(newPos);
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        float snappedZ = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(snappedX, snappedY, snappedZ);
    }


    public void resetPosition(){
        if(reset || TrueReset){
            gameObject.transform.position = origin;
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
