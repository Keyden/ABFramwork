using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW
{
    /// <summary>
    /// AB资源加载类
    /// </summary>
    public class AssetLoader : System.IDisposable
    {
        //当前AssetBundle
        private AssetBundle _CurrentAssetBundle;

        //缓存容器集合
        private Hashtable _Ht;

        public AssetLoader(AssetBundle abObj)
        {
            if(abObj != null)
            {
                _CurrentAssetBundle = abObj;
                _Ht = new Hashtable();

            }
            else
            {
                Debug.LogError("abObj==null");
            }

        }

        /// <summary>
        /// 加载当前包中指定的资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否开启缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName,bool isCache = false)
        {
            return LoadResource<UnityEngine.Object>(assetName, isCache);
        }

        /// <summary>
        /// 加载当前AB包的资源，带缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否需要缓存处理</param>
        /// <returns></returns>
        private T LoadResource<T> (string assetName, bool isCache) where T : UnityEngine.Object
        {
            if (_Ht.Contains(assetName))
            {
                return _Ht[assetName] as T;
            }

            T tmpTResource = _CurrentAssetBundle.LoadAsset<T>(assetName);

            if(tmpTResource!=null && isCache)
            {
                _Ht.Add(assetName, tmpTResource);
            }else if(tmpTResource == null)
            {
                Debug.LogError("tmpTResource == null");
            }
            return tmpTResource;
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset">资源名称</param>
        /// <returns></returns>
        public bool UnLoadAsset(UnityEngine.Object asset)
        {
            if (asset != null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }
            Debug.LogError("asset == null");
            return false;
        }

        /// <summary>
        /// 释放当前AssetBundle内存镜像资源
        /// </summary>
        public void Dispose()
        {
            _CurrentAssetBundle.Unload(false);
        }

        /// <summary>
        /// 释放当前AssetBundle内存镜像资源 且释放内存资源
        /// </summary>
        public void DisposeAll()
        {
            _CurrentAssetBundle.Unload(true);
        }

        /// <summary>
        /// 查询当前AssetBundle中包含的所有资源
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName()
        {
            return _CurrentAssetBundle.GetAllAssetNames();
        }
    }
}

