using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public GameObject rupeeDrop;
    public GameObject heartDrop;
    public GameObject bombDrop;
    public GameObject keyDrop;
    public bool invincible;
    public bool is_alive;
    public Sprite death_sprite;

    public bool eggDrop;

    public GameObject Egg;

    private AudioClip damageSound; //= Resources.Load("Zelda/Sound-Effects/DamageEnemy") as AudioClip;
    // Start is called before the first frame update
    void Start()
    {
        invincible = false;
        health = maxHealth;
        is_alive = true;
        damageSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/DamageEnemy");
    }

    // Update is called once per frame
    public void AlterHealth(int health_change) {
        if (invincible) {
            return;
        }

        AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
        health += health_change;
        
        StartCoroutine(DamageColor());
        if (health <= 0 && gameObject != null) {
            is_alive = false;
            StartCoroutine(Die(transform.position));
        }
    }

    IEnumerator Die(Vector2 drop_position) {
        if (gameObject == null) {
            yield return null;
        }
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        GetComponent<BoxCollider>().enabled = false;
        if (TryGetComponent<OmniMovement>(out OmniMovement mov)) {
            mov.can_move = false;
        }
        else {
            if (TryGetComponent<EnemyMovement>(out EnemyMovement mov_)) {
                mov_.can_move = false;
            }
        }
        GetComponent<SpriteRenderer>().sprite = death_sprite;
        GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        
        if (eggDrop) {
            //drop heart container?
            yield return new WaitForSeconds(0.8f);
            if(eggDrop){
                Instantiate(Egg, drop_position, Quaternion.identity);
            }
            if (gameObject != null) {
                Destroy(gameObject);
                yield break;
            }
        }

        else if (gameObject.name == "StalfosKey") {
            Instantiate(keyDrop, drop_position, Quaternion.identity);
            if (gameObject != null) {
                Destroy(gameObject);
            }
            yield break;
        }
        else {
            int drop_chance = Random.Range(0, 10);
            if (drop_chance < 5) {
                Instantiate(rupeeDrop, drop_position, Quaternion.identity);
            }
            else if (drop_chance > 7) {
                Instantiate(heartDrop, drop_position, Quaternion.identity);
            }
            else if (drop_chance == 6 && GameObject.Find("Player").GetComponent<FormController>() == null) {
                Instantiate(bombDrop, drop_position, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.8f);
            if (gameObject != null) {
                Destroy(gameObject);
            }
        }
    }

    public int GetHealth() {
        return health;
    }

    IEnumerator DamageColor() {
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.4f);
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }
}
