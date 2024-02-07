using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public GameObject fire;

    private bool lit = false;

    void OnTriggerEnter(Collider collider){
        if(collider.CompareTag("Enemy")){
            lit = true;
            fire.SetActive(true);
        }
    }

    public bool isLit(){
        return lit;
    }

}
