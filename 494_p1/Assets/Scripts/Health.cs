using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public string DamageFromTag;

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

    IEnumerator Death(){

        
        return null;
    }

    


}
