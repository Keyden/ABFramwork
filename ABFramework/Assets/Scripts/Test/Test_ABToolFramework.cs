using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW{
    /// <summary>
    /// 框架测试类
    /// </summary>
    public class Test_ABToolFramework :MonoBehaviour
    {
        public string _SceneName = "";

        public string _AssetBundleName = "";

        public string _AssetName = "";

        private void Start()
        {
            StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(_SceneName, _AssetBundleName, LoadAllABComplete));

        }
        private void LoadAllABComplete(string abName)
        {
            UnityEngine.Object tmpObj = null;

            tmpObj = (UnityEngine.Object)AssetBundleMgr.GetInstance().LoadAsset(_SceneName, _AssetBundleName, _AssetName, false);
            Debug.Log(tmpObj);
            if (tmpObj != null)
            {
                Instantiate(tmpObj);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AssetBundleMgr.GetInstance().DisposeAllAssets(_SceneName);
            }
        }

    }
}

