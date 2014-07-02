using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SimpleJSON;

using HTTP_ON_DATAERROR = System.Action<string>;
using HTTP_ON_TIMEOUT = System.Action;
using HTTP_ON_DISCONNECT = System.Action;


//  HttpSession.cs
//  Author: Lu Zexi
//  2013-11-12


namespace Game.Network
{

    /// <summary>
    /// HTTP会话
    /// </summary>
    public class HTTPSession
    {
        private string m_strURL = "";   //主地址

		public HTTP_ON_DATAERROR onDataError = null;
		public HTTP_ON_TIMEOUT onTimeOut = null;
		public HTTP_ON_DISCONNECT onDisconnect = null;

        public HTTPSession(string url)
        {
            this.m_strURL = url;
        }

		/// <summary>
		/// Raises the data error event.
		/// </summary>
		/// <param name="error">Error.</param>
		private void OnDataError( string error )
		{
			if(this.onDataError != null )
			{
				this.onDataError(error);
			}
		}

		/// <summary>
		/// Raises the time out event.
		/// </summary>
		private void OnTimeOut()
		{
			if(this.onTimeOut != null )
			{
				this.onTimeOut();
			}
		}

		/// <summary>
		/// Raises the disconnect event.
		/// </summary>
		private void OnDisconnect()
		{
			if(this.onDisconnect != null )
			{
				this.onDisconnect();
			}
		}

        /// <summary>
        /// Sends the GET data.
        /// </summary>
        /// <param name="packet">Packet.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void SendGET<T>(HTTPPacketRequest packet ,System.Action<T> callback) where T : HTTPPacketAck
        {
			HTTPLoader.GoWWW<T>(this.m_strURL + packet.GetAction() + packet.ToParam() , null , FinishCallBack , callback);
        }

		/// <summary>
		/// Post the specified packet and callback.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void SendPOST<T>(HTTPPacketRequest packet ,System.Action<T> callback) where T : HTTPPacketAck
		{
			HTTPLoader.GoWWW<T>(this.m_strURL + packet.GetAction(), packet.ToForm() , FinishCallBack , callback);
		}

		/// <summary>
		/// Calls the back.
		/// </summary>
		/// <param name="www">Www.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void FinishCallBack<T>(WWW www , System.Action<T> callback) where T : HTTPPacketAck
		{
			try
			{
				if (www.error != null)
				{
					Debug.LogError("ERROR HTTP : " + www.error);
					OnDisconnect();
				}
				else
				{
					Debug.Log(www.text);
					JSONNode obj = JSON.Parse(www.text);
					Debug.Log(obj.ToString());
					T packet = JsonPacker.Unpack(obj , typeof(T)) as T;
					callback(packet);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.StackTrace);
				OnDataError(ex.StackTrace);
			}
		}
    }


}