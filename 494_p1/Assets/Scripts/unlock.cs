using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlock : MonoBehaviour
{

    public GameObject key;
    private Collider box;
    public Sprite open;

    private SpriteRenderer renderer;

    public bool primed = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        box = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!key.activeSelf){
            renderer.sprite = open;
            box.enabled = false;
        }
    }
}