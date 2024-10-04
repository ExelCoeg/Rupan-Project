using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class UIBlackScreen : UIBase
{
    private int day = 1;
    public TextMeshProUGUI dayText;

    public override void Show(){
        ResetOpacity();
        base.Show();
        UIManager.instance.DisableCursor();
        dayText.text = "Day " + day;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetComponent<Image>().DOFade(0,2f));
        sequence.Join(dayText.DOFade(0,2f).OnComplete(()=>{
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.EnableControls();
            UIManager.instance.EnableCursor();
            Hide();    
        })); 
    }
    public void ResetOpacity(){
        GetComponent<Image>().color = new Color(0,0,0,1);
        dayText.color = new Color(1,1,1,1);
    }
    public void IncreaseDay(){
        day++;
    }
}
