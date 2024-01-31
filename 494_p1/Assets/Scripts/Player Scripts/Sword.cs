using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour
{

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy")) {
            Debug.Log("Sword hit");
            collision.gameObject.GetComponent<EnemyHealth>().AlterHealth(-1);
        }
    }
     void OnTriggerEnter(Collider other) {    
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Sword hit");
            other.gameObject.GetComponent<EnemyHealth>().AlterHealth(-1);
        }
    }
}
