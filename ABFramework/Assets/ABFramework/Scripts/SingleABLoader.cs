using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ABFW{
    /// <summary>
    /// WWW加载AssetBundle
    /// </summary>
    public class SingleABLoader : System.IDisposable
    {
        //资源加载类
        private AssetLoader _AssetLoader;

        //委托
        private DelLoadComplete _LoadCompleteHandle;

        //AssetBundle名称
        private string _ABName;

        //AssetBundle下载路径
        private string _ABDownLoadPath;

       


        public SingleABLoader(string abName,DelLoadComplete loadComplete)
        {
            _AssetLoader = null;
            _ABName = abName;
            _LoadCompleteHandle = loadComplete;

            _ABDownLoadPath = PathTool.GetWWWPath() + "/" + _ABName;
        }

        /// <summary>
        /// 加载Asset Bundle资源包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle()
        {
            using(UnityWebRequest www =  UnityWebRequest.Get(_ABDownLoadPath))
            {
                yield return www.SendWebRequest();

                if (www.isDone)
                {
                    byte[] results = www.downloadHandler.data;

                    AssetBundle abObj = AssetBundle.LoadFromMemory(results);
                    if(abObj != null)
                    {
                        _AssetLoader = new AssetLoader(abObj);
                        _LoadCompleteHandle?.Invoke(_ABName);
                    }
                    else
                    {
                        Debug.LogError("abObj == null, results.Length:" + results.Length+ ",_ABDownLoadPath:"+ _ABDownLoadPath);
                    }
                }
            }
        }

        /// <summary>
        /// 加载AB包内资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache)
        {
            if (_AssetLoader != null)
            {
                return _AssetLoader.LoadAsset(assetName, isCache);
            }

            Debug.LogError("_AssetLoader == null");
            return null;
        }

        /// <summary>
        /// 卸载AB包中资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnLoadAsset(UnityEngine.Object asset)
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.UnLoadAsset(asset);
            }
            else
            {
                Debug.LogError("_AssetLoader == null");
            }
            
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.Dispose();
                _AssetLoader = null;
            }
            else
            {
                Debug.LogError("_AssetLoader == null");
            }
        }

        /// <summary>
        /// 释放当前AssetBundle资源包，且卸载所有资源
        /// </summary>
        public void DisposAll()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.DisposeAll();
                _AssetLoader = null;
            }
            else
            {
                Debug.LogError("_AssetLoader == null");
            }
        }

        /// <summary>
        /// 查询当前AssetBundle包中的所有资源
        /// </summary>
        /// <returns></returns>
        public string[] RetrivalAllAssetName()
        {
            if (_AssetLoader != null)
            {
                return _AssetLoader.RetriveAllAssetName();
            }

            Debug.LogError("_AssetLoader == null");
            return null;
        }
    }
}

