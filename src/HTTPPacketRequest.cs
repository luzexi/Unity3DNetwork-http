using UnityEngine;
using System;
using System.IO;
using MsgPack;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


//	HTTPPacketRequest.cs
//	Author: Lu Zexi
//	2014-06-30


namespace Game.Network
{
	/// <summary>
	/// HTTP packet request.
	/// </summary>
	public class HTTPPacketRequest
	{
		protected string m_strAction;

		/// <summary>
		/// Gets the action path.
		/// </summary>
		/// <returns>The action.</returns>
		public virtual string GetAction()
		{
			return this.m_strAction;
		}

		/// <summary>
		/// Tos the parameter.
		/// </summary>
		/// <returns>The parameter.</returns>
		public string ToParam()
		{
			//string ret = "";
			Type t = GetType();
			FieldInfo[] fis = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
			List<string> paraList = new List<string>();
			foreach (FieldInfo f in fis)
			{
				if (f.FieldType.IsArray)
				{
					// array
					var v = (IList)f.GetValue(this);
					for (int i = 0, il = v.Count; i < il; i++)
					{
						paraList.Add(f.Name + "[]=" + v[i]);
					}
				}
				else
				{
					paraList.Add(f.Name + "=" + f.GetValue(this));
				}
			}
			return string.Join("&", paraList.ToArray());
		}

		/// <summary>
		/// Tos the form.
		/// </summary>
		/// <returns>The form.</returns>
		public WWWForm ToForm()
		{
			WWWForm form = new WWWForm();
			Type t = GetType();
			FieldInfo[] fis = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo f in fis)
			{
				object val = f.GetValue(this);
				form.AddField(f.Name, val != null ? val.ToString() : "");
			}
			
			return form;
		}


		/// <summary>
		/// Tos the message packet byte.
		/// </summary>
		/// <returns>The message packet byte.</returns>
		public byte[] ToMsgPacketByte()
		{
			byte[] res = null;
			CustomPacker customPacker = new CustomPacker();
			res = customPacker.Pack(this);
//			MemoryStream resMS = new MemoryStream();
//			Type t = GetType();
//			FieldInfo[] fis = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
//			foreach (FieldInfo f in fis)
//			{
//				object val = f.GetValue(this);
//				BinaryFormatter bf = new BinaryFormatter();
//				MemoryStream ms = new MemoryStream();
//				bf.Serialize(ms, val);
//				byte[] dataRes = ms.ToArray();
//				resMS.Write(dataRes , 0 , dataRes.Length);
//				//form.AddField(f.Name, val != null ? val.ToString() : "");
//			}
			return res;
		}
	}

}