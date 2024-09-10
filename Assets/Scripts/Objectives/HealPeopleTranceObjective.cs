using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPeopleTranceObjective : Objective
{
    // public List<HealPeople> people;
    public int peopleToHeal = 11;
    private int peopleHealed = 0;
    // private void Start() {
        // foreach (HealPeople person in people)
        // {
        //     person.enabled = true;
        // }
    // }
    public void HealPerson(){
        peopleHealed++;
        if(peopleHealed >= peopleToHeal){
            CompleteObjective();
        }
    }
}
