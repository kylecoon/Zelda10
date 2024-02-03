using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DongMovement : MonoBehaviour
{
    private Vector2 current_direction;
    
    public Sprite[] sprites; //0 = up, 1 = down, 2 = horizontal1, 3 = horizontal2

    private SpriteRenderer sprt;

    private Dictionary<Vector2, Sprite> sprite_dictionary = new Dictionary<Vector2, Sprite>();

    private FormController form;

    private Rigidbody rb;

    public float movement_speed = 2.5f;

    private BoxCollider box;


    void Awake()
    {
        box = GetComponent<BoxCollider>();

        rb = GetComponent<Rigidbody>();

        form = GetComponent<FormController>();

        sprite_dictionary.Add(Vector2.up, sprites[0]);
        sprite_dictionary.Add(Vector2.down, sprites[1]);
        sprite_dictionary.Add(Vector2.right, sprites[2]);
        sprite_dictionary.Add(Vector2.left, sprites[2]);
        sprt = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        current_direction = GetComponent<FormController>().direction_controller;
        sprt.sprite = sprite_dictionary[current_direction];

        if (current_direction == Vector2.left) {
            sprt.flipX = true;
        }

        if (current_direction == Vector2.left || current_direction == Vector2.right) {
            box.size = new Vector3(2.3f, 1.3f, 1.0f);
        }
        else {
            box.size = new Vector3(1.3f, 1.3f, 1.0f);
        }

        box.center = new Vector2(0.0f, 0.26f);

        //transform.localScale = new Vector3 (0.8f, 0.8f, 1.0f);

        StartCoroutine(UpdateSprite(current_direction));
    }
    
    void OnDisable()
    {
        GetComponent<FormController>().direction_controller = current_direction;
        GetComponent<SpriteRenderer>().flipX = false;
    }

    void Update()
    {
        if (GetComponent<CamControl>().Is_Cam_Moving()) {
            rb.velocity = Vector2.zero;
        }
        else if (form.can_move) {
            rb.velocity = movement_speed * GetInput();
        }
    }

    Vector2 GetInput() {

        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        // prioritize horizontal movement over vertical movement
        // we are nott allowing diagonal movement
        if (Mathf.Abs(horizontal_input) > 0.0f) {
            vertical_input = 0.0f;
        }

        Vector2 new_direction = new Vector2(horizontal_input, vertical_input);

        //return if not moving or cant move
        if (new_direction == new Vector2(0.0f, 0.0f)) {
            return new_direction;
        }

        if (new_direction == Vector2.down || new_direction == Vector2.up) {
            box.size = new Vector3(1.3f, 1.3f, 1.0f);
        }
        else {
            box.size = new Vector3(2.3f, 1.3f, 1.0f);
        }

        //attempting to walk vertically
        if (new_direction.x == 0.0f) {

            //pretty much on grid, snap to left
            if (transform.position.x % 0.5f < 0.1f) {
                transform.position = transform.position + new Vector3(-(transform.position.x % 0.5f), 0.0f, 0.0f);
            }

            //pretty much on grid, snap to right
            else if (transform.position.x % 0.5f > 0.4f){
                transform.position = transform.position + new Vector3(0.5f - (transform.position.x % 0.5f), 0.0f, 0.0f);
            }
            //not close enough to grid, override input direction
            else {
                if (transform.position.x % 0.5f > 0.25f){
                    new_direction = Vector2.right;
                }
                else {
                    new_direction = Vector2.left;
                }
            }
        }

        //attempting to walk horizontally
        else {

            //pretty much on grid, snap to down
            if (transform.position.y % 0.5f < 0.1f) {
                transform.position = transform.position + new Vector3(0.0f, -(transform.position.y % 0.5f), 0.0f);
            }

            //pretty much on grid, snap to up
            else if (transform.position.y % 0.5f > 0.4f){
                transform.position = transform.position + new Vector3(0.0f, 0.5f - (transform.position.y % 0.5f), 0.0f);
            }
            //not close enough to grid, override input direction
            else {
                if (transform.position.y % 0.5f > 0.25f){
                    new_direction = Vector2.up;
                }
                else {
                    new_direction = Vector2.down;
                }
            }
        }
        
        StartCoroutine(UpdateSprite(new_direction));

        current_direction = new_direction;

        return new_direction;
    }

    public IEnumerator UpdateSprite(Vector2 new_direction) {
        yield return new WaitForEndOfFrame();
        if (new_direction == Vector2.up || new_direction == Vector2.down) {
            sprt.sprite = sprite_dictionary[new_direction];
            if (transform.position.y % 1.0f < 0.5f) {
                sprt.flipX = false;
            }
            else {
                sprt.flipX = true;
            }
        }
        if (new_direction == Vector2.left || new_direction == Vector2.right) {
            if (new_direction == Vector2.left) {
                sprt.flipX = true;
            }
            else {
                sprt.flipX = false;
            }
            if (transform.position.x % 1.0f < 0.5f) {
                sprt.sprite = sprite_dictionary[current_direction];
            } 
            else {
                sprt.sprite = sprites[3];
            }
        }
    }

    public Vector2 GetCurrentDirection() {
        return current_direction;
    }
}
