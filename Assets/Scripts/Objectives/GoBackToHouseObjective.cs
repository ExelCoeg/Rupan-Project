using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToHouseObjective : Objective
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(isDebug){
                Debug.Log("Player entered the house");
            }
            CompleteObjective();
        }
    }
}