using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class XluaConfig
{
    public static List<Type> luaCallCSharpList = new List<Type>
    {
        typeof(GameObject),
        typeof(Transform),
    };
}
