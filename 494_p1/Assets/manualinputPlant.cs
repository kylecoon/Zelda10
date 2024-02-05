using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manualinputPlant : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject plant;

    bool attack = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && attack){
            Debug.Log("test plant attacking");
            plant.GetComponent<plantAttack>().move(attack);
            attack = false;
        } else if(Input.GetKeyDown(KeyCode.Space) && !attack){
            Debug.Log("test plant retracting");
            plant.GetComponent<plantAttack>().move(attack);
            attack = true;
        }
    }
}
