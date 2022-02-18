using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFull : MonoBehaviour
{
    //设置SpriteRender和摄像机的距离
    public float distance = 1.0f;
    //注意：Sprite是Cam的子物体
    private SpriteRenderer spriteRender = null;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        spriteRender.material.renderQueue = 2980;//这段代码非常重要，必须要加上，否则透明的渲染层级会出错
        if (cam != null)
        {
            cam.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        FixSpriteSize();
    }

    //自适应Sprite的Size
    public void FixSpriteSize()
    {
        if(cam == null)
        {
            return;
        }

        float width = spriteRender.sprite.bounds.size.x;
        float height = spriteRender.sprite.bounds.size.y;

        float worldScreenWidth, worldScreenHeight;
        if(cam.orthographic)
        {
            //正交摄像机
            worldScreenHeight = cam.orthographicSize * 2;
            worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
            Debug.Log("worldScreenHeight = " + worldScreenHeight.ToString() + " worldScreenWidth = " + worldScreenWidth.ToString());
        }
        else
        {
            //投影相机
            worldScreenHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            worldScreenWidth = worldScreenHeight * cam.aspect;
        }
        transform.localPosition = new Vector3(0, 0, distance);
        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 0);
    }
}
