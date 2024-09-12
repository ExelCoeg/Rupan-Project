using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorObjective :Objective
{
    public DialogueTrigger npc1DialogueTrigger;
    public override void Update()
    {   
        base.Update();
        if(npc1DialogueTrigger.isPlayed)
        {
            CompleteObjective();
        }
    }
    public void Destroy(){
        Destroy(gameObject);
    }
}
