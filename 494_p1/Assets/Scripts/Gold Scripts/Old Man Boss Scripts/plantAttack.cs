using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class plantAttack : MonoBehaviour
{
    // Start is called before the first frame update

    private UnityEngine.Vector2 origin;

    public Sprite open;

    public GameObject head;

    private Sprite closed;

    public bool flipped;
    void Start()
    {
        closed = head.GetComponent<SpriteRenderer>().sprite;
        origin = gameObject.transform.position;
        // flipped = gameObject.transform.rotation.x == 180;
    }

    // Update is called once per frame
    public void move(bool attacking){
        int upORdown = 7;

        Debug.Log("Plant attack triggered");
        if(attacking){

            if(flipped){
                upORdown*= -1;
            } 

            head.GetComponent<SpriteRenderer>().sprite = open;
            UnityEngine.Vector2 finalPos = new UnityEngine.Vector2(origin.x, origin.y + upORdown);

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, origin, finalPos, 0.5f));
            Debug.Log("Plant attack should move");
            // head.GetComponent<SpriteRenderer>().sprite = open;
        } else {
            // UnityEngine.Vector2 finalPos = cu
            head.GetComponent<SpriteRenderer>().sprite = closed;
            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, gameObject.transform.position, origin, 0.5f));
            Debug.Log("Plant attack should return");
        }
    }
}
