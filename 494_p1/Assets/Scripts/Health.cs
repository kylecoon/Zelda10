using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update

    public bool Invincible;
    public int health;

    public int MaxHP;
    public string DamageFromTag;

    private Animation animations;

    private Sprite[] sprites;

    public TextMeshProUGUI HPshown;

    public Sprite[] hurtSprites;

    private int lastHurtFrame = -6;

    //private GameObject parent; 

    void Start()
    {
        //parent = GetComponentInParent<>();
       animations = GetComponent<Animation>();
       sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");

    }

    // Update is called once per frame
    void Update()
    {
        // cheat mode toggle
        if(gameObject.CompareTag("Player") && Input.GetKey(KeyCode.Alpha1)){
            if(!Invincible){
                Invincible = true;
                Inventory inven = GetComponent<Inventory>();
                inven.rupee_count = int.MaxValue;
                inven.UpdateRupCount();
                inven.numBombs = int.MaxValue;
                inven.numKeys = int.MaxValue;
                
            }else{
                Invincible = false;
                Inventory inven = GetComponent<Inventory>();
                // inven.rupee_count = inven.TrueCountRup;
                // inven.numBombs = inven.numBombsTrue;
                // inven.numKeys = inven.numKeysTrue;
            }
        }
    }

    void OnTriggerEnter(Collider coll){
        if(coll.CompareTag(DamageFromTag)){
            health--;
            if(health <= 0){
                StartCoroutine(Death());
            }
        }
        
    }

    public void UpdateHP(){
        HPshown.text = "HP: " + health;
    }

    IEnumerator Death(){

        //Animation hurt = GetComponent<Animation>();
        animations.Play();
        GetComponent<Movement>().movement_speed = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GetComponent<Movement>().movement_speed = 4;  
        health = MaxHP;
        UpdateHP();
        return null;
    }

    void OnCollisionEnter(Collision collision){

        
        if(collision.gameObject.CompareTag(DamageFromTag)){
            hit(collision.transform);
        }

        if(health <= 0){
            Death();
        }
        
    }

    IEnumerator hit(Transform collider){

        Debug.Log("hit");
        // if(!Invincible && Time.frameCount > lastHurtFrame + 5){
            Vector2 finalPos = gameObject.transform.position;

            float xDif = collider.position.x - finalPos.x;
            float yDif = collider.position.y - finalPos.y;

            //Vector2 finalPos = collider.transform.position;
            if(xDif == 0){ // so above or below
                if(yDif > 0){
                    finalPos.y -= 3;
                } else {
                    finalPos.y += 3;
                }
            } else { //ydif == 0
                if(xDif > 0){
                    finalPos.x -= 3;
                } else {
                    finalPos.x += 3;
                }
            }

            health--; 
            UpdateHP();
            GetComponent<Movement>().movement_speed = 0;
            
            //animations.Play();
            Debug.Log("hit");
            Rigidbody rb = GetComponent<Rigidbody>();
            yield return StartCoroutine(CoroutineUtilities.MoveObjectOverTime(rb.transform, gameObject.transform.position, finalPos, 0.3f));

            GetComponent<Movement>().movement_speed = 4;
            lastHurtFrame = Time.frameCount;
            
        //}

        yield return null;
    }

    void hurtAnimation(){

    }

    


}
