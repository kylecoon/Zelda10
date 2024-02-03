using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    SpriteRenderer sprt;
    private Rigidbody rb;
    public float movement_speed = 4;
    private Dictionary<Vector2, Sprite[]> sprite_dictionary = new Dictionary<Vector2, Sprite[]>();
    public bool in_knockback;
    private BoxCollider box;

    private Vector2 current_direction;
    private FormController form;

    void OnEnable()
    {
        current_direction = GetComponent<FormController>().direction_controller;
        StartCoroutine(UpdateSprite(current_direction));

        box.size = new Vector2(0.9f, 0.9f);
        box.center = new Vector2(0.0f, 0.0f);
    }

    
    void OnDisable()
    {
        GetComponent<FormController>().direction_controller = current_direction;
    }
    void Awake()
    {
        box = GetComponent<BoxCollider>();

        form = GetComponent<FormController>();

        in_knockback = false;
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

        current_direction = Vector2.down;
    }

    // Update is called once per frame
    void Update(){
        Vector2 current_input = Vector2.zero;

        if (form.can_move) {
            current_input = GetInput();
        }

        if (!GetComponent<FormController>().in_knockback) {
            rb.velocity = current_input * movement_speed;
        }

    }

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
        
        StartCoroutine(UpdateSprite(new_direction));

        current_direction = new_direction;

        return new_direction;
    }

    public IEnumerator UpdateSprite(Vector2 new_direction) {
        yield return new WaitForEndOfFrame();
        //walking vertically
        if (new_direction.x == 0.0f && new_direction.y != 0) {
            if (transform.position.y % 1.0f < 0.5) {
                sprt.sprite = sprite_dictionary[new_direction][0];
            }
            else {
                sprt.sprite = sprite_dictionary[new_direction][1];
            }
        }
        //walking horizontally
        else if (new_direction.x != 0){
            if (transform.position.x % 1.0f < 0.5) {
                sprt.sprite = sprite_dictionary[new_direction][0];
            }
            else {
                sprt.sprite = sprite_dictionary[new_direction][1];
            }
        }
        else {
            yield return null;
        }
    }

    public Vector2 GetCurrentDirection() {
        return current_direction;
    }
}
