using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    SpriteRenderer sprt;
    private Rigidbody rb;
    public float movement_speed = 4;
    public GameObject cam;
    private Dictionary<Vector2, Sprite[]> sprite_dictionary = new Dictionary<Vector2, Sprite[]>();
    private bool can_move;

    private Vector2 current_direction;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1020, 960, false);

        sprt = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");

        //match left direction to left facing sprites
        sprite_dictionary.Add(Vector2.left, new Sprite[] {sprites[1], sprites[13]});
        //match right direction to right facing sprites
        sprite_dictionary.Add(Vector2.right, new Sprite[] {sprites[3], sprites[15]});
        //match up direction to up facing sprites
        sprite_dictionary.Add(Vector2.up, new Sprite[] {sprites[2], sprites[14]});
        //match down direction to down facing sprites
        sprite_dictionary.Add(Vector2.down, new Sprite[] {sprites[0], sprites[12]});

        can_move = true;

        current_direction = Vector2.down;
    }

    // Update is called once per frame
    void Update(){
        Vector2 current_input = Vector2.zero;

        if (can_move) {
            current_input = GetInput();
        }

        rb.velocity = current_input * movement_speed;

    }


    /*
    Vector2 inputDir = Vector2.zero;
    Vector2 getInputDir() 
    {
        Dictionary<KeyCode, Vector2> keymap = new Dictionary<KeyCode, Vector2>();
        keymap.Add(KeyCode.LeftArrow, Vector2.left);
        keymap.Add(KeyCode.RightArrow, Vector2.right);
        keymap.Add(KeyCode.UpArrow, Vector2.up);
        keymap.Add(KeyCode.DownArrow, Vector2.down);

        foreach(KeyValuePair<KeyCode, Vector2> k in keymap) {
            if()inputDir = k.Value;
        }
    }
    */

    Vector2 GetInput() {

        /*

        */
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
        
        UpdateSprite(new_direction);

        current_direction = new_direction;

        return new_direction;
    }

    public void UpdateSprite(Vector2 new_direction) {
        //walking vertically
        if (new_direction.x == 0.0f) {
            if (transform.position.y % 1.0f < 0.5) {
                sprt.sprite = sprite_dictionary[new_direction][0];
            }
            else {
                sprt.sprite = sprite_dictionary[new_direction][1];
            }
        }
        //walking horizontally
        else {
            if (transform.position.x % 1.0f < 0.5) {
                sprt.sprite = sprite_dictionary[new_direction][0];
            }
            else {
                sprt.sprite = sprite_dictionary[new_direction][1];
            }
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.CompareTag("->North") && rb.velocity.y > 0) {
            StartCoroutine(WaitForPlayerInputToTransition(new Vector3(0, 11, 0) ));
        }
        else if (collider.gameObject.CompareTag("->South") && rb.velocity.y < 0) {
            StartCoroutine(WaitForPlayerInputToTransition(new Vector3(0, -11, 0) ));
        }
        else if (collider.gameObject.CompareTag("->East") && rb.velocity.x > 0) {
            StartCoroutine(WaitForPlayerInputToTransition(new Vector3(16, 0, 0) ));
        }
        else if (collider.gameObject.CompareTag("->West") && rb.velocity.x < 0) {
            StartCoroutine(WaitForPlayerInputToTransition(new Vector3(-16, 0, 0) ));
        }
        
    }

    IEnumerator WaitForPlayerInputToTransition(Vector3 delta)
    {

                
            Vector3 initial_position = cam.transform.position;
            Vector3 final_position = initial_position + delta;
            Debug.Log(rb.velocity.x);

            movement_speed = 0;
            rb.velocity = Vector2.zero;                //final_position.z = -10;
                //Vector3 final_position = new Vector3(transform.position.x + 20, 0, transform.position.z -10);

                /* Transition to new "room" */
            yield return StartCoroutine(
                CoroutineUtilities.MoveObjectOverTime(cam.transform, initial_position, final_position, 2.5f)
            );

                /* Hang around a little bit */
                //yield return new WaitForSeconds(2.5f);
            movement_speed = 4;

            /* We must yield here to let time pass, or we will hardlock the game (due to infinite while loop) */
            //yield return null;
        
    }

    public void Flip_CanMove() {
        can_move = !can_move;
    }

    public bool Check_CanMove() {
        return can_move;
    }

    public Vector2 Get_CurrentDirection() {
        return current_direction;
    }
}
