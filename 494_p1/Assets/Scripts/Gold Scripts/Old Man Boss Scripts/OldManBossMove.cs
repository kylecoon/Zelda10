using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManBossMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveBackAndForth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator moveBackAndForth(){

        Vector2 start = gameObject.transform.position;
        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2), 2.5f));

        while(!GetComponent<oldManBosHealth>().isDead){

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 4), 2.5f));

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 4), 2.5f));

        }

        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(gameObject.transform, gameObject.transform.position, start, 2.5f));


        yield return null;
    }
}
