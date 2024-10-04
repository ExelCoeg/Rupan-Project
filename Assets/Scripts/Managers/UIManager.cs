using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public UISprintBar uiSprintBarPrefab;
    public UIHitEffect uiHitEffectPrefab;
    public UISettings uiSettingsPrefab;

    //------ScriptReferences----
    [Header("UI References")]
    public UIPause uiPause;
    public UIInteract uiInteract;
    public UIObjectiveTexts uiObjectiveTexts;
    public UIBook uiBook;
    public UIBlackScreen uiBlackScreen;
    public UIUse uiUse;
    public UISprintBar uiSprintBar;
    public UIHitEffect uiHitEffect;
    public UISettings uiSettings;

    [Header("Canvas")]
    public Transform canvas;
    public PlayerInputActions inputActions;
    public InputAction pauseAction;
    private void Start() {
        uiObjectiveTexts = Instantiate(uiObjectiveTextsPrefab,canvas);
        uiInteract = Instantiate(uiInteractPrefab,canvas);
        uiBook = Instantiate(uiBookPrefab,canvas);
        uiUse = Instantiate(uiUsePrefab,canvas);
        uiSprintBar = Instantiate(uiSprintBarPrefab,canvas);
        uiHitEffect = Instantiate(uiHitEffectPrefab,canvas);
        uiPause = Instantiate(uiPausePrefab,canvas);
        uiSettings = Instantiate(uiSettingsPrefab,canvas);
        uiBlackScreen = Instantiate(uiBlackScreenPrefab,canvas);
        uiSettings.Hide();
        uiBlackScreen.Show();
        uiHitEffect.Hide();
        uiUse.Hide();
        uiBook.Hide();
        uiInteract.Hide();
        uiPause.Hide();
        uiSprintBar.Hide();
    }
    private void Update() {
        Cursor.visible = enableCursor;
        if(currentUI == UI.GAMEPLAY){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void ShowUI(UI ui){
        print("Showing UI: " + ui);
        if(currentUI == ui){
            return;
        }
        HideUI(currentUI);

        switch(ui){
            case UI.PAUSE:
                uiPause.Show();
                break;
            case UI.BOOK:
                uiBook.Show();
                break;
            case UI.SETTINGS:
                uiSettings.Show();
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
            case UI.BOOK:
                uiBook.Hide();
                break;
            case UI.SETTINGS:
                uiSettings.Hide();
                break;
        }
        currentUI = UI.GAMEPLAY;
    }

    public void HideUIForCutscene(){
        uiObjectiveTexts.Hide();
        uiInteract.Hide();
        uiUse.Hide();
        enableCursor = false;

    }
    public void ShowUIForCutscene(){
        uiObjectiveTexts.Show();
        uiInteract.Show();
        uiUse.Show();
        enableCursor = true;
    }
    
    public void UpdateObjectiveTexts(string mainText){
        uiObjectiveTexts.objectiveText.text = mainText;
    }
    public void DisableCursor(){
        enableCursor = false;
    }
    public void EnableCursor(){
        enableCursor = true;
    }
    public void OnEnable(){
        inputActions = new PlayerInputActions();
        pauseAction = inputActions.UI.Pause;
        pauseAction.Enable();
        pauseAction.performed += ctx => {
            if(currentUI == UI.BOOK){
                ShowUI(UI.GAMEPLAY);
            }
            else if(currentUI == UI.PAUSE){
                ShowUI(UI.GAMEPLAY);
                Cursor.lockState = CursorLockMode.Locked;
                GameManager.instance.ResumeGame();
            }
            else if(currentUI == UI.SETTINGS){
                ShowUI(UI.GAMEPLAY);
                GameManager.instance.ResumeGame();
            }
            else{
                GameManager.instance.PauseGame();
                ShowUI(UI.PAUSE);
            }
        };
    }
    public void PlayButtonClick(){
        SoundManager.instance.PlaySound2D("UI_ButtonClick");
    }
}



public enum UI{
    PAUSE,
    GAMEPLAY,
    BOOK,
    SETTINGS
}
