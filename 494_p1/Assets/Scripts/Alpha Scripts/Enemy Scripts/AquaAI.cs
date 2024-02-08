using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaAI : MonoBehaviour
{
    bool canShoot = true;
    private Vector2 start_position;
    private Vector2 end_position;
    public GameObject fireball;
    public Sprite[] sprites;
    private SpriteRenderer sprt;

    public Sprite spawn_sprite;
    bool spawned = false;
    void Start()
    {
        start_position = transform.position;
        end_position = start_position - new Vector2(2.0f, 0);
        sprt = GetComponent<SpriteRenderer>();
        spawned = false;
    }

    void Update()
    {
        if (!spawned) {
            spawned = true;
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Movement() {
        yield return new WaitForEndOfFrame();
        Debug.Log(GetComponent<EnemyHealth>().GetHealth());
        while (GetComponent<EnemyHealth>().GetHealth() > 0) {

            float duration = 7.0f;
            float elapsedTime = 0.0f;

            while (elapsedTime < duration) {
                if (GetComponent<EnemyHealth>().GetHealth() <= 0) {
                    yield break;
                }
                if (elapsedTime % 1.0f <= 0.5f) {
                    sprt.sprite = sprites[0];
                }
                else {
                    sprt.sprite = sprites[1];
                }
                transform.position = Vector3.Lerp(start_position, end_position, elapsedTime / duration);
                elapsedTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
                if (Random.Range(0, 20) == 10 && canShoot) {
                    StartCoroutine(ShootFireballs());
                }
            }

            elapsedTime = 0.0f;
            while (elapsedTime < duration) {
                if (GetComponent<EnemyHealth>().GetHealth() <= 0) {
                    yield break;
                }
                if (elapsedTime % 1.0f <= 0.5f) {
                    sprt.sprite = sprites[0];
                }
                else {
                    sprt.sprite = sprites[1];
                }
                transform.position = Vector3.Lerp(end_position, start_position, elapsedTime / duration);
                elapsedTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
                if (Random.Range(0, 30) == 10 && canShoot) {
                    StartCoroutine(ShootFireballs());
                }
            }
        }
    }

    IEnumerator ShootFireballs() {
        canShoot = false;

        GameObject fireball1 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, 0.5f), Quaternion.identity);
        GameObject fireball2 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, 0.0f), Quaternion.identity);
        GameObject fireball3 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, -0.5f), Quaternion.identity);

        fireball1.GetComponent<Rigidbody>().velocity = new Vector2(-1.0f, 0.5f) * 4;
        fireball2.GetComponent<Rigidbody>().velocity = new Vector2(-1.0f, 0.0f) * 4;
        fireball3.GetComponent<Rigidbody>().velocity = new Vector2(-1.0f, -0.5f) * 4;

        while (fireball1 != null || fireball2 != null || fireball3 != null) {
            yield return new WaitForSeconds(0.1f);
        }

        canShoot = true;

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Weapon" && !GetComponent<EnemyHealth>().invincible) {
            StartCoroutine(InvincibilityTimer());
        }
    }

    IEnumerator InvincibilityTimer() {
        yield return new WaitForEndOfFrame();
        GetComponent<EnemyHealth>().invincible = true;
        yield return new WaitForSeconds(0.4f);
        GetComponent<EnemyHealth>().invincible = false;
    }

    IEnumerator Spawn() {
        Debug.Log("Spawning");
        sprt.sprite = spawn_sprite;
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Movement());
    }
}
