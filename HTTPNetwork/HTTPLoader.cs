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
		public delegate void CALLBACK<T>(WWW www , System.Action<T> callback);
		//public delegate void CALLBACK2<T>(System.Action<T> callback);

        /// <summary>
        /// 开始WWW
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
		public static void GoWWW<T>(string url,WWWForm form , CALLBACK<T> callback , System.Action<T> callback2 )
        {
            GameObject obj = new GameObject("HTTPLoader");
            HTTPLoader loader = obj.AddComponent<HTTPLoader>();
            loader.StartCoroutine(loader.StartHTTP<T>(url , form , callback , callback2));
        }

        /// <summary>
        /// 开始HTTP
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator StartHTTP<T>(string url , WWWForm form, CALLBACK<T> callback , System.Action<T> callback2 )
        {
            Debug.Log("send url " + url);
            url = System.Uri.EscapeUriString(url);
            WWW www = new WWW(url,form);

            yield return www;

            if (callback != null)
                callback(www , callback2);

            GameObject.Destroy(this.gameObject);
        }

    }

}