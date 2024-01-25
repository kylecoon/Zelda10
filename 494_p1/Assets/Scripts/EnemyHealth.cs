using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public Sprite death_sprite;
    public GameObject rupeeDrop;
    public GameObject heartDrop;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    public void AlterHealth(int health_change) {
        health += health_change;
        if (health <= 0) {
            StartCoroutine(Die());
        }
        else {
            //GetComponent<EnemyMovement>().Knockback();
        }
    }

    /*    void Knockback() {
        Vector2 player_position = GameObject.FindWithTag("Player").transform.position;
        stunned = true;
        //push vertically
        if (Mathf.Abs(transform.position.x - player_position.x) < Mathf.Abs(transform.position.y - player_position.y)) {
            //push up
            if (player_position.y < transform.position.y) {
                rb.velocity = Vector3.up * knockback_force;
            }
            //push down
            else {
                rb.velocity = Vector3.down * knockback_force;
            }
        }
        //push horizontally
        else {
            //push right
            if (player_position.x < transform.position.x) {
                rb.velocity = Vector3.right * knockback_force;
            }
            //push left
            else {
                rb.velocity = Vector3.left * knockback_force;
            }
        }
        StartCoroutine(Stall());
    }

    IEnumerator Stall() {
        Debug.Log("Starting stall");

        yield return new WaitForSeconds(1.0f);

        SnapToGrid();
        start_position = transform.position;
        interpolator = 0.0f;
        stunned = false;
        rb.velocity = Vector2.zero;
        Debug.Log("Ending stall");
    }*/

    IEnumerator Die() {
        GetComponent<SpriteRenderer>().sprite = death_sprite;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<StalfosAI>().isAlive = false;

        int drop_chance = Random.Range(0, 10);
        if (drop_chance < 5) {
            Instantiate(rupeeDrop, transform.position, Quaternion.identity);
        }
        else if (drop_chance > 7) {
            Instantiate(heartDrop, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);

        Destroy(gameObject);
    }
}
