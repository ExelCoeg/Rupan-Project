using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToHouseObjective : Objective
{
    public override void Update() {
        base.Update();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(isDebug){
                Debug.Log("Player entered the house");
            }
            CompleteObjective();
        }
    }
}