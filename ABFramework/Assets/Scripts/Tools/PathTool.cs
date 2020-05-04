using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW{
	public class PathTool 
	{
        public const string AB_RES = "AB_Res";
		
        public static string GetABResPath()
        {
            return Application.dataPath + "/" + AB_RES;
        }

        /// <summary>
        /// 获取AB输出路径
        /// </summary>
        /// <returns></returns>
        public static string GetABOutPath()
        {
            return GetPlatformPath() + "/" + GetPlatformName();
        }
        private static string GetPlatformPath()
        {
            string strReturnPlatformPath = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformPath = Application.streamingAssetsPath;
                    break;

                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    strReturnPlatformPath = Application.persistentDataPath;
                    break;
                default:
                    break;
            }

            return strReturnPlatformPath;
        }

        private static string GetPlatformName()
        {
            string strReturnPlatformName = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturnPlatformName = "Windows";
                    break;

                case RuntimePlatform.IPhonePlayer:
                    strReturnPlatformName = "Iphone";
                    break;
                case RuntimePlatform.Android:
                    strReturnPlatformName = "Android";
                    break;
                default:
                    break;
            }

            return strReturnPlatformName;
        }

        /// <summary>
        /// 获取WWW协议下载（AB包）路径
        /// </summary>
        /// <returns></returns>
        public static string GetWWWPath()
        {
            string strReturnWWWPath = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    strReturnWWWPath = "file://" + GetABOutPath();
                    //strReturnWWWPath =  GetABOutPath();
                    break;

                case RuntimePlatform.Android:
                    strReturnWWWPath = "jar:file://" + GetABOutPath();
                    break;

                default:break;
            }    

            return strReturnWWWPath;
        }
	}
}