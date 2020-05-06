using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW{
    /// <summary>
    /// 所有场景的 AssetBundle 管理
    /// </summary>
    public class AssetBundleMgr :MonoBehaviour
    {
        private static AssetBundleMgr _Instance;

        //场景集合
        private Dictionary<string, MultiABMgr> _DicAllScene = new Dictionary<string, MultiABMgr>();

        //AssetBundle 系统类
        private AssetBundleManifest _ManifestObj = null;

        private AssetBundleMgr() { }

        public static AssetBundleMgr GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new GameObject("_AssetBundleMgr").AddComponent<AssetBundleMgr>();
            }
            return _Instance;
        }

        private void Awake()
        {
            //加载Manifest清单文件
            StartCoroutine(ABManifestLoader.GetInstance().LoadManifestFile());
        }

        /// <summary>
        /// 下载AssetBundle指定包
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle包名称</param>
        /// <param name="loadAllCompleteHandle">完成回调</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundlePack(string sceneName,string abName,DelLoadComplete loadAllCompleteHandle)
        {
            if(string.IsNullOrEmpty(sceneName) || string.IsNullOrEmpty(abName))
            {
                Debug.LogError("sceneName:" + sceneName + ", abName:" + abName);
                yield break;
            }

            while (!ABManifestLoader.GetInstance().IsLoadFinish)
            {

                yield return null;
            }

            _ManifestObj = ABManifestLoader.GetInstance().GetABManifest();

            if(_ManifestObj == null)
            {
                Debug.LogError("_ManifestObj == null");
                yield break;
            }

            //把当前场景加入集合中
            if (!_DicAllScene.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = new MultiABMgr(sceneName, abName, loadAllCompleteHandle);
                _DicAllScene.Add(sceneName, multiABMgrObj);
            }
            //调用 多包管理类
            MultiABMgr tmpMultiABMgrObj = _DicAllScene[sceneName];
            if(tmpMultiABMgrObj == null)
            {
                Debug.LogError("tmpMultiABMgrObj == null");
                yield break;
            }

            //加载指定AB包
            yield return tmpMultiABMgrObj.LoadAssetBundle(abName);
        }

        /// <summary>
        /// 加载 AB包中的资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle包名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string sceneName,string abName, string assetName,bool isCache)
        {
            if (_DicAllScene.ContainsKey(sceneName))
            {
                MultiABMgr multObj = _DicAllScene[sceneName];
                return multObj.LoadAsset(abName, assetName, isCache);

            }
            Debug.LogError("_DicAllScene不包含" + sceneName);
            return null;
        }

        public void DisposeAllAssets(string sceneName)
        {
            if (_DicAllScene.ContainsKey(sceneName))
            {
                MultiABMgr multObj = _DicAllScene[sceneName];
                multObj.DisposeAllAsset();
            }
            else
            {
                Debug.LogError("_DicAllScene 不包含" + sceneName);
            }
        }
    }
}

