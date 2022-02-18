/***
*
*	Title:"AB框架"项目
*          所有场景的AssetBundle管理
*
*	Description:
*           功能：
*               1：读取Manifest清单文件，缓存本脚本
*               2：以场景为单位管理整个项目中所有的AssetBundle包
*
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

    public class AssetBundleMgr : MonoBehaviour
    {
        //本类实例
        private static AssetBundleMgr _Instance;

        //场景集合
        private Dictionary<string, MultiABMgr> _DicAllScenes = new Dictionary<string, MultiABMgr>();

        //AssetBundle清单文件
        private AssetBundleManifest _ManifestObj = null;


        private AssetBundleMgr() { }

        //获取本类实例
        public static AssetBundleMgr GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new GameObject("AssetBundleMgr").AddComponent<AssetBundleMgr>();
            }
            return _Instance;
        }

        void Awake()
        {
            //加载清单文件
            StartCoroutine(ABManifestLoader.GetInstance().DownLoadManifestFile());
        }

        /// <summary>
        /// 下载AssetBundle 指定包
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="loadAllCompleteHandle">委托方法</param>
        /// <returns></returns>
        public IEnumerator DownLoadAssetBundlePack(string sceneName, string abName, DelABDownLoadComplete loadAllCompleteHandle)
        {
            //参数检查
            if (string.IsNullOrEmpty(sceneName) || string.IsNullOrEmpty(abName))
            {
                Debug.LogError(GetType() + " /DownLoadAssetBundlePack()/ sceneName or abName is null,请检查！");
                yield break;//类似于C#的return null
            }
            //等待Manifest加载完毕
            while (!ABManifestLoader.GetInstance().IsLoadFinish)
            {
                yield return null;//跳过当前帧，类似于C#的continue
            }

            _ManifestObj = ABManifestLoader.GetInstance().GetABManifest();
            if (_ManifestObj == null)
            {
                Debug.LogError(GetType() + " /DownLoadAssetBundlePack()/ _ManifestObj is null,请确保Manifest清单文件加载成功！");
                yield break;
            }

            //把当前场景加入集合中
            if (!_DicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiMgrObj = new MultiABMgr(sceneName, abName, loadAllCompleteHandle);
                _DicAllScenes.Add(sceneName, multiMgrObj);
            }

            //调用多宝管理类
            MultiABMgr tmpMultiMgrObj = _DicAllScenes[sceneName];
            if (tmpMultiMgrObj == null)
            {
                Debug.LogError(GetType() + " /DownLoadAssetBundlePack()/ tmpMultiMgrObj is null,请检查！");
                yield break;
            }
            //加载指定AB包
            yield return tmpMultiMgrObj.DownLoadAssetBundle(abName);
        }

        /// <summary>
        /// 加载AB包中的资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用资源缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string sceneName, string abName, string assetName, bool isCache)
        {
            if (_DicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiMgrObj = _DicAllScenes[sceneName];
                return multiMgrObj.LoadAsset(abName, assetName, isCache);
            }
            else
            {
                Debug.LogError(GetType() + " /LoadAsset()/ 找不到场景名称，无法加载AB包中资源，请检查！sceneName = " + sceneName);
            }
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void DisposeAllAssets(string sceneName)
        {
            if (_DicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiMgrObj = _DicAllScenes[sceneName];
                multiMgrObj.DisposeAllAssets();
            }
            else
            {
                Debug.LogError(GetType() + " /DisposeAllAssets()/ 找不到场景名称，无法释放资源，请检查！sceneName = " + sceneName);
            }
        }

    }//Class_end

}

