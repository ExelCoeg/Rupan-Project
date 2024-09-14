using System;
public class SitCouchObjective : Objective
{
    public static event Action onSitCouch; 
    public static void SitOnCouch(){
        onSitCouch?.Invoke();
    }
    public void OnEnable(){
        onSitCouch += CompleteObjective;
    }

    public void OnDisable(){
        onSitCouch -= CompleteObjective;
    }

}
