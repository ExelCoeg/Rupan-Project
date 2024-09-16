using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPeopleTranceObjective : Objective
{
    public override void Update() {
        base.Update();
        if (NPCManager.instance.GetUnHealedNPC() <= 0)
        {
            base.CompleteObjective();
        }
    }
}
