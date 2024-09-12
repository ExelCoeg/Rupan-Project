using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBook : UIBase
{
    public override void Show()
    {
        base.Show();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Hide();
        }
    }
    public override void Hide()
    {
        base.Hide();
    }
}
