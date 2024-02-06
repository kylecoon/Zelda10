using Unity.VisualScripting;
using UnityEngine;

public class MovableBlockDetector : MonoBehaviour
{
    public Collider targetCollider;
    public float marginOfError = 0.1f;

    private bool isBlockPlaced = false;

    public bool isWater;

    private BoxCollider box;

    void Start(){
        if(isWater){
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("MoveBlock"));
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("MoveBlock")){
            // Check if the movable block is fully placed within the target box collider
            if (IsBlockFullyPlaced()){
                isBlockPlaced = true;
                collision.collider.transform.position = gameObject.transform.position;

                Debug.Log("Movable block is fully placed!");
            }
        }
    }

    private bool IsBlockFullyPlaced()
    {
        // Check if the bounds of the movable block are fully contained within the target box collider
        Bounds movableBlockBounds = GetComponent<Collider>().bounds;
        Bounds targetBounds = targetCollider.bounds;

        return targetBounds.Contains(movableBlockBounds.min) && targetBounds.Contains(movableBlockBounds.max);
    }

    public bool IsBlockPlaced()
    {
        return isBlockPlaced;
    }
}
