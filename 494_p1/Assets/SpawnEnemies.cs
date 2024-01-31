using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public bool handy = false;

    public GameObject hand;
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
            if(!handy){
            for (int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }
            } else {
               Instantiate(hand, new Vector3(0,0,0), Quaternion.identity);
                //Instantiate(weapons[0], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
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
