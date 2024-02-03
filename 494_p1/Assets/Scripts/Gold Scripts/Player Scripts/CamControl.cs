using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject cam;

    private bool cam_moving;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam_moving = false;
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider){
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

            rb.velocity = Vector2.zero;   
            
            GetComponent<FormController>().can_move = false;
                         //final_position.z = -10;
                //Vector3 final_position = new Vector3(transform.position.x + 20, 0, transform.position.z -10);

                /* Transition to new "room" */

            cam_moving = true;
            yield return StartCoroutine(
                CoroutineUtilities.MoveObjectOverTime(cam.transform, initial_position, final_position, 2.5f)
            );

            GetComponent<FormController>().can_move = true;

            cam_moving = false;

                /* Hang around a little bit */
                //yield return new WaitForSeconds(2.5f);

            /* We must yield here to let time pass, or we will hardlock the game (due to infinite while loop) */
            //yield return null;

    }

    public bool Is_Cam_Moving() {
        return cam_moving;
    }
}
