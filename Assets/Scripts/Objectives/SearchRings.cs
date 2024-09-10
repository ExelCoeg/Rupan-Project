using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchRings : Objective
{
    public int ringsToFind;
    public int ringsFound;

    public override void Update() {
        base.Update();
        description = "Collected rings " + ringsFound + "/" + ringsToFind;
        
        if(ringsFound >= ringsToFind){
            CompleteObjective();
        }
    }
}
