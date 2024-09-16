using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoBackToHouseObjective : Objective
{
    public Door door;
    private void Start() {
        door = GameObject.FindGameObjectWithTag("MainHouseDoor").GetComponent<Door>();
        door.AddComponent<GoBackToHouseDoor>();
        Destroy(door);
    }
    public override void Update() {
        base.Update();
    }
}