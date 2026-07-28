using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ABManager : SingletonAutoMono<ABManager>
{
    private AssetBundle mainAB = null; //主包
    private AssetBundleManifest manifest = null; //依赖包获取的文件
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>(); // AB包不能重复加载，重复加载会报错，用字典来存储加载过的AB包

    /// <summary>
    /// 这个AB包存放的路径 方便以后修改
    /// </summary>
    private string PathUrl => Application.streamingAssetsPath + "/";

    /// <summary>
    /// 主包名 方便使用
    /// </summary>
    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    /// <summary>
    /// 加载AB包
    /// </summary>
    public void LoadAB(string abName)
    {
        //加载主包和manifest文件 只加载一次
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //我们获取依赖包相关的包名 依赖包的加载顺序是先加载依赖包 再加载主包
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //如果没有加载过主包 再加载主包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    /// <summary>
    /// 同步加载 不指定类型
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public Object LoadRes(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
        //如果是 直接实例化了 再返回给外部
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    /// <summary>
    /// 同步加载 根据type指定类型
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <returns></returns>
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
        //如果是 直接实例化了 再返回给外部
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    /// <summary>
    /// 同步加载 根据泛型指定类型
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
        //如果是 直接实例化了 再返回给外部
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    //异步加载的方法
    //这里的异步加载 AB包并没有使用异步加载
    //只是从AB包中 加载资源时 使用异步
    //根据名字异步加载资源
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
        //如果是 直接实例化了 再返回给外部
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    //根据Type异步加载资源
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
        //如果是 直接实例化了 再返回给外部
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    //根据泛型 异步加载资源
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        //为了外面方便 在加载资源时 判断一下 资源是不是GameObject
        //如果是 直接实例化了 再返回给外部
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        //异步加载结束后 通过委托 传递给外部 外部来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset) as T);
        else
            callBack(abr.asset as T);
    }

    //单个包卸载
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //所有包的卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}