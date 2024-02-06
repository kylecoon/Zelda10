using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaAttack : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject fireball;
    private Dictionary<Vector2, Sprite> sprite_dictionary = new Dictionary<Vector2, Sprite>();
    bool canShoot = true;
    SpriteRenderer sprt;

    private Sprite[] sprites;
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Zelda/aqua-new");
        sprt = GetComponent<SpriteRenderer>();

        sprite_dictionary.Add(Vector2.left, sprites[0]);

        //match right direction to right facing sprites
        sprite_dictionary.Add(Vector2.right, sprites[7]);
        //match up direction to up facing sprites
        sprite_dictionary.Add(Vector2.up, sprites[1]);
        //match down direction to down facing sprites
        sprite_dictionary.Add(Vector2.down, sprites[6]);

    
    }

    // Update is called once per frame
    void Update()
    {
        if(canShoot){
            GetInput();
        }
    }

    void GetInput() {

        // if (GetComponent<AquaMove>().Check_CanMove()) {
        
            //use sword
        if(Input.GetKey(KeyCode.Z)){
            Debug.Log("Z");
            StartCoroutine(ShootFireballs());
        }
        // }
    }

    IEnumerator ShootFireballs() {

        if(sprt.sprite == sprites[0]){
            sprt.sprite = sprites[2];
        } else if(sprt.sprite == sprites[3]){
            sprt.sprite = sprites[1];
        } else if(sprt.sprite == sprites[4]){
            sprt.sprite = sprites[6];
        } else if(sprt.sprite == sprites[5]){
            sprt.sprite = sprites[7];
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        // rb.isKinematic = true;

        rb.velocity = Vector2.zero; 

        sprite_dictionary.Add(Vector2.zero, sprt.sprite);

        Debug.Log("fireballs");
        sprt.sprite = sprite_dictionary[GetComponent<Movement>().Get_CurrentDirection()];

        canShoot = false;

        float flip = 1;

        if(sprt.sprite == sprites[4] || sprt.sprite == sprites[5] ||  sprt.sprite == sprites[6] || sprt.sprite == sprites[7]){
            flip = -1;
        }
        

        GameObject fireball1 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f * flip, 0.5f), Quaternion.identity);
        GameObject fireball2 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f * flip, 0.0f), Quaternion.identity);
        GameObject fireball3 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f * flip, -0.5f), Quaternion.identity);


        fireball1.GetComponent<Rigidbody>().velocity = new Vector2(flip * -1.0f, 0.5f) * 4;
        fireball2.GetComponent<Rigidbody>().velocity = new Vector2(flip * -1.0f, 0.0f) * 4;
        fireball3.GetComponent<Rigidbody>().velocity = new Vector2(flip * -1.0f, -0.5f) * 4;

        while (fireball1 != null || fireball2 != null || fireball3 != null) {
            yield return new WaitForSeconds(0.1f);
            // rb.isKinematic = false;
            // yield return null;
        }

        sprite_dictionary.Remove(Vector2.zero);

        canShoot = true;
        // yield return null;

    }
}

