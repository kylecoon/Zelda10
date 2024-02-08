using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private List<Vector2> directions = new List<Vector2>();
    private Vector2 previous_direction;
    private Rigidbody rb;

    public bool can_move;
    public bool moving;

    RaycastHit hit;

    void Start() {
        can_move = true;
        rb = GetComponent<Rigidbody>();
        moving = false;
        previous_direction = Vector2.zero;
    }

    public IEnumerator MoveEnemy(Vector2 direction, float speed) {
        if (!can_move || direction == Vector2.zero || speed == 0) {
            yield return null;
        }

        moving = true;
        SnapToGrid(0.4f);

        Vector2 endPosition = (Vector2)transform.position + direction;
        rb.velocity = direction * speed;

        // wait until enemy reaches its destination
        while ((direction.x != 0 && Mathf.Abs(transform.position.x - endPosition.x) > 0.1f) ||
               (direction.y != 0 && Mathf.Abs(transform.position.y - endPosition.y) > 0.1f)) {
                if (GetComponent<EnemyHealth>().GetHealth() <= 0) {
                    break;
                }
            rb.velocity = direction * speed;
            yield return null;
            if (!can_move || direction == Vector2.zero || speed == 0) {
                yield break;
            }
        }

        rb.velocity = Vector2.zero;
        moving = false;
        SnapToGrid(0.4f);

        yield return null;
    }
    
    void SnapToGrid(float range) {
        //lock onto x axis
        if (transform.position.x % 1.0f < range) {
            transform.position = new Vector2(System.MathF.Floor(transform.position.x), transform.position.y);
        }
        else if (transform.position.x % 1.0f > 1.0f - range) {
            transform.position = new Vector2(System.MathF.Ceiling(transform.position.x), transform.position.y);
        }


        //lock onto y axis
        if (transform.position.y % 1.0f < range) {
            transform.position = new Vector2(transform.position.x, System.MathF.Floor(transform.position.y));
        }
        else if (transform.position.y % 1.0f > 1.0f - range) {
            transform.position = new Vector2(transform.position.x, System.MathF.Ceiling(transform.position.y));
        }
    }
    public Vector2 PickDirection() {
        directions.Clear();

        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.up), out hit, 1.0f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            if (transform.position.y % 11.0f < 7.5f) {
                directions.Add(Vector2.up);
            }
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.down), out hit, 1.0f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            if (transform.position.y % 11.0f > 2.5f) {
                directions.Add(Vector2.down);
            }
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.left), out hit, 1.0f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            if (transform.position.x % 16.0f > 2.5f) {
                directions.Add(Vector2.left);
            }
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.right), out hit, 1.0f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            if (transform.position.x % 16.0f < 12.5f) {
                directions.Add(Vector2.right);
            }
        }
        //if can't go in any direction, return 0
        if (directions.Count == 0) {
            return Vector2.zero;
        }
        //make it more likely to keep moving in same direction
        if (previous_direction != Vector2.zero && directions.Contains(previous_direction)) {
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
        }
        previous_direction = directions[UnityEngine.Random.Range(0, directions.Count)];
        return previous_direction;
    }

    void OnCollisionEnter (Collision other) {
        if (other.gameObject.CompareTag("Weapon")) {
            KnockbackCalculation(other.gameObject);
        }
    }

    public void KnockbackCalculation(GameObject other) {
        if (GetComponent<EnemyHealth>().invincible || !can_move || GetComponent<EnemyHealth>().GetHealth() <= 0) {
            return;
        }
        //calculate knockback direction
            rb.velocity = Vector2.zero;

            Vector2 direction;

            //pushback vertically
            if (Mathf.Abs(other.transform.position.x - transform.position.x) < Mathf.Abs(other.transform.position.y - transform.position.y)) {

                //push down
                if (other.transform.position.y > transform.position.y) {
                    direction = Vector2.down;
                }
                //push up
                else {
                    direction = Vector2.up;
                }
            }
            //pushback horizontally
            else {
                //push left
                if (other.transform.position.x > transform.position.x) {
                    direction = Vector2.left;
                }
                //push right
                else {
                    direction = Vector2.right;
                }
            }
            StartCoroutine(PerformKnockback(direction));
    }
    IEnumerator PerformKnockback(Vector2 direction) {
        can_move = false;
        yield return new WaitForEndOfFrame();
        Debug.Log("Knocking back");
        moving = true;
        if (GetComponent<EnemyHealth>().GetHealth() <= 0) {
            yield break;
        }
        GetComponent<EnemyHealth>().invincible = true;
        rb.velocity = direction * 8;
        SnapToGrid(0.5f);

        yield return new WaitForSeconds(0.1f);
        GetComponent<EnemyHealth>().invincible = false;

        yield return new WaitForSeconds(0.2f);

        moving = false;
        rb.velocity = Vector2.zero;
        SnapToGrid(0.5f);

        yield return new WaitForSeconds(0.2f);
        can_move = true;
    }
}
