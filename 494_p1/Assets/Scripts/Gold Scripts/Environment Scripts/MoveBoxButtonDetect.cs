using Unity.VisualScripting;
using UnityEngine;

public class MovableBlockDetector : MonoBehaviour
{
    // public Collider targetCollider;

    private GameObject movedBox;
    // public float marginOfError = 0.1f;

    public bool isBlockPlaced = false;

    public bool isWater;

    // private BoxCollider box;

    void Start(){
        if(isWater){
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("MoveBlock");
            foreach (GameObject obj in objectsWithTag){
            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider != null){
                // Ignore collision between the colliders of this GameObject and the object with the specified tag
                Physics.IgnoreCollision(GetComponent<Collider>(), objCollider);
            }
        }
        }
    }

    private void OnTriggerStay(UnityEngine.Collider other){
        if (other.gameObject.CompareTag("MoveBlock") && !isBlockPlaced){
            // Check if the movable block is fully placed within the target box collider
            if (IsBlockFullyPlaced(other)){
                isBlockPlaced = true;
                other.transform.position = gameObject.transform.position;
                other.GetComponent<Rigidbody>().isKinematic = true;

                if(isWater){
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    other.GetComponent<BoxCollider>().enabled = false;
                }
                movedBox = other.gameObject;

                // Debug.Log("Movable block is fully placed!");
                
            }
        }
    }

    private bool IsBlockFullyPlaced(Collider other)
    {
        bool Xcheck = Mathf.Abs(other.transform.position.x - gameObject.transform.position.x) <= 0.3;
        // Debug.Log(Xcheck);
        bool Ycheck = Mathf.Abs(other.transform.position.y - gameObject.transform.position.y) <= 0.3;
        // Debug.Log(Ycheck);
        return Xcheck && Ycheck;

        // Check if the bounds of the movable block are fully contained within the target box collider
        // Get the corners of the movable block
        // Vector3[] corners = new Vector3[8];
        // Bounds movableBlockBounds = GetComponent<Collider>().bounds;
        // corners[0] = movableBlockBounds.min;
        // corners[1] = movableBlockBounds.max;
        // corners[2] = new Vector3(movableBlockBounds.min.x, movableBlockBounds.min.y, movableBlockBounds.max.z);
        // corners[3] = new Vector3(movableBlockBounds.min.x, movableBlockBounds.max.y, movableBlockBounds.min.z);
        // corners[4] = new Vector3(movableBlockBounds.max.x, movableBlockBounds.min.y, movableBlockBounds.min.z);
        // corners[5] = new Vector3(movableBlockBounds.max.x, movableBlockBounds.min.y, movableBlockBounds.max.z);
        // corners[6] = new Vector3(movableBlockBounds.min.x, movableBlockBounds.max.y, movableBlockBounds.max.z);
        // corners[7] = new Vector3(movableBlockBounds.max.x, movableBlockBounds.max.y, movableBlockBounds.min.z);

        // // Check if all corners are contained within the target collider
        // for (int i = 0; i < corners.Length; i++){
        //     if (!other.bounds.Contains(corners[i])){
        //         return false;
        //     }
        // }

    }

    public bool IsBlockPlaced()
    {
        return isBlockPlaced;
    }

    public void ResetBlock(){
        if(isBlockPlaced){
            movedBox.GetComponent<MoveableBlock>().resetPosition();
        }
        gameObject.GetComponent<BoxCollider>().enabled = true;

        isBlockPlaced = false;
        movedBox = null;
    }
}
