using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private BoxCollider box;
    private FormController form;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        form = GameObject.Find("Player").GetComponent<FormController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (form.IsInWallMode()) {
            box.enabled = false;
        }
        else {
            box.enabled = true;
        }
    }
}
