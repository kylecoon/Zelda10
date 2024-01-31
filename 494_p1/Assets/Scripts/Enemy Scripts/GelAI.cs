using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelAI : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer sprt;
    private int frameCount;
    private EnemyMovement mov;
    private bool isDead;
    private EnemyHealth healthComp;
    public Sprite death_sprite;
    public Sprite spawn_sprite;
    bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();   
        frameCount = 0;
        mov = GetComponent<EnemyMovement>();
        isDead = false;
        healthComp = GetComponent<EnemyHealth>();
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!spawned) {
            spawned = true;
            StartCoroutine(Spawn());
        }
        //animation
        if (healthComp.is_alive && mov.can_move) {
            if (frameCount == 15) {
                sprt.sprite = sprites[0];
            }
            else if (frameCount == 30) {
                sprt.sprite = sprites[1];
                frameCount = 0;
            }
        ++frameCount;
        }
        else {
            sprt.sprite = death_sprite;
        }
    }

    //movement timer
    IEnumerator Move() {
        while (!isDead) {
            Vector3 direction = mov.PickDirection();
            yield return StartCoroutine(mov.MoveEnemy(direction, 3.0f));
            yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));
        }
    }

    // Handle enemy destruction
    void OnDestroy() {
        isDead = true;
    }

    IEnumerator Spawn() {
        mov.can_move = false;
        Sprite temp = sprt.sprite;
        sprt.sprite = spawn_sprite;
        yield return new WaitForSeconds(3.0f);
        sprt.sprite = temp;
        mov.can_move = true;
        spawned = true;
        StartCoroutine(Move());
        yield return null;
    }
}
