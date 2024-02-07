using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candleDoorParent : MonoBehaviour
{
    public GameObject[] candles;

    public GameObject middle;

    public GameObject[] doorSides;

    public Sprite open;

    private AudioClip sound;

    private bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        sound = Resources.Load<AudioClip>("Zelda/Sound-Effects/Secret Found");
    }

    // Update is called once per frame
    void Update()
    {
        bool check = true;
        for(int i = 0; i < candles.Length; ++i){
            check = candles[i].GetComponent<Candle>().isLit();
        }

        if(check && !isOpen){
            // AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
            openDoor();
        }
    }

    void openDoor(){

        isOpen = false;
        
        for(int i = 0 ; i < doorSides.Length; ++i){
            doorSides[i].GetComponent<SpriteRenderer>().color = Color.white;
            doorSides[i].GetComponent<SpriteRenderer>().sprite = open;
            doorSides[i].GetComponent<BoxCollider>().enabled = false;
        }
        
        middle.GetComponent<BoxCollider>().enabled = false;
        middle.GetComponent<SpriteRenderer>().sprite = open;

    }
}
