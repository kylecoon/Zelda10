using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltPickup : MonoBehaviour
{   
    public GameObject player;
    public string weaponName;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            player.GetComponent<Attacking>().AddAlt("bow");
            gameObject.SetActive(false);
        }
    }
}
