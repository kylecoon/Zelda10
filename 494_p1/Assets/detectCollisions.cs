using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollisions : MonoBehaviour
{

    public string[] tags2Ignore;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < tags2Ignore.Length; ++i){
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(tags2Ignore[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
