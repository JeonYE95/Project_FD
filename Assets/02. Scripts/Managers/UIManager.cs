using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : SingletonDontDestory<UIManager>
{
    private Dictionary<string, UIBase> _uiDic = new Dictionary<string, UIBase>();

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            OpenUI<UIStart>();
        }
    }

    // 딕셔너리 체크 후 있다면 리턴, 없다면 새로 생성
    public T GetUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;

        if (IsExist<T>())
            return _uiDic[uiName] as T;
        else
            return CreateUI<T>();
    }

    private T CreateUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;
        
        T uiRes = Resources.Load<T>($"UI/{uiName}");
        var uiObj = Instantiate(uiRes);

        if (IsExist<T>())
            _uiDic[uiName] = uiObj;
        else
            _uiDic.Add(uiName, uiObj); 

        return uiObj;
    }

    // UI 일괄처리 시 유용
    public T OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui.Open();

        return ui;
    }

    public T CloseUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui.Close();

        return ui;
    }

    public bool IsExist<T>()
    {
        var uiName = typeof(T).Name;
        return _uiDic.ContainsKey(uiName) && _uiDic[uiName] != null;
    }

    // 씬 전환 시 사용하면 좋다.
    public void Clear()
    {
        _uiDic.Clear();
    }
}
