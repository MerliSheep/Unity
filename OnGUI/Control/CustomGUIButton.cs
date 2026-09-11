public class CustomGUIButton : CustomGUIControl
{
    public event UnityAction clickEvent;//提供给外部，用于响应按钮点击的事件
    protected override void StyleOffDraw()
    {
        if( GUI.Button(guiPos.Pos, content))
        {
            clickEvent?.Invoke();
        }
    }
    protected override void StyleOnDraw()
    {
        if (GUI.Button(guiPos.Pos, content, style))
        {
            clickEvent?.Invoke();
        }
    }
}
