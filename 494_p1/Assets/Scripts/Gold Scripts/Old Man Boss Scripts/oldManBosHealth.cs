using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldManBosHealth : MonoBehaviour
{

       public int health = 12;
    // Start is called before the first frame update

    public bool isDead = false;

    public AudioClip damageSound;

    public Sprite[] hurtSprites;

    public GameObject shield;

    public static Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    void Start()
    {
        
        StartCoroutine(useQueue());
    }

    public static void EnqueueCoroutine(IEnumerator coroutine) {
        coroutineQueue.Enqueue(coroutine);
    }

    

    IEnumerator useQueue(){

        while (true){
            while (coroutineQueue.Count > 0){
                yield return StartCoroutine(coroutineQueue.Dequeue());
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DamageColor() {
        Sprite hold =  GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = hurtSprites[0];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[1];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[2];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[3];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hold;
    }

     public void AlterHealth(int health_change) {
        
        AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
        health += health_change;
        
        StartCoroutine(DamageColor());
        if (health <= 0 && gameObject != null) {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die(){
        coroutineQueue.Clear();

        StartCoroutine(shudder());

        //death stuff here
        yield return null;
    }

    void OnCollisionEnter(UnityEngine.Collision collision){
        if(collision.collider.CompareTag("Attack")){
            StartCoroutine(showShield());

        } else if(collision.collider.CompareTag("Enemy")){
            AlterHealth(-1);
        }
    }

    IEnumerator showShield(){
        shield.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        shield.SetActive(false);
    }

    IEnumerator shudder(){
        transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        while(true){
            transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y);
            yield return new WaitForSeconds(0.01f);
            transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
        }
        
    }
}

