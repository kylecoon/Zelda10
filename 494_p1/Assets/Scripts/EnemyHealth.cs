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
    public GameObject bombDrop;
    public bool invincible;
    // Start is called before the first frame update
    void Start()
    {
        invincible = false;
        health = maxHealth;
    }

    // Update is called once per frame
    public void AlterHealth(int health_change) {
        if (invincible) {
            return;
        }
        health += health_change;
        if (health <= 0 && gameObject != null) {
            StartCoroutine(Die(transform.position));
        }
    }

    IEnumerator Die(Vector2 drop_position) {
        if (gameObject == null) {
            yield return null;
        }
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().sprite = death_sprite;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<EnemyMovement>().can_move = false;

        int drop_chance = Random.Range(0, 10);
        if (drop_chance < 5) {
            Instantiate(rupeeDrop, drop_position, Quaternion.identity);
        }
        else if (drop_chance > 7) {
            Instantiate(heartDrop, drop_position, Quaternion.identity);
        }
        else if (drop_chance == 6) {
            Instantiate(bombDrop, drop_position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.8f);
        if (gameObject != null) {
            Destroy(gameObject);
        }
    }

    public int GetHealth() {
        return health;
    }
}
