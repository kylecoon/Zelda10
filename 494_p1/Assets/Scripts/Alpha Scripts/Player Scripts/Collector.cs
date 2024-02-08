using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : MonoBehaviour
{
    Inventory inventory;

    public AudioClip RupCollect;
    public AudioClip keyCollect;

    public AudioClip BombCollect;
    public AudioClip HeartCollect;
    private bool adding_form;
    void Start()
    {
        adding_form = false;
        inventory = GetComponent<Inventory>();
        if (inventory == null) {
            Debug.LogWarning("WARNING: Gameobject with a collector has no inventory to store things in!");
        }
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider coll)
    {
        
        GameObject object_collided_with = coll.gameObject;

        Debug.Log("trigger");

        if (object_collided_with.CompareTag("rupee")) {
            if (inventory) {
                inventory.AddRupees(1);
            }
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(RupCollect, Camera.main.transform.position);
            
        } else if(object_collided_with.CompareTag("key")){
            
            inventory.AddKey();
            Destroy(object_collided_with);
            //Debug.Log("worked");
            AudioSource.PlayClipAtPoint(keyCollect, Camera.main.transform.position);

        } else if(object_collided_with.CompareTag("heart")){

            AudioSource.PlayClipAtPoint(HeartCollect, Camera.main.transform.position);

            Debug.Log("pickup heart");
            Health curHP = GetComponent<Health>();
            if(curHP.health < curHP.MaxHP) curHP.health++;
            Destroy(object_collided_with);
            curHP.UpdateHP();

        } else if(object_collided_with.CompareTag("bomb")){

        AudioSource.PlayClipAtPoint(keyCollect, Camera.main.transform.position);

            inventory.Addbombs();
            Destroy(object_collided_with);
            //Debug.Log("worked");
        }
        else if (object_collided_with.CompareTag("AltItem")) {

            AudioSource.PlayClipAtPoint(BombCollect, Camera.main.transform.position);

            GetComponent<Attacking>().AddAlt(object_collided_with.name);
            Destroy(object_collided_with);

        } else if(object_collided_with.CompareTag("Omni") || object_collided_with.CompareTag("Egg")){
            AudioSource.PlayClipAtPoint(BombCollect, Camera.main.transform.position);
            Destroy(object_collided_with);
            if (!adding_form) {
                adding_form = true;
                StartCoroutine(AddForm());
            }
        }

    }

    IEnumerator AddForm() {
        GetComponent<FormController>().AddForm();
        yield return new WaitForSeconds(1.0f);
        adding_form = false;
    }
}
