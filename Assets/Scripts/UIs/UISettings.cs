using System.Collections.Generic;
using UnityEngine.UI;
public class UISettings : UIBase
{
    public List<NavigationButton> navigationButtons;
    public int currentButtonIndex = 0;
    public Button backButton;
    public Button closeButton;
    public UIAudioSettings uiAudioSettings;
    public UIVideoSettings uiVideoSettings;
    private void Start() {
        navigationButtons[0].GetComponent<Button>().onClick.AddListener(()=> {
            ChangeSettingMenu(0);
        });
        navigationButtons[1].GetComponent<Button>().onClick.AddListener(()=> {
            ChangeSettingMenu(1);
        });

        backButton.onClick.AddListener(()=> {
            UIManager.instance.ShowUI(UI.PAUSE);
        });
        closeButton.onClick.AddListener(()=> {
            GameManager.instance.ResumeGame();
            UIManager.instance.ShowUI(UI.GAMEPLAY);
        });
    }

    
#region Enable Disable
    public void DisableCurrentSelectedButton(){
        navigationButtons[currentButtonIndex].selectedImageOne.SetActive(false);
        navigationButtons[currentButtonIndex].selectedImageTwo.SetActive(false);
    }
    public void EnableCurrentSelectedButton(){
        navigationButtons[currentButtonIndex].selectedImageOne.SetActive(true);
        navigationButtons[currentButtonIndex].selectedImageTwo.SetActive(true);
    }
    public void OnEnable(){
        EnableCurrentSelectedButton();
    }
    public void OnDisable(){
        ChangeSettingMenu(0);
    }
#endregion 
#region Show Hide
    public void ChangeSettingMenu(int index){
        // DisableCurrentSelectedButton's selected image
        DisableCurrentSelectedButton();
        ShowSettingMenu(currentButtonIndex);
        
        currentButtonIndex= index;

        EnableCurrentSelectedButton();

        // Enable this button's selected image
        HideSettingMenu(currentButtonIndex);

    }
    public void ShowSettingMenu(int index){
        switch(index){
            case 0:
                uiAudioSettings.Show();
                break;
            case 1:
                uiVideoSettings.Show();
                break;
        }
    }
    public void HideSettingMenu(int index){
        switch(index){
            case 0:
                uiAudioSettings.Hide();
                break;
            case 1:
                uiVideoSettings.Hide();
                break;
        }
    }
#endregion
}
