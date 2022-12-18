using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool GOAL_MET = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Projectile")
        {
            Goal.GOAL_MET = true;
            Material mat = this.GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
