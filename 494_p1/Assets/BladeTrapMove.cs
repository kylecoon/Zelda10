using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrapMove : MonoBehaviour
{
    // Start is called before the first frame update
 
    public LayerMask detectionLayer; // Set this in the Unity Editor to the layer you want to detect
    //public string playerTag = "Player";
    private float baseMoveSpeed = 5f;

    public float distance;

    void Update()
    {
        // Cast a ray from the object's position forward
        Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hit;

        // Check if the ray hits an object with the specified tag
        if (Physics.Raycast(ray, out RaycastHit hit, distance, detectionLayer) && hit.collider.CompareTag("Player"))
        {
            // Check if the player is directly above, below, to the left, or to the right
            if (IsPlayerInCardinalDirection(hit.transform.position))
            {
                // Move towards the player, adjusting speed based on distance
                Vector3 targetPosition = hit.transform.position;
                MoveTowards(targetPosition);
            }
        }
    }

    void MoveTowards(Vector2 targetPosition)
    {
        // Calculate the direction to the target in 2D
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Calculate speed based on distance
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        float moveSpeed = baseMoveSpeed + distanceToTarget * 0.5f; // Adjust multiplier as needed

        // Move the object in that direction in 2D
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    bool IsPlayerInCardinalDirection(Vector2 playerPosition)
    {
        float distanceX = Mathf.Abs(playerPosition.x - transform.position.x);
        float distanceY = Mathf.Abs(playerPosition.y - transform.position.y);

        // Check if the player is directly above, below, to the left, or to the right in 2D
        return distanceX < 1.0f || distanceY < 1.0f;
    }
}
