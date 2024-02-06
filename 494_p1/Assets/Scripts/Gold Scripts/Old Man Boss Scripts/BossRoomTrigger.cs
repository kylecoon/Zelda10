using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject door;

    public Sprite doorClosed;

    public GameObject boss;

    public GameObject cover;

    public AudioClip doorClose;

    public GameObject cammy;

    private bool triggered = false;

    public GameObject[] darkness;
    // Start is called before the first frame update

    IEnumerator removeDarkeness(){

        triggered = true;
        Camera cam = cammy.GetComponent<Camera>();
        cam.GetComponent<AudioSource>().enabled = false;
        gameObject.GetComponent<AudioSource>().enabled = true;

        door.GetComponent<SpriteRenderer>().sprite = doorClosed;
        door.GetComponent<BoxCollider>().enabled = true;
        AudioSource.PlayClipAtPoint(doorClose, Camera.main.transform.position);

        

        Color hold = cover.GetComponent<SpriteRenderer>().color;
        for(int i = 0; i < 5; ++i){
            cam.orthographicSize += 1;
            cam.transform.position = new Vector3(cam.transform.position.x - 1.125f, cam.transform.position.y, cam.transform.position.z);
            // hold.a -= 0.25f;
            yield return new WaitForSeconds(1);
        }
        darkness[0].SetActive(false);
        yield return new WaitForSeconds(1);
        darkness[1].SetActive(false);
        yield return new WaitForSeconds(1);
        darkness[2].SetActive(false);
        yield return new WaitForSeconds(1);

        boss.SetActive(true);

    }

    void OnTriggerExit(UnityEngine.Collider other){
        if(!triggered) StartCoroutine(removeDarkeness());
        
    }
    
}
