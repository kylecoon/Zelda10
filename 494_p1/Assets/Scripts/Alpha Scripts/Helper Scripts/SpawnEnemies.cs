using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public bool handy = false;

    private bool in_room;

    public bool boomerang_room;

    public bool keese_key_room;
    private bool spawned_children;

    public GameObject boomerang;
    public GameObject key;

    public GameObject hand;

    private bool picked_up_key;
    private bool picked_up_boomerang;
    // Start is called before the first frame update
    void Start()
    {
        in_room = false;
        spawned_children = false;
        picked_up_boomerang = false;
        picked_up_key = false;
        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if (spawned_children && transform.childCount == 0) {
            spawned_children = false;
            if (keese_key_room && !picked_up_key) {
                picked_up_key = true;
                Instantiate(key, transform.position + new Vector3(8.5f, 5.5f), Quaternion.identity);
            }
            if (boomerang_room && !picked_up_boomerang) {
                picked_up_boomerang = true;
                Instantiate(boomerang, transform.position + new Vector3(8.5f, 5.5f), Quaternion.identity);
            }
        }
    }


    void OnTriggerEnter(Collider collider){
        in_room = true;
        Debug.Log("Spawning Enemies");
        if(collider.CompareTag("Player")){
            if(!handy){
                for (int i = 0; i < transform.childCount; i++){
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                spawned_children = true;
            } else {
               StartCoroutine(SpawnHands());
                //Instantiate(weapons[0], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
            }
        }
    }
    void OnTriggerExit(Collider collider){
        in_room = false;
        Debug.Log("despawning Enemies");
        if(collider.CompareTag("Player")){
            for (int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator SpawnHands() {
        yield return new WaitForSeconds(5.0f);
        while (in_room) {
            Instantiate(hand, GameObject.Find("Player").transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2.5f, 4.5f));
        }
    }
}
