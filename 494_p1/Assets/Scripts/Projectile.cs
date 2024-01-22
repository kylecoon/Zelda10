using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int projectile_speed = 4;
    public SpriteRenderer sprt;
    private Dictionary<Vector2, Sprite> sprite_dictionary = new Dictionary<Vector2, Sprite>();
    public Sprite[] sprites;
    private Vector3 original_color;

    private BoxCollider hitbox;

    public bool isActive;
    private Vector2 direction;
    private Rigidbody rb;
    public Sprite brokenProjectile;
    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        isActive = false;
        rb = GetComponent<Rigidbody>();
        direction = Vector2.zero;
        gameObject.SetActive(false);
        hitbox = GetComponent<BoxCollider>();

        sprite_dictionary.Add(Vector2.up, sprites[0]);
        sprite_dictionary.Add(Vector2.down, sprites[1]);
        sprite_dictionary.Add(Vector2.left, sprites[2]);
        sprite_dictionary.Add(Vector2.right, sprites[3]);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            rb.velocity = direction * projectile_speed;
        }   
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall" || rb.velocity == Vector3.zero) {
            Debug.Log("collided");
            StartCoroutine(DestoryProjectile());
        }
    }

    public void Shoot(Vector2 new_direction) {

        hitbox.enabled = true;
        isActive = true;

        if (new_direction == Vector2.left || new_direction == Vector2.right) {
            hitbox.size = new Vector2(hitbox.size.y, hitbox.size.x);

        }

        transform.localPosition = new_direction;
        sprt.sprite = sprite_dictionary[new_direction];
        gameObject.SetActive(true);

        direction = new_direction;
    }

    IEnumerator DestoryProjectile() {
        isActive = false;

        hitbox.enabled = false;

        rb.velocity = Vector2.zero;

        sprt.color = new Color(255, 255, 255);
        sprt.sprite = brokenProjectile;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
