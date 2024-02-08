using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 current_direction;
    private FormController form;
    public Sprite[] sprites;
    private SpriteRenderer sprt;
    public GameObject indicatorObject;

    private GameObject indicator;
    public float movement_speed = 5.5f;
    private Rigidbody rb;
    private BoxCollider box;
    private bool touchingUp;
    private bool touchingDown;
    private bool touchingLeft;
    private bool touchingRight;
    public Vector2 most_recent_input;
    private Vector2 last_direction_touched;
    
    private AudioClip wallModeSound;

    private AudioClip failedWallModeSound;


    // Start is called before the first frame update
    void OnEnable()
    {
        current_direction = GetComponent<FormController>().direction_controller;
        StartCoroutine(UpdateSprite(current_direction));
        box.center = Vector3.zero;
        box.size = new Vector3(2.0f, 2.0f, 1.0f);
        transform.localScale = new Vector3(0.95f, 0.95f, 1.0f);
        wallModeSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/SoundEffect12");
        failedWallModeSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/SoundEffect13");
    }

    
    void OnDisable()
    {
        GetComponent<FormController>().direction_controller = current_direction;
        sprt.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        if (indicator != null) {
            Destroy(indicator);
        }
    }

    void Awake()
    {   
        sprt = GetComponent<SpriteRenderer>();
        form = GetComponent<FormController>();
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();

        touchingUp = false;
        touchingDown = false;
        touchingLeft = false;
        touchingRight = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            //attempt activate wall mode
            if (!form.wall_mode) {
                if (TouchingWall()) {
                    AudioSource.PlayClipAtPoint(wallModeSound, Camera.main.transform.position);
                    form.wall_mode = true;
                    indicator = Instantiate(indicatorObject, transform.position, Quaternion.identity);
                    indicator.transform.parent = gameObject.transform;
                    indicator.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                }
                else {
                    StartCoroutine(FailedWallMode());
                }
            }
            //deactivate wall mode
            else {
                AudioSource.PlayClipAtPoint(wallModeSound, Camera.main.transform.position);
                form.wall_mode = false;
                Destroy(indicator);
                touchingUp = false;
                touchingDown = false;
                touchingLeft = false;
                touchingRight = false;

            }
        }
        if (!form.wall_mode && !form.in_knockback) {
            rb.velocity = GetInput() * movement_speed;
        }
        else if (form.wall_mode){
            touchingUp = CheckUp();
            touchingDown = CheckDown();
            touchingLeft = CheckLeft();
            touchingRight = CheckRight();

            if (!form.in_knockback) {
                rb.velocity = GetWallInput() * movement_speed;
            }

            if (!TouchingWall()) {
                StartCoroutine(GoAroundCorner());
            }

            if (rb.velocity.x != 0 && !(touchingUp || touchingDown)) {
                rb.velocity = Vector2.zero;
            }
            if (rb.velocity.y != 0 && !(touchingLeft || touchingRight)) {
                rb.velocity = Vector2.zero;
            }
        }
    }

    Vector2 GetInput() {

        /*

        */
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        // prioritize horizontal movement over vertical movement
        // we are not allowing diagonal movement
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

    private Vector2 GetWallInput() {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        // prioritize horizontal movement over vertical movement
        // we are nott allowing diagonal movement
        if (Mathf.Abs(horizontal_input) > 0.0f) {
            vertical_input = 0.0f;
        }

        Vector2 new_direction = new Vector2(horizontal_input, vertical_input);

        if (new_direction == Vector2.zero) {
            return Vector2.zero;
        }

        StartCoroutine(UpdateSprite(new_direction));

        //moving vertically
        if (new_direction == Vector2.up || new_direction == Vector2.down) {
            //snap to grid
            if (transform.position.x % 1.0f > 0.5f) {
                transform.position = new Vector3(transform.position.x - (transform.position.x % 0.5f), transform.position.y, transform.position.z);
            }
            else if (transform.position.x % 1.0f < 0.5f) {
                transform.position = new Vector3(transform.position.x + (0.5f - (transform.position.x % 0.5f)), transform.position.y, transform.position.z);
            }
            if (touchingLeft || touchingRight) {
                if (touchingLeft) {
                    last_direction_touched = Vector2.left;
                }
                else {
                    last_direction_touched = Vector2.right;
                }
                most_recent_input = new_direction;
                return new_direction;
            }
            else {
                return Vector2.zero;
            }
        }
        //moving horizontally
        else if (new_direction != Vector2.zero){
            if (transform.position.y % 1.0f > 0.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y % 0.5f), transform.position.z);
            }
            else if (transform.position.y % 1.0f < 0.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y + (0.5f - (transform.position.y % 0.5f)), transform.position.z);
            }
            if (touchingUp || touchingDown) {
                if (touchingUp) {
                    last_direction_touched = Vector2.up;
                }
                else {
                    last_direction_touched = Vector2.down;
                }
                most_recent_input = new_direction;
                return new_direction;
            }
            else {
                return Vector2.zero;
            }
        }

        return Vector2.zero;
    }

    public IEnumerator UpdateSprite(Vector2 new_direction) {
        yield return new WaitForEndOfFrame();
        //walking vertically
        if (new_direction == Vector2.up) {
            sprt.sprite = sprites[1];
            sprt.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }
        else if (new_direction == Vector2.down) {
            sprt.sprite = sprites[0];
            sprt.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }
        else if (new_direction == Vector2.left) {
            sprt.sprite = sprites[0];
            sprt.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (new_direction == Vector2.right) {
            sprt.sprite = sprites[1];
            sprt.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            
        }
    }

    public Vector2 GetCurrentDirection() {
        return current_direction;
    }

    private bool TouchingWall() {
        touchingUp = CheckUp();
        touchingDown = CheckDown();
        touchingLeft = CheckLeft();
        touchingRight = CheckRight();
        return touchingUp || touchingDown || touchingLeft || touchingRight;
    }

    IEnumerator FailedWallMode() {
        AudioSource.PlayClipAtPoint(failedWallModeSound, Camera.main.transform.position);
        GameObject failedIndicator = Instantiate(indicatorObject, transform.position, Quaternion.identity);
        failedIndicator.transform.parent = gameObject.transform;
        failedIndicator.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(0.7f);
        Destroy(failedIndicator);
    }

    private bool CheckUp() {
        if (transform.position.y % 11.0f >= 7.5f) {
            return true;
        }
        RaycastHit hit;
        //check up
        if (Physics.Raycast(transform.position, Vector2.up, out hit, 1.1f)) {
            if (hit.transform.CompareTag("Wall")) {
                return true;
            }
        }
        return false;
    }

    private bool CheckDown() {
        if (transform.position.y % 11.0f <= 2.5f) {
            return true;
        }
        RaycastHit hit;
        //check up
        if (Physics.Raycast(transform.position, Vector2.down, out hit, 1.1f)) {
            if (hit.transform.CompareTag("Wall")) {
                return true;
            }
        }
        return false;
    }

    private bool CheckLeft() {
        if (transform.position.x % 16.0f <= 2.5f) {
            return true;
        }
        RaycastHit hit;
        //check up
        if (Physics.Raycast(transform.position, Vector2.left, out hit, 1.1f)) {
            if (hit.transform.CompareTag("Wall")) {
                return true;
            }
        }
        return false;
    }

    private bool CheckRight() {
        if (transform.position.x % 16.0f >= 12.5) {
            return true;
        }
        RaycastHit hit;
        //check up
        if (Physics.Raycast(transform.position, Vector2.right, out hit, 1.1f)) {
            if (hit.transform.CompareTag("Wall")) {
                return true;
            }
        }
        return false;
    }

    IEnumerator GoAroundCorner() {
        yield return new WaitForEndOfFrame();
        if (most_recent_input == Vector2.up) {
            if (last_direction_touched == Vector2.left) {
                transform.position = new Vector3(Mathf.Floor(transform.position.x - 1), transform.position.y - (transform.position.y % 0.5f) + 1.0f, transform.position.z);
            }

            else if (last_direction_touched == Vector2.right) {
                transform.position = new Vector3(Mathf.Floor(transform.position.x + 2), transform.position.y - (transform.position.y % 0.5f) + 1.0f, transform.position.z);
            }
            last_direction_touched = Vector2.down;
        }
        else if (most_recent_input == Vector2.down) {

            //37.5 4.39
            //39   3.5
            if (last_direction_touched == Vector2.left) {
                transform.position = new Vector3(Mathf.Floor(transform.position.x - 1), transform.position.y + (0.5f - (transform.position.y % 0.5f)) - 1.0f, transform.position.z);
            }
            else if (last_direction_touched == Vector2.right) {
                transform.position = new Vector3(Mathf.Floor(transform.position.x + 2), transform.position.y + (0.5f - (transform.position.y % 0.5f)) - 1.0f, transform.position.z);
            }
            last_direction_touched = Vector2.up;
        }
        else if (most_recent_input == Vector2.left) {
            if (last_direction_touched == Vector2.up) {
                transform.position = new Vector3(transform.position.x + (0.5f - (transform.position.x % 0.5f)) - 1.0f, (int)(transform.position.y + 2), transform.position.z);
            }
            else if (last_direction_touched == Vector2.down) {
                transform.position = new Vector3(transform.position.x + (0.5f - (transform.position.x % 0.5f)) - 1.0f, (int)(transform.position.y - 1), transform.position.z);
            }
            last_direction_touched = Vector2.right;
        }
        else if (most_recent_input == Vector2.right) {
            if (last_direction_touched == Vector2.up) {
                transform.position = new Vector3(transform.position.x - (transform.position.x % 0.5f) + 1.0f, (int)(transform.position.y + 2), transform.position.z);
            }
            else if (last_direction_touched == Vector2.down) {
                transform.position = new Vector3(transform.position.x - (transform.position.x % 0.5f) + 1.0f, (int)(transform.position.y - 1), transform.position.z);
            }
            last_direction_touched = Vector2.left;
        }
        yield return null;
    }
}
