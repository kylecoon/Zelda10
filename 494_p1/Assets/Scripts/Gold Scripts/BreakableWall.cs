using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public Sprite broken_sprite;
    private SpriteRenderer sprt;

    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == "Player") {
            if (other.gameObject.GetComponent<DongAttack>().attacking) {
                StartCoroutine(Break());
            }
        }
    }

    IEnumerator Break() {
        //previously 245 102 101
        GetComponent<BoxCollider>().enabled = false;
        //after      191 138 167
        sprt.sprite = broken_sprite;
        sprt.color = new Color(0.33f, 0.18f, 0.17f);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
