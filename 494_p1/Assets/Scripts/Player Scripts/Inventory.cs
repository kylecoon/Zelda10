using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int rupee_count = 0;
    public TextMeshProUGUI RupShow;
    // public int TrueCountRup = 0;

    public int numKeys = 0;
    public TextMeshProUGUI KeyShow;
    // public int numKeysTrue = 0;

    public int numBombs = 0;
    public TextMeshProUGUI BombShow;
    // public int numBombsTrue = 0;
    public int numArrows = 0;
    
    // Start is called before the first frame update
    public void AddRupees(int num_rupees) {
        rupee_count += num_rupees;
        UpdateRupCount();
        // TrueCountRup += num_rupees;
    }
    public void UpdateRupCount(){
        RupShow.text = "X " + rupee_count;
    }

    // Update is called once per frame
    public int GetRupees() {
        return rupee_count;
    }

    public void AddKey(){
        numKeys++;
        UpdateKeyCount();
    }

    public void UpdateKeyCount(){
        KeyShow.text = "X " + numKeys;
    }

    public void Addbombs(){
        numBombs++;
        UpdateBombCount();
    }

    public void UpdateBombCount(){
        BombShow.text = "X " + numBombs;
    }


}
