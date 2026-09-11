public class CustomGUITexture : CustomGUIControl
{
    public ScaleMode scaleMode = ScaleMode.StretchToFill;//图片的缩放模式
    protected override void StyleOffDraw()
    {
        GUI.DrawTexture(guiPos.Pos, content.image, scaleMode);
    }
    protected override void StyleOnDraw()
    {
        GUI.DrawTexture(guiPos.Pos, content.image, scaleMode);
    }
}
