using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoriyaAI : MonoBehaviour

{
    private EnemyMovement mov;
    private bool isDead;
    public GameObject rang;

    public bool has_boomerang;
    private Vector2 direction;
    public Sprite[] sprites;
    private SpriteRenderer sprt;
    private EnemyHealth healthComp;
    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<EnemyMovement>();
        isDead = false;
        StartCoroutine(Move());
        has_boomerang = true;
        sprt = GetComponent<SpriteRenderer>();
        healthComp = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {   
        //animation
        if (healthComp.is_alive && mov.can_move) {
            if (direction == Vector2.right) {
                sprt.flipX = false;
                if (transform.position.x % 1.0f < 0.5f) {
                    sprt.sprite = sprites[0];
                }
                else {
                    sprt.sprite = sprites[1];
                }
            }
            else if (direction == Vector2.left) {
                sprt.flipX = true;
                if (transform.position.x % 1.0f < 0.5f) {
                    sprt.sprite = sprites[0];
                }
                else {
                    sprt.sprite = sprites[1];
                }
            }
            else if (direction == Vector2.up) {
                sprt.sprite = sprites[2];
                if (transform.position.y % 1.0f < 0.5f) {
                    sprt.flipX = true;
                }
                else {
                    sprt.flipX = false;
                }
            }
            else if (direction == Vector2.down) {
                sprt.sprite = sprites[3];
                if (transform.position.y % 1.0f < 0.5f) {
                    sprt.flipX = true;
                }
                else {
                    sprt.flipX = false;
                }
            }
        }
    }

    IEnumerator Move() {
        while (!isDead) {
            if (!GetComponent<EnemyMovement>().can_move) {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            direction = mov.PickDirection();
            yield return StartCoroutine(mov.MoveEnemy(direction, 3.0f));
            int odds = Random.Range(0, 8);
            if (odds == 7) {
                GetComponent<EnemyMovement>().can_move = false;
                has_boomerang = false;
                GameObject boomer = Instantiate(rang, (Vector2)transform.position + direction, Quaternion.identity);
                boomer.GetComponent<Boomerang>().ThrowBoomerang(direction);
                while (boomer != null) {
                    if (isDead && boomer != null) {
                        Destroy(boomer);
                        yield break;
                    }
                    yield return null;
                }
                has_boomerang = true;
                GetComponent<EnemyMovement>().can_move = true;
            }
        }
        yield return null;
    }

    void OnDestroy() {
        isDead = true;
    }
}
