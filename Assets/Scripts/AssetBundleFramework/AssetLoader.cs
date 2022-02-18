/***
*
*	Title:"AssetBundle简单框架"项目
*       主流程：AB资源加载类
*
*
*	Description:
*           功能：
*               1：管理与加载指定AB的资源
*               2：加载具有“缓存功能”的资源，带有可选参数
*               3：卸载、释放AB资源
*               4：查看当前AB包资源
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


    public class AssetLoader : System.IDisposable
    {
        //当前AssetBundle
        private AssetBundle _CurrentAssetBundle;

        //缓存容器集合
        private Hashtable _Ht;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abObj">给定的已加载的AssetBundle实例</param>
        public AssetLoader(AssetBundle abObj)
        {
            if (abObj != null)
            {
                _CurrentAssetBundle = abObj;
                _Ht = new Hashtable();
            }
            else
            {
                Debug.LogError(GetType() + "/构造函数 AssetLoader() /参数 abObj == null， 请检查");
            }
        }

        /// <summary>
        /// 加载当前AB包中的指定资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache = false)
        {

            return LoadResource<UnityEngine.Object>(assetName, isCache);
        }

        /// <summary>
        /// 加载当前AB包的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        private T LoadResource<T>(string assetName, bool isCache) where T : UnityEngine.Object
        {
            if (_Ht.Contains(assetName))
            {
                return _Ht[assetName] as T;
            }

            //正式加载
            T tmpResource = _CurrentAssetBundle.LoadAsset<T>(assetName);
            if (tmpResource != null && isCache)
            {
                _Ht.Add(assetName, tmpResource);
            }
            else if (tmpResource == null)
            {
                Debug.LogError(GetType() + "LoadResource<T> tmpResource == null,加载资源失败，请检查");
            }

            return tmpResource;
        }

        /// <summary>
        /// 卸载指定的资源
        /// </summary>
        /// <param name="asset">资源名称</param>
        /// <returns></returns>
        public bool UnLoadAsset(UnityEngine.Object asset)
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }
            Debug.LogError(GetType() + " UnLoadAsset()/ 参数asset==null，请检查！");
            return false;
        }

        /// <summary>
        /// 释放当前AssetBundle内存镜像资源
        /// </summary>
        public void Dispose()
        {
            _CurrentAssetBundle.Unload(false);
        }


        /// <summary>
        /// 释放当前AssetBundle内存镜像资源，且释放内存资源
        /// </summary>
        public void DisposeAll()
        {
            _CurrentAssetBundle.Unload(true);
        }

        /// <summary>
        /// 查询当前AB包中包含的所有资源名称
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName()
        {
            return _CurrentAssetBundle.GetAllAssetNames();
        }
    }

}

