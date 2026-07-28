using UnityEngine;

/// <summary>
/// Lua无法直接访问C#的类
/// 先C#调用Lua脚本后，才把核心逻辑交给Lua脚本去处理
/// </summary>
public class Main : MonoBehaviour
{
    void Start()
    {
        LuaManager.Instance.Init();
        LuaManager.Instance.DoLuaFile("Main");
    }
}
