public class PlayerPrefsDataMgr
{
    private static PlayerPrefsDataMgr instance = new PlayerPrefsDataMgr();
    public static PlayerPrefsDataMgr Instance => instance;
    private PlayerPrefsDataMgr(){}
    /// <param name="data">数据对象</param>
    /// <param name="keyName">数据对象的唯一key，自定义</param>
    public void SaveData( object data, string keyName )//存储数据
    {
        //通过Type得到传入数据对象的所有的字段，然后结合PlayerPrefs来进行存储
        Type dataType = data.GetType();
        FieldInfo[] infos = dataType.GetFields();//得到所有的字段
        string saveKeyName = "";
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            //自定义一个规则，keyName_数据类型_字段类型_字段名
            //通过FieldInfo可以直接获取到字段的类型 info.FieldType.Name 和字段的名字 info.Name
            saveKeyName = keyName + "_" + dataType.Name + 
                "_" + info.FieldType.Name + "_" + info.Name;
            SaveValue(info.GetValue(data), saveKeyName);//info.GetValue()取值
        }
        PlayerPrefs.Save();
    }
    private void SaveValue(object value, string keyName)
    {
        Type fieldType = value.GetType();
        //类型判断
        if (fieldType == typeof(int))
        {
            int rValue = (int)value;//数据加密
            rValue += 10;
            PlayerPrefs.SetInt(keyName, rValue);
        }
        else if (fieldType == typeof(float))
        {
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        else if (fieldType == typeof(string))
        {
            PlayerPrefs.SetString(keyName, value.ToString());
        }
        else if (fieldType == typeof(bool))
        {
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);//自定义一个存储bool的规则
        }
        else if(typeof(IList).IsAssignableFrom(fieldType))//判断是否为List<T>
        { 
            IList list = value as IList;//父类装子类 
            PlayerPrefs.SetInt(keyName, list.Count);//先存储数量
            int index = 0;
            foreach (object item in list)
            {
                SaveValue(item, keyName + index);//再递归存储具体的值
                ++index;
            }
        }
        else if( typeof(IDictionary).IsAssignableFrom(fieldType) )
        {
            IDictionary dic = value as IDictionary;
            PlayerPrefs.SetInt(keyName, dic.Count);
            int index = 0;
            foreach (object key in dic.Keys)
            {
                SaveValue(key, keyName + "_key_" + index);//分别存储键值对
                SaveValue(dic[key], keyName + "_value_" + index);
                ++index;
            }
        }
        else//自定义类型
        {
            SaveData(value, keyName);
        }
    }
    public object LoadData( Type type, string keyName )//读取数据
    {
        object data = Activator.CreateInstance(type);//根据传入的Type创建一个实例化对象，来存储数据
        FieldInfo[] infos = type.GetFields();//得到所有字段
        string loadKeyName = "";
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            loadKeyName = keyName + "_" + type.Name +
                "_" + info.FieldType.Name + "_" + info.Name; 
            info.SetValue(data, LoadValue(info.FieldType, loadKeyName));//info.SetValue()赋值
        }
        return data;
    }
    private object LoadValue(Type fieldType, string keyName)//读取单个数据
    {
        //根据 字段类型 判断 用哪个API来读取
        if( fieldType == typeof(int) )
        {
            return PlayerPrefs.GetInt(keyName, 0) - 10;//解密
        }
        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName, 0);
        }
        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyName, "");
        }
        else if (fieldType == typeof(bool))
        {
            return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;//根据自定义存储bool的规则来获取
        }
        else if(typeof(IList).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);//得到长度
            IList list = Activator.CreateInstance(fieldType) as IList;//实例化一个List对象来赋值
            for (int i = 0; i < count; i++)
            {
                list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));//通过第一个数据得到类型后再赋值
            }
            return list;
        }
        else if(typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
            Type[] kvType = fieldType.GetGenericArguments();//获取每组键值对的类型
            for (int i = 0; i < count; i++)
            {
                dic.Add(LoadValue(kvType[0], keyName + "_key_" + i),
                         LoadValue(kvType[1], keyName + "_value_" + i));
            }
            return dic;
        }
        else
        {
            return LoadData(fieldType, keyName);
        }

    }
}