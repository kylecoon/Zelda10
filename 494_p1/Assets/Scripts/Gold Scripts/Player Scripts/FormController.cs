using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FormController : MonoBehaviour
{
    private int formID; // 1 = human, 2 = dong, 3 = ball, 4 = aqua, 5 = oldMan
    private int numForms;
    public Vector2 direction_controller;
    public bool can_move;
    private BoxCollider box;

    public bool in_knockback;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        
        can_move = true;
        formID = 1;

        numForms = 5;

        DeactivateComponents();
        gameObject.GetComponent<HumanMovement>().enabled = true;
        gameObject.GetComponent<HumanAttack>().enabled = true;

        direction_controller = Vector2.down;

        in_knockback = false;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput() {
        if (Input.GetKeyDown(KeyCode.Alpha1) && numForms >= 1) {
            formID = 1;

            DeactivateComponents();

            gameObject.GetComponent<HumanMovement>().enabled = true;
            gameObject.GetComponent<HumanAttack>().enabled = true;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && numForms >= 2) {
            formID = 2;

            DeactivateComponents();

            gameObject.GetComponent<DongMovement>().enabled = true;
            gameObject.GetComponent<DongAttack>().enabled = true;

        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && numForms >= 3) {
            formID = 3;

            DeactivateComponents();

            gameObject.GetComponent<BallMovement>().enabled = true;
            gameObject.GetComponent<BallAttack>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && numForms >= 4) {
            formID = 4;

            DeactivateComponents();

            gameObject.GetComponent<AquaMovement>().enabled = true;
            gameObject.GetComponent<AquaAttack>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && numForms >= 5) {
            formID = 5;

            DeactivateComponents();

            gameObject.GetComponent<OldManMovement>().enabled = true;
            gameObject.GetComponent<OldManAttack>().enabled = true;
        }
    }

    void DeactivateComponents() {
        gameObject.GetComponent<HumanMovement>().enabled = false;
        gameObject.GetComponent<HumanAttack>().enabled = false;

        gameObject.GetComponent<DongMovement>().enabled = false;
        gameObject.GetComponent<DongAttack>().enabled = false;

        gameObject.GetComponent<BallMovement>().enabled = false;
        gameObject.GetComponent<BallAttack>().enabled = false;

        gameObject.GetComponent<AquaMovement>().enabled = false;
        gameObject.GetComponent<AquaAttack>().enabled = false;

        gameObject.GetComponent<OldManMovement>().enabled = false;
        gameObject.GetComponent<OldManAttack>().enabled = false;
    }

    public int GetFormID() {
        return formID;
    }

    public int GetNumForms() {
        return numForms;
    }

    public void AddForm() {
        numForms += 1;
    }
}
