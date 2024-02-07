using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Sprite locked;

    private Sprite open;
    // private Color color;

    private bool triggered;

    private AudioClip closeSound;
    // Start is called before the first frame update
    void Start()
    {
        closeSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/doorClose");
        open = GetComponent<SpriteRenderer>().sprite;
        // color = GetComponent<SpriteRenderer>().color;
    }

    void OnTriggerExit(){
        if(!triggered){
            AudioSource.PlayClipAtPoint(closeSound, Camera.main.transform.position);
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
