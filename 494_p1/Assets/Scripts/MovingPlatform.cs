using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{

    public GameObject platform;

    public float finalX;

    public float finalY;

    public bool TwoWay;

    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("attack")){
            
            Vector3 initial_position = collider.transform.position;
            Vector3 final_position = new Vector3(initial_position.x + finalX, initial_position.y + finalY,0);

            StartCoroutine(movePlat(initial_position,final_position));
        }
    }

    IEnumerator movePlat(Vector3 initial, Vector3 final){

        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(platform.transform, initial, final, 1.0f));

        if(TwoWay){
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(platform.transform, final, initial, 1.0f));
        }


        yield return null;
    }


}
