using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// Lua管理器
/// 提供Lua解析器的唯一性，避免重复创建LuaEnv
/// 通过自定义Loader加载Lua脚本
/// </summary>
public class LuaManager : Singleton<LuaManager>
{
    // 释放垃圾
    // 销毁
    // 重定向
    private LuaEnv luaEnv;

    /// <summary>
    /// 得到Lua中的_G全局表
    /// </summary>
    public LuaTable Global => luaEnv.Global;

    public void Init()
    {
        if (luaEnv != null) return;

        luaEnv = new LuaEnv();
        // 添加自定义Loader
        //luaEnv.AddLoader(CustomLoader);
        luaEnv.AddLoader(CustomABLoader);
    }

    #region 重定向

    // 重定向加载本地Assets/LuaScripts文件夹下的Lua脚本
    private byte[] CustomLoader(ref string filepath)
    {
        string path = Application.dataPath + "/Lua/" + filepath + ".lua";
        //Debug.Log(path);
        if (File.Exists(path))
            return File.ReadAllBytes(path);
        else
            Debug.LogError("自定义Loader,执行的lua脚本文件不存在: " + filepath);
        return null;
    }

    //重定向加载AB包中的Lua脚本
    private byte[] CustomABLoader(ref string filepath)
    {
        Debug.Log("AB包加载重定向");
        /*
        {
            // 从AB包中加载Lua脚本
            // 加载AB包
            string abPath = Application.streamingAssetsPath + "/lua";
            AssetBundle ab = AssetBundle.LoadFromFile(abPath);
            // 加载Lua脚本 返回
            TextAsset luaText = ab.LoadAsset<TextAsset>(filepath + ".lua");

            return luaText.bytes;
        }
        */

        ABManager.Instance.LoadAB("lua");
        TextAsset luaAsset = ABManager.Instance.LoadRes<TextAsset>("lua", filepath + ".lua");
        Debug.Log("filepath: " + filepath + ".lua");
        if (luaAsset == null)
        {
            Debug.LogError("自定义AB包Loader,执行的lua脚本文件不存在: " + filepath);
            return null;
        }
        return luaAsset.bytes;
    }

    #endregion

    /// <summary>
    /// 执行Lua脚本
    /// </summary>
    public void DoString(string luaStr)
    {
        if (luaEnv == null)
        {
            Debug.LogError("Lua解析器未初始化吗,请先调用Init()方法");
            return;
        }
        luaEnv.DoString(luaStr);
    }

    /// <summary>
    /// 执行Lua脚本
    /// </summary>
    public void DoLuaFile(string fileName)
    {
        string luaStr = $"require '{fileName}'";
        luaEnv.DoString(luaStr);
    }

    /// <summary>
    /// 释放Lua垃圾
    /// </summary>
    public void Trik()
    {
        if (luaEnv == null)
        {
            Debug.LogError("Lua解析器未初始化吗,请先调用Init()方法");
            return;
        }
        luaEnv.Tick();
    }

    /// <summary>
    /// 销毁解析器
    /// </summary>
    public void Dispose()
    {
        if (luaEnv == null)
        {
            Debug.LogError("Lua解析器未初始化吗,请先调用Init()方法");
            return;
        }
        luaEnv.Dispose();
        luaEnv = null;
    }
}
