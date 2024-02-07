using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorButtonParent : MonoBehaviour
{
    public GameObject[] buttons;

    public GameObject[] blocks;

    public bool open = false;

    public Sprite[] openSprt;

    public GameObject[] door;

    public bool HasDoor;


    private AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = Resources.Load<AudioClip>("Zelda/Sound-Effects/Secret Found");
    }

    // Update is called once per frame
    void Update()
    {
        if(!open){
            bool check = true;
            for(int i = 0; i < buttons.Length; ++i){
                check = buttons[i].GetComponent<MovableBlockDetector>().IsBlockPlaced();
            }

            if(check){
                open = true;
                for(int i = 0; i < door.Length; ++i){
                    door[i].GetComponent<SpriteRenderer>().sprite = openSprt[i];
                    door[i].GetComponent<BoxCollider>().enabled = false;
                }
                AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);

            } 
        } 
    }

    void OnTriggerExit(){
        if(!open){
            Debug.Log("reseting blocks");
            for(int i = 0; i < buttons.Length; ++i){
                buttons[i].GetComponent<MovableBlockDetector>().ResetBlock();
                blocks[i].GetComponent<MoveableBlock>().resetPosition();
            }
        }
    }
}
