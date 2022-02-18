/***
*
*	Title:"AB框架"项目
*       框架内部验证测试
*
*	Description:
*           功能：
*               测试SingleABLoader
*
*	Author: Zhaiyurong
*
*	Date: 2022.2
*
*	Modify:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AssetBundleFramework;

public class TestClass_SingleABLoader : MonoBehaviour
{
    //引用类
    private SingleABLoader _Loader = null;

    //AB包名称
    private string _ABName1 = "common_ui/prefab.ab";
    //AB包中资源名称
    private string _AssetName1 = "uilogin.prefab";

    public Transform parent;


    #region 简单（无依赖包）预制加载
    // Start is called before the first frame update
    void Start()
    {
        _Loader = new SingleABLoader(_ABName1, OnABDownLoadComplete);
        StartCoroutine(_Loader.DownLoadAssetBundle());
    }

    /// <summary>
    /// AB包加载完毕
    /// </summary>
    /// <param name="abName"></param>
    private void OnABDownLoadComplete(string abName)
    {
        //Debug.Log("回调函数 abName = " + abName);
        //加载AB包中的资源
        UnityEngine.Object tempObj = _Loader.LoadAsset(_AssetName1, false);
        Instantiate(tempObj, parent);
        
        //查询包中的资源
        string[] strArray = _Loader.RetrivalAllAssetNames();
        foreach(string str in strArray)
        {
            Debug.Log(str);
        }
    }
    #endregion

}
