using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManBossAttack : MonoBehaviour
{
    // Start is called before the first frame update
        // Start is called before the first frame update
    public GameObject[] plantAttacks;
    public GameObject[] plants;

    public GameObject[] spikeAttacks;

    public GameObject stone;

    public GameObject breakableWall;

    public GameObject spikeBlockWall;

    public Sprite broken;

    


    void Start()
    {
        oldManBosHealth.EnqueueCoroutine(opening());
        // oldManBosHealth.EnqueueCoroutine(fighting());
    }

    // Update is called once per frame
    void Update(){
        
    }

    IEnumerator opening(){

        GetComponent<BoxCollider>().enabled = false;

        for(int i = 0; i < plants.Length; ++i){
            plants[i].SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }

        GetComponent<BoxCollider>().enabled = true;

        StartCoroutine(fighting());
        
        yield return null;


    }

    IEnumerator fighting(){
        while(!GetComponent<oldManBosHealth>().isDead){
            int attack = Random.Range(0,3);

            if(attack == 0){
                stone.GetComponent<SpriteRenderer>().color = Color.green;
                yield return new WaitForSecondsRealtime(5f);
                oldManBosHealth.EnqueueCoroutine(MakeplantAttack());

            } else if(attack == 1){
                stone.GetComponent<SpriteRenderer>().color = Color.blue;
                yield return new WaitForSecondsRealtime(5f);
                oldManBosHealth.EnqueueCoroutine(MakeFireAttack());

            } else {
                stone.GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSecondsRealtime(5f);
                oldManBosHealth.EnqueueCoroutine(MakeBlockAttack());
                // breakable wall attack
            }
            stone.GetComponent<SpriteRenderer>().color = Color.black;
            yield return new WaitForSecondsRealtime(5f);
            

        }

        stone.GetComponent<SpriteRenderer>().sprite = broken;

        yield return new WaitForSeconds(0.5f);

        Destroy(stone);

        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }

        
    }

    IEnumerator MakeplantAttack(){
        for(int i = 0; i < plantAttacks.Length; ++i){
            plantAttacks[i].GetComponent<plantAttack>().move(true);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2f);
        for(int i = plantAttacks.Length - 1 ; i >= 0; --i){
            plantAttacks[i].GetComponent<plantAttack>().move(false);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    IEnumerator MakespikeAttack(){
        for(int i = 0; i < spikeAttacks.Length; ++i){
            spikeAttacks[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < spikeAttacks.Length; ++i){
            spikeAttacks[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator MakeBlockAttack(){

        spikeBlockWall.SetActive(true);
        
        GameObject wall = Instantiate(breakableWall, new Vector2(58, 40.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        // GameObject fireball2 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, 0.0f), Quaternion.identity);
        // GameObject fireball3 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, -0.5f), Quaternion.identity);

        wall.GetComponent<Rigidbody>().velocity = new Vector2(1.0f, 0) * 2;
        // for(int i = 0; i < spikeAttacks.Length; ++i){
        //     spikeAttacks[i].SetActive(true);
        //     yield return new WaitForSeconds(0.1f);
        // }
        // yield return new WaitForSeconds(2f);
        // for(int i = 0; i < spikeAttacks.Length; ++i){
        //     spikeAttacks[i].SetActive(false);
        //     yield return new WaitForSeconds(0.1f);
        // }
        yield return new WaitForSeconds(10f);
        spikeBlockWall.SetActive(false);
        yield return null;
    }


    public GameObject fireWall;
    IEnumerator MakeFireAttack(){

        // spikeBlockWall.SetActive(true);
        
        for(int i = 0; i < 3; ++i){
            GameObject wall = Instantiate(fireWall, new Vector2(67, 43.5f), Quaternion.identity);
            // yield return new WaitForSeconds(0.5f);
        // GameObject fireball2 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, 0.0f), Quaternion.identity);
        // GameObject fireball3 = Instantiate(fireball, (Vector2)transform.position + new Vector2(-1.5f, -0.5f), Quaternion.identity);
            // wall.GetComponent<Rigidbody>().velocity = new Vector2(2.0f, 0) * 2;

            wall.GetComponent<Rigidbody>().AddForce(1.0f, 0, 0, ForceMode.Impulse);
            
            yield return new WaitForSeconds(3f);

        }
        // for(int i = 0; i < spikeAttacks.Length; ++i){
        //     spikeAttacks[i].SetActive(true);
        //     yield return new WaitForSeconds(0.1f);
        // }
        // yield return new WaitForSeconds(2f);
        // for(int i = 0; i < spikeAttacks.Length; ++i){
        //     spikeAttacks[i].SetActive(false);
        //     yield return new WaitForSeconds(0.1f);
        // }
        yield return new WaitForSeconds(1f);
        // spikeBlockWall.SetActive(false);
        yield return null;
    }

}
