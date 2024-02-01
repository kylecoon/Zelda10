using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    public bool dynamicPosistion;
    public GameObject[] enemies;

    public GameObject child;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!dynamicPosistion){
            bool allGone = true;

            if(enemies.Length == 0){ 
                child.SetActive(true);
                Destroy(gameObject);
            }

            for(int i = 0; i < enemies.Length; ++i){
                if(enemies[i].activeSelf){ 
                    allGone = false;
                    continue;
                }
            }

            if(allGone){
                child.SetActive(true);
                Destroy(gameObject);
            }

        
        } else {
            if(enemies[0]){
                child.transform.position = enemies[0].transform.position;
            }
        }

    }


}
