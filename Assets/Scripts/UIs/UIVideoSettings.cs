using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVideoSettings : UIBase
{
    public Button lowButton;
    public Button mediumButton;
    public Button highButton;
    public List<Button> buttons;

    private void Start() {
        buttons.Add(lowButton);
        buttons.Add(mediumButton);
        buttons.Add(highButton);
        SetQualityLevel(PlayerPrefs.GetInt("qualityLevel",2));
        lowButton.onClick.AddListener(()=> {
            SetQualityLevel(0);
        });
        mediumButton.onClick.AddListener(()=> {
            SetQualityLevel(1);
        });
        highButton.onClick.AddListener(()=> {
            SetQualityLevel(2);
        });
    }

    public void SetQualityLevel(int level){
        switch(level){
            case 0:
                lowButton.interactable = false;
                break;
            case 1:
                mediumButton.interactable = false;
                break;
            case 2:
                highButton.interactable = false;
                break;
        }
        SetNotInteractable(level);
        QualitySettings.SetQualityLevel(level);
        
    }
    public void SetNotInteractable(int currentButton){
        for(int i=0;i<buttons.Count;i++){
            if(i!=currentButton){
                buttons[i].interactable = true;
            }
        }
    }
}
