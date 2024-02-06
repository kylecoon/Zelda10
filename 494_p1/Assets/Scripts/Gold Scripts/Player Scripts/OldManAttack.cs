using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManAttack : MonoBehaviour
{
    // Start is called before the first frame update
        public GameObject[] plantAttacks;
    void Start()
    {
        StartCoroutine(opening());
    }

    // Update is called once per frame
    void Update(){
        
    }

    IEnumerator opening(){


        yield return null;
    }

    IEnumerator fighting(){
        while(true){
            int attack = Random.Range(0,2);

            if(attack == 0){
                StartCoroutine(MakeplantAttack());
            } else if(attack == 1){
                StartCoroutine(spikeAttack());
            }
        }
    }

    IEnumerator MakeplantAttack(){
        for(int i = 0; i < plantAttacks.Length; ++i){
            plantAttacks[i].GetComponent<plantAttack>().move(true);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2f);
        for(int i = plantAttacks.Length -1 ; i >= 0; ++i){
            plantAttacks[i].GetComponent<plantAttack>().move(false);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    IEnumerator spikeAttack(){

        yield return null;
    }

    void OnCollisionEnter(UnityEngine.Collision collision){
        if(collision.collider.CompareTag("Enemy")){
            
        }

    }
}
