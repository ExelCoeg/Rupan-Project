public class UIBook : UIBase
{
   
    public override void Show()
    {
        base.Show();
        UIManager.instance.DisableCursor();
        GameManager.instance.player.DisableCamera();
        GameManager.instance.player.DisableMove();
        
    }
    public override void Hide()
    {
        UIManager.instance.EnableCursor();
        GameManager.instance.player.EnableCamera();
        GameManager.instance.player.EnableMove();
        base.Hide();
    }
}
