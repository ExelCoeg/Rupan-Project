using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourceObjective : Objective
{
    public Window window;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            // timelne watching the window
        }
    }
}
