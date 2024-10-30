using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchRings : Objective
{
    public int ringsToFind;
    public int ringsFound;

    private void Start() {
        GameManager.instance.player.EnableSprint();
    }
    public override void Update() {
        base.Update();
        mainText = "Collected rings " + ringsFound + "/" + ringsToFind;
        
        if(ringsFound >= ringsToFind){
            CompleteObjective();
        }
    }
}
