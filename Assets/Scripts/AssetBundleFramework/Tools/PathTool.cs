/***
*
*	Title:"AB框架"项目
*           路径工具类
*
*	Description:
*           功能：
*               包含所有路径常量、路径方法
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

    public class PathTool
    {
        //路径常量
        //AB资源文件夹
        public static string AB_RESOURCE = "AB_Resources";

        //校验文件名称
        public static string VERIFY_FILE_PATH = "/VerifyFile.txt";

        /*定义拷贝lua文件的路径常量*/
        //定义拷贝lua文件的源目录（lua编辑区）
        public const string LUA_DIR_PATH = "LuaScripts/";

        //定义目标目录（lua文件的发布区）
        public const string LUA_DEPLOY_PATH = "/Lua";


        //定义HTTP服务器连接地址
        public const string SERVER_URL = "http://127.0.0.1:8080/UpdateAssets";


        //路径方法
        /// <summary>
        /// 获取AB资源路径
        /// </summary>
        /// <returns></returns>
        public static string GetABResourcePath()
        {
            return Application.dataPath + "/" + AB_RESOURCE;
        }

        /// <summary>
        /// 获取打AB包输出路径
        /// 算法：
        ///     1：平台(PC/移动端)路径
        ///     2：平台名称
        /// </summary>
        /// <returns></returns>
        public static string GetABOutPath()
        {
            return GetPlatformPath() + "/" + GetPlatformName();
        }

        /// <summary>
        /// 获取平台路径
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformPath()
        {
            string strPlatformPath = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strPlatformPath = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    strPlatformPath = Application.persistentDataPath;
                    break;
                default:
                    break;
            }
            return strPlatformPath;
        }

        /// <summary>
        /// 获取平台名称
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformName()
        {
            string strPlatformName = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strPlatformName = "Win";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    strPlatformName = "Iphone";
                    break;
                case RuntimePlatform.Android:
                    strPlatformName = "Android";
                    break;
                default:
                    break;
            }
            return strPlatformName;
        }

        /// <summary>
        /// 获取UnityWebRequest下AB包下载路径
        /// </summary>
        /// <returns></returns>
        public static string GetABDownLoadPath()
        {
            string strABDownLoadPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strABDownLoadPath = "file://" + GetABOutPath();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    strABDownLoadPath = GetABOutPath() + "/Raw/";
                    break;
                case RuntimePlatform.Android:
                    strABDownLoadPath = "jar:file://" + GetABOutPath();
                    break;
                default:
                    break;
            }
            return strABDownLoadPath;
        }

        /// <summary>
        /// 获取校验文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetMD5VerifyFilePath()
        {
            string strVerifyFilePath = string.Empty;
            strVerifyFilePath = GetABOutPath() + VERIFY_FILE_PATH;
            return strVerifyFilePath;
        }

    }//Class_end

}

