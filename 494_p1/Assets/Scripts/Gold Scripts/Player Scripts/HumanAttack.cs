using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class HumanAttack : MonoBehaviour
{
    SpriteRenderer sprt;
    private Sprite[] sprites;

    private Dictionary<Vector2, Sprite> sprite_dictionary = new Dictionary<Vector2, Sprite>();

    private Dictionary<Vector2, Vector3> RotationDictionary = new Dictionary<Vector2, Vector3>();

    public GameObject[] weapons;

    private GameObject sword;
    private GameObject swordBeam;

    // private int curDirection = 0;
    // Start is called before the first frame update

    void Start()
    {   
        sword = null;
        swordBeam = null;

        sprt = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");

        sprite_dictionary.Add(Vector2.up, sprites[26]);
        sprite_dictionary.Add(Vector2.down, sprites[24]);
        sprite_dictionary.Add(Vector2.left, sprites[25]);
        sprite_dictionary.Add(Vector2.right, sprites[27]);


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

        if (GetComponent<HumanMovement>().can_move) {
            //use sword
            if(GetComponent<HumanMovement>().can_move && Input.GetKeyDown(KeyCode.X)){
                StartCoroutine(SwordAttack());
            }
        }
    }

    IEnumerator SwordAttack() {
        GetComponent<HumanMovement>().can_move = false;
        sword = Instantiate(weapons[0], (Vector2)transform.position + (GetComponent<HumanMovement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<HumanMovement>().Get_CurrentDirection()]));
        sprt.sprite = sprite_dictionary[GetComponent<HumanMovement>().Get_CurrentDirection()];

        yield return new WaitForSeconds(0.3f);

        GetComponent<HumanMovement>().can_move = true;
        Destroy(sword);
        GetComponent<HumanMovement>().UpdateSprite(GetComponent<HumanMovement>().Get_CurrentDirection());

        //shoot beam if at full health and no other beams spawned
        if (GetComponent<Health>().health == GetComponent<Health>().MaxHP && swordBeam == null) {
            swordBeam = Instantiate(weapons[1], (Vector2)transform.position + (GetComponent<HumanMovement>().Get_CurrentDirection() * 0.8f), Quaternion.Euler(RotationDictionary[GetComponent<HumanMovement>().Get_CurrentDirection()]));
            swordBeam.GetComponent<Beam>().Shoot(GetComponent<HumanMovement>().Get_CurrentDirection());
        }
    }
}
