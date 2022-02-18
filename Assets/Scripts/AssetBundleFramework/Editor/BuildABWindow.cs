/***
*
*	Title:"AB框架"项目
*           打包资源窗口
*
*	Description:
*           开发思路：
*               1：标记AB资源
*               2：对标记的资源进行打包
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
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.EditorTools;
    using System.IO;


    //打AB包窗口
    public class BuildABWindow : EditorWindow
    {
        private static string abRootPath = "AssetBundles";
        private int toolbarIdx = 0;
        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        /// <summary>
        /// 打开打包窗口 
        /// </summary>
        [MenuItem("Tools/PackToolsWindow")]
        public static void OpenPackToolWindow()
        {
            BuildABWindow window = GetWindow<BuildABWindow>(false, "打包窗口");
        }

        /// <summary>
        /// 绘制窗口UI
        /// </summary>
        void OnGUI()
        {
            toolbarIdx = GUILayout.Toolbar(toolbarIdx, new[] { "PC", "Android", "IOS" });
            switch (toolbarIdx)
            {
                case 0:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    break;
                case 1:
                    buildTarget = BuildTarget.Android;
                    break;
                case 2:
                    buildTarget = BuildTarget.iOS;
                    break;
                default:
                    break;
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("自动标记AB包名"))
            {
                AutoSetAssetBundleLabel();
            }

            if (GUILayout.Button("打包AssetBundles"))
            {
                BuildABResources(buildTarget);
            }

            GUILayout.EndHorizontal();

        }


        /// <summary>
        /// 自动给资源文件添加标记
        /// </summary>
        private void AutoSetAssetBundleLabel()
        {
            AutoSetAssetBundleLabels.SetABLabel();
        }

        /// <summary>
        /// 打包生成所有的AB包
        /// </summary>
        /// <param name="abOutPathDir">打AB包输出路径</param>
        /// <param name="target">打AB包目标平台</param>
        private void BuildABResources(BuildTarget target)
        {
            string abOutPathDir = PathTool.GetABOutPath();
            if (!Directory.Exists(abOutPathDir))
            {
                Directory.CreateDirectory(abOutPathDir);
            }
            else
            {
                //删除所有的AB包文件
                Directory.Delete(abOutPathDir, true);
                //去除删除警告，删除*.meta文件
                File.Delete(abOutPathDir + ".meta");
                //刷新
                AssetDatabase.Refresh();
            }

            BuildPipeline.BuildAssetBundles(abOutPathDir, BuildAssetBundleOptions.None, target);
            Debug.Log("AssetBundle 本次打包完成！");
        }


        //写入版本号
        public static void SaveVersion(string version, string package)
        {

        }

    }

}

