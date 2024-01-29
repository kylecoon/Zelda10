using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textMess;

    public string[] stringArr; 

    [SerializeField] float delay;

    [SerializeField] float WordDelay;
//     public TextMeshProUGUI messageObject;
//     //public GameObject test;
//     public float speed;
//     private string Message;

    bool Triggered = false;

//     private string currentMessage = "";
    int i = 0;
    // void Start(){
    //     // Message = this.te
    //     Endcheck();
    // }

    private IEnumerator ShowTest(){
        
        textMess.ForceMeshUpdate();
        int NumVisChars = textMess.textInfo.characterCount;
        int counter = 0;

        while(true){
            int visCount = counter % (NumVisChars + 1);
            textMess.maxVisibleCharacters = visCount;

            if(visCount >= NumVisChars){
                i++;
                Invoke("Endcheck", WordDelay);
                break;
            }

            counter += i;

            yield return new WaitForSeconds(delay);
        }
        // for(int i = 0; i < Message.Length; ++i){
        //     currentMessage = Message.Substring(0,i);
        //     messageObject.GetComponent<TextMeshProUGUI>().text = currentMessage;
        //     yield return new WaitForSeconds(speed);
        // }
        yield return null;
    }

    void Endcheck(){
        if(i <= stringArr.Length - 1){
            textMess.text = stringArr[i];
            StartCoroutine(ShowTest());
        }
    }

    void OnTriggerEnter(){
        Debug.Log("old man room triggered");
        if(!Triggered) Endcheck();
        Triggered = true;

        
    }
}