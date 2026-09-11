[ExecuteAlways]//全模式下都运行生命周期函数
public class CustomGUIRoot : MonoBehaviour
{
    private CustomGUIControl[] allControls;//存储所有GUI控件
    void Start()
    {
        allControls = this.GetComponentsInChildren<CustomGUIControl>();
    }
    private void OnGUI()
    {
        //每次绘制之前，都得到所有控件的父类脚本，浪费性能
        //添加条件，使其在编辑状态下才会执行，但会导致运行状态下无法实时增删控件
        //if(!Application.isPlaying)
        //{
        allControls = this.GetComponentsInChildren<CustomGUIControl>();
        //}
        for (int i = 0; i < allControls.Length; i++)
        {
            allControls[i].DrawGUI();
        }
    }
}
