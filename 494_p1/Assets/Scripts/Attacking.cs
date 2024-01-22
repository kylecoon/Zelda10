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

    // private int curDirection = 0;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sprt = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");

        sprite_dictionary.Add(Vector2.up, sprites[38]);
        sprite_dictionary.Add(Vector2.down, sprites[36]);
        sprite_dictionary.Add(Vector2.left, sprites[37]);
        sprite_dictionary.Add(Vector2.right, sprites[39]);

        alt_sprite_dictionary.Add("bow", sprites[163]);

        sprites = Resources.LoadAll<Sprite>("Zelda/Black_pixel");
        alt_sprite_dictionary.Add("empty", sprites[0]);

        alt_dictionary.Add(0, "empty");
        alt_index = 1;
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
                    case "bow":
                        if (GetComponent<Inventory>().GetRupees() > 0 && !transform.GetChild(2).gameObject.activeSelf) {
                            GetComponent<Inventory>().AddRupees(-1);
                            StartCoroutine(BowAttack());
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
        transform.GetChild(0).GetComponent<Sword>().hitbox.enabled = true;
        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];

        yield return new WaitForSeconds(0.3f);

        GetComponent<Movement>().Flip_CanMove();
        transform.GetChild(0).GetComponent<Sword>().hitbox.enabled = false;
        GetComponent<Movement>().UpdateSprite(GetComponent<Movement>().Get_CurrentDirection());

        //shoot beam if at full health and no other beams spawned
        if (GetComponent<Health>().health == GetComponent<Health>().MaxHP && !transform.GetChild(1).gameObject.activeSelf) {
            transform.GetChild(1).GetComponent<Beam>().Shoot(GetComponent<Movement>().Get_CurrentDirection());
        }
    }

    public void AddAlt(string new_alt) {
        alt_dictionary.Add(alt_dictionary.Count, new_alt);
        alt_index = alt_dictionary.Count - 1;
        UpdateAltUI();
    }

    void UpdateAltUI() {
        altRender.GetComponent<Image>().sprite = alt_sprite_dictionary[alt_dictionary[alt_index]];
    }

    IEnumerator BowAttack() {
        
        GetComponent<Movement>().Flip_CanMove();
        transform.GetChild(2).GetComponent<Beam>().Shoot(GetComponent<Movement>().Get_CurrentDirection());

        yield return new WaitForSeconds(0.1f);

        GetComponent<Movement>().Flip_CanMove();
    }
}
