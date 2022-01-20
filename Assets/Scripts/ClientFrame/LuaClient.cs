using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;
using System.Text;
using System;

public class LuaClient : MonoBehaviour
{
    private LuaEnv luaenv;

    Action LuaUpdate;
    Action LuaFixedUpdate;
    Action LuaLateUpdate;

    string luainit = @"package.path = luapath
                        print('package.path = ',package.path)
                        require 'Main'
                    ";

    // Start is called before the first frame update
    void Start()
    {
        luaenv = new LuaEnv();
        InitPackagePath(luaenv);
        LuaUpdate = luaenv.Global.Get<Action>("Update");
        LuaFixedUpdate = luaenv.Global.Get<Action>("FixedUpdate");
        LuaLateUpdate = luaenv.Global.Get<Action>("LateUpdate");
    }

    //≥ı ºªØlua package.path
    private void InitPackagePath(LuaEnv env)
    {
        LuaFileUtil.Instance.AddSearchPath(LuaConst.luaDir);
        string newpath = LuaFileUtil.Instance.GetPackagePath();
        env.Global.Set<string, string>("luapath", newpath);
        luaenv.DoString(luainit);
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
        LuaUpdate = null;
        LuaLateUpdate = null;
        LuaFixedUpdate = null;
        Debug.Log("GameManager Destroyed");
        luaenv.Dispose();
    }
}
