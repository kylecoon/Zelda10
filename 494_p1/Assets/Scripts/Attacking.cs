using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class ArrowKeyMovement : MonoBehaviour
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
    void Awake(){
        Screen.SetResolution(1020, 960, true);
    }

    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");
    }
    // Update is called once per frame
    void Update()
    {

        usingWeapon = false;

    }

    void GetInput() {

        if(Input.GetKey(KeyCode.Space)){
            curAltIndex = curAltIndex + 1 % altWeapons.Length;
            curAlt = altWeapons[curAltIndex];

        } 

        if(Input.GetKey(KeyCode.X)){ // use alt
            usingWeapon = true;
            altWeapon();

        } else if(Input.GetKey(KeyCode.Z)){ // use main
            usingWeapon = true;
            SwordSwing();

        }
    }


    void SwordSwing(){
        sprt.sprite = this.mSword.SwapSprite(0);
        mSword.useWeapon(3,3,0);
    }

    void altWeapon(){
        sprt.sprite = this.curAlt.SwapSprite(0);
        // use weapon
    }
}
