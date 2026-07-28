using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaCopyEditor : Editor
{
    [MenuItem("XLua/自动生成txt后缀的Lua文件")]
    public static void CopyLuaFiles()
    {
        string sourcePath = Application.dataPath + "/Lua";
        string destinationPath = Application.dataPath + "/LuaTxt";

        if (Directory.Exists(destinationPath)) // 如果目标目录存在，则删除
        {
            Directory.Delete(destinationPath, true); // true表示递归删除子目录和文件
        }

        if (!Directory.Exists(sourcePath)) return;

        // 获取源码路径下所有Lua文件
        string[] luaFiles = Directory.GetFiles(sourcePath, "*.lua", SearchOption.AllDirectories); // SearchOption.AllDirectories表示递归搜索子目录

        foreach (string file in luaFiles)
        {
            // file:D:/Unity/Project/BlueStudy/Lua/Assets/Lua\BagPanel.lua
            // sourcePath :D:/Unity/Project/BlueStudy/Lua/Assets/Lua
            // relativePath : BagPanel.lua
            string relativePath = file.Substring(sourcePath.Length + 1); // 获取相对路径
            // destinationFile:D:/Unity/Project/BlueStudy/Lua/Assets/LuaTxt\BagPanel.lua
            string destinationFile = Path.Combine(destinationPath, relativePath) + ".txt";

            // 如果目标目录不存在则创建
            string destinationDirectory = Path.GetDirectoryName(destinationFile);
            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            // 将文件复制到目的地
            File.Copy(file, destinationFile, true);
        }

        AssetDatabase.Refresh();

        Debug.Log("Lua文件复制完成");

        // 修改AssetBundle为lua
        string[] txtFiles = Directory.GetFiles(destinationPath, "*.txt", SearchOption.AllDirectories);
        foreach (string txtFile in txtFiles)
        {
            // txtFile: D:/Unity/Project/BlueStudy/Lua/Assets/LuaTxt\UI\总结.lua.txt
            // Application.dataPath: D:/Unity/Project/BlueStudy/Lua/Assets
            // relativePath: Assets/LuaTxt\UI\总结.lua.txt
            string relativePath = txtFile.Substring(Application.dataPath.Length - "Assets".Length); // 获取相对路径
            // relativePath : Assets/LuaTxt/UI/总结.lua.txt
            relativePath = relativePath.Replace("\\", "/");
            AssetImporter assetImporter = AssetImporter.GetAtPath(relativePath); // 获取AssetImporter
            if (assetImporter != null)
            {
                assetImporter.assetBundleName = "lua";
            }
        }
        Debug.Log("Lua文件AssetBundle修改完成");
    }
}
