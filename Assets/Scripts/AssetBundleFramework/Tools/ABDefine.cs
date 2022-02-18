/***
*
*	Title:"AB框架"项目
*
*
*	Description:
*           功能：
*               1：本框架项目所有的常量定义
*               2：所有的委托定义
*               3：所有的枚举定义
*
*	Author: Zhaiyurong
*
*	Date: 2022.2
*
*	Modify:
*
*/

namespace AssetBundleFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /*委托定义区*/
    //AB包下载完毕
    public delegate void DelABDownLoadComplete(string abName);

    /*枚举定义区*/

    public class ABDefine
    {
        /*框架常量区*/
        //Manifest文件资源名称
        public static string ASSETBUNDLE_MANIFEST = "AssetBundleManifest";

        //向下通知常量
        public static string ReceiveInfoStartRuning = "ReceiveInfoStartRuning";
    }
}


