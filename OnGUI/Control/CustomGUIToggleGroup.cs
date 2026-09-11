public class CustomGUIToggleGroup : MonoBehaviour//单选框
{
    public CustomGUIToggle[] toggles;
    private CustomGUIToggle frontTurTog;//记录上一次为true的Toggle
    void Start()
    {
        if (toggles.Length == 0)
        {
            return;
        }
        for (int i = 0; i < toggles.Length; i++)
        {
            CustomGUIToggle toggle = toggles[i];
            toggle.changeValue += (value) =>
            {
                if(value)//当传入的值是ture时 需要把另外几个变成false
                {
                    for (int j = 0; j < toggles.Length; j++)
                    {
                        if( toggles[j] != toggle )
                        {
                            toggles[j].isSel = false;
                        }
                    }
                    frontTurTog = toggle;
                }
                else if( toggle == frontTurTog)//防取消，保证始终有一个选项被激活
                {
                    toggle.isSel = true;
                }
            };
        }
    }
}
