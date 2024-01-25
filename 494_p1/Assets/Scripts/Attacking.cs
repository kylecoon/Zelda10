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

    private Dictionary<Vector2, Vector3> RotationDictionary = new Dictionary<Vector2, Vector3>();

    public GameObject[] weapons;

    private GameObject sword;
    private GameObject swordBeam;
    private GameObject arrow;
    private GameObject boomerang;
    private GameObject bomb;

    // private int curDirection = 0;
    // Start is called before the first frame update

    void Start()
<<<<<<< HEAD
    {   
        sword = null;
        swordBeam = null;
        arrow = null;
        boomerang = null;
=======
    {
       // sprt = GetComponent<SpriteRenderer>();
       
>>>>>>> 4484b1c (arrow Room done)

        rb = GetComponent<Rigidbody>();
        sprt = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");
<<<<<<< HEAD

        sprite_dictionary.Add(Vector2.up, sprites[26]);
        sprite_dictionary.Add(Vector2.down, sprites[24]);
        sprite_dictionary.Add(Vector2.left, sprites[25]);
        sprite_dictionary.Add(Vector2.right, sprites[27]);

        alt_sprite_dictionary.Add("BowAlt", sprites[163]);
        alt_sprite_dictionary.Add("BombAlt", sprites[146]);

        sprites = Resources.LoadAll<Sprite>("Zelda/Enemies");

        alt_sprite_dictionary.Add("BoomerangAlt", sprites[20]);

        sprites = Resources.LoadAll<Sprite>("Zelda/Black_pixel");
        alt_sprite_dictionary.Add("empty", sprites[0]);

        alt_dictionary.Add(0, "empty");
        alt_index = 1;

        RotationDictionary.Add(Vector2.up, new Vector3(0, 0, 0));
        RotationDictionary.Add(Vector2.left, new Vector3(0, 0, 90));
        RotationDictionary.Add(Vector2.down, new Vector3(0, 0, 180));
        RotationDictionary.Add(Vector2.right, new Vector3(0, 0, 270));
=======
>>>>>>> 4484b1c (arrow Room done)
    }
    // Update is called once per frame
    void Update()
    {

        usingWeapon = false;
        GetInput();

    }

    void GetInput() {


<<<<<<< HEAD
            //use alt
            else if(Input.GetKeyDown(KeyCode.Z)){
                switch (alt_dictionary[alt_index]) {
                    case "empty":
                        break;
                    case "BowAlt":
                        if (GetComponent<Inventory>().GetRupees() > 0 && arrow == null) {
                            GetComponent<Inventory>().AddRupees(-1);
                            StartCoroutine(BowAttack());
                        }
                        break;
                    case "BoomerangAlt":
                        if (boomerang == null) {
                            StartCoroutine(BoomerangAttack());
                        }
                        break;
                    case "BombAlt":
                        RaycastHit hit;
                        if (bomb == null && GetComponent<Inventory>().numBombs > 0 && !(Physics.Raycast (transform.position, GetComponent<Movement>().Get_CurrentDirection(), out hit, 1) && hit.transform.CompareTag("Wall"))) {
                            StartCoroutine(BombAttack());
                        }
                        break;
                }
            }
=======
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
>>>>>>> 4484b1c (arrow Room done)

        }
    }

<<<<<<< HEAD
    IEnumerator SwordAttack() {
        GetComponent<Movement>().Flip_CanMove();
        sword = Instantiate(weapons[0], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];
=======
    
>>>>>>> 4484b1c (arrow Room done)

    void createSword(){

<<<<<<< HEAD
        GetComponent<Movement>().Flip_CanMove();
        Destroy(sword);
        GetComponent<Movement>().UpdateSprite(GetComponent<Movement>().Get_CurrentDirection());

        //shoot beam if at full health and no other beams spawned
        if (GetComponent<Health>().health == GetComponent<Health>().MaxHP && swordBeam == null) {
            swordBeam = Instantiate(weapons[1], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
            swordBeam.GetComponent<Beam>().Shoot(GetComponent<Movement>().Get_CurrentDirection());
=======
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

>>>>>>> 4484b1c (arrow Room done)
        }

<<<<<<< HEAD
    public void AddAlt(string new_alt) {
        if (new_alt == "BombAlt" && alt_dictionary.Count > 1) {
            GetComponent<Inventory>().Addbombs();
            alt_dictionary.Add(alt_dictionary.Count, new_alt);
        }
        else {
            if (new_alt == "BombAlt") {
                GetComponent<Inventory>().Addbombs();
            }
            alt_dictionary.Add(alt_dictionary.Count, new_alt);
            alt_index = alt_dictionary.Count - 1;
            UpdateAltUI();
        }
    }

    void UpdateAltUI() {
        altRender.GetComponent<Image>().sprite = alt_sprite_dictionary[alt_dictionary[alt_index]];
    }

    IEnumerator BowAttack() {
        GetComponent<Movement>().Flip_CanMove();
        arrow = Instantiate(weapons[2], (Vector2)transform.position + GetComponent<Movement>().Get_CurrentDirection(), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
        arrow.GetComponent<Beam>().Shoot(GetComponent<Movement>().Get_CurrentDirection());
=======
        
>>>>>>> 4484b1c (arrow Room done)

        
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

        if(GetComponent<Health>().health == GetComponent<Health>().MaxHP) Instantiate(prefabs[0], new UnityEngine.Vector3(0,0,0), rb.rotation, this.transform);

        sprt.sprite = hold;

        DeleteSwordHitbox();
        

        yield return null;


    }

    void altWeapon(){
        sprt.sprite = this.curAlt.SwapSprite(0);
        // use weapon
        
    }

    IEnumerator BoomerangAttack() {
        GetComponent<Movement>().Flip_CanMove();
        boomerang = Instantiate(weapons[3], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 1.5f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
        boomerang.GetComponent<Boomerang>().ThrowBoomerang(GetComponent<Movement>().Get_CurrentDirection());

        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];

        yield return new WaitForSeconds(0.1f);

        GetComponent<Movement>().UpdateSprite(GetComponent<Movement>().Get_CurrentDirection());
        GetComponent<Movement>().Flip_CanMove();
    }

    IEnumerator BombAttack() {
        GetComponent<Inventory>().numBombs--;
        GetComponent<Movement>().Flip_CanMove();
        bomb = Instantiate(weapons[4], (Vector2)transform.position + GetComponent<Movement>().Get_CurrentDirection(), Quaternion.identity);

        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];

        yield return new WaitForSeconds(0.1f);

        GetComponent<Movement>().UpdateSprite(GetComponent<Movement>().Get_CurrentDirection());
        GetComponent<Movement>().Flip_CanMove();

        yield return new WaitForSeconds(1.9f);

        if (bomb != null) {
            yield return StartCoroutine(bomb.GetComponent<Bomb>().Explode());
        }
        else {
            yield return null;
        }
    }
}
