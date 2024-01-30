using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
