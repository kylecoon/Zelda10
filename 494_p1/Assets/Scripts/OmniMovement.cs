using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniMovement : MonoBehaviour
{
    private List<Vector2> directions = new List<Vector2>();
    private Vector2 previous_direction;
    public bool can_move;
    public bool moving = false;
    private Rigidbody rb;
    private RaycastHit hit;
    
    void Start()
    {
        previous_direction = Vector2.zero;
        can_move = true;
        moving = false;
        rb = GetComponent<Rigidbody>();
    }

    public IEnumerator MoveEnemy(Vector2 direction, float speed) {
        if (!can_move || direction == Vector2.zero || speed == 0) {
            yield break;
        }

        moving = true;
        SnapToGrid(0.5f);

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
        }

        rb.velocity = Vector2.zero;
        moving = false;
        SnapToGrid(0.5f);

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

        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.up), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.up);
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.down), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.down);
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.left), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.left);
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.right), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.right);
        }

        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.right + Vector2.up), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.right + Vector2.up);
        }

        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.right + Vector2.down), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.right + Vector2.down);
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.left + Vector2.down), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.left + Vector2.down);
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector2.left + Vector2.up), out hit, 1.5f);
        if (hit.transform == null || !hit.transform.gameObject.CompareTag("Wall")) {
            directions.Add(Vector2.left + Vector2.up);
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
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
            directions.Add(previous_direction);
        }

        previous_direction = directions[UnityEngine.Random.Range(0, directions.Count)];
        return previous_direction;
    }
}
