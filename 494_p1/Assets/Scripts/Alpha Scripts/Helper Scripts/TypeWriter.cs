using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{

//     public TextMeshProUGUI textMess;

//     public string[] stringArr; 

//     public float delay;

//     public float WordDelay;
// //     public TextMeshProUGUI messageObject;
// //     //public GameObject test;
// //     public float speed;
// //     private string Message;

//     bool Triggered = false;

// //     private string currentMessage = "";
//     int i = 0;
//     // void Start(){
//     //     // Message = this.te
//     //     Endcheck();
//     // }

//     public IEnumerator ShowTest(){
        
//         textMess.ForceMeshUpdate();
//         int NumVisChars = textMess.textInfo.characterCount;
//         int counter = 0;

//         while(true){
//             int visCount = counter % (NumVisChars + 1);
//             textMess.maxVisibleCharacters = visCount;

//             if(visCount >= NumVisChars){
//                 i++;
//                 Invoke("Endcheck", WordDelay);
//                 break;
//             }

//             counter += i;

//             yield return new WaitForSeconds(delay);
//         }
//         // for(int i = 0; i < Message.Length; ++i){
//         //     currentMessage = Message.Substring(0,i);
//         //     messageObject.GetComponent<TextMeshProUGUI>().text = currentMessage;
//         //     yield return new WaitForSeconds(speed);
//         // }
//         yield return null;
//     }

//     void Endcheck(){
//         if(i <= stringArr.Length - 1){
//             textMess.text = stringArr[i];
//             StartCoroutine(ShowTest());
//         }
//     }

//     void OnTriggerEnter(){
//         Debug.Log("old man room triggered");
//         if(!Triggered) Endcheck();
//         Triggered = true;

        
//     }

    //all chatGPT below this point
    public TextMeshProUGUI textMeshPro;
    public float typingSpeed = 0.05f;

    public string[] sentences;
    private int currentSentenceIndex = 0;

    // private void Start(){

    //     StartCoroutine(TypeSentence(sentences[currentSentenceIndex]));
    // }

    private IEnumerator TypeSentence(string sentence)
    {
        textMeshPro.text = "";
        
        foreach (char letter in sentence)
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1f); // Optional pause after a sentence is complete

        NextSentence();
    }

    private void NextSentence()
    {
        currentSentenceIndex++;

        if (currentSentenceIndex < sentences.Length)
        {
            StartCoroutine(TypeSentence(sentences[currentSentenceIndex]));
        }
        else
        {
            Debug.Log("All sentences are displayed.");
            // You can perform any action or trigger a new event when all sentences are displayed.
        }
    }

    void OnTriggerExit(){
        StartCoroutine(TypeSentence(sentences[currentSentenceIndex]));
    }
}