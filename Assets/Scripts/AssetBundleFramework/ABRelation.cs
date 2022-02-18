/***
*
*	Title:"AB框架"项目
*           工具辅助类：AssetBundle关系类
*
*	Description:
*           功能：
*               1：存储指定AB包的所有依赖关系包
*               2：存储指定AB包的所有引用关系包
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

    public class ABRelation
    {
        //当前AssetBundle名称
        private string _ABName;

        //本包所有的依赖包集合
        private List<string> _ListAllDependencesAB;

        //本包所有的引用包集合
        private List<string> _ListAllReferencesAB;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abName"></param>
        public ABRelation(string abName)
        {
            if (!string.IsNullOrEmpty(abName))
            {
                _ABName = abName;
            }
            _ListAllDependencesAB = new List<string>();
            _ListAllReferencesAB = new List<string>();
        }

        /*依赖关系*/
        /// <summary>
        /// 增加依赖关系
        /// </summary>
        /// <param name="abName">AssetBundle 包名称</param>
        public void AddDependence(string abName)
        {
            if (!_ListAllDependencesAB.Contains(abName))
            {
                _ListAllDependencesAB.Add(abName);
            }
        }

        /// <summary>
        /// 删除依赖关系
        /// </summary>
        /// <param name="abName">移除包的名称</param>
        /// <returns>
        /// true：此AssetBundle 没有依赖项
        /// false：此AssetBundle 还有其他的依赖项
        /// </returns>
        public bool RemoveDependence(string abName)
        {
            if (_ListAllDependencesAB.Contains(abName))
            {
                _ListAllDependencesAB.Remove(abName);
            }
            return _ListAllDependencesAB.Count > 0;
        }

        /// <summary>
        /// 获取所有的依赖关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDependences()
        {
            return _ListAllDependencesAB;
        }

        /*引用关系*/
        /// <summary>
        /// 增加引用关系
        /// </summary>
        /// <param name="abName"></param>
        public void AddReference(string abName)
        {
            if (!_ListAllReferencesAB.Contains(abName))
            {
                _ListAllReferencesAB.Add(abName);
            }
        }

        //删除引用关系
        /// <summary>
        /// 删除引用关系
        /// </summary>
        /// <param name="abName">移除包的名称</param>
        /// <returns>
        /// true：此AssetBundle 没有引用项
        /// false：此AssetBundle 还有其他的引用项
        /// </returns>
        public bool RemoveReference(string abName)
        {
            if (_ListAllReferencesAB.Contains(abName))
            {
                _ListAllReferencesAB.Remove(abName);
            }
            return _ListAllReferencesAB.Count > 0;
        }

        /// <summary>
        /// 获取所有的引用关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllReferences()
        {
            return _ListAllReferencesAB;
        }
    }

}

