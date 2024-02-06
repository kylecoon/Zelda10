using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManBossMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator moveBackAndForth(){

        while(!GetComponent<oldManBosHealth>().isDead){
            
        }

        yield return null;
    }
}
