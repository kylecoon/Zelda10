using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DongAI : MonoBehaviour
{
    private bool can_move;
    private SpriteRenderer sprt;

    public Sprite[] sprites;
    public Sprite[] spawn_sprites;
    private Rigidbody rb;
    public float walk_speed = 1;

    public float ram_speed = 6;
    private int health = 3;

    public GameObject cam;

    private GameObject player;
    private Vector2 initial_position;
    public GameObject egg;
    // Start is called before the first frame update
    void OnEnable()
    {
        can_move = false;
        sprt = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        initial_position = transform.position;
        StartCoroutine(Spawn());   
    }

    IEnumerator Spawn() {
        yield return new WaitForEndOfFrame();
        can_move = false;
        sprt.color = new Color(0, 255, 0, 255);
        for (int i = 0; i < 9; ++i) {
            sprt.sprite = spawn_sprites[i % 2];
            yield return new WaitForSeconds(0.33f);
        }
        can_move = true;
        sprt.color = new Color(255, 255, 255, 255);
        sprt.sprite = sprites[0];
        StartCoroutine(Fight());
        yield return null;
    }

    IEnumerator Fight() {
        while (health > 0) {
            if (!can_move) {
                yield return new WaitForSeconds(0.01f);
                continue;
            }
            //if at top of room, attack
            if (transform.position.y % 11 >= 7.4f) {
                transform.position -= new Vector3(0, 0.1f, 0);
                can_move = false;
                rb.velocity = Vector2.zero;
                StartCoroutine(Attack());
            }
            else if (transform.position.y % 11 <= 2.1f) {
                transform.position += new Vector3(0, 0.1f, 0);
                can_move = false;
                rb.velocity = Vector2.zero;
                StartCoroutine(Attack());
            }
            //if in line with player, attack
            else if (Mathf.Abs(transform.position.y - player.transform.position.y) < 0.1f) {
                can_move = false;
                rb.velocity = Vector2.zero;
                StartCoroutine(Attack());
            }
            //if lower than player, move up
            else if (transform.position.y < player.transform.position.y) {
                if (transform.position.y % 1.0f < 0.5f) {
                    sprt.sprite = sprites[0];
                }
                else {
                    sprt.sprite = sprites[1];
                }
                rb.velocity = Vector2.up * walk_speed;
            }
            //if higher than player, move down
            else {
                if (transform.position.y % 1.0f < 0.5f) {
                    sprt.sprite = sprites[0];
                }
                else {
                    sprt.sprite = sprites[1];
                }
                rb.velocity = Vector2.down * walk_speed;
            }

            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    IEnumerator Attack() {
        for (int i = 0; i < 20; ++i) {
            transform.localScale -= new Vector3(0.01f, 0);
            yield return new WaitForSeconds(0.05f);
        }
        transform.localScale = Vector3.one;
        rb.velocity = Vector2.left * ram_speed;
        while (rb.velocity.x < 0) {
            if (transform.position.x % 1.0f < 0.5f) {
                sprt.sprite = sprites[0];
            }
            else {
                sprt.sprite = sprites[1];
            }
            if (transform.position.x % 16.0f <= 2.8) {
                StartCoroutine(ShakeCamera());
                StartCoroutine(WalkBackwards());
            }
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(WalkBackwards());
        }
        else if (other.gameObject.CompareTag("Wall")) {
            if (other.gameObject.GetComponent<BreakableWall>() != null) {
                StartCoroutine(other.gameObject.GetComponent<BreakableWall>().Break());
            }
            StartCoroutine(ShakeCamera());
            StartCoroutine(WalkBackwards());
        }
        else if (other.gameObject.CompareTag("Wallspike")) {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y, transform.position.z);
            StartCoroutine(ShakeCamera());
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator WalkBackwards() {
        rb.velocity = Vector2.right * walk_speed * 3;

        while (Mathf.Abs(transform.position.x - initial_position.x) > 0.25f) {
            rb.velocity = Vector2.right * walk_speed * 3;
            if (transform.position.x % 1.0f < 0.5f) {
                sprt.sprite = sprites[0];
            }
            else {
                sprt.sprite = sprites[1];
            }
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = new Vector3(initial_position.x, transform.position.y, transform.position.z);
        can_move = true;
        yield return null;
    }

    IEnumerator ShakeCamera() {
        Vector3 original_position = cam.transform.position;
        for (int i = 0; i < 8; ++i) {
            cam.transform.position = original_position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            yield return new WaitForSeconds(0.03f);
        }
        cam.transform.position = original_position;
        yield return null;
    }

    IEnumerator TakeDamage() {
        sprt.color = new Color(255, 0, 0, 255);
        sprt.sprite = sprites[2];
        health--;
        yield return new WaitForSeconds(0.75f);
        sprt.color = new Color(255, 255, 255, 255);
        if (health > 0) {
            StartCoroutine(WalkBackwards());
        }
        else {
            Instantiate(egg, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        yield return null;
    }
}
