using UnityEngine;
using System.Collections;

namespace Game.Network
{
	public abstract class IHttpSession
	{
		public delegate HTTPPacketAck PROCESS_HANDLE(HTTPPacketRequest req);

		/// <summary>
		/// Sends the GET data.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual void SendGET<T>(HTTPPacketRequest packet ,System.Action<T>  callback = null, PROCESS_HANDLE process = null) where T : HTTPPacketAck
		{
			//
		}
		
		/// <summary>
		/// Post the specified packet and callback.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public abstract void SendPOST<T>(HTTPPacketRequest packet ,System.Action<T> callback = null, PROCESS_HANDLE process = null) where T : HTTPPacketAck;
		
		/// <summary>
		/// Sends the Bytes Data.
		/// </summary>
		/// <param name="packet">Packet.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="headers">Headers.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public abstract void SendBYTE<T>(HTTPPacketRequest packet ,System.Action<T> callback = null, PROCESS_HANDLE process = null) where T : HTTPPacketAck;

	}

}