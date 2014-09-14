using UnityEngine;
using System.Collections;

//	HttpDummySession.cs
//	Author: Lu Zexi
//	2014-09-13



namespace Game.Network
{

	public class HttpDummySession
	{
		public HttpDummySession(string url)
		{
			//
		}

		/// <summary>
		/// Sends the GET data.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void SendGET<T>(HTTPPacketRequest packet ,System.Action<T>  callback = null, IHttpSession.PROCESS_HANDLE process = null)
			where T : HTTPPacketAck
		{
			T ack = null;
			if( process != null )
				ack = process(packet) as T;

			if(callback != null)
				callback(ack);
		}
		
		/// <summary>
		/// Post the specified packet and callback.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void SendPOST<T>(HTTPPacketRequest packet ,System.Action<T> callback = null, IHttpSession.PROCESS_HANDLE process = null) where T : HTTPPacketAck
		{
			T ack = null;
			if(process != null)
				ack = process(packet) as T;

			if(callback != null)
				callback(ack);
		}
		
		/// <summary>
		/// Sends the Bytes Data.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="headers">Headers.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void SendBYTE<T>(HTTPPacketRequest packet ,System.Action<T> callback = null, IHttpSession.PROCESS_HANDLE process = null) where T : HTTPPacketAck
		{
			T ack = null;
			if(process != null)
				ack = process(packet) as T;
			
			if(callback != null)
				callback(ack);
		}
	}

}