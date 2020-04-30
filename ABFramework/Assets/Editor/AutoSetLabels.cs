using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ABFW{
	public class AutoSetLabels 
	{
		[MenuItem("AssetBundelTools/Set AB Label")]
        public static void SetABLabel()
        {
            //需要给AB做标记的根目录
            string strNeedSetLabelRoot = string.Empty;

            //目录信息（场景目录信息数组，表示所有的根目录下场景目录）
            DirectoryInfo[] dirScenesDIRArray = null;

            //清空无用AR标记
            AssetDatabase.RemoveUnusedAssetBundleNames();

            //需要打包资源的文件夹根目录
            strNeedSetLabelRoot = PathTool.GetABResPath();
            DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetLabelRoot);
            dirScenesDIRArray = dirTempInfo.GetDirectories();

            foreach(DirectoryInfo currentDIR in dirScenesDIRArray)
            {
                //如果是目录则继续递归访问里面的文件，知道定位到文件
                string tmpScenesDIR = strNeedSetLabelRoot + "/" + currentDIR.Name;//全路径
                
                int tmpIndex = tmpScenesDIR.LastIndexOf("/");

                string tmpScenesName = tmpScenesDIR.Substring(tmpIndex + 1);

                JudgeDIRorFileByRrecursive(currentDIR, tmpScenesName);

            }

            //刷新
            AssetDatabase.Refresh();
            //提示信息
            Debug.Log("AssetBundle 本次操作设置标记完成！");
        }

        /// <summary>
        /// 递归判断是否为目录与文件，修改AssetBundle的标记（label）
        /// </summary>
        /// <param name="currentDIR"></param>
        /// <param name="sceneName"></param>
        private static void JudgeDIRorFileByRrecursive(FileSystemInfo fileSysInfo, string sceneName)
        {
            if (!fileSysInfo.Exists)
            {
                Debug.LogError("文件或目录名称：" + fileSysInfo + " 不存在，请检查！");


                return;

            }
            //得到当前目录下一级文件集合
            DirectoryInfo dirInfoObj = fileSysInfo as DirectoryInfo;
            FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();

            foreach(FileSystemInfo fileInfo in fileSysArray)
            {
                FileInfo fileinfoObj = fileInfo as FileInfo;
                //文件类型

                if (fileinfoObj != null)
                {
                    SetFileABLabel(fileinfoObj, sceneName);
                }
                else
                {
                    JudgeDIRorFileByRrecursive(fileInfo, sceneName);
                }


            }

        }

        /// <summary>
        /// 对指定文件设置 AB包名称
        /// </summary>
        /// <param name="fileinfoObj"></param>
        /// <param name="scenesName"></param>
        private static void SetFileABLabel(FileInfo fileinfoObj, string scenesName)
        {
            //AB包名称
            string strABName = string.Empty;

            //文件路径（相对路径）
            string strAssetFilePath = string.Empty;

            if (fileinfoObj.Extension == ".meta") return;

            //得到AB包名称
            strABName = GetABName(fileinfoObj, scenesName);

            int tmpIndex = fileinfoObj.FullName.IndexOf("Assets");

            strAssetFilePath = fileinfoObj.FullName.Substring(tmpIndex);

            //给资源文件设置AB名称以及后缀
            AssetImporter tmpImporterObj = AssetImporter.GetAtPath(strAssetFilePath);
            tmpImporterObj.assetBundleName = strABName;

            if(fileinfoObj.Extension == ".unity")
            {
                tmpImporterObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpImporterObj.assetBundleVariant = "ab";
            }
        }
        /// <summary>
        /// 获取AB包名称
        /// </summary>
        /// <param name="fileinfoObj"></param>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private static string GetABName(FileInfo fileinfoObj, string sceneName)
        {
            string strABName = string.Empty;

            string tmpWinPath = fileinfoObj.FullName;

            string tmpUnityPath = tmpWinPath.Replace("\\", "/");

            //定位 场景名称 后面字符位置
            int tmpSceneNamePosition = tmpUnityPath.IndexOf(sceneName) + sceneName.Length;

            //AB包中 类型名称 所在区域
            string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePosition + 1);

            if (strABFileNameArea.Contains("/"))
            {
                string[] tempStrArray = strABFileNameArea.Split('/');
                strABName = sceneName + "/" + tempStrArray[0];
            }
            else
            {
                strABName = sceneName + "/" + sceneName;
            }

            return strABName;
        }

	}
}