using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaAttack : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject fireball;
    bool canShoot = true;
    SpriteRenderer sprt;
    private Rigidbody rb;
    public Sprite attack_sprite;
    void Awake()
    {
        sprt = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
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
        if(Input.GetKey(KeyCode.X)){
            StartCoroutine(ShootFireballs());
        }
        // }
    }

    IEnumerator ShootFireballs() {

        // rb.isKinematic = true;
        sprt.sprite = attack_sprite;
        rb.velocity = Vector2.zero; 

        Debug.Log("fireballs");

        canShoot = false;

        float flip = -1;

        if(sprt.flipX == false){
            flip = 1;
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

        canShoot = true;
        // yield return null;

    }
}

