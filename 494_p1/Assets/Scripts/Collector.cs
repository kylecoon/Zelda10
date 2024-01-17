using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    Inventory inventory;
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

        if (object_collided_with.CompareTag("rupee")) {
            if (inventory != null) {
                inventory.AddRupees(1);
            }
            Destroy(object_collided_with);
        }
    }
}