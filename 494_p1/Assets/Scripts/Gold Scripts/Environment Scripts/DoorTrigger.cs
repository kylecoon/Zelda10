using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Sprite locked;

    private Sprite open;
    // private Color color;

    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        open = GetComponent<SpriteRenderer>().sprite;
        // color = GetComponent<SpriteRenderer>().color;
    }

    void OnTriggerExit(){
        if(!triggered){
            GetComponent<SpriteRenderer>().sprite = locked;
            // GetComponent<SpriteRenderer>().color = color;
            GetComponent<BoxCollider>().isTrigger = false;
            triggered = true;
        }
    }

    public void reOpen(){
        GetComponent<SpriteRenderer>().sprite = open;
        // GetComponent<SpriteRenderer>().color = color;
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
