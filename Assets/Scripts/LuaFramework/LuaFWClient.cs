/***
*
*	Title:"Lua框架"项目
*			启动Lua代码
*
*	Description:
*		功能：
*			启动Lua
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

using XLua;
using System.Text;
using System;
using System.IO;

namespace LuaFramework
{
    public class LuaFWClient : MonoBehaviour
    {
        //Lua环境
        private LuaEnv luaenv;

        //Update事件
        private Action LuaUpdate;
        private Action LuaFixedUpdate;
        private Action LuaLateUpdate;


        void Start()
        {
            //定义Lua环境
            luaenv = new LuaEnv();

            //添加loader
            luaenv.AddLoader(LuaLoader);

            //开始执行Lua代码
            luaenv.DoString(LuaFWDefine.LUA_START);

            
            //定义Update
            LuaUpdate = luaenv.Global.Get<Action>("Update");
            LuaFixedUpdate = luaenv.Global.Get<Action>("FixedUpdate");
            LuaLateUpdate = luaenv.Global.Get<Action>("LateUpdate");
        }

        /// <summary>
        /// 自定义Loader
        /// </summary>
        /// <param name="filepath">require语句中填的lua文件路径</param>
        /// <returns></returns>
        public byte[] LuaLoader(ref string filepath)
        {
            //拼接Lua文件下载路径
            string loadPath = LuaFWPathTool.GetLuaScriptPath() + "/" + filepath + ".lua";
            //Debug.Log("lua filepath = " + loadPath);

            //读取Lua文件内容
            string content = File.ReadAllText(loadPath);
            //Debug.Log("lua text = "+ content);

            return System.Text.Encoding.UTF8.GetBytes(content);
        }

        
        // Update is called once per frame
        void Update()
        {
            LuaUpdate();
        }

        void LateUpdate()
        {
            LuaLateUpdate();
        }

        void FixedUpdate()
        {
            LuaFixedUpdate();
        }
        

        void OnDestroy()
        {
            Debug.Log("GameManager Destroyed");

            //清理引用数据
            LuaUpdate = null;
            LuaLateUpdate = null;
            LuaFixedUpdate = null;

            //释放资源
            luaenv.Dispose();
        }
    }

}
