using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        Debug.Log("Spawning Enemies");
        if(collider.CompareTag("Player")){
            for (int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider collider){
        Debug.Log("despawning Enemies");
        if(collider.CompareTag("Player")){
            for (int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
