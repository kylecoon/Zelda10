using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
        int countdown = 100;
        void Update(){
            countdown--;

            if(countdown == 0){
                Destroy(gameObject);
            }
        }
    // public static int direction;
    // public static int num;
    // Start is called before the first frame update
    // void OnCollisionEnter(Collision other)
    // {
    //     Destroy(gameObject);
    // }
        void OnTriggerEnter(Collider other){
            if(other.CompareTag("Wall") || other.CompareTag("Player")){
                Destroy(gameObject);
            }
        }
}
