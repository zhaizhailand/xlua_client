using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

public enum EnumControlComponent
{
    RectTransform,
    Image,
    RawImage,
    Text,
    OutLine,
    Gradient,
    Shadow
}

[Serializable]
public class StateContent
{
    public string m_Name = "";
    public bool enable = true;
    List<Component> list = new List<Component>();
    
    public StateContent(string name)
    {
        m_Name = name;
    }
    
    public void AddComponent(Component com)
    {
        list.Add(com);
    }
}

[Serializable]
public class CustomRectTransform
{

}

public class UIState : MonoBehaviour
{
    [SerializeField]
    public List<StateContent> allStates = new List<StateContent>();
    public RectTransform tran;
    public Image img;

    //µ±Ç°UI×´Ì¬
    public int curState = 0;
    
    public void AddState(string name)
    {
        allStates.Add(new StateContent(name));
    }

}
