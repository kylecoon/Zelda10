using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockUp : MonoBehaviour
{

    public GameObject lDoor;
    public GameObject rDoor;

    private Collider box;
    public Sprite openL;
    public Sprite openR;

    //private SpriteRenderer renderer;

    //public bool primed = false;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<Collider>();
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
            if(inven.numKeys > 0){
                inven.numKeys--;
                lDoor.GetComponent<SpriteRenderer>().sprite = openL;
                rDoor.GetComponent<SpriteRenderer>().sprite = openR;
                box.enabled = false;
            }
        }
    }
}
