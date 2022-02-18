/***
*
*	Title:"AB框架"项目
*           辅助类：读取AB包的manifest依赖文件
*
*	Description:
*           功能：加载manifest文件
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using UnityEngine.Networking;

    public class ABManifestLoader : System.IDisposable
    {
        //本类实例
        private static ABManifestLoader _Instance;

        //AsssetBundle清单文件
        private AssetBundleManifest _ManifestObj;

        //AssetBundle清单文件下载路径
        private string _StrManifestPath;

        //清单文件AB包
        private AssetBundle _ABManifest;

        //清单文件是否加载完成
        private bool _IsLoadFinish;
        public bool IsLoadFinish
        {
            get { return _IsLoadFinish; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private ABManifestLoader()
        {
            _StrManifestPath = PathTool.GetABDownLoadPath() + "/" + PathTool.GetPlatformName();
            _ManifestObj = null;
            _ABManifest = null;
            _IsLoadFinish = false;
        }

        /// <summary>
        /// 获取本类实例
        /// </summary>
        /// <returns></returns>
        public static ABManifestLoader GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new ABManifestLoader();
            }
            return _Instance;
        }

        //加载Manifest清单文件
        public IEnumerator DownLoadManifestFile()
        {
            using (UnityWebRequest webReq = UnityWebRequestAssetBundle.GetAssetBundle(_StrManifestPath))
            {
                yield return webReq.SendWebRequest();
                if (webReq.result == UnityWebRequest.Result.Success)
                {
                    _ABManifest = DownloadHandlerAssetBundle.GetContent(webReq);
                    //读取清单文件资源（读取到系统类中）
                    _ManifestObj = _ABManifest.LoadAsset<AssetBundleManifest>(ABDefine.ASSETBUNDLE_MANIFEST);//"AssetBundleManifest"是固定常量，通过这个名字读取清单文件。
                    _IsLoadFinish = true;
                    Debug.Log("本次加载和读取清单资源完毕！");
                }
                else
                {
                    Debug.LogError(GetType() + "/DownLoadManifestFile()/ 下载manifest清单文件失败！url = " + _StrManifestPath + webReq.error);
                }
            }
        }

        /// <summary>
        /// 获取AssetBundleManifest系统类实例
        /// </summary>
        /// <returns></returns>
        public AssetBundleManifest GetABManifest()
        {
            if (_IsLoadFinish)
            {
                if (_ManifestObj != null)
                {
                    return _ManifestObj;
                }
                Debug.LogError(GetType() + " /GetABManifest/ _ManifestObj==null，请检查！");
            }
            else
            {
                Debug.LogError(GetType() + " /GetABManifest/ _IsLoadFinish == false，Manifest没有加载完毕，请检查！");
            }
            return null;
        }

        //获取AssetBundleManifest中指定Ab包的所有依赖项
        public string[] RetrivalDependences(string abName)
        {
            if (_ManifestObj != null && !string.IsNullOrEmpty(abName))
            {
                return _ManifestObj.GetAllDependencies(abName);
            }
            //Debug.LogError(GetType() + " /RetrivalDependences()/ _ManifestObj==null，请检查！abName = "+ abName);
            return null;
        }

        /// <summary>
        /// 释放本类资源
        /// </summary>
        public void Dispose()
        {
            if (_ABManifest != null)
            {
                _ABManifest.Unload(true);
            }
        }
    }

}

