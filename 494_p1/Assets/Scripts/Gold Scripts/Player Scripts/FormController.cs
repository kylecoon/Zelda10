using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FormController : MonoBehaviour
{
    private int formID; // 1 = human, 2 = dong, 3 = ball, 4 = aqua, 5 = oldMan
    public int numForms;
    public Vector2 direction_controller;
    public bool can_move;
    private BoxCollider box;

    public bool wall_mode;
    public bool in_knockback;

    public GameObject[] newFormRevealer;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        
        can_move = true;
        formID = 1;

        numForms = 1;

        DeactivateComponents();
        gameObject.GetComponent<HumanMovement>().enabled = true;
        gameObject.GetComponent<HumanAttack>().enabled = true;

        direction_controller = Vector2.down;

        in_knockback = false;

        wall_mode = false;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput() {
        if (!can_move || wall_mode) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && numForms >= 1 && formID != 1) {
            formID = 1;

            DeactivateComponents();

            gameObject.GetComponent<HumanMovement>().enabled = true;
            gameObject.GetComponent<HumanAttack>().enabled = true;

            StartCoroutine(GreenFlash());

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && numForms >= 2 && formID != 2) {
            formID = 2;

            DeactivateComponents();

            gameObject.GetComponent<DongMovement>().enabled = true;
            gameObject.GetComponent<DongAttack>().enabled = true;

            StartCoroutine(GreenFlash());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && numForms >= 3 && formID != 3) {
            formID = 3;

            DeactivateComponents();

            gameObject.GetComponent<BallMovement>().enabled = true;

            StartCoroutine(GreenFlash());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && numForms >= 4 && formID != 4) {
            formID = 4;

            DeactivateComponents();

            gameObject.GetComponent<AquaMoveHuman>().enabled = true;
            gameObject.GetComponent<AquaAttack>().enabled = true;

            StartCoroutine(GreenFlash());
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && numForms >= 5 && formID != 5) {
            formID = 5;

            DeactivateComponents();

            gameObject.GetComponent<OldManMovement>().enabled = true;

            StartCoroutine(GreenFlash());
        }
    }

    void DeactivateComponents() {
        gameObject.GetComponent<HumanMovement>().enabled = false;
        gameObject.GetComponent<HumanAttack>().enabled = false;

        gameObject.GetComponent<DongMovement>().enabled = false;
        gameObject.GetComponent<DongAttack>().enabled = false;

        gameObject.GetComponent<BallMovement>().enabled = false;

        gameObject.GetComponent<AquaMoveHuman>().enabled = false;
        gameObject.GetComponent<AquaAttack>().enabled = false;

        gameObject.GetComponent<OldManMovement>().enabled = false;
    }

    public int GetFormID() {
        return formID;
    }

    public int GetNumForms() {
        return numForms;
    }

    public void AddForm() {
        numForms += 1;

        if(numForms == 0){
            newFormRevealer[0].SetActive(true);

        } else if(numForms == 1){
            newFormRevealer[1].SetActive(true);

        } else if(numForms == 2){
            newFormRevealer[2].SetActive(true);

        } else if(numForms == 3){
            newFormRevealer[3].SetActive(true);

        } else if(numForms == 4){
            newFormRevealer[4].SetActive(true);

        } else if(numForms == 5){
            newFormRevealer[5].SetActive(true);
        } 
    }

    IEnumerator GreenFlash() {
        GetComponent<SpriteRenderer>().color = new Color (0, 255, 0, 255);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color (255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color (0, 255, 0, 255);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color (255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color (0, 255, 0, 255);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color (255, 255, 255, 255);
    }

    public bool IsInWallMode() {
        return wall_mode;
    }
}
