using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPeopleTranceObjective : Objective
{
<<<<<<< Updated upstream
=======
    // public List<HealPeople> people;
    public int peopleToHeal = 10;
    private int peopleHealed = 0;
    // private void Start() {
        // foreach (HealPeople person in people)
        // {
        //     person.enabled = true;
        // }
    // }
>>>>>>> Stashed changes
    public override void Update() {
        base.Update();
        if (NPCManager.instance.GetUnHealedNPC() <= 0)
        {
            base.CompleteObjective();
        }
    }
}
