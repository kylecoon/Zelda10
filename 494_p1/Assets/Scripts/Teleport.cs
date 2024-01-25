using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Start is called before the first frame update

    //public bool IsEntrance;

    public GameObject cam;

    public GameObject BlackSquare;

    public Vector3 player2Position;

    public Vector3 cam2Position;

    public int fadeSpeed;

    //public UIcontroller uIcontroller;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(UnityEngine.Collider other) {
        // bool fade = true;
        //StartCoroutine(UIcontroller.fadeToBlack(3,true, BlackSquare));

        if(other.CompareTag("Player")){

            StartCoroutine(UIcontroller.fadeToBlack(fadeSpeed,true, BlackSquare));
            other.transform.position = player2Position;//new Vector3(19.11f,76.18f,0);
            cam.transform.position = cam2Position; //new Vector3();
            StartCoroutine(UIcontroller.fadeToBlack(fadeSpeed,false, BlackSquare));

        } /*else if(!IsEntrance && other.CompareTag("Player")){

            StartCoroutine(UIcontroller.fadeToBlack(3,true, BlackSquare));
            other.transform.position = new Vector3(6f + 16f,3f + 55f,0); // need to change
            cam.transform.position = new Vector3();
            StartCoroutine(UIcontroller.fadeToBlack(3,false, BlackSquare));
        }
        */
    }

    
}
