using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Reflection;

//引起网格重建元素打印
public class FliterUIRebuildElement : MonoBehaviour
{
    IList<ICanvasElement> m_LayoutRebuildQueue;
    IList<ICanvasElement> m_GraphicRebuildQueue;

    void Awake()
    {
        System.Type type = typeof(CanvasUpdateRegistry);
        FieldInfo field = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        m_LayoutRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);

        field = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
        m_GraphicRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < m_LayoutRebuildQueue.Count; i++)
        {
            var rebuild = m_LayoutRebuildQueue[i];
            if(ObjectValidForUpdate(rebuild))
            {
                Debug.LogFormat("{0} 引起网格重建", rebuild.transform.name);
            }
        }

        for (int i = 0; i < m_GraphicRebuildQueue.Count; i++)
        {
            var element = m_GraphicRebuildQueue[i];
            if (ObjectValidForUpdate(element))
            {
                Debug.LogFormat("{0} 引起{1}网格重建", element.transform.name, element.transform.GetComponent<Graphic>().canvas.name);
            }
        }
    }

    private bool ObjectValidForUpdate(ICanvasElement element)
    {
        var valid = element != null;

        var isUnityObject = element is Object;

        if (isUnityObject)
            valid = (element as Object) != null;

        return valid;
    }
}
