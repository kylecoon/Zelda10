using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class WallmasterAI : MonoBehaviour
{
    private Vector2 player_position;
    private Vector2 start_position;
    private Vector2 start_direction;
    float roomTop;
    float roomBottom;
    float roomLeft;
    float roomRight;
    public Sprite[] sprites;
    private SpriteRenderer sprt;
    public GameObject hand;
    public bool can_move = false;
    private bool snatched;
    // Start is called before the first frame update
    void Start()
    {
        can_move = false;
        snatched = false;
        start_position = CalculateStartingPosition();
        transform.position = start_position;
        sprt = GetComponent<SpriteRenderer>();
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GetComponent<EnemyHealth>().GetHealth() > 0 && can_move) {
            if (Time.time % 1.5f < 0.75f) {
                sprt.sprite = sprites[0];
            }
            else {
                sprt.sprite = sprites[1];
            }
        }
        if (GetComponent<EnemyHealth>().GetHealth() <= 0) {
            can_move = false;
        }
    }

    Vector2 CalculateStartingPosition() {
        player_position = GameObject.Find("Player").transform.position;
        
        float xDif;
        float yDif;
        //closer to right wall
        if (player_position.x % 16.0f > 8.0f) {
            xDif = 16.0f - (player_position.x % 16.0f);
        }
        //closer to left wall
        else {
            xDif = -(player_position.x % 16.0f);
        }

        //closer to top wall
        if (player_position.y % 10.0f > 5.0f) {
            yDif = 10.0f - (player_position.y % 10.0f);
        }
        //closer to bottom wall
        else {
            yDif = -(player_position.y % 10.0f);
        }

        //closer to left/right wall than to top/bottom wall
        if (Mathf.Abs(xDif) < Mathf.Abs(yDif)) {
            roomBottom = player_position.y - (player_position.y % 10.0f) + 2.0f;
            roomTop = player_position.y + (10.0f - (player_position.y % 10.0f)) - 2.0f;
            //closer to left wall
            if (xDif < 0) {
                start_direction = Vector2.right;
                return new Vector2(player_position.x - (player_position.x % 16.0f) + 1.0f, Random.Range(roomBottom, roomTop));
            }
            //closer to right wall
            else {
                start_direction = Vector2.left;
                return new Vector2(player_position.x + (16.0f - (player_position.x % 16.0f)) - 2.0f, Random.Range(roomBottom, roomTop));
            }

        }
        //closer to top/bottom wall than to left/right wall
        else {
            roomLeft = player_position.x - (player_position.x % 16.0f) + 2.0f;
            roomRight = player_position.x + (16.0f - (player_position.x % 16.0f)) - 2.0f;
            //closer to bottom wall
            if (yDif < 0) {
                start_direction = Vector2.up;
                return new Vector2(Random.Range(roomLeft, roomRight), player_position.y - (player_position.y % 10.0f) + 1.0f);
            }
            //closer to top wall;
            else {
                start_direction = Vector2.down;
                return new Vector2(Random.Range(roomLeft, roomRight), player_position.y + (10.0f - (player_position.y % 10.0f)) - 1.0f);
            }
        }
    }

    IEnumerator Move() {
        yield return StartCoroutine(Emerge());
        can_move = true;
        float interpolator = 0.0f;
        //move out of wall
        while (interpolator <= 1.0f) {
            if (!can_move) {
                yield break;
            }
            transform.position = Vector3.Lerp(start_position, start_position+start_direction, interpolator);
            interpolator += 0.02f;
            yield return null;
        }
        interpolator = 0.0f;

        start_position = transform.position;
        if (start_direction == Vector2.up || start_direction == Vector2.down) {
            //move right
            if (GameObject.Find("Player").transform.position.x > transform.position.x) {
                float interpolator_rate = 1.0f / Mathf.Abs(roomRight - start_position.x);
                while (interpolator <= 1.0f) {
                    if (!can_move) {
                        yield break;
                    }
                    if (snatched) {
                        yield return StartCoroutine(Snatch());
                        yield break;
                    }
                    transform.position = Vector3.Lerp(start_position, new Vector3(roomRight, start_position.y), interpolator);
                    interpolator += 0.04f * interpolator_rate;
                    yield return null;
                }
            }
            //move left
            else {
                while (interpolator <= 1.0f) {
                    if (!can_move) {
                        yield break;
                    }
                    if (snatched) {
                        yield return StartCoroutine(Snatch());
                        yield break;
                    }
                    float interpolator_rate = 1.0f / Mathf.Abs(start_position.x - roomLeft - 1.0f);
                    transform.position = Vector3.Lerp(start_position, new Vector3(roomLeft - 1.0f, start_position.y), interpolator);
                    interpolator += 0.04f * interpolator_rate;
                    yield return null;
                }
            }
        }
        else {
            //move up
            if (GameObject.Find("Player").transform.position.y > transform.position.y) {
                float interpolator_rate = 1.0f / Mathf.Abs(roomTop + 1.0f - start_position.y);
                while (interpolator <= 1.0f) {
                    if (!can_move) {
                        yield break;
                    }
                    if (snatched) {
                        yield return StartCoroutine(Snatch());
                        yield break;
                    }
                    transform.position = Vector3.Lerp(start_position, new Vector3(start_position.x, roomTop + 1.0f), interpolator);
                    interpolator += 0.04f * interpolator_rate;
                    yield return null;
                }
            }
            //move down
            else {
                while (interpolator <= 1.0f) {
                    if (!can_move) {
                        yield break;
                    }
                    if (snatched) {
                        yield return StartCoroutine(Snatch());
                        yield break;
                    }
                    float interpolator_rate = 1.0f / Mathf.Abs(start_position.y - roomBottom - 1.0f);
                    transform.position = Vector3.Lerp(start_position, new Vector3(start_position.x, roomBottom - 1.0f), interpolator);
                    interpolator += 0.04f * interpolator_rate;
                    yield return null;
                }            
            }
        }
        can_move = false;
        yield return StartCoroutine(Emerge());
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator Emerge() {
        sprt.sprite = sprites[2];
        yield return new WaitForSeconds(0.8f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player") {
            snatched = true;
        }
    }

    IEnumerator Snatch() {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Health>().Invincible = true;
        player.GetComponent<Movement>().Flip_CanMove();
        transform.position = player.transform.position;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
