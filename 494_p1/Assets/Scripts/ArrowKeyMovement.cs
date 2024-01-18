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
    private int spriteVersion = 0;
    public Sprite[] sprites;

    // private int curDirection = 0;
    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        // sprites = Resources.LoadAll<Sprite>("Link_Sprites");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 current_input = GetInput();
        
        rb.velocity = current_input * movement_speed;

        

        int hold = spriteVersion;
        if(Mathf.Abs(rb.velocity.x) > math.abs(rb.velocity.y)){
            if(rb.velocity.x > 0){
                spriteVersion = 3;
            } else {
                spriteVersion = 1;
            }
        } else {
            if(rb.velocity.y > 0){
                spriteVersion = 2;
            } else {
                spriteVersion = 0;
            }
        }
        if(hold == spriteVersion && rb.velocity != Vector3.zero) spriteVersion += 4;

        sprt.sprite = sprites[spriteVersion];

    }

    Vector2 GetInput() {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal_input) > 0.0f) {
            vertical_input = 0.0f;
        }

        

        return new Vector2(horizontal_input, vertical_input);
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
