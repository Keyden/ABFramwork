using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW{
    /// <summary>
    /// SingleABLoader 测试类
    /// </summary>
    public class Test_SingleABLoader : MonoBehaviour
    {
        private SingleABLoader _LoadObj = null;

        public string _ABName = "";

        public string _AssetName = "";

        /*依赖包*/
        private string _ABDependName1 = "scene_1/textures.ab";//贴图
        private string _ABDependName2 = "scene_1/materials.ab";//材质

        private void Start()
        {
            

            SingleABLoader load1 = new SingleABLoader(_ABDependName1, LoadComplete1);
            StartCoroutine(load1.LoadAssetBundle());
        }

        private void LoadComplete1(string _ABName1)
        {
            SingleABLoader load2 = new SingleABLoader(_ABDependName2, LoadComplete2);
            StartCoroutine(load2.LoadAssetBundle());
        }
        private void LoadComplete2(string _ABName2)
        {
            _LoadObj = new SingleABLoader(_ABName, LoadComplete);
            StartCoroutine(_LoadObj.LoadAssetBundle());
        }

        private void LoadComplete(string _ABName)
        {
            UnityEngine.Object tmpObj = _LoadObj.LoadAsset(_AssetName, false);
            Instantiate(tmpObj);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("释放资源");
                _LoadObj.Dispose();
            }
        }

    }
}

