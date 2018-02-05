using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour {
    public Activator[] activators;
    public GameObject target;


    public bool IsMet() {
        bool result = true;
        foreach(Activator activator in activators) {
            result = activator.Test();
            if (result == false) break;
        }
        return result;
    }
}
