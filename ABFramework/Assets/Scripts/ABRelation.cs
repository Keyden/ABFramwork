using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW{
    /// <summary>
    /// AssetBundle关系类
    /// </summary>
    public class ABRelation 
    {
        private string _ABName;//当前AssetBundle名称

        private List<string> _ListAllDependenceAB;//本包所有的依赖包集合

        private List<string> _ListAllReferenceAB;//本包所有的引用包集合

        public ABRelation(string abName)
        {
            if (string.IsNullOrEmpty(abName))
            {
                _ABName = abName;
            }
            _ListAllDependenceAB = new List<string>();
            _ListAllReferenceAB = new List<string>();
        }
        /// <summary>
        /// 增加依赖关系
        /// </summary>
        /// <param name="abName">AssetBundle包名称</param>
        public void AddDependence(string abName)
        {
            if (!_ListAllDependenceAB.Contains(abName))
            {
                _ListAllDependenceAB.Add(abName);
            }
        }

        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <param name="abName">移除包的名称</param>
        /// <returns></returns>
        public bool RemoveDependence(string abName)
        {
            if (_ListAllDependenceAB.Contains(abName))
            {
                _ListAllDependenceAB.Remove(abName);
            }
            if (_ListAllDependenceAB.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取所有依赖关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDependence()
        {
            return _ListAllDependenceAB;
        }


        /// <summary>
        /// 增加引用关系
        /// </summary>
        /// <param name="abName">AssetBundle包名称</param>
        public void AddReference(string abName)
        {
            if (!_ListAllReferenceAB.Contains(abName))
            {
                _ListAllReferenceAB.Add(abName);
            }
        }

        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// <param name="abName">移除包的名称</param>
        /// <returns></returns>
        public bool RemoveReference(string abName)
        {
            if (_ListAllReferenceAB.Contains(abName))
            {
                _ListAllReferenceAB.Remove(abName);
            }
            if (_ListAllReferenceAB.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取所有引用关系
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllReference()
        {
            return _ListAllReferenceAB;
        }
    }
}

