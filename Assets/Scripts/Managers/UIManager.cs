using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public UI currentUI;
    public bool enableCursor =true;
    [Space]
    [Header("UI Prefabs")]
    public UIPause uiPausePrefab;
    public UIInteract uiInteractPrefab;
    public UIObjectiveTexts uiObjectiveTextsPrefab;
    public UIBook uiBookPrefab;
    public UIBlackScreen uiBlackScreenPrefab;
    public UIUse uiUsePrefab;

    //------ScriptReferences----
    [Header("UI References")]
    public UIPause uiPause;
    public UIInteract uiInteract;
    public UIObjectiveTexts uiObjectiveTexts;
    public UIBook uiBook;
    public UIBlackScreen uiBlackScreen;
    public UIUse uiUse;

    [Header("Canvas")]
    public Transform canvas;
    private void Start() {
        uiObjectiveTexts = Instantiate(uiObjectiveTextsPrefab,canvas);
        uiInteract = Instantiate(uiInteractPrefab,canvas);
        uiPause = Instantiate(uiPausePrefab,canvas);
        uiBook = Instantiate(uiBookPrefab,canvas);
        uiBlackScreen = Instantiate(uiBlackScreenPrefab,canvas);
        uiUse = Instantiate(uiUsePrefab,canvas);
        uiUse.Hide();
        uiBook.Hide();
        uiObjectiveTexts.Hide();
        uiInteract.Hide();
        uiPause.Hide();
    }
    private void Update() {
        Cursor.visible = enableCursor;
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

    public void HideUIForCutscene(){
        uiObjectiveTexts.Hide();
        uiInteract.Hide();
        enableCursor = false;

    }
    public void ShowUIForCutscene(){
        uiObjectiveTexts.Show();
        uiInteract.Show();
        enableCursor = true;
    }
    public void UpdateObjectiveTexts(string mainText){
        uiObjectiveTexts.objectiveText.text = mainText;
    }

}



public enum UI{
    PAUSE,
    GAMEPLAY
}
