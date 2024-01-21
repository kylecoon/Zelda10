using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordShot : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite[] compareSprites;
    private SpriteRenderer sprt;

    private int curSprite = 1; //
    
    int direction = 0; //  get direction

    private Sprite sprt1;
    private Sprite sprt2;

    private float totalDistance = 0;
    // Start is called before the first frame update
    void Start(){

        sprt = GetComponent<SpriteRenderer>();
        Sprite sprtDir = GetComponentInParent<SpriteRenderer>().sprite;
        if(sprtDir == compareSprites[0]){
            sprt1 = sprites[0];
            sprt2 = sprites[1];
            direction = 1;

        } else if(sprtDir == compareSprites[1]){
            sprt1 = sprites[2];
            sprt2 = sprites[3];
            direction = 2;

        } else if(sprtDir == compareSprites[2]){
            sprt1 = sprites[4];
            sprt2 = sprites[5];
            direction = 3;

        } else {
            sprt1 = sprites[6];
            sprt2 = sprites[7];
            direction = 4;
        }

        sprt.sprite = sprt1;
        //collider = GetComponent<BoxCollider>();
        


    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("fire");
        if(curSprite % 2 == 0){
            sprt.sprite = sprt1;
        } else {
            sprt.sprite = sprt2;
        }
        curSprite++;

        if(direction == 1){
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);

        } else if(direction == 2){
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y - 0.5f);

        } else if(direction == 3){
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f);
        
        } else {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y);

        }

        totalDistance += 0.5f;

        if(totalDistance >= 7){
            Destroy(this);
        }
        
    }

    void OnTriggerEnter(Collider collider){

        if(collider.CompareTag("Enemy")){
            Destroy(this);
        }
        //collider
    }
}
