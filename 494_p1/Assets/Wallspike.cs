using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallspike : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Pokey");
            StartCoroutine(other.gameObject.GetComponent<Health>().Hit(transform.position));
        }
    }
}
