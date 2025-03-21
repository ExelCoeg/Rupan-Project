using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourceObjective : Objective
{
    public Window window;
    private void Start() {
        window.enabled = true;
    }
    public override void Update() {
        base.Update();
        if(window.interacted){
            CompleteObjective();
        }
    }
}
