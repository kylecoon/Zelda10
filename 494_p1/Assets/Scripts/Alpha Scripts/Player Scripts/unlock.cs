using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlock : MonoBehaviour
{

    //public GameObject key;
    private Collider box;
    public Sprite[] open;

    private SpriteRenderer renderer;

    public GameObject lDoor;
    public GameObject rDoor;

    //private Collider box;
    //public Sprite openL;
    //public Sprite openR;

    public bool doubleDoor;

    private AudioClip openSound;

    //public bool primed = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        box = GetComponent<Collider>();
        openSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/key");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!key.activeSelf){
            renderer.sprite = open;
            box.enabled = false;
        }
        */
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if(collision.collider.CompareTag("Player")){

            Inventory inven = collision.collider.gameObject.GetComponent<Inventory>();
            
            if(doubleDoor){
                //Inventory inven = collision.collider.gameObject.GetComponent<Inventory>();
                if(inven.numKeys > 0){
                    AudioSource.PlayClipAtPoint(openSound, Camera.main.transform.position);
                    inven.numKeys--;
                    inven.UpdateKeyCount();
                    lDoor.GetComponent<SpriteRenderer>().sprite = open[0];
                    rDoor.GetComponent<SpriteRenderer>().sprite = open[1];
                    box.enabled = false;
                }
            } else {
            //Inventory inven = collision.collider.gameObject.GetComponent<Inventory>();
                if(inven.numKeys > 0){
                    AudioSource.PlayClipAtPoint(openSound, Camera.main.transform.position);
                    inven.numKeys--;
                    inven.UpdateKeyCount();
                    renderer.sprite = open[0];
                    box.enabled = false;
                }
            }
        }
    }
}
