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
    public Sprite brokenProjectile;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        sprt = GetComponent<SpriteRenderer>();
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

    public void Shoot(Vector2 new_direction) {

        GetComponent<Rigidbody>().velocity = new_direction * projectile_speed;

    }

    IEnumerator DestoryProjectile() {

        gameObject.GetComponent<BoxCollider>().enabled = false;

        rb.velocity = Vector2.zero;

        sprt.sprite = brokenProjectile;

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}
