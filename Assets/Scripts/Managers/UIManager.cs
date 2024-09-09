using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    //------Prefabs---------
    public UIPause uiPausePrefab;

    //------ScriptReferences----
    public UIPause uiPause;


    public UI currentUI;
    private void Start() {
        uiPause = Instantiate(uiPausePrefab,transform);
        uiPause.Hide();
    }
    private void Update() {
        
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentUI == UI.PAUSE){
                HideUI(UI.PAUSE);
            }else{
                ShowUI(UI.PAUSE);
            }
        }
    }

    public void ShowUI(UI ui){
        if(currentUI == ui){
            return;
        }

        HideUI(currentUI);

        switch(ui){
            case UI.PAUSE:
                uiPause.Show();
                break;
        }

        currentUI = ui;
    }

    public void HideUI(UI ui){
        if(currentUI != ui){
            return;
        }

        switch(ui){
            case UI.PAUSE:
                uiPause.Hide();
                break;
        }
        currentUI = UI.GAMEPLAY;


    }
}


public enum UI{
    PAUSE,
    GAMEPLAY
}
