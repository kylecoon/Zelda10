using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sword : MonoBehaviour
{
    public BoxCollider hitbox;
    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider>();
        hitbox.enabled = false;
    }

    public void UpdateDirection(Vector2 new_direction) {
        transform.localPosition = new_direction * 0.8f;
        if (new_direction == Vector2.left || new_direction == Vector2.right) {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
