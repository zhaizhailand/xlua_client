/***
*
*	Title:"AB框架"项目
*           自动标记资源文件
*
*	Description:
*           开发思路：
*               1：定义需要打包资源的文件夹根目录
*               2：遍历每个场景文件夹
*                    2.1:遍历本场景目录下所有文件
*                       如果是目录，则需要递归访问里面的文件，直到定位到文件
*                    2.2: 如果找到文件，则使用AssetImporter类，标记包名和后缀名
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

    using UnityEditor;//编辑器命名空间
    using System.IO;//文件和目录操作命名空间

    public class AutoSetAssetBundleLabels
    {
        /// <summary>
        /// 设置AB包名
        /// </summary>
        //[MenuItem("AssetBundleTools/Set AB Label")]
        public static void SetABLabel()
        {
            //给AB做标记的根目录
            string strSetLabelRoot = string.Empty;
            //根目录下所有场景目录信息
            DirectoryInfo[] dirSceneArray = null;

            //清空无用AB标记
            AssetDatabase.RemoveUnusedAssetBundleNames();
            strSetLabelRoot = PathTool.GetABResourcePath();
            //Debug.Log("### strSetLabelRoot = " + strSetLabelRoot);

            //遍历每个场景的文件夹
            DirectoryInfo dirTempInfo = new DirectoryInfo(strSetLabelRoot);
            dirSceneArray = dirTempInfo.GetDirectories();
            foreach (DirectoryInfo currentDir in dirSceneArray)
            {
                //string tempSceneDir = strSetLabelRoot + "/" + currentDir.Name;
                //Debug.Log("### curdir name = " + currentDir.Name);
                JudgeDIRforFileByRecursive(currentDir, currentDir.Name);

            }

            //刷新
            AssetDatabase.Refresh();
            //提示标记包名完成
            Debug.Log("AsetBundle 本次操作设置标记完成！");
        }

        /// <summary>
        /// 递归判断是否是文件或目录，修改AssetBundle的标记 
        /// </summary>
        /// <param name="currentDIR">当前目录信息</param>
        /// <param name="sceneName">场景名称</param>
        private static void JudgeDIRforFileByRecursive(FileSystemInfo fileSysInfo, string sceneName)
        {
            //检查参数
            if (!fileSysInfo.Exists)
            {
                Debug.LogError("文件或目录名称：" + fileSysInfo + "不存在，请检查");
                return;
            }

            DirectoryInfo dirInfo = fileSysInfo as DirectoryInfo;
            FileSystemInfo[] fileSysArray = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fileInfo in fileSysArray)
            {
                FileInfo fileInfoObj = fileInfo as FileInfo;
                //文件类型
                if (fileInfoObj != null)
                {
                    //设置此文件AssetBundle标签
                    SetFileAssetBundleLabel(fileInfoObj, sceneName);
                }
                //目录类型
                else
                {
                    JudgeDIRforFileByRecursive(fileInfo, sceneName);
                }
            }
        }

        /// <summary>
        /// 对指定文件设置AB包名
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="name">场景名称</param>
        private static void SetFileAssetBundleLabel(FileInfo fileInfo, string sceneName)
        {
            //参数检查 meta文件
            if (fileInfo.Extension == ".meta")
            {
                return;
            }

            //AssetBundle 包名称
            string strABName = string.Empty;
            //文件相对路径
            string strAssetFilePath = string.Empty;

            int index = fileInfo.FullName.IndexOf("Assets");
            strAssetFilePath = fileInfo.FullName.Substring(index);
            AssetImporter tempImporter = AssetImporter.GetAtPath(strAssetFilePath);

            //设置包名
            strABName = GetABName(fileInfo, sceneName);
            tempImporter.assetBundleName = strABName;

            //定义AB包扩展名
            if (fileInfo.Extension == ".unity")
            {
                //场景(Scene)文件
                tempImporter.assetBundleVariant = "u3d";
            }
            else
            {
                tempImporter.assetBundleVariant = "ab";
            }
        }

        /// <summary>
        /// 获取AB包名
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="sceneName">场景名称</param>
        /// <returns></returns>
        private static string GetABName(FileInfo fileInfo, string sceneName)
        {
            string strABName = string.Empty;

            //WIN路径
            string tempWinPath = fileInfo.FullName;

            //unity路径
            string tempUnityPath = tempWinPath.Replace("\\", "/");

            //Scene在路径中的位置
            int tempSceneIndex = tempUnityPath.IndexOf(sceneName) + sceneName.Length;
            //三级目录开始的路径
            string strABFileNameArea = tempUnityPath.Substring(tempSceneIndex + 1);

            if (strABFileNameArea.Contains("/"))
            {
                string[] tempStrArray = strABFileNameArea.Split("/");
                strABName = sceneName + "/" + tempStrArray[0];
            }
            else
            {
                //定义*.untiy文件形成的特殊AB包名称
                strABName = sceneName + "/" + sceneName;
            }
            return strABName;
        }

    }

}

