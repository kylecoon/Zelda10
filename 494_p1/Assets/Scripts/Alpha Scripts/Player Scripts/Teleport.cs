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

    void OnTriggerEnter(Collider other) {
        // bool fade = true;
        //StartCoroutine(UIcontroller.fadeToBlack(3,true, BlackSquare));

        if(other.CompareTag("Player")){
            //other.transform.
            Movement move = other.GetComponent<Movement>();
            move.Flip_CanMove();
            StartCoroutine(fadeToBlack(fadeSpeed,true, BlackSquare));
            other.transform.position = player2Position;//new Vector3(19.11f,76.18f,0);
            cam.transform.position = cam2Position; //new Vector3();
            StartCoroutine(fadeToBlack(fadeSpeed,false, BlackSquare));
            move.Flip_CanMove();

        } /*else if(!IsEntrance && other.CompareTag("Player")){

            StartCoroutine(UIcontroller.fadeToBlack(3,true, BlackSquare));
            other.transform.position = new Vector3(6f + 16f,3f + 55f,0); // need to change
            cam.transform.position = new Vector3();
            StartCoroutine(UIcontroller.fadeToBlack(3,false, BlackSquare));
        }
        */
    }

    
        public static IEnumerator fadeToBlack(int fadeSpeed, bool fade2Black, GameObject BlackSquare){
        
        Color objectColor = new Color(0, 0, 0, 255);
        float fadeAmount;

        if(fade2Black){
            Debug.Log("fade");
            while(BlackSquare.GetComponent<UnityEngine.UI.Image>().color.a < 1){
                Debug.Log("black");
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackSquare.GetComponent<UnityEngine.UI.Image>().color = objectColor;
                yield return null;
            }
        } else {
            Debug.Log("fadeBack");
            while(BlackSquare.GetComponent<UnityEngine.UI.Image>().color.a > 0){
                Debug.Log("white");
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackSquare.GetComponent<UnityEngine.UI.Image>().color = objectColor;
                yield return null;

            }
        }
        yield return new WaitForSeconds(1);
    }
    
}


