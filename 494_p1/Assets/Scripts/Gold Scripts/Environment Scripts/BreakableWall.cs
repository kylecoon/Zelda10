using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private bool broken;
    public Sprite broken_sprite;
    private SpriteRenderer sprt;
    private AudioClip breakSound;

    void Start()
    {
        broken = false;
        sprt = GetComponent<SpriteRenderer>();
        breakSound = Resources.Load<AudioClip>("Zelda/Sound-Effects/SoundEffect11");
    }
    // Start is called before the first frame update
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == "Player" && gameObject != null && !broken) {
            if (other.gameObject.GetComponent<DongAttack>().attacking) {
                StartCoroutine(Break());
            }
        } else if(other.gameObject.name == "Wallspike"){
            StartCoroutine(Break());
        }
    }

    public IEnumerator Break() {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        broken = true;
        GetComponent<BoxCollider>().enabled = false;
        sprt.sprite = broken_sprite;
        sprt.color = new Color(0.33f, 0.18f, 0.17f);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
