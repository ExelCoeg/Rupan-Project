using UnityEngine;
using DG.Tweening;
public class UIDialog : UIBase
{
    GameObject player;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().DisableControls();
    }
    public override void Show(){
        base.Show();
        Camera.main.GetComponent<FPSCamera>().DisableCamera();
        GetComponentInChildren<RectTransform>().DOAnchorPosY(300, 0.5f).SetEase(Ease.OutBack);
    }
    public override void Hide()
    {
        UIManager.instance.ShowUIForCutscene();
        player.GetComponent<Player>().EnableControls();
        Camera.main.GetComponent<FPSCamera>().EnableCamera();
        GetComponentInChildren<RectTransform>().DOAnchorPosY(-300, 0.5f).SetEase(Ease.InBack).OnComplete(()=>base.Hide());  
    }
}
