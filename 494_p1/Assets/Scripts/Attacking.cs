using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
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

    public GameObject[] prefabs;

    // private int curDirection = 0;
    // Start is called before the first frame update

    void Start()
    {
       // sprt = GetComponent<SpriteRenderer>();
        Screen.SetResolution(1020, 960, false);

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


        //Debug.Log("gotInput");
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
            usingWeapon = false;

        }
    }

    

    void createSword(){

        int direction = 0;
        BoxCollider2D col = GetComponentInChildren<BoxCollider2D>();
        if(sprt.sprite == sprites[36]){
            col.offset = new UnityEngine.Vector2(-0.1579781f, 3.20673e-05f);
            col.size = new UnityEngine.Vector2(0.6840439f,0.1939981f);
            direction = 1;

        } else if(sprt.sprite == sprites[37]){
            col.offset = new UnityEngine.Vector2(0.001522064f, -0.1694662f);
            col.size = new UnityEngine.Vector2(0.1936332f,0.690215f);
            direction = 2;

        } else if(sprt.sprite == sprites[38]){
            col.offset = new UnityEngine.Vector2(0.001522064f, 0.1593881f);
            col.size = new UnityEngine.Vector2(0.1937332f,0.6894231f);
            direction = 3;

        } else {
            col.offset = new UnityEngine.Vector2(0.1579781f,0.001184821f);
            col.size = new UnityEngine.Vector2(0.6905212f,0.1955926f);
            direction = 4;

        }

        

        
    }

    void DeleteSwordHitbox(){
        BoxCollider2D col = GetComponentInChildren<BoxCollider2D>();
        col.offset = new UnityEngine.Vector2(0,0);
        col.size = new UnityEngine.Vector2(0,0);
    }

    IEnumerator holdInPlace(float numTime){
        
        //gameObject.transform.

        yield return new WaitForSeconds(numTime);
    }
    
    IEnumerator SwordSwing(){

        int direction = 0;

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

        Debug.Log("here");
        createSword();
        //GameObject.Instantiate(swordHitbox);
        //sprt.sprite = this.mSword.SwapSprite(0);
        //mSword.useWeapon(3,3,0);
        StartCoroutine(holdInPlace(2f));
       // yield return new WaitForSeconds(2.0f);

        //rb = GetComponent<Rigidbody>();
        //Vector3 or
        //int curHp = GetComponent<Health>().health;

        if(GetComponent<Health>().health == 6) Instantiate(prefabs[0], new UnityEngine.Vector3(0,0,0), rb.rotation, this.transform);

        sprt.sprite = hold;

        DeleteSwordHitbox();
        

        yield return null;


    }

    void altWeapon(){
        sprt.sprite = this.curAlt.SwapSprite(0);
        // use weapon
        
    }
}
