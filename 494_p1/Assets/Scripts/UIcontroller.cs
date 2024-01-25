using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{

    public static IEnumerator fadeToBlack(int fadeSpeed, bool fade2Black, GameObject BlackSquare){
        
        Color objectColor = BlackSquare.GetComponent<Image>().color;
        float fadeAmount;

        if(fade2Black){
            while(BlackSquare.GetComponent<Image>().color.a < 1){
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        } else {
            while(BlackSquare.GetComponent<Image>().color.a < 0){
                 fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackSquare.GetComponent<Image>().color = objectColor;
                yield return null;

            }
        }
        //yield return null;
    }
}

