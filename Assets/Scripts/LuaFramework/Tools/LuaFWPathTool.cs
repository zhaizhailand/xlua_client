/***
*
*	Title:"Lua框架"项目
*			启动Lua代码
*
*	Description:
*		功能：
*			定义、管理Lua文件路径
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

namespace LuaFramework
{
	public class LuaFWPathTool
	{
		//Lua代码路径
		public static string GetLuaScriptPath()
        {
			string strLuaPath = string.Empty;
			strLuaPath = Application.dataPath + "/" + LuaFWDefine.LUA_DIR + "/";
			return strLuaPath;
        }
	}
}