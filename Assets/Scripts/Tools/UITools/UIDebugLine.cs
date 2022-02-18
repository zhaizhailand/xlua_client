using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UIDebugLine : MonoBehaviour
{
    static Vector3[] fourCorners = new Vector3[4];
    void OnDrawGizmos()
    {
        //Debug.Log("invoke OnDrawGizmos");
        foreach(MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
        {
            if(g.raycastTarget)
            {
                g.rectTransform.GetWorldCorners(fourCorners);
                Gizmos.color = Color.blue;
                for(int i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                }
            }
        }
    }

    /*void OnDrawGizmosSelected()
    {
        Debug.Log("invoke OnDrawGizmosSelected");
    }*/
}
