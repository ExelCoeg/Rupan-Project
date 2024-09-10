using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public UI currentUI;
    [Space]
    [Header("UI Prefabs")]
    public UIPause uiPausePrefab;
    public UIInteract uiInteractPrefab;
    public UIObjectiveTexts uiObjectiveTextsPrefab;

    //------ScriptReferences----
    [Header("UI References")]
    public UIPause uiPause;
    public UIInteract uiInteract;
    public UIObjectiveTexts uiObjectiveTexts;

    [Header("Canvas")]
    public Transform canvas;
    private void Start() {
        uiObjectiveTexts = Instantiate(uiObjectiveTextsPrefab,canvas);
        uiInteract = Instantiate(uiInteractPrefab,canvas);
        uiPause = Instantiate(uiPausePrefab,canvas);
        // uiObjectiveTexts.Hide();
        uiInteract.Hide();
        uiPause.Hide();
    }
    private void Update() {
        Cursor.visible = true;
        if(currentUI == UI.GAMEPLAY){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }


        if(Input.GetKeyDown(KeyCode.Escape)){
            if(currentUI == UI.PAUSE){
                HideUI(UI.PAUSE);
                GameManager.instance.ResumeGame();
            }else{
                GameManager.instance.PauseGame();
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
    public void UpdateObjectiveTexts(string mainText, string description){
        uiObjectiveTexts.objectiveText.text = mainText;
        uiObjectiveTexts.objectiveDescription.text = description;
    }
}



public enum UI{
    PAUSE,
    GAMEPLAY
}
