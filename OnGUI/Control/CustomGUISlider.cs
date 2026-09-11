public enum E_Slider_Type//水平或竖直样式
{
    Horizontal,
    Vertical,
}
public class CustomGUISlider : CustomGUIControl
{
    public float minValue = 0;
    public float maxValue = 1;
    public float nowValue = 0;//当前值
    private float oldValue = 0;//上一次的值
    public E_Slider_Type type = E_Slider_Type.Horizontal;
    public GUIStyle styleThumb;//小按钮的样式
    public event UnityAction<float> changeValue;
    protected override void StyleOffDraw()
    {
        switch (type)
        {
            case E_Slider_Type.Horizontal:
                nowValue = GUI.HorizontalSlider(guiPos.Pos, nowValue, minValue, maxValue);
                break;
            case E_Slider_Type.Vertical:
                nowValue = GUI.VerticalSlider(guiPos.Pos, nowValue, minValue, maxValue);
                break;
        }
        if(oldValue != nowValue)
        {
            changeValue?.Invoke(nowValue);
            oldValue = nowValue;
        }
        
    }
    protected override void StyleOnDraw()
    {
        switch (type)
        {
            case E_Slider_Type.Horizontal:
                nowValue = GUI.HorizontalSlider(guiPos.Pos, nowValue, minValue, maxValue, style, styleThumb);
                break;
            case E_Slider_Type.Vertical:
                nowValue = GUI.VerticalSlider(guiPos.Pos, nowValue, minValue, maxValue, style, styleThumb);
                break;
        }
        if (oldValue != nowValue)
        {
            changeValue?.Invoke(nowValue);
            oldValue = nowValue;
        }
    }
}
