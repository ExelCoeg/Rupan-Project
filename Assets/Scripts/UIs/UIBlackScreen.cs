using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
public class UIBlackScreen : UIBase
{
    public void Start(){
        GetComponent<Image>().DOFade(0,1f).OnComplete(()=>{
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.EnableControls();
            
            Hide();    
        });
    }
}
