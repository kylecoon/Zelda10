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

    //private Animation animations;

    private Sprite[] sprites;

    private SpriteRenderer SRenderer;

    public TextMeshProUGUI HPshown;

    public Sprite[] hurtSprites;

    private int lastHurtFrame = -13;

    AnimationClip[] animations;

    //private GameObject parent; 

    void Start()
    {
        SRenderer = GetComponent<SpriteRenderer>();
        //parent = GetComponentInParent<>();
        Screen.SetResolution(1020, 960, true);
       //animations = GetComponent<Animation>();
        sprites = Resources.LoadAll<Sprite>("Zelda/Link_Sprites");

    }

    // Update is called once per frame
    void Update()
    {
        // cheat mode toggle
        if(gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Alpha1)){
            if(!Invincible){
                Invincible = true;
                Inventory inven = GetComponent<Inventory>();
                inven.rupee_count = int.MaxValue;
                inven.UpdateRupCount();
                inven.numBombs = int.MaxValue;
                inven.UpdateBombCount();
                inven.numKeys = int.MaxValue;
                inven.UpdateKeyCount();
                
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
        //animations.Play();
        GetComponent<Movement>().movement_speed = 0;
       
        
        //yield  WaitForSecondsRealtime(0.1f);
        
        GetComponent<Movement>().movement_speed = 4;  
        health = MaxHP;
        UpdateHP();
        yield return null;
    }

    void OnCollisionEnter(Collision c){
        Debug.Log("collide");
        
        if(c.gameObject.tag == DamageFromTag){
            Debug.Log("hit");
            StartCoroutine(hit(c.transform.position));
        }

        if(health <= 0){
            Movement mov = GetComponent<Movement>();
            mov.cam.transform.position = new Vector2(39.4778f,70.975f);
            Death();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    IEnumerator hit(Vector3 collider){

        Debug.Log("hit");
         if(!Invincible && Time.frameCount > lastHurtFrame + 12){
            Vector2 finalPos = gameObject.transform.position;

            float xDif = collider.x - finalPos.x;
            float yDif = collider.y - finalPos.y;

            //Vector2 finalPos = collider.transform.position;
            if(Mathf.Abs(xDif) < Mathf.Abs(yDif)){ // so above or below
                if(yDif > 0){
                    finalPos.y -= 2;
                } else {
                    finalPos.y += 2;
                }
            } else { //ydif == 0
                if(xDif > 0){
                    finalPos.x -= 2;
                } else {
                    finalPos.x += 2;
                }
            }

            health--; 
            UpdateHP();
            GetComponent<Movement>().movement_speed = 0;
            
            //animations.Play();
            Debug.Log("hit2");
            Rigidbody rb = GetComponent<Rigidbody>();
            
            //animations.Play("linkHurt");
            yield return StartCoroutine(CoroutineUtilities.MoveObjectOverTime(rb.transform, gameObject.transform.position, finalPos, 0.2f));
            
            DamageFromTag = "Respawn";
            for(int i = 0; i < 30; ++i){
                SRenderer.color = new Color(0,255,255,255);
                yield return new WaitForSecondsRealtime(0.01f);
                SRenderer.color = new Color(255,255,255,255);
            }
            DamageFromTag = "Enemy";
            GetComponent<Movement>().movement_speed = 4;
            lastHurtFrame = Time.frameCount;
            
        }

        yield return null;
    }


    


}
