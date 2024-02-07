using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
        public bool enemyAttack = false;
        int countdown;
        void Start(){
            if(enemyAttack){
                countdown = 100000;
            } else{
                countdown = 200;
            }
        }
        
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
