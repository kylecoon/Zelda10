using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blink : MonoBehaviour
{
    public Sprite sprt1;

    public Sprite sprt2;

    //private int curSprite = 0;

    private SpriteRenderer sprt;
    // Start is called before the first frame update
    void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 12 > 6){
            sprt.sprite = sprt1;
        } else {
            sprt.sprite = sprt2;
        }
        //curSprite++;
    }
}
