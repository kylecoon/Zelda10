using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

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
        if (coll.gameObject.TryGetComponent<WallmasterAI>(out WallmasterAI dummy)) {
            return;
        }
        if(coll.CompareTag(DamageFromTag)){
            if(health <= 1){
                health--;
                StartCoroutine(Death());
            }
            else {
                StartCoroutine(hit(coll.transform.position));
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

        if (c.gameObject.TryGetComponent<WallmasterAI>(out WallmasterAI dummy)) {
            return;
        }
        
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

            Vector2 direction;

            //Vector2 finalPos = collider.transform.position;
            if(Mathf.Abs(xDif) < Mathf.Abs(yDif)){ // so above or below
                if(yDif > 0){
                    direction = Vector2.down;
                } else {
                    direction = Vector2.up;
                }
            } else { //ydif == 0
                if(xDif > 0){
                    direction = Vector2.left;
                } else {
                    direction = Vector2.right;
                }
            }

            health--; 
            UpdateHP();
            GetComponent<Movement>().Flip_CanMove();
            GetComponent<Movement>().in_knockback = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = direction * 6.0f;

            SRenderer.color = new Color(255,0,0,255);
            Invincible = true;

            
            //animations.Play();
            Debug.Log("hit2");
            
            //animations.Play("linkHurt");
            yield return new WaitForSeconds(0.3f);
            
            GetComponent<Movement>().in_knockback = false;
            Invincible = false;
            SRenderer.color = new Color(255,255,255,255);
            GetComponent<Movement>().Flip_CanMove();
            
        }

        yield return null;
    }

    public void AlterHealth(int health_change) {

    } 

    


}
