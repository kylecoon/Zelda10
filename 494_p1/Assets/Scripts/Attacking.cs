using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class Attacking : MonoBehaviour
{
    SpriteRenderer sprt;
    private Rigidbody rb;
    private int spriteVersion = 0;
    private Sprite[] sprites;

    private sword mSword;

    private weapon[] altWeapons;
    private int curAltIndex;
    private weapon curAlt;

    private bool usingWeapon = false;

    // private int curDirection = 0;
    // Start is called before the first frame update

    void Start()
    {
       // sprt = GetComponent<SpriteRenderer>();
        Screen.SetResolution(1020, 960, true);
        rb = GetComponent<Rigidbody>();
        sprt = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");
    }
    // Update is called once per frame
    void Update()
    {

        usingWeapon = false;
        GetInput();

    }

    void GetInput() {


        Debug.Log("gotInput");
        if(Input.GetKey(KeyCode.Space)){
            curAltIndex = curAltIndex + 1 % altWeapons.Length;
            curAlt = altWeapons[curAltIndex];

        } 

        if(Input.GetKey(KeyCode.X)){ // use alt
            usingWeapon = true;
            altWeapon();

        } else if(Input.GetKey(KeyCode.Z)){ // use main
            Debug.Log("swing");
            usingWeapon = true;
            StartCoroutine(SwordSwing());

        }
    }

    

    void createSword(){
        Collider collider = GetComponentInChildren<Collider>();
        
    }

    
    IEnumerator SwordSwing(){

        Sprite hold = sprt.sprite;
        if(hold == sprites[0] || hold == sprites[12] ){
            Debug.Log("RSword");
            sprt.sprite = sprites[36];

        } else if(hold == sprites[1] || hold == sprites[13]){
            sprt.sprite = sprites[37];

        } else if(hold == sprites[2] || hold == sprites[14]){
            sprt.sprite = sprites[38];

        } else if(hold == sprites[3] || hold == sprites[15]){
            sprt.sprite = sprites[39];
        } else {
             yield return null;
        }
        //GameObject.Instantiate(swordHitbox);
        //sprt.sprite = this.mSword.SwapSprite(0);
        mSword.useWeapon(3,3,0);
        yield return new WaitForSeconds(1.0f);

        sprt.sprite = hold;

        yield return null;
    }

    void altWeapon(){
        sprt.sprite = this.curAlt.SwapSprite(0);
        // use weapon
        
    }
}
