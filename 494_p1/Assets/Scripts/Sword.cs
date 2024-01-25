using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Sword hit");
            other.gameObject.GetComponent<EnemyHealth>().AlterHealth(-1);
        }
    }
}
