using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ABFW{
    /// <summary>
    /// 辅助类，读取AssetBundle依赖关系文件(系统清单文件)
    /// </summary>
    public class ABManifestLoader : System.IDisposable
    {
        private static ABManifestLoader _Instance;

        private AssetBundleManifest _ManifestObj;//AssetBundle(清单文件) 系统类

        private string _StrManifestPath;//AssetBundle 清单文件的路径

        private AssetBundle _ABReadManifest;//读取AB清单文件的AssetBundle

        private bool _IsLoadFinish;//是否加载 Manifest 完成

        public bool IsLoadFinish
        {
            get { return _IsLoadFinish; }
        }

        private ABManifestLoader()
        {
            _StrManifestPath = PathTool.GetWWWPath() + "/" + PathTool.GetPlatformName();
            _ManifestObj = null;
            _ABReadManifest = null;
            _IsLoadFinish = false;
        }

        public static ABManifestLoader GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new ABManifestLoader();
            }
            return _Instance;
        }

        /// <summary>
        /// 加载Manifest清单文件
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadManifestFile()
        {
            using(UnityWebRequest www = UnityWebRequest.Get(_StrManifestPath))
            {
                yield return www.SendWebRequest();
                if (www.isDone)
                {
                    AssetBundle abObj = AssetBundle.LoadFromMemory(www.downloadHandler.data);
                    if(abObj != null)
                    {
                        Debug.Log("加载完成");
                        _ABReadManifest = abObj;
                        _ManifestObj = _ABReadManifest.LoadAsset(ABDefine.ASSETBUNDLW_MANIFEST) as AssetBundleManifest;
                        _IsLoadFinish = true;
                    }
                    else
                    {
                        Debug.LogError("abObj == null");
                    }
                }
            }
        }

        /// <summary>
        /// 获取AssetBundleManifest 系统类实例
        /// </summary>
        /// <returns></returns>
        public AssetBundleManifest GetABManifest()
        {
            if (_IsLoadFinish)
            {
                if(_ManifestObj != null)
                {
                    return _ManifestObj;
                }
                else
                {
                    Debug.LogError("_ManifestObj == null");
                }
            }
            else
            {
                Debug.LogError("获取AssetBundleManifest 未加载完成");
            }
            return null;
        }

        /// <summary>
        /// 获取AssetBundleManifest（系统类）指定包名称依赖项
        /// </summary>
        /// <param name="abName">AB包名称</param>
        /// <returns></returns>
        public string[] RetrivalDependence(string abName)
        {
            if(_ManifestObj!=null && !string.IsNullOrEmpty(abName))
            {
                return _ManifestObj.GetAllDependencies(abName);
            }
            return null;
        }

        /// <summary>
        /// 释放本类资源
        /// </summary>
        public void Dispose()
        {
            if (_ABReadManifest != null)
            {
                _ABReadManifest.Unload(true);
            }
            else
            {
                Debug.LogError("_ABReadManifest == null");
            }
        }
    }
}

