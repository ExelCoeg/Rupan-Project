using UnityEngine;
using System.Collections.Generic;



public class ObjectiveManager: SingletonMonoBehaviour<ObjectiveManager>{
    public List<Objective> objectives;
    public Objective currentObjective;
    public int currentObjectiveIndex = 0;
    public void Init() {
        Instantiate(objectives[currentObjectiveIndex],Vector3.zero,Quaternion.identity,transform);
    }
}