using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIDialog : UIBase
{
    GameObject player;
    [SerializeField] Button nextButton;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().DisableControls();
    }
    private void Update() {
        if(DialogueManager.instance.enableNextButton){
            nextButton.gameObject.SetActive(true);
        }
        else{
            nextButton.gameObject.SetActive(false);
        }
    }
    public override void Show(){
        base.Show();
        UIManager.instance.HideUIForCutscene();
        GameManager.instance.player.DisableCamera();
        GetComponentInChildren<RectTransform>().DOAnchorPosY(300, 0.5f).SetEase(Ease.OutBack);
    }
    public override void Hide()
    {
        if(DialogueManager.instance.enableUIAfterDialogue){
            UIManager.instance.ShowUIForCutscene();
        }
        player.GetComponent<Player>().EnableControls();
        GameManager.instance.player.EnableCamera();
        GetComponentInChildren<RectTransform>().DOAnchorPosY(-300, 0.5f).SetEase(Ease.InBack).OnComplete(()=>base.Hide());  
    }
}
