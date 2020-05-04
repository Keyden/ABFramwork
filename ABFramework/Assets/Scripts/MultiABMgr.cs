using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW{
    /// <summary>
    /// 一个场景中的多个AssetBundle管理
    /// </summary>
    public class MultiABMgr 
    {
        //单个AB包加载实现类
        private SingleABLoader _CurrentSingleABLoader;

        //AB包加载 实现类缓存集合，缓存AB包，防止重复加载
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;

        //当前场景（调试使用）
        private string _CurrentSceneName;

        //当前AssetBundle名称
        private string _CurrentABName;

        //AB包与对应依赖关系集合
        private Dictionary<string, ABRelation> _DicABRelaton;

        //委托：所有AB包加载完成调用
        private DelLoadComplete _LoadAllABPackageCompleteHandle;

        public MultiABMgr(string sceneName, string abName,DelLoadComplete loadAllABPackageCompleteHandle)
        {
            _CurrentSceneName = sceneName;
            _CurrentABName = abName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelaton = new Dictionary<string, ABRelation>();
            _LoadAllABPackageCompleteHandle = loadAllABPackageCompleteHandle;

        }

        /// <summary>
        /// 完成指定AB包调用
        /// </summary>
        /// <param name="abName">AB包名称</param>
        private void CompleteLoadAB(string abName)
        {
            if (abName.Equals(_CurrentABName))
            {
                _LoadAllABPackageCompleteHandle?.Invoke(abName);
            }
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName">AB包名</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string abName)
        {
            if (!_DicABRelaton.ContainsKey(abName))
            {
                ABRelation abRelationObj = new ABRelation(abName);
                _DicABRelaton.Add(abName, abRelationObj);
            }
            ABRelation tmpABRelationObj = _DicABRelaton[abName];

            //得到指定AB包所有的依赖关系
            string[] strDependenceArray = ABManifestLoader.GetInstance().RetrivalDependence(abName);

            foreach(string item_Dependence in strDependenceArray)
            {
                //添加依赖
                tmpABRelationObj.AddDependence(item_Dependence);

                //添加引用 递归
                yield return LoadReference(item_Dependence, abName);
            }
            //真正加载AB包
            if (_DicSingleABLoaderCache.ContainsKey(abName))
            {
                yield return _DicSingleABLoaderCache[abName].LoadAssetBundle();
            }
            else
            {
                _CurrentSingleABLoader = new SingleABLoader(abName, CompleteLoadAB);
                _DicSingleABLoaderCache.Add(abName, _CurrentSingleABLoader);
                yield return _CurrentSingleABLoader.LoadAssetBundle();
            }
        }

        /// <summary>
        /// 加载引用AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private IEnumerator LoadReference(string abName,string refABName)
        {
            //ab包已经加载
            if (_DicABRelaton.ContainsKey(abName))
            {
                ABRelation tmpABRelationObj = _DicABRelaton[abName];
                tmpABRelationObj.AddReference(refABName);
            }
            else
            {
                ABRelation tmpABRelationObj = new ABRelation(abName);
                tmpABRelationObj.AddReference(refABName);
                _DicABRelaton.Add(abName, tmpABRelationObj);
                //开始加载依赖的包
                yield return LoadAssetBundle(abName);
            }
        }

        /// <summary>
        /// 加载（ab包中）资源
        /// </summary>
        /// <param name="abName">AB包名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string abName, string assetName,bool isCache)
        {
            foreach(string item_abName in _DicSingleABLoaderCache.Keys)
            {
                if(abName == item_abName)
                {
                    return _DicSingleABLoaderCache[item_abName].LoadAsset(assetName, isCache);
                }
            }
            Debug.LogError("无法加载资源，abName：" + abName + ",assetName: " + assetName);
            return null;
        }

        /// <summary>
        /// 释放本场景中所有的资源
        /// </summary>
        public void DisposeAllAsset()
        {
            try
            {
                foreach (SingleABLoader item_sABLoader in _DicSingleABLoaderCache.Values)
                {
                    item_sABLoader.DisposAll();
                }
            }
            finally
            {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;

                _DicABRelaton.Clear();
                _DicABRelaton = null;
                _CurrentABName = null;
                _CurrentSceneName = null;
                _LoadAllABPackageCompleteHandle = null;

                Resources.UnloadUnusedAssets();

                System.GC.Collect();
            }
            
        }
    }
}

