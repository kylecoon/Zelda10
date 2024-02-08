using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DongAttack : MonoBehaviour
{
    public bool attacking;
    public Sprite[] sprites; //0 = up, 1 = down, 2 = horizontal

    public int launch_amount;
    private Rigidbody rb;
    public int attack_speed = 8;

    private bool can_attack;

    private AudioClip battleCry;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attacking = false;
        launch_amount = 0;
        can_attack = true;
        battleCry = Resources.Load<AudioClip>("Zelda/Sound-Effects/SoundEffect17");
    }
    void Update()
    {
        if (!can_attack) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            can_attack = false;
            GetComponent<FormController>().can_move = false;
            rb.velocity = Vector2.zero;
            SnapToGrid();
            transform.position = (Vector2)transform.position + GetComponent<DongMovement>().GetCurrentDirection() * (-0.2f); //prevent weird clipping glitch
            StartCoroutine(ChargeUp());
        }
    }

    private void SnapToGrid() {
        if (transform.position.x % 0.5f < 0.25f) {
            transform.position = new Vector2(transform.position.x - (transform.position.x % 0.5f), transform.position.y);
        }
        else {
            transform.position = new Vector2(transform.position.x + (0.5f - (transform.position.x % 0.5f)), transform.position.y);
        }

        if (transform.position.y % 0.5f < 0.25f) {
            transform.position = new Vector2(transform.position.x, transform.position.y - (transform.position.y % 0.5f));
        }
        else {
            transform.position = new Vector2(transform.position.x, transform.position.y + (0.5f - (transform.position.y % 0.5f)));
        }
    }

    IEnumerator ChargeUp() {
        Vector3 original_scale = transform.localScale;
        Vector3 shrink_amount;
        AudioSource.PlayClipAtPoint(battleCry, Camera.main.transform.position);
        if (GetComponent<DongMovement>().GetCurrentDirection() == Vector2.up || GetComponent<DongMovement>().GetCurrentDirection() == Vector2.down) {
            shrink_amount = new Vector3(0, 0.01f, 0);
        }
        else {
            shrink_amount = new Vector3(0.01f, 0, 0);
        }

        //forced charge
        for (launch_amount = 0; launch_amount < 10; ++launch_amount) {
            transform.localScale -= shrink_amount * 1.5f;
            yield return new WaitForSeconds(0.05f);
        }

        //optional charge
        while (Input.GetKey(KeyCode.X)) {
            if (launch_amount < 20) {
                ++launch_amount;
                transform.localScale -= shrink_amount;
            }
            yield return new WaitForSeconds(0.05f);
        }
        transform.localScale = original_scale;
        yield return StartCoroutine(Attack());
    }

    IEnumerator Attack() {
        attacking = true;

        GetComponent<Health>().Invincible = true;
        for (int i = 0; i <= launch_amount; ++i) {
            rb.velocity = attack_speed * GetComponent<DongMovement>().GetCurrentDirection();
            StartCoroutine(GetComponent<DongMovement>().UpdateSprite(GetComponent<DongMovement>().GetCurrentDirection()));
            yield return new WaitForSeconds(0.02f);
        }
        attacking = false;
        GetComponent<Health>().Invincible = false;
        GetComponent<FormController>().can_move = true;
        launch_amount = 0;
        can_attack = true;
        yield return null;
    }

    void OnCollisionEnter(Collision other)
    {
        if (attacking) {
            if (other.gameObject.CompareTag("Enemy")) {
                other.gameObject.GetComponent<EnemyHealth>().AlterHealth(-5);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("->North") || other.CompareTag("->South") || other.CompareTag("->East") || other.CompareTag("->West")) {
            can_attack = false;
        } 
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("->North") || other.CompareTag("->South") || other.CompareTag("->East") || other.CompareTag("->West")) {
            can_attack = true;
        } 
    }
}
