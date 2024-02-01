using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltPickup : MonoBehaviour
{   
    public GameObject player;
    public string weaponName;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
// <<<<<<< HEAD
        if (collision.gameObject.CompareTag("Player")) {
            player.GetComponent<Attacking>().AddAlt(gameObject.name);
            Destroy(this.gameObject);

        }
    }
}
