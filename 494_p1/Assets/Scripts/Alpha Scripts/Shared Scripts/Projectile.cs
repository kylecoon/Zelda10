using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int projectile_speed;
    public int projectile_damage;
    public SpriteRenderer sprt;
    private Rigidbody rb;
    public GameObject contact;

    private AudioClip projectileBreakSound;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        sprt = GetComponent<SpriteRenderer>();
        projectileBreakSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/SoundEffect3");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall") || rb.velocity == Vector3.zero) {
            StartCoroutine(DestoryProjectile());
            if (collision.gameObject.CompareTag("Enemy")) {
                Debug.Log("Beam hit");
                collision.gameObject.GetComponent<EnemyHealth>().AlterHealth(projectile_damage);
            }
        }
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall") || rb.velocity == Vector3.zero) {
            StartCoroutine(DestoryProjectile());
            if (collision.gameObject.CompareTag("Enemy")) {
                Debug.Log("Beam hit");
                collision.gameObject.GetComponent<EnemyHealth>().AlterHealth(projectile_damage);
            }
        }
    }

    public void Shoot(Vector2 new_direction) {

        GetComponent<Rigidbody>().velocity = new_direction * projectile_speed;

    }

    IEnumerator DestoryProjectile() {
        AudioSource.PlayClipAtPoint(projectileBreakSound, Camera.main.transform.position);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        rb.velocity = Vector2.zero;

        GameObject topLeft = Instantiate(contact, transform.position, Quaternion.identity);
        topLeft.GetComponent<Rigidbody>().velocity = (Vector2.up + Vector2.left) * 3.0f;

        GameObject topRight = Instantiate(contact, transform.position, Quaternion.Euler(0, 0, -90));
        topRight.GetComponent<Rigidbody>().velocity = (Vector2.up + Vector2.right) * 3.0f;

        GameObject bottomLeft = Instantiate(contact, transform.position, Quaternion.Euler(0, 0, 90));
        bottomLeft.GetComponent<Rigidbody>().velocity = (Vector2.down + Vector2.left) * 3.0f;

        GameObject bottomRight = Instantiate(contact, transform.position, Quaternion.Euler(0, 0, 180));
        bottomRight.GetComponent<Rigidbody>().velocity = (Vector2.down + Vector2.right) * 3.0f;

        yield return new WaitForSeconds(0.3f);

        Destroy(topLeft);
        Destroy(topRight);
        Destroy(bottomLeft);
        Destroy(bottomRight);
        Destroy(this.gameObject);
    }
}
