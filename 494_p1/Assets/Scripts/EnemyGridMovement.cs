using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    private List<Vector2> directions = new List<Vector2>();
    private float interpolator;
    private Vector2 start_position;
    private Vector2 end_position;
    public bool can_move;

    private Rigidbody rb;

    void Start() {
        interpolator = 0.0f;
        can_move = true;
        rb = GetComponent<Rigidbody>();
    }

    public void MoveEnemy() {
        //pick new direction to go if in center of tile
        if (!can_move) {
            return;
        }
        if (interpolator == 0.0f) {
            
            SnapToGrid();

            start_position = transform.position;

            CheckDirections();

            if (directions.Count != 0) {
                end_position = start_position + directions[UnityEngine.Random.Range(0, directions.Count)];
            }
            else {
                end_position = start_position;
            }
        }

        //move
        transform.position = Vector2.Lerp(start_position, end_position, interpolator);

        //increment interpolator. Increment amount determines enemy speed
        interpolator += 0.03f;

        //reset if in center of tile
        if (interpolator >= 1.0f) {
            interpolator = 0.0f;
        }
    }
    
    void SnapToGrid() {
        //lock onto x axis
        if (transform.position.x % 1.0f < 0.1f) {
            transform.position = new Vector2(System.MathF.Floor(transform.position.x), transform.position.y);
        }
        else if (transform.position.x % 1.0f > 0.9f) {
            transform.position = new Vector2(System.MathF.Ceiling(transform.position.x), transform.position.y);
        }


        //lock onto y axis
        if (transform.position.y % 1.0f < 0.1f) {
            transform.position = new Vector2(transform.position.x, System.MathF.Floor(transform.position.y));
        }
        else if (transform.position.y % 1.0f > 0.9f) {
            transform.position = new Vector2(transform.position.x, System.MathF.Ceiling(transform.position.y));
        }
    }
    void CheckDirections() {
        directions.Clear();
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector2.up), 1.0f)) {
            directions.Add(Vector2.up);
        }
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector2.down), 1.0f)) {
            directions.Add(Vector2.down);
        }
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector2.left), 1.0f)) {
            directions.Add(Vector2.left);
        }
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector2.right), 1.0f)) {
            directions.Add(Vector2.right);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (GetComponent<EnemyHealth>().invincible || !can_move || GetComponent<EnemyHealth>().GetHealth() <= 0) {
            return;
        }
        //calculate knockback direction
        if (other.gameObject.CompareTag("Weapon")) {

            float xDif = other.transform.position.x - transform.position.x;
            float yDif = other.transform.position.y - transform.position.y;
            Vector2 direction = Vector2.zero;

            //pushback vertically
            if (Mathf.Abs(xDif) < Mathf.Abs(yDif)) {

                //push down
                if (yDif > 0) {
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
                if (xDif > 0) {
                    direction = Vector2.left;
                }
                //push right
                else {
                    direction = Vector2.right;
                }
            }
            StartCoroutine(PerformKnockback(direction));
        }
    }
    IEnumerator PerformKnockback(Vector2 direction) {
        GetComponent<EnemyHealth>().invincible = true;
        can_move = false;

        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        rb.velocity = direction * 8;

        yield return new WaitForSeconds(0.3f);

        rb.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        GetComponent<EnemyHealth>().invincible = false;
        SnapToGrid();
        interpolator = 0;

        yield return new WaitForSeconds(0.2f);
        can_move = true;
    }

}
