using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;

public class KeeseAI : MonoBehaviour
{
    public float maxSpeed = 2;
    private SpriteRenderer sprt;
    public Sprite[] sprites;
    private float current_speed;
    private int frame_count;
    private OmniMovement mov;
    private EnemyHealth healthComp;
    public Sprite death_sprite;
    private float cycle;
    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        current_speed = 0;
        frame_count = 0;
        mov = GetComponent<OmniMovement>();
        healthComp = GetComponent<EnemyHealth>();
        cycle = Random.Range(20.0f, 32.0f);
        

    }

    // Update is called once per frame
    void Update()
    {
        //animation
        if (healthComp.is_alive) {
            if (frame_count <= 24) {
                sprt.sprite = sprites[0];
            }
            else if (frame_count <= 48) {
                sprt.sprite = sprites[1];
            }
            else {
                frame_count = 0;
            }
            frame_count += (int)(1 * (int)(current_speed*2));
        }
        else {
            sprt.sprite = death_sprite;
        }

        if (!mov.moving && mov.can_move) {

            StartCoroutine(mov.MoveEnemy(mov.PickDirection(), current_speed));

            //speed up
            if (Time.time % cycle <= cycle * (1.0f/4.0f)) {
                current_speed = Mathf.Min(current_speed + 0.5f, maxSpeed);
            }
            //fly around
            else if (Time.time % cycle <= cycle * (1.0f/2.0f)) {
                current_speed = maxSpeed;
            }
            //slow down
            else if (Time.time % cycle <= cycle * (3.0f/4.0f)) {
                current_speed = Mathf.Max(current_speed - 0.5f, 0);
            }
            else {
                current_speed = 0;
            }
        }
    }
}
