using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BladeTrapMove : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject SpikeTile;
 
    public LayerMask detectionLayer; // Set this in the Unity Editor to the layer you want to detect
    //public string playerTag = "Player";
    private float baseMoveSpeed = 3f;

    public float distance;

    public Vector3 startPos;

    public Vector3[] stopPositions; 

    void Start(){

        startPos = transform.position;
    }

    void Update()
    {
        //Debug.Log("update");
        // Cast a ray from the object's position forward
        Ray rayUp = new Ray(transform.position, transform.up);
        Ray rayDown = new Ray(transform.position, -transform.up);
        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);

        RaycastHit Uphit;
        RaycastHit Downhit;
        RaycastHit Lefthit;
        RaycastHit Righthit;
        
        Debug.DrawRay(rayUp.origin, rayUp.direction);
        Debug.DrawRay(rayDown.origin, rayDown.direction);
        Debug.DrawRay(rayLeft.origin, rayLeft.direction);
        Debug.DrawRay(rayRight.origin, rayRight.direction);

        bool rayCasting = transform.position == startPos;

        // Check if the ray hits an object with the specified tag
        // ... (your existing code)

// Check if the ray hits an object with the specified tag
    if(rayCasting){
        int direction = 3;
        if (Physics.Raycast(rayUp, out Uphit, distance, detectionLayer) && Uphit.collider.CompareTag("Player"))
        {
            Debug.Log("Udetect");
           
            if (IsPlayerInCardinalDirection(Uphit.transform.position, out direction))
            {
                Vector3 targetPosition = Uphit.transform.position;
                MoveTowards(targetPosition, direction);
            }
        }
        else if (Physics.Raycast(rayDown, out Downhit, distance, detectionLayer) && Downhit.collider.CompareTag("Player"))
        {
            Debug.Log("Ddetect");
           
            if (IsPlayerInCardinalDirection(Downhit.transform.position, out direction))
            {
                Vector3 targetPosition = Downhit.transform.position;
                MoveTowards(targetPosition, direction);
            }
        }
        else if (Physics.Raycast(rayRight, out Righthit, distance, detectionLayer) && Righthit.collider.CompareTag("Player"))
        {
            Debug.Log("Rdetect");
            if (IsPlayerInCardinalDirection(Righthit.transform.position, out direction))
            {
                Vector3 targetPosition = Righthit.transform.position;
                MoveTowards(targetPosition, direction);
            }
        }
        else if (Physics.Raycast(rayLeft, out Lefthit, distance, detectionLayer) && Lefthit.collider.CompareTag("Player"))
        {
            Debug.Log("Ldetect");
            if (IsPlayerInCardinalDirection(Lefthit.transform.position, out direction))
            {
                Vector3 targetPosition = Lefthit.transform.position;
                MoveTowards(targetPosition, direction);
            }
        }
    } else {
        transform.position = Vector3.MoveTowards(transform.position, startPos, 1 * Time.deltaTime);
    }

// ... (rest of your existing code)

        
    }

    void MoveTowards(Vector3 targetPosition, int direction){
        // Debug.Log("moving");
        // // Calculate the direction to the target in 2D
        // Vector3 direction = (targetPosition - (Vector3)transform.position).normalized;
        // direction.z = 0;

        // // Calculate speed based on distance
        // float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        // float moveSpeed = baseMoveSpeed + distanceToTarget * 0.5f; // Adjust multiplier as needed

        // // Move the object in that direction in 2D
        // //transform.position = Vector3.MoveTowards(SpikeTile.transform.position, targetPosition, moveSpeed * Time.deltaTime)
        
        // transform.Translate(direction * moveSpeed * Time.deltaTime);
        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, transform.position, transform.position + stopPositions[direction], baseMoveSpeed));
        //return to start posistion
        Vector3.MoveTowards(transform.position, startPos, 1 * Time.deltaTime);

    }

    bool IsPlayerInCardinalDirection(Vector3 playerPosition, out int direction){
        float distanceX = playerPosition.x - startPos.x;
        float distanceY = playerPosition.y - startPos.y;

        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY)){
            // Player is either to the left or right
            if (distanceX > 0){
                direction = 2; // right
            } else {
                direction = 3; // left
            }
        } else {
            // Player is either above or below
            if (distanceY > 0){
                direction = 0; // up
            } else {
                direction = 1; // down
            }
        }

        // Check if the player is roughly in the same plane in 3D space
        return Mathf.Abs(distanceX) < 1.0f || Mathf.Abs(distanceY) < 1.0f;
    }

    // void OnCollisionEnter(UnityEngine.Collision collision)
    // {
    //     if(collision.gameObject.CompareTag("Player")) baseMoveSpeed = 0;
    // }
    // void OnCollisionExit(UnityEngine.Collision collision){
    //     if(collision.gameObject.CompareTag("Player")) baseMoveSpeed = 3f;
    // 

}
