/***
*
*	Title:"AB框架"项目
*
*
*	Description:
*           功能：加载单个AB包
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

    using UnityEngine.Networking;
    using System;

    public class SingleABLoader : IDisposable
    {
        //引用类：资源加载类
        private AssetLoader _AssetLoader;

        //AB包名称
        private string _ABName;

        //AB包下载路径
        private string _ABDownLoadPath;

        private DelABDownLoadComplete _ABDownLoadCompleteHandler;

        public SingleABLoader(string abName, DelABDownLoadComplete loadCompleteHandler)
        {
            _ABName = abName;
            _ABDownLoadPath = PathTool.GetABDownLoadPath() + "/" + _ABName;
            _ABDownLoadCompleteHandler = loadCompleteHandler;
        }

        /// <summary>
        /// 下载AB包
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IEnumerator DownLoadAssetBundle()
        {
            using (UnityWebRequest webReq = UnityWebRequestAssetBundle.GetAssetBundle(_ABDownLoadPath))
            {
                yield return webReq.SendWebRequest();
                if (webReq.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(GetType() + "LoadAB UnityWebRequest 下载失败，请检查！url = " + _ABDownLoadPath + " 错误信息：" + webReq.error);
                }
                else
                {
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(webReq);
                    _AssetLoader = new AssetLoader(ab);
                    Debug.Log("下载AB包成功，abName = " + _ABName);
                    if (_ABDownLoadCompleteHandler != null)
                    {
                        _ABDownLoadCompleteHandler(_ABName);
                    }
                }
            }
        }

        /// <summary>
        /// 加载AB包内资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache)
        {
            if (_AssetLoader != null)
            {
                return _AssetLoader.LoadAsset(assetName, isCache);
            }
            Debug.LogError(GetType() + " /LoadAsset()/ 参数_AssetLoader==null，请检查！");
            return null;
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnLoadAsset(UnityEngine.Object asset)
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.UnLoadAsset(asset);
            }
            else
            {
                Debug.LogError(GetType() + " /UnLoadAsset()/ 参数_AssetLoader==null，请检查！");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.Dispose();
                _AssetLoader = null;

            }
            else
            {
                Debug.LogError(GetType() + " /Dispose()/ 参数_AssetLoader==null，请检查！");
            }
        }

        /// <summary>
        /// 释放当前AB资源包，且卸载所有资源
        /// </summary>
        public void DisposeAll()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.DisposeAll();
                _AssetLoader = null;

            }
            else
            {
                Debug.LogError(GetType() + " /DisposeAll()/ 参数_AssetLoader==null，请检查！");
            }
        }

        /// <summary>
        /// 查询当前AssetBundle包中所有的资源
        /// </summary>
        /// <returns></returns>
        public string[] RetrivalAllAssetNames()
        {
            if (_AssetLoader != null)
            {
                return _AssetLoader.RetriveAllAssetName();
            }
            Debug.LogError(GetType() + " /DisposeAll()/ 参数_AssetLoader==null，请检查！");
            return null;
        }
    }
}


