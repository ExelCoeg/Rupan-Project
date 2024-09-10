using UnityEngine;
public class Window : InteractableObject
{
    public override void Awake()
    {
        base.Awake();
    }
    public void Start(){
        enabled = false;
    }
    public override void Interacted()
    {
        //timeline watching the window
    }
}