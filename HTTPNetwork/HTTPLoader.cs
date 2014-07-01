using System.Collections.Generic;
using System.Collections;
using UnityEngine;


//  HTTPLoader.cs
//  Author: Lu Zexi
//  2013-12-16

namespace Game.Network
{

    /// <summary>
    /// HTTP加载
    /// </summary>
    public class HTTPLoader : MonoBehaviour
    {
        public delegate void CALLBACK(WWW www);

        /// <summary>
        /// 开始WWW
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static void GoWWW(string url, CALLBACK callback)
        {
            GameObject obj = new GameObject("HTTPLoader");
            HTTPLoader loader = obj.AddComponent<HTTPLoader>();
            loader.StartCoroutine(loader.StartHTTP(url, callback));
        }

        /// <summary>
        /// 开始HTTP
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator StartHTTP(string url, CALLBACK callback)
        {
            Debug.Log("send url " + url);
            url = System.Uri.EscapeUriString(url);
            WWW www = new WWW(url);

            yield return www;

            if (callback != null)
                callback(www);

            GameObject.Destroy(this.gameObject);
        }

    }

}