using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ParticleClip : MonoBehaviour
{
    private RectTransform maskRectTrans;
    private List<Material> matList = new List<Material>();

    private Transform canvasTrans;

    private Vector4 area = new Vector4(0, 0, 0, 0);
    private float maskWidth = 0;
    private float maskHeight = 0;
    private float canvasScale = 0;

    void Awake()
    {
        var mask = this.transform.GetComponentInParent<Mask>();
        if(mask != null)
        {
            maskRectTrans = mask.GetComponent<RectTransform>();
        }
        var canvas = this.transform.GetComponentInParent<Canvas>();
        if(canvas != null)
        {
            this.canvasTrans = canvas.transform;
            this.canvasScale = this.canvasTrans.localScale.x;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(this.maskRectTrans == null || this.canvasTrans == null)
        {
            Debug.LogError("maskRectTrans null or canvasTrans null");
            return;
        }

        var renders = this.GetComponentsInChildren<ParticleSystemRenderer>();
        for (int i = 0; i < renders.Length ;i ++)
        {
            var render = renders[i];
            matList.Add(render.material);
        }

        var pivot = this.maskRectTrans.pivot;
        this.maskWidth = this.maskRectTrans.rect.width * this.canvasScale;
        this.maskHeight = this.maskRectTrans.rect.height * this.canvasScale;

        area.x = this.maskRectTrans.position.x - this.maskWidth * pivot.x;
        area.z = this.maskRectTrans.position.x + this.maskWidth * (1 - pivot.x);
        area.y = this.maskRectTrans.position.y - this.maskHeight * pivot.y;
        area.w = this.maskRectTrans.position.y + this.maskHeight * (1 - pivot.y);

        for(int i = 0; i < matList.Count; i ++)
        {
            this.matList[i].SetInt("_IsClip", 1);
            this.matList[i].SetVector("_Area", area);
        }
    }
   
}
