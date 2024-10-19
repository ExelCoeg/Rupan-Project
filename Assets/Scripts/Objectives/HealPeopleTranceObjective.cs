
using UnityEngine;

public class HealPeopleTranceObjective : Objective
{
    [SerializeField] private int healPeopleTarget;
    public override void Update() {
        base.Update();  
        UIManager.instance.UpdateObjectiveTexts("Help the people that are possesed!");
        if (NPCManager.instance.GetHealedNPC() >= healPeopleTarget)
        {
            base.CompleteObjective();
        }
    }
}
