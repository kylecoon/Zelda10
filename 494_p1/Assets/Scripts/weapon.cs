using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
   
    public Sprite[] weaponSprites;
    //int curSprite = 0;

    public virtual Sprite SwapSprite(int curDirection){
        if(curDirection == 0 || curDirection == 4){
            return weaponSprites[0];
        } else if(curDirection == 1 || curDirection == 5){
            return weaponSprites[1];
        } else if(curDirection == 2 || curDirection == 6){
            return weaponSprites[2];
        } else {
            return weaponSprites[3];
        }
    }


}

public class sword : weapon{
    
    public GameObject hitbox;
    private Collider2D collider;
    
    void Start(){
        collider = GetComponent<Collider2D>();
    }
    public void useWeapon(int curHealth, int maxHealth, int curDirection){
        if(curHealth == maxHealth) shoot();
        if(curDirection == 0 || curDirection == 4){ // ->south
            hitbox.transform.localScale = new Vector3(1,1,1);
            hitbox.transform.position = new Vector2(0.034f,-0.718f);
            collider.offset = new Vector2(-0.1579781f,3.20673e-05f);

        } else if(curDirection == 1 || curDirection == 5){ //->West
            hitbox.transform.localScale = new Vector3(1,1,1);
            hitbox.transform.position = new Vector2(-0.716f,-0.061f);
            collider.offset = new Vector2(-0.1579781f,3.20673e-05f);

        } else if(curDirection == 2 || curDirection == 6){ // ->North
            hitbox.transform.localScale = new Vector3(1,1,1);
            hitbox.transform.position = new Vector2(0.034f,-0.718f);
            collider.offset = new Vector2(0.001522064f,0.1593881f);


        } else if(curDirection == 3 || curDirection == 7){ // ->East
            hitbox.transform.localScale = new Vector3(1,1,1);
            hitbox.transform.position = new Vector2(0.034f,-0.718f);
            collider.offset = new Vector2(-0.1582451f,0.001184821f);

        } else { // if -1, reset
            hitbox.transform.localScale = new Vector3(0,0,0);
            //hitbox.transform.position = new Vector2(0,0);
            //collider.offset = new Vector2(-0.1579781f,3.20673e-05f);

        }

    }

    public void shoot(){

    }
}

public class bow : weapon{
    
    public void useWeapon(){

    }

}
