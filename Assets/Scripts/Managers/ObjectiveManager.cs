using UnityEngine;
using System.Collections.Generic;



public class ObjectiveManager: SingletonMonoBehaviour<ObjectiveManager>{
    public List<Objective> objectives;
    public Objective currentObjective;
    public int currentObjectiveIndex = 0;
    public void Init() {
        SpawnObjective();
    }
    private void Start() {
       Init();
    }
    public void SpawnObjective(){
        currentObjective = Instantiate(objectives[currentObjectiveIndex],Vector3.zero,Quaternion.identity,transform);
    }
    public void NextObjective(){
        currentObjective.CompleteObjective();
        currentObjectiveIndex++;
        SpawnObjective();
        SoundManager.instance.PlaySound2D("ObjectiveUpdate");
    }
}