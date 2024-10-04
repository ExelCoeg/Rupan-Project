using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : UIBase
{
    public Button resumeButton;
    public Button SettingsButton;
    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(()=> {
            GameManager.instance.ResumeGame();
            UIManager.instance.HideUI(UI.PAUSE);
        });
        SettingsButton.onClick.AddListener(()=> {
            UIManager.instance.ShowUI(UI.SETTINGS);
        });
    }
}
