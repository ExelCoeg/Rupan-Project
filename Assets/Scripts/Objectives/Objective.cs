using UnityEngine;
using System;
public class Objective : MonoBehaviour {
    public bool isDebug;
    public string mainText;
    public string description;
    public bool isComplete;
    public event Action OnObjectiveComplete;

    public virtual void Update(){
        UIManager.instance.UpdateObjectiveTexts(mainText,description);
    }
    public void CompleteObjective(){
        isComplete = true;
        Destroy(gameObject);
        ObjectiveManager.instance.currentObjectiveIndex++;
    }
    public void OnEnable(){

    }
}