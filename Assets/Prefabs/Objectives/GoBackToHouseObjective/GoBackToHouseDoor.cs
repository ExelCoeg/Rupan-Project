using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToHouseDoor : Door
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void Interacted(){
        base.Interacted();
        FindObjectOfType<GoBackToHouseObjective>().CompleteObjective();
    }

}
