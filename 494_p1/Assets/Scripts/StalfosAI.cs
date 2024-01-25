using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class StalfosAI : MonoBehaviour
{
    private SpriteRenderer sprt;
    private int frameCount;
    private List<Vector2> directions = new List<Vector2>();
    private Vector2 start_position;
    private Vector2 end_position;
    private float interpolator;

    public bool isAlive;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        frameCount = 0;
        interpolator = 0.0f;
        isAlive = true;
        //stunned = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //animate on 15 frame cycle
        if (isAlive) {
            if (frameCount == 15) {
                sprt.flipX = !sprt.flipX;
                frameCount = 0;
            }
            ++frameCount;

            MoveEnemy();
        }
    }
    void MoveEnemy() {
        //pick new direction to go if in center of tile
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
}
