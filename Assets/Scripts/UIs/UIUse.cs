using TMPro;

public class UIUse : UIBase
{
    public TextMeshProUGUI itemNameText;
    private void Update() {
        if(FindObjectOfType<Player>().currentPickupableObject != null){
            itemNameText.gameObject.SetActive(true);
        }
        else{
            itemNameText.text = "";
            itemNameText.gameObject.SetActive(false);
        }
    }
    public void UpdateText(string itemName){
        itemNameText.text = "Press E to use " + itemName;
    }
}
