/***
*
*	Title:"AB框架"项目
*           一个场景中多个AssetBundle管理
*
*	Description:
*           功能：
*               1：获取AB包之间的依赖和引用关系
*               2：管理AssetBundle包之间的自动连锁（递归）加载机制
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

    public class MultiABMgr
    {
        //引用类：单个AB包加载实现类
        private SingleABLoader _CurrentSingleABLoader;

        //AB包缓存集合（作用：缓存AB包，防止重复加载）
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;

        //当前场景(调试使用)
        private string _CurrentSceneName;

        //当前AssetBundle 名称
        private string _CurrentABName;

        //AB包与对应依赖关系集合
        Dictionary<string, ABRelation> _DicABRelation;

        //委托：所有的AB包是否加载完成
        private DelABDownLoadComplete _LoadAllABCompleteHandler;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AB包名称</param>
        /// <param name="loadAllABCompleteHandler">委托：是否加载完成</param>
        public MultiABMgr(string sceneName, string abName, DelABDownLoadComplete loadAllABCompleteHandler)
        {
            _CurrentSceneName = sceneName;
            _CurrentABName = abName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelation = new Dictionary<string, ABRelation>();
            _LoadAllABCompleteHandler = loadAllABCompleteHandler;
        }

        /// <summary>
        /// 完成加载指定AB包后调用
        /// </summary>
        /// <param name="abName"></param>
        private void OnCompleteLoadAB(string abName)
        {
            if (abName.Equals(_CurrentABName))
            {
                if (_LoadAllABCompleteHandler != null)
                {
                    _LoadAllABCompleteHandler(abName);
                }
            }
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public IEnumerator DownLoadAssetBundle(string abName)
        {
            //AB包关系建立
            if (!_DicABRelation.ContainsKey(abName))
            {
                ABRelation abRelation = new ABRelation(abName);
                _DicABRelation.Add(abName, abRelation);
            }

            ABRelation tmpABRelationObj = _DicABRelation[abName];

            //得到指定AB包所有的依赖关系(查询清单文件）
            string[] strDependenceArray = ABManifestLoader.GetInstance().RetrivalDependences(abName);
            if (strDependenceArray == null)
            {
                Debug.LogError(GetType() + " /DownLoadAssetBundle() strDependenceArray==null， 请检查！abName = " + abName);
            }
            else
            {
                foreach (string item_Depence in strDependenceArray)
                {
                    //添加依赖项
                    tmpABRelationObj.AddDependence(item_Depence);
                    //添加引用项
                    yield return DownLoadReferences(item_Depence, abName);
                }
            }

            //真正加载AB包
            if (_DicSingleABLoaderCache.ContainsKey(abName))
            {
                yield return _DicSingleABLoaderCache[abName];
            }
            else
            {
                _CurrentSingleABLoader = new SingleABLoader(abName, OnCompleteLoadAB);
                _DicSingleABLoaderCache.Add(abName, _CurrentSingleABLoader);
                yield return _CurrentSingleABLoader.DownLoadAssetBundle();
            }
        }

        /// <summary>
        /// 加载引用AB包
        /// </summary>
        /// <param name="abName">AB包名称</param>
        /// <param name="refABName">被引用AB包名称</param>
        /// <returns></returns>
        private IEnumerator DownLoadReferences(string abName, string refABName)
        {
            if (_DicABRelation.ContainsKey(abName))
            {
                ABRelation tmpABRelationObj = _DicABRelation[abName];
                //添加AB包引用关系（被依赖）
                tmpABRelationObj.AddReference(refABName);
            }
            else
            {
                ABRelation tmpABRelationObj = new ABRelation(abName);
                //添加AB包引用关系（被依赖）
                tmpABRelationObj.AddReference(refABName);
                _DicABRelation.Add(abName, tmpABRelationObj);

                //开始加载依赖包
                yield return DownLoadAssetBundle(abName);
            }

            yield return null;
        }


        /// <summary>
        /// 提取AB包中的资源
        /// </summary>
        /// <param name="abName">AB包名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用（资源）缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache)
        {
            foreach (var item_ABName in _DicSingleABLoaderCache.Keys)
            {
                if (abName == item_ABName)
                {
                    return _DicSingleABLoaderCache[item_ABName].LoadAsset(assetName, isCache);
                }
            }
            Debug.LogError(GetType() + " /LoadAsset()/找不到AssetBundle包，无法加载资源，请检查！abName = " + abName + " assetName = " + assetName);
            return null;
        }

        /// <summary>
        /// 释放本场景中所有的资源，场景和场景之间转换的时候调用
        /// </summary>
        public void DisposeAllAssets()
        {
            try
            {
                //逐一释放所有加载过的AssetBundle中的资源
                foreach (SingleABLoader item in _DicSingleABLoaderCache.Values)
                {
                    item.DisposeAll();
                }
            }
            finally
            {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;
                _DicABRelation.Clear();
                _DicABRelation = null;

                _CurrentABName = null;
                _CurrentSceneName = null;
                _LoadAllABCompleteHandler = null;

                //卸载没有使用到的资源
                Resources.UnloadUnusedAssets();

                //强制垃圾收集
                System.GC.Collect();
            }

        }

    }
}


