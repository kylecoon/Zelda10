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
    public int health;

    public int MaxHP;
    public string DamageFromTag;

    public TextMeshProUGUI HPshown;

    //private GameObject parent; 

    void Start()
    {
        //parent = GetComponentInParent<>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Animation hurt = GetComponent<Animation>();
        hurt.Play("Die");
        GetComponent<Movement>().movement_speed = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GetComponent<Movement>().movement_speed = 4;  
        health = MaxHP;
        UpdateHP();
        return null;
    }

    void OnCollisionEnter(Collision collision){

        Debug.Log("hit");
        if(collision.transform.CompareTag(DamageFromTag)){
            hit(collision.transform);
        }

        if(health <= 0){
            Death();
        }
        
    }

    IEnumerator hit(Transform collider){

        Vector2 finalPos = collider.position;

        float xDif = finalPos.x - gameObject.transform.position.x;
        float yDif = finalPos.y - gameObject.transform.position.y;

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
        Animation hurt = GetComponent<Animation>();
        hurt.Play("linkHurt");
        Rigidbody rb = GetComponent<Rigidbody>();
        CoroutineUtilities.MoveObjectOverTime(rb.transform, gameObject.transform.position, finalPos, 0.3f);

        GetComponent<Movement>().movement_speed = 4;

        return null;
    }

    


}
