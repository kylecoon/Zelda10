using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class Attacking : MonoBehaviour
{
    SpriteRenderer sprt;
    private Rigidbody rb;
    private Sprite[] sprites;

    private Dictionary<Vector2, Sprite> sprite_dictionary = new Dictionary<Vector2, Sprite>();

    private Dictionary<int, string> alt_dictionary = new Dictionary<int, string>();
    private int alt_index;
    private Dictionary<string, Sprite> alt_sprite_dictionary = new Dictionary<string, Sprite>();
    public GameObject altRender;

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
    {   
        sword = null;
        swordBeam = null;
        arrow = null;
        boomerang = null;

        rb = GetComponent<Rigidbody>();
        sprt = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");

        sprite_dictionary.Add(Vector2.up, sprites[26]);
        sprite_dictionary.Add(Vector2.down, sprites[24]);
        sprite_dictionary.Add(Vector2.left, sprites[25]);
        sprite_dictionary.Add(Vector2.right, sprites[27]);

        alt_sprite_dictionary.Add("BowAlt", sprites[163]);
        alt_sprite_dictionary.Add("BombAlt(Clone)", sprites[146]);

        sprites = Resources.LoadAll<Sprite>("Zelda/Enemies");

        alt_sprite_dictionary.Add("BoomerangAlt(Clone)", sprites[20]);

        sprites = Resources.LoadAll<Sprite>("Zelda/Black_pixel");
        alt_sprite_dictionary.Add("empty", sprites[0]);

        alt_dictionary.Add(0, "empty");
        alt_index = 1;

        RotationDictionary.Add(Vector2.up, new Vector3(0, 0, 0));
        RotationDictionary.Add(Vector2.left, new Vector3(0, 0, 90));
        RotationDictionary.Add(Vector2.down, new Vector3(0, 0, 180));
        RotationDictionary.Add(Vector2.right, new Vector3(0, 0, 270));
    }
    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput() {

        if (GetComponent<Movement>().Check_CanMove()) {
        
            //use sword
            if(Input.GetKeyDown(KeyCode.X)){
                StartCoroutine(SwordAttack());
            }

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
                    case "BoomerangAlt(Clone)":
                        if (boomerang == null) {
                            StartCoroutine(BoomerangAttack());
                        }
                        break;
                    case "BombAlt(Clone)":
                        RaycastHit hit;
                        if (bomb == null && GetComponent<Inventory>().numBombs > 0 && !(Physics.Raycast (transform.position, GetComponent<Movement>().Get_CurrentDirection(), out hit, 1) && hit.transform.CompareTag("Wall"))) {
                            StartCoroutine(BombAttack());
                        }
                        break;
                }
            }

            //swap alt
            else if(Input.GetKeyDown(KeyCode.Space)){
                alt_index += 1;
                if (alt_index >= alt_dictionary.Count) {
                    alt_index = 0;
                }
                UpdateAltUI();
            } 
        }
    }

    IEnumerator SwordAttack() {
        GetComponent<Movement>().Flip_CanMove();
        sword = Instantiate(weapons[0], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];

        yield return new WaitForSeconds(0.3f);

        GetComponent<Movement>().Flip_CanMove();
        Destroy(sword);
        GetComponent<Movement>().UpdateSprite(GetComponent<Movement>().Get_CurrentDirection());

        //shoot beam if at full health and no other beams spawned
        if (GetComponent<Health>().health == GetComponent<Health>().MaxHP && swordBeam == null) {
            swordBeam = Instantiate(weapons[1], (Vector2)transform.position + (GetComponent<Movement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<Movement>().Get_CurrentDirection()]));
            swordBeam.GetComponent<Beam>().Shoot(GetComponent<Movement>().Get_CurrentDirection());
        }
    }

    public void AddAlt(string new_alt) {
        if (new_alt == "BombAlt(Clone)" && alt_dictionary.Count > 1) {
            GetComponent<Inventory>().Addbombs();
            alt_dictionary.Add(alt_dictionary.Count, new_alt);
        }
        else {
            if (new_alt == "BombAlt(Clone)") {
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

        yield return new WaitForSeconds(0.1f);

        GetComponent<Movement>().Flip_CanMove();
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
        GetComponent<Inventory>().UpdateBombCount();
        GetComponent<Movement>().Flip_CanMove();
        bomb = Instantiate(weapons[4], (Vector2)transform.position + GetComponent<Movement>().Get_CurrentDirection(), Quaternion.identity);

        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];

        yield return new WaitForSeconds(0.1f);

        GetComponent<Movement>().UpdateSprite(GetComponent<Movement>().Get_CurrentDirection());
        GetComponent<Movement>().Flip_CanMove();

        yield return new WaitForSeconds(1.4f);

        if (bomb != null) {
            yield return StartCoroutine(bomb.GetComponent<Bomb>().Explode());
        }
        else {
            yield return null;
        }
    }
}
