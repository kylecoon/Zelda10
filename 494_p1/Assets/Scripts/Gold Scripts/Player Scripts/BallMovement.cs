using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 current_direction;
    // Start is called before the first frame update
    void OnEnable()
    {
        current_direction = GetComponent<FormController>().direction_controller;
    }

    
    void OnDisable()
    {
        GetComponent<FormController>().direction_controller = current_direction;
    }

    public Vector2 GetCurrentDirection() {
        return current_direction;
    }
}
