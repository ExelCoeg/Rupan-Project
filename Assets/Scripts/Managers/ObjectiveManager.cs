using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class ObjectiveManager: SingletonMonoBehaviour<ObjectiveManager>{
    public bool isDebug;
    [SerializeField] private  List<Objective> objectives;
    public Objective currentObjective;
    public int currentObjectiveIndex = 0;
    public PlayerInputActions playerInputActions;
    [Header("CHEATS")]
    public InputAction nextObjectiveAction;
    public override void Awake() {
      
        base.Awake();
        playerInputActions = new PlayerInputActions();

    }
    private void OnEnable() {
        nextObjectiveAction = playerInputActions.Cheats.NextObjective;
        nextObjectiveAction.performed += ctx => NextObjective();
    }
    private void OnDisable() {
        nextObjectiveAction.Disable();
    }
    public void Init() {
        SpawnObjective();
    }
    private void Start() {
       Init();
    }
    private void Update() {
        if(isDebug){
            nextObjectiveAction.Enable();
        }
        else{
            nextObjectiveAction.Disable();
        }
    }
    public void SpawnObjective(){
        currentObjective = Instantiate(objectives[currentObjectiveIndex],Vector3.zero,Quaternion.identity,transform).GetComponent<Objective>();
    }
    public void NextObjective(){
        print("to next objective");
        currentObjective.CompleteObjective();
        currentObjectiveIndex++;
        SpawnObjective();
    }
}