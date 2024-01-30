using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class StalfosAI : MonoBehaviour
{
    private SpriteRenderer sprt;
    private int frameCount;
    private Rigidbody rb;
    private EnemyMovement mov;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        frameCount = 0;

        rb = GetComponent<Rigidbody>();
        mov = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //animate on 15 frame cycle
        if (mov.can_move) {
            if (frameCount == 15) {
                sprt.flipX = !sprt.flipX;
                frameCount = 0;
            }
            ++frameCount;
            if (!mov.moving && mov.can_move) {
                mov.moving = true;
                StartCoroutine(mov.MoveEnemy(mov.PickDirection(), speed));
            }
        }
    }
}
