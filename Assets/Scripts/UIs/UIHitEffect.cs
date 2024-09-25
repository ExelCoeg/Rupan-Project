using DG.Tweening;
using UnityEngine.UI;

public class UIHitEffect : UIBase
{
    public void Activate(){
        Show();
        Sequence sequence  = DOTween.Sequence();
        sequence.Append(GetComponent<Image>().DOFade(0.05f,0.1f));
        sequence.Append(GetComponent<Image>().DOFade(0f,0.25f).OnComplete(Hide));
    }

    
}
