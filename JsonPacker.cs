using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
//using Geme.SimpleJSON;


//	JsonPacket.cs
//	Author: Lu Zexi
//	2014-06-30


namespace Game.Network
{
	/// <summary>
	/// Json packer.
	/// </summary>
	public class JsonPacker
	{
		/// <summary>
		/// Unpack the specified json and t.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <param name="t">T.</param>
		public static object Unpack( JSONNode json , Type t )
		{
			if (t.IsPrimitive)
			{
				if (t.Equals (typeof (int))) return int.Parse(json.Value);
				else if (t.Equals (typeof (int))) return int.Parse(json.Value);
				else if (t.Equals (typeof (uint))) return uint.Parse(json.Value);
				else if (t.Equals (typeof (float))) return float.Parse(json.Value);
				else if (t.Equals (typeof (double))) return double.Parse(json.Value);
				else if (t.Equals (typeof (long))) return long.Parse(json.Value);
				else if (t.Equals (typeof (ulong))) return ulong.Parse(json.Value);
				else if (t.Equals (typeof (bool))) return bool.Parse(json.Value);
				else if (t.Equals (typeof (byte))) return byte.Parse(json.Value);
				else if (t.Equals (typeof (sbyte))) return sbyte.Parse(json.Value);
				else if (t.Equals (typeof (short))) return short.Parse(json.Value);
				else if (t.Equals (typeof (ushort))) return ushort.Parse(json.Value);
				else if (t.Equals (typeof (char))) return char.Parse(json.Value);
				else if (t.Equals (typeof(string))) return json.Value;
				else
				{
					Debug.LogError(t.Name);
					throw new NotSupportedException (); 
				}
			}

			if( t.Equals(typeof(string)))
				return json.Value;

			if (t.IsArray)
			{
				if ( !(json is JSONArray) )
					throw new FormatException ();
				Type et = t.GetElementType ();
				Array ary = Array.CreateInstance (et, json.Count);
				for (int i = 0; i < ary.Length; i ++)
					ary.SetValue (Unpack ( json[i] , et), i);
				return ary;
			}
			
			object o;
			if(t.IsSubclassOf(typeof(ScriptableObject)))
			{
				o = ScriptableObject.CreateInstance (t);
			}
			else
			{
				o = FormatterServices.GetUninitializedObject (t);
			}
			ReflectionCacheEntry entry = ReflectionCache.Lookup (t);
			foreach( KeyValuePair<string,JSONNode> item in json.AsObject)
			{
				string name = item.Key;
				FieldInfo f;
				if (!entry.FieldMap.TryGetValue (name, out f))
				{
					//error
					Debug.Log("no property name: " + name);
					continue;
					//throw new FormatException ();
				}
				f.SetValue (o, Unpack ( item.Value , f.FieldType));
			}
			return o;
		}
	}

}
