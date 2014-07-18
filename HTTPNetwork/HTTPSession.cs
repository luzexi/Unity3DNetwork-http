using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SimpleJSON;

using HTTP_ON_DATAERROR = System.Action<string,System.Action,System.Action>;


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

        public HTTPSession(string url)
        {
            this.m_strURL = url;
        }

        /// <summary>
        /// Sends the GET data.
        /// </summary>
        /// <param name="packet">Packet.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void SendGET<T>(HTTPPacketRequest packet ,System.Action<T>  callback = null, Hashtable headers = null) where T : HTTPPacketAck
        {
			HTTPLoader.GoWWW<T>(this.m_strURL + packet.GetAction() + packet.ToParam() , null , null , headers , onDataError , callback);
        }

		/// <summary>
		/// Post the specified packet and callback.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void SendPOST<T>(HTTPPacketRequest packet ,System.Action<T> callback = null, Hashtable headers = null) where T : HTTPPacketAck
		{
			HTTPLoader.GoWWW<T>(this.m_strURL + packet.GetAction(), packet.ToForm() , null , headers , onDataError , callback);
		}

		/// <summary>
		/// Sends the Bytes Data.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="headers">Headers.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void SendBYTE<T>(HTTPPacketRequest packet ,System.Action<T> callback = null, Hashtable headers = null) where T : HTTPPacketAck
		{
			HTTPLoader.GoWWW<T>(this.m_strURL + packet.GetAction(), null , packet.ToMsgPacketByte() , headers , onDataError , callback);
		}
    }


}