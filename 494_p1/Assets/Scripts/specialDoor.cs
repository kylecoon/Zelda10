using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class specialDoor : MonoBehaviour
{

    public GameObject door;

    public Sprite replace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("->East")){
            Rigidbody Box = collider.GetComponent<Rigidbody>();
            Box.velocity = Vector2.zero;
            Box.detectCollisions = false;

            SpriteRenderer Renderer = door.GetComponent<SpriteRenderer>();
            Renderer.sprite = replace;
            Collider col = door.GetComponent<Collider>();
            col.enabled = false;
        }
    }
}
