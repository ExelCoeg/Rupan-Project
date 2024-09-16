using UnityEngine;
using System;
public class Objective : MonoBehaviour {
    public bool isDebug;
    public string mainText;
    public bool isComplete;

    public virtual void Update(){
        UIManager.instance.UpdateObjectiveTexts(mainText);
    }
    public void CompleteObjective(){
        isComplete = true;
        Destroy(gameObject);
    }
  
}