using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;
    // Start is called before the first frame update
    public void AddRupees(int num_rupees) {
        rupee_count += num_rupees;
    }

    // Update is called once per frame
    public int GetRupees() {
        return rupee_count;
    }
}
