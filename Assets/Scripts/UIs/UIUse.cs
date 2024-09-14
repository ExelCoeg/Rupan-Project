using TMPro;

public class UIUse : UIBase
{
    public TextMeshProUGUI itemNameText;
    public void UpdateText(string itemName){
        itemNameText.text = "Press E to use " + itemName;
    }
}
