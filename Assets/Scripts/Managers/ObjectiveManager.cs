using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class ObjectiveManager: SingletonMonoBehaviour<ObjectiveManager>{
    public List<Objective> objectives;
    public Objective currentObjective;
    public int currentObjectiveIndex = 0;
    public PlayerInputActions playerInputActions;
    public InputAction nextObjectiveAction;
    public override void Awake() {
        base.Awake();
        playerInputActions = new PlayerInputActions();

    }
    private void OnEnable() {
        nextObjectiveAction = playerInputActions.Cheats.NextObjective;
        nextObjectiveAction.performed += ctx => {
            NextObjective();    
        };
        nextObjectiveAction.Enable();
    }
    private void OnDisable() {
        nextObjectiveAction.performed -= ctx => {
            NextObjective();
        };
        nextObjectiveAction.Disable();
    }
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
        SoundManager.instance.PlaySound2D("ObjectiveUpdate");
        SpawnObjective();
    }
}