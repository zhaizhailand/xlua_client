/***
*
*	Title:"热更新框架"项目
*			拷贝所有的Lua脚本文件到发布区
*
*	Description:
*			功能：
*
*	Author: Zhaiyurong
*
*	Date: 2022.2
*
*	Modify:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using AssetBundleFramework;

namespace HotFixFramework
{
	public class CopyLuaToStreamingAssets
	{

		//定义拷贝lua文件的源目录（lua编辑区）
		private static string _LuaDIRPath = Application.dataPath + PathTool.LUA_DIR_PATH;

		//定义目标目录（lua文件的发布区）
		private static string _CopyTargetDIRPath = PathTool.GetABOutPath() + PathTool.LUA_DEPLOY_PATH;

		[MenuItem("AssetsBunldeTools/CopyLuaFileToStreamingAssets")]
		public static void CopyLuaFileToStreamingAssets()
        {
			//参数检查

			//定义目录和文件结构
			DirectoryInfo dirInfo = new DirectoryInfo(_LuaDIRPath);
			FileInfo[] fileInfos = dirInfo.GetFiles();
			if(!Directory.Exists(_CopyTargetDIRPath))
            {
				Directory.CreateDirectory(_CopyTargetDIRPath);
            }

			//循环拷贝文件
			foreach(FileInfo fileObj in fileInfos)
            {
				File.Copy(fileObj.FullName, _CopyTargetDIRPath + "/" + fileObj.Name, true);
            }

			//Unity编辑器窗体刷新
			AssetDatabase.Refresh();
			Debug.Log("LUA文件拷贝到指定区域完成！");
        }

	}//Class_end
}