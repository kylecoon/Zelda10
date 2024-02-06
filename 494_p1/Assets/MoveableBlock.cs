using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour
{
    public bool reset = true;

    public bool TrueReset; // reset even if in place

    private Vector2 origin;
    // Start is called before the first frame update
    void Start(){
        origin = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetPosition(){
        if(reset || TrueReset){
            gameObject.transform.position = origin;
        }
    }
}
