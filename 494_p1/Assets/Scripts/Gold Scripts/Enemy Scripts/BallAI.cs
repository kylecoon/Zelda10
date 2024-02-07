using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BallAI : MonoBehaviour
{
    public Sprite[] spawn_sprites;
    public Sprite[] sprites;
    private SpriteRenderer sprt;
    private Rigidbody rb;
    private int health = 3;
    private Vector2 current_direction;
    public float movement_speed = 7.0f;
    private bool did_half_loop = true;
    private bool attacking = false;
    private bool returning = false;
    private GameObject player;

    public GameObject indicatorObject;
    private GameObject indicator;

    public GameObject egg;

    void OnEnable()
    {
        sprt = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Spawn());  
        player = GameObject.Find("Player");
    }

    IEnumerator Spawn() {
        yield return new WaitForEndOfFrame();
        sprt.color = new Color(0, 255, 0, 255);
        for (int i = 0; i < 9; ++i) {
            sprt.sprite = spawn_sprites[i % 2];
            yield return new WaitForSeconds(0.33f);
        }
        sprt.color = new Color(255, 255, 255, 255);
        sprt.sprite = sprites[0];
        // parent.GetComponent<DoorTrigger>().Close();
        StartCoroutine(Fight());
        indicator = Instantiate(indicatorObject, transform.position, Quaternion.identity);
        indicator.transform.parent = gameObject.transform;
        yield return null;
    }

    IEnumerator Fight() {
        int loop_counter = 0;
        int loop_amount = Random.Range(1, 5);
        while (health > 0) {
            //perform actions if in center of room
            if (Mathf.Abs((transform.position.x % 16.0f) - 7.5f) < 0.05f && did_half_loop) {
                StartCoroutine(LoopTimer());
                Debug.Log("In center");
                //pick direction to loop if on fresh loop cycle
                if (loop_counter == 0) {
                    //snap to grid
                    transform.position = new Vector3( Mathf.Floor(transform.position.x + 1) - 0.5f, transform.position.y, transform.position.z);
                    //randomly choose to go left or right
                    if (Random.Range(0, 2) == 0) {
                        current_direction = Vector2.left;
                    }
                    else {
                        current_direction = Vector2.right;
                    }
                    ++loop_counter;
                }
                //continue looping
                else if (loop_counter < loop_amount) {
                    ++loop_counter;
                }
                //stop looping and attack
                else if (loop_counter >= loop_amount) {
                    loop_counter = 0;
                    transform.position = new Vector3( Mathf.Floor(transform.position.x + 1) - 0.5f, transform.position.y, transform.position.z);
                    attacking = true;
                    indicator.GetComponent<SpriteRenderer>().enabled = false;
                    yield return StartCoroutine(Attack());
                }
            }

            //change direction if hit left or right wall
            else if ((transform.position.x % 16.0f <= 2.5f && current_direction == Vector2.left) || (transform.position.x % 16.0f >= 12.5f && current_direction == Vector2.right)) {
                //snap to grid
                transform.position = new Vector3(Mathf.Floor(transform.position.x + 1.0f) - 0.5f, transform.position.y, transform.position.z);
                //go down
                if (transform.position.y % 11.0f > 5.5f) {
                    current_direction = Vector2.down;
                }
                //go up
                else {
                    current_direction = Vector2.up;
                }
            }
            //change direction if hit top or bottom wall
            else if ((transform.position.y % 11.0f <= 2.5f && current_direction == Vector2.down) || (transform.position.y % 11.0f >= 7.5f && current_direction == Vector2.up)) {
                //snap to grid
                transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y + 1.0f) - 0.5f, transform.position.z);
                //go left
                if (transform.position.x % 16.0f > 7.5f) {
                    current_direction = Vector2.left;
                }
                //go right
                else {
                    current_direction = Vector2.right;
                }
            }
            rb.velocity = current_direction * movement_speed;
            UpdateSprite(current_direction);

            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    private void UpdateSprite(Vector2 direction) {
        if (direction == Vector2.up) {
            sprt.sprite = sprites[1];
            sprt.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.down) {
            sprt.sprite = sprites[0];
            sprt.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.left) {
            sprt.sprite = sprites[0];
            sprt.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.right) {
            sprt.sprite = sprites[1];
            sprt.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator LoopTimer() {
        did_half_loop = false;
        yield return new WaitForSeconds(1.5f);
        did_half_loop = true;
    }

    IEnumerator Attack() {
        //go up
        if (transform.position.y % 11.0f <= 5.5f) {
            current_direction = Vector2.up;
        }
        //go down
        else {
            current_direction = Vector2.down;
        }
        while (attacking) {
            if (health <= 0) {
                yield break;
            }
            rb.velocity = current_direction * 4.0f;
            yield return new WaitForSeconds(0.01f);
        }
        if (health <= 0) {
            yield break;
        }

        //return to wall
        while (transform.position.y % 11.0f <= 7.5f && transform.position.y % 11.0f >= 2.5f) {
            if (health <= 0) {
                yield break;
            }
            rb.velocity = current_direction * 3.0f;
            yield return new WaitForSeconds(0.01f);
        }

        //made it to wall continue loop
        returning = false;
        transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y) + 0.5f, transform.position.z);
        indicator.GetComponent<SpriteRenderer>().enabled = true;
        yield return null;
    }

    void OnCollisionEnter(Collision other)
    {
        if (returning) {
            return;
        }
        if (other.gameObject.CompareTag("Weapon")) {
            SwitchDirection();
            if (attacking) {
                attacking = false;
                returning = true;
                StartCoroutine(TakeDamage());
            }
        }
        else if (other.gameObject.CompareTag("Player")) {
            //switch direction on hit
            SwitchDirection();
            if (other.gameObject.GetComponent<DongAttack>().isActiveAndEnabled) {
                if (other.gameObject.GetComponent<DongAttack>().attacking) {
                    if (attacking) {
                        StartCoroutine(TakeDamage());
                        attacking = false;
                        returning = true;
                    }
                }
                else {
                    if (attacking) {
                        attacking = false;
                        returning = true;
                    }
                    other.gameObject.GetComponent<DongAttack>().launch_amount = 0;
                    StartCoroutine(other.gameObject.GetComponent<Health>().Hit(transform.position));
                    StartCoroutine(ResetPlayer());
                }
            }
            else {
                StartCoroutine(other.gameObject.GetComponent<Health>().Hit(transform.position));
                StartCoroutine(ResetPlayer());
                if (attacking) {
                    attacking = false;
                    returning = true;
                }
            }
        }
    }

    public GameObject door;

    IEnumerator TakeDamage() {
        --health;
        sprt.color = new Color(255, 0, 0, 255);
        if (player.GetComponent<DongAttack>() != null) {
            player.GetComponent<DongAttack>().launch_amount = 0;
        }
        yield return new WaitForSeconds(0.75f);
        Vector2 death_position = transform.position;
        if (health <= 0) {
            rb.velocity = Vector3.zero;
            Instantiate(egg, death_position, Quaternion.identity);
            door.GetComponent<DoorTrigger>().reOpen();
            Destroy(gameObject);
            yield break;
        }
        sprt.color = new Color(255, 255, 255, 255);
    }

    IEnumerator ResetPlayer() {
        if (player.GetComponent<HumanAttack>().isActiveAndEnabled) {
            yield break;
        }
        yield return new WaitForSeconds(0.75f);
        player.transform.position = new Vector3(player.transform.position.x - (player.transform.position.x % 16.0f) + 7.5f, player.transform.position.y, player.transform.position.z);
    }

    private void SwitchDirection() {
        if (current_direction == Vector2.left) {
            current_direction = Vector2.right;
        }
        else if (current_direction == Vector2.right) {
            current_direction = Vector2.left;
        }
        else if (current_direction == Vector2.up) {
            current_direction = Vector2.down;
        }
        else if (current_direction == Vector2.down) {
            current_direction = Vector2.up;
        }
        UpdateSprite(current_direction);
    }
}
