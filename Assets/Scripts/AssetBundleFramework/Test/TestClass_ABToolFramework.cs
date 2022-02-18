/***
*
*	Title:"AB框架"项目
*           框架内部验证测试
*
*	Description:
*            功能：框架整体验证测试
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

public class TestClass_ABToolFramework : MonoBehaviour
{
    //场景名称
    private string _SceneName = "common_ui";

    //AB包名称
    private string _AssetBundleName = "common_ui/prefab.ab";

    //资源名称
    private string _AssetName = "uilogin.prefab";

    public Transform parent;

    void Start()
    {
        StartCoroutine(AssetBundleMgr.GetInstance().DownLoadAssetBundlePack(_SceneName, _AssetBundleName, LoadAllABComplete));
    }

    //所有的AB包都已加载完毕
    private void LoadAllABComplete(string abName)
    {
        Debug.Log("所有的AB包都已加载完毕！ abName = " + abName);
        //提取资源
        UnityEngine.Object tmpObj = AssetBundleMgr.GetInstance().LoadAsset(_SceneName, _AssetBundleName, _AssetName, false);
        if(tmpObj != null)
        {
            Instantiate(tmpObj, parent);
        }
        else
        {
            Debug.LogError("提取资源失败！_AssetName = "+ _AssetName);
        }

    }

    //测试销毁资源
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AssetBundleMgr.GetInstance().DisposeAllAssets(_SceneName);
        }
    }

}
