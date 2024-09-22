using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISprintBar : UIBase
{
    public Slider staminaBarLeft;
    public Slider staminaBarRight;
    public Image borderImage;
    Player player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        staminaBarLeft.value  = staminaBarRight.value = player.currentStamina/player.maxStamina;
    }

    public override void Show()
    {
        base.Show();
        staminaBarLeft.fillRect.GetComponent<Image>().DOFade(1, 0.5f);
        staminaBarRight.fillRect.GetComponent<Image>().DOFade(1, 0.5f);
        borderImage.DOFade(0.5f, 0.5f);
    }
    public override void Hide()
    {
        staminaBarLeft.fillRect.GetComponent<Image>().DOFade(0, 0.5f);
        staminaBarRight.fillRect.GetComponent<Image>().DOFade(0, 0.5f);
        borderImage.DOFade(0, 0.5f);
        // base.Hide();
    }
}
