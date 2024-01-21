using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    Inventory inventory;


    public AudioClip RupCollect;
    public AudioClip keyCollect;
    void Start()
    {
        
        inventory = GetComponent<Inventory>();
        if (inventory == null) {
            Debug.LogWarning("WARNING: Gameobject with a collector has no inventory to store things in!");
        }
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider coll)
    {
        
        GameObject object_collided_with = coll.gameObject;

        Debug.Log("trigger");
        
        if (object_collided_with.CompareTag("rupee")) {
            if (inventory != null) {
                inventory.AddRupees(1);
            }
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(RupCollect, Camera.main.transform.position);
            
        } 
        if(object_collided_with.CompareTag("key")){
            coll.gameObject.SetActive(false);
            Debug.Log("worked");
            AudioSource.PlayClipAtPoint(keyCollect, Camera.main.transform.position);

        }
    }
}
