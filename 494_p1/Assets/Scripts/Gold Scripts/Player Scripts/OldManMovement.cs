using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OldManMovement : MonoBehaviour
{
    private Vector2 current_direction;

    public Sprite[] sprites;
    Rigidbody rb;

    SpriteRenderer sprt;
    public GameObject cam;
    // Start is called before the first frame update
    void OnEnable()
    {
        current_direction = GetComponent<FormController>().direction_controller;
        GetComponent<BoxCollider>().enabled = false;
        cam.transform.parent = gameObject.transform;

        StartCoroutine(UpdateSprite(current_direction));
    }

    
    void OnDisable()
    {
        GetComponent<FormController>().direction_controller = current_direction;
        GetComponent<BoxCollider>().enabled = true;
    }
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();  
        rb = GetComponent<Rigidbody>(); 
    }
    void Update()
    {
        rb.velocity = GetInput() * 10.0f;
    }

    Vector2 GetInput() {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        // prioritize horizontal movement over vertical movement
        // we are not allowing diagonal movement
        if (Mathf.Abs(horizontal_input) > 0.0f) {
            vertical_input = 0.0f;
        }
        Vector2 new_direction = new Vector2(horizontal_input, vertical_input);
        if (new_direction == Vector2.up) {
            sprt.sprite = sprites[0];
        }
        else if (new_direction == Vector2.down) {
            sprt.sprite = sprites[1];
        }
        else if (new_direction == Vector2.left) {
            sprt.sprite = sprites[2];
        }
        else if (new_direction == Vector2.right) {
            sprt.sprite = sprites[3];
        }
        return new_direction;
    }
    public Vector2 GetCurrentDirection() {
        return current_direction;
    }

    public IEnumerator UpdateSprite(Vector2 new_direction) {
        yield return new WaitForEndOfFrame();
        if (current_direction == Vector2.up) {
            sprt.sprite = sprites[0];
        }
        else if (current_direction == Vector2.down) {
            sprt.sprite = sprites[0];
        }
        else if (current_direction == Vector2.left) {
            sprt.sprite = sprites[0];
        }
        else if (current_direction == Vector2.right) {
            sprt.sprite = sprites[0];
        }
    }
}
