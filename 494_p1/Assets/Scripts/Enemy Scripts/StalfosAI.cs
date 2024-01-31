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
    public Sprite spawn_sprite;
    bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        frameCount = 0;

        rb = GetComponent<Rigidbody>();
        mov = GetComponent<EnemyMovement>();

        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned) {
            spawned = true;
            mov.can_move = false;
            StartCoroutine(Spawn());
        }
        //animate on 15 frame cycle
        if (mov.can_move && spawned) {
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
        //Debug.Log(mov.can_move);
    }

    IEnumerator Spawn() {
        mov.can_move = false;
        Sprite temp = sprt.sprite;
        sprt.sprite = spawn_sprite;
        yield return new WaitForSeconds(3.0f);
        sprt.sprite = temp;
        mov.can_move = true;
        spawned = true;
        yield return null;
    }
}
