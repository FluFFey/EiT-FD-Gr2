using System;
using UnityEngine;


public class EnumUtil
{
	public static T RandomEnumValue<T> ()
	{
		var v = Enum.GetValues (typeof (T));
		return (T) v.GetValue (UnityEngine.Random.Range(0,v.Length));
	}
}

