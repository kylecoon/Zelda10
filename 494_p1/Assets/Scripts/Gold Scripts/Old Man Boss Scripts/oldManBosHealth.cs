using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    IEnumerator DamageColor() {

        //NES - The Legend of Zelda - NPCs_scaled_17x_pngcrushed
        // Sprite hold = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = hurtSprites[0];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[1];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[2];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[3];
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().sprite = hurtSprites[4];
    }

    public AudioClip LastSound;
     public IEnumerator AlterHealth(int health_change) {
        
        if(health == 1){
             AudioSource.PlayClipAtPoint(LastSound, Camera.main.transform.position);
        } else {
            AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
        }
        
        health += health_change;

        GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(DamageColor());
        if (health <= 0 && gameObject != null) {
            isDead = true;
            StartCoroutine(Die());
        }
        GetComponent<BoxCollider>().enabled = true;

        yield return null;
    }


    public GameObject egg;
    IEnumerator Die(){
        coroutineQueue.Clear();

        GetComponent<TypeWriter>().DoSentences();

        StartCoroutine(shudder());

        yield return new WaitForSeconds(5); // children will be gone by then

        GameObject Egg = Instantiate(egg, gameObject.transform.position, Quaternion.identity);

        CoroutineUtilities.MoveObjectOverTime(Egg.transform, Egg.transform.position, new Vector2(67,45), 5);

        GetComponent<SpriteRenderer>().sprite = GetComponent<OldManBossAttack>().broken;

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
        //death stuff here
        // yield return null;
    }

    void OnCollisionEnter(UnityEngine.Collision collision){
        if(collision.collider.CompareTag("Attack")){
            StartCoroutine(showShield());

        } 
    }

    IEnumerator showShield(){
        shield.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shield.SetActive(false);
    }

    IEnumerator shudder(){
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
        while(true){
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
            yield return new WaitForSeconds(0.05f);
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);

        }
        
    }
}

