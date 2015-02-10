using System.Collections.Generic;
using System.Collections;
using UnityEngine;
//using Geme.SimpleJSON;
using System;


//  HttpRequest.cs
//  Author: Lu Zexi
//  2013-12-16

namespace Game.Network
{

    /// <summary>
    /// HTTP加载
    /// </summary>
    public class HttpRequest : MonoBehaviour 
    {
        private STATE m_eState;
        private enum STATE
        {
            START = 0,
            STOP,
            CLOSE
        }

        /// <summary>
        /// Res the send.
        /// </summary>
        internal void ReSend()
        {
            this.m_eState = STATE.START;
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        internal void Close()
        {
            this.m_eState = STATE.CLOSE;
        }

        public static void Request(string url , WWWForm form , byte[] byteData , Hashtable headers ,
            System.Action<string> callback , System.Action<string,System.Action,System.Action> error_callback)
        {
            GameObject obj = new GameObject("HttpRequest");
            HttpRequest loader = obj.AddComponent<HttpRequest>();
            loader.m_eState = STATE.START;
            loader.StartCoroutine(loader.request(url , form , byteData , headers , callback , error_callback));
        }

        //request
        internal IEnumerator request(string url , WWWForm form , byte[] byteData , Hashtable headers , System.Action<string> callback ,
                                        System.Action<string,System.Action,System.Action> error_callback)
        {
            WWW www = null;
            bool ok = true;
            for( ; ok ;)
            {
                switch(this.m_eState)
                {
                case STATE.START:
                    url = System.Uri.EscapeUriString(url);

                    if(byteData != null)
                    {
                        if(headers == null)
                        {
                            www = new WWW(url, byteData);
                        }
                        else
                        {
                            www = new WWW(url, byteData, headers);
                        }
                    }
                    else if(form != null)
                    {
                        if( headers == null )
                        {
                            www = new WWW(url, form);
                        }
                        else
                        {
                            www = new WWW(url, form.data, headers);
                        }
                    }
                    else if( form == null )
                    {
                        if( headers == null )
                        {
                            www = new WWW(url);
                        }
                        else
                        {
                            www = new WWW(url, new byte[]{(byte) 0}, headers);
                        }
                    }

                    yield return www;
                    try
                    {
                        if (www.error != null)
                        {
                            Debug.LogError("ERROR HTTP : " + www.error);
                            this.m_eState = STATE.STOP;
                            if(error_callback != null)
                                error_callback(www.error,ReSend , Close);
                        }
                        else
                        {
                            this.m_eState = STATE.CLOSE;
                            if(callback != null )
                                callback(www.text);
                        }
                    }
                    catch( Exception ex )
                    {
                        this.m_eState = STATE.STOP;
                        if(error_callback != null)
                            error_callback(ex.StackTrace,ReSend , Close);
                    }
                    www.Dispose();
                    www = null;
                    break;
                case STATE.STOP:
                    yield return new WaitForFixedUpdate();
                    break;
                case STATE.CLOSE:
                    GameObject.Destroy(this.gameObject);
                    ok = false;
                    break;
                }
            }
        }

    }

}