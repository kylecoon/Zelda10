using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class Boomerang : MonoBehaviour
{
    public float speed;
    public float range;
    public int damage;
    public string ThrowerTag;

    private bool returning = false;

    private Vector2 initial_position;

    private GameObject thrower;

    private Rigidbody rb;

    public Sprite[] sprites;
    private SpriteRenderer sprt;

    void Awake()
    {   
        sprt = GetComponent<SpriteRenderer>();
        returning = false;
        thrower = GameObject.FindWithTag(ThrowerTag);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (thrower == null) {
            Destroy(gameObject);
        }
        //animation
        if (Time.time % 0.33f < 0.11f) {
            sprt.sprite = sprites[0];
        }
        else if (Time.time % 0.33f < 0.22f) {
            sprt.sprite = sprites[1];
        }
        else {
            sprt.sprite = sprites[2];
        }

        if (returning && thrower != null && gameObject != null) {
            rb.velocity = (thrower.transform.position - transform.position).normalized * speed;
        }
        else {
            if ( Mathf.Abs(transform.position.x - initial_position.x) > range || Mathf.Abs(transform.position.y - initial_position.y) > range) {
                returning = true;
            }
        }
    }

    public void ThrowBoomerang(Vector2 direction) {
        initial_position = transform.position;
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (ThrowerTag == "Player") {
            if (other.gameObject.CompareTag("Enemy")) {
                other.gameObject.GetComponent<EnemyHealth>().AlterHealth(damage);
                returning = true;
            }
            else if (other.gameObject.CompareTag("Player")) {
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Wall")) {
                returning = true;
            }
        }
        else if (ThrowerTag == "Enemy") {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall")) {
                returning = true;
            }
            if (other.gameObject == thrower) {
                Destroy(gameObject);
            }
        }
    }
}
