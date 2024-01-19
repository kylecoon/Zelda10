using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    SpriteRenderer sprt;
    private Rigidbody rb;
    public float movement_speed = 4;

    public GameObject cam;

    private Dictionary<Vector2, Sprite[]> sprite_dictionary = new Dictionary<Vector2, Sprite[]>();

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 current_input = GetInput();
        
        rb.velocity = current_input * movement_speed;

    }

    Vector2 GetInput() {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal_input) > 0.0f) {
            vertical_input = 0.0f;
        }

        Vector2 new_direction = new Vector2(horizontal_input, vertical_input);

        //return if not moving
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
            else if (transform.position.x % 0.5 > 0.4){
                transform.position = transform.position + new Vector3(0.5f - (transform.position.x % 0.5f), 0.0f, 0.0f);
            }
            //not close enough to grid, override input direction
            else {
                if (transform.position.x % 0.5 > 0.25){
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

        return new_direction;
    }

    void UpdateSprite(Vector2 new_direction) {
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
        StartCoroutine(WaitForPlayerInputToTransition(collider));
        
    }

    IEnumerator WaitForPlayerInputToTransition(Collider other)
    {

                Vector3 initial_position = cam.transform.position;
                Vector3 final_position = cam.transform.position;
                Debug.Log(rb.velocity.x);
                if(other.tag == "->West" && rb.velocity.x < 0){
                    final_position.x -= 16;
                    // final_position.y += 100;
                } else if(other.tag == "->East" && rb.velocity.x  > 0){
                    final_position.x += 16;
                    // final_position.y += 10;
                } else if(other.tag == "->North" && rb.velocity.y > 0 ){
                    final_position.y += 16;
                } else if (other.tag == "->South" && rb.velocity.y < 0){ 
                    final_position.y -= 16;
                } else{ 
                    yield return null;
                }

                movement_speed = 0;
                rb.velocity = Vector2.zero;                //final_position.z = -10;
                //Vector3 final_position = new Vector3(transform.position.x + 20, 0, transform.position.z -10);

                /* Transition to new "room" */
                yield return StartCoroutine(
                    CoroutineUtilities.MoveObjectOverTime(cam.transform, initial_position, final_position, 2.5f)
                );

                /* Hang around a little bit */
                yield return new WaitForSeconds(2.5f);
                movement_speed = 4;

            /* We must yield here to let time pass, or we will hardlock the game (due to infinite while loop) */
            yield return null;
        
    }
}
