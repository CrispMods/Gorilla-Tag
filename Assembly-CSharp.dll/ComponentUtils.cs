﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000842 RID: 2114
public static class ComponentUtils
{
	// Token: 0x06003374 RID: 13172 RVA: 0x00137DBC File Offset: 0x00135FBC
	public static T EnsureComponent<T>(this Component ctx, ref T target) where T : Component
	{
		if (ctx.AsNull<Component>() == null)
		{
			return default(T);
		}
		if (target.AsNull<T>() != null)
		{
			return target;
		}
		return target = ctx.GetComponent<T>();
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x0005104C File Offset: 0x0004F24C
	public static bool TryEnsureComponent<T>(this Component ctx, ref T target) where T : Component
	{
		if (ctx.AsNull<Component>() == null)
		{
			return false;
		}
		if (target.AsNull<T>() != null)
		{
			return true;
		}
		target = ctx.GetComponent<T>();
		return true;
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x00051085 File Offset: 0x0004F285
	public static T AddComponent<T>(this Component c) where T : Component
	{
		return c.gameObject.AddComponent<T>();
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x00051092 File Offset: 0x0004F292
	public static void GetOrAddComponent<T>(this Component c, out T result) where T : Component
	{
		if (!c.TryGetComponent<T>(out result))
		{
			result = c.gameObject.AddComponent<T>();
		}
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x000510AE File Offset: 0x0004F2AE
	public static bool GetComponentAndSetFieldIfNullElseLogAndDisable<T>(this Behaviour c, ref T fieldRef, string fieldName, string fieldTypeName, string msgSuffix = "Disabling.", [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : Component
	{
		if (c.GetComponentAndSetFieldIfNullElseLog(ref fieldRef, fieldName, fieldTypeName, msgSuffix, caller))
		{
			return true;
		}
		c.enabled = false;
		return false;
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x00137E10 File Offset: 0x00136010
	public static bool GetComponentAndSetFieldIfNullElseLog<T>(this Behaviour c, ref T fieldRef, string fieldName, string fieldTypeName, string msgSuffix = "", [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : Component
	{
		if (fieldRef != null)
		{
			return true;
		}
		fieldRef = c.GetComponent<T>();
		if (fieldRef != null)
		{
			return true;
		}
		Debug.LogError(string.Concat(new string[]
		{
			caller,
			": Could not find ",
			fieldTypeName,
			" \"",
			fieldName,
			"\" on \"",
			c.name,
			"\". ",
			msgSuffix
		}), c);
		return false;
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x000510C9 File Offset: 0x0004F2C9
	public static bool DisableIfNull<T>(this Behaviour c, T fieldRef, string fieldName, string fieldTypeName, [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : UnityEngine.Object
	{
		if (fieldRef != null)
		{
			return true;
		}
		c.enabled = false;
		return false;
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x000510E3 File Offset: 0x0004F2E3
	public static Hash128 ComputeStaticHash128(Component c, string k)
	{
		return ComponentUtils.ComputeStaticHash128(c, StaticHash.Compute(k));
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x00137EA4 File Offset: 0x001360A4
	public static Hash128 ComputeStaticHash128(Component c, int k = 0)
	{
		if (c == null)
		{
			return default(Hash128);
		}
		Transform transform = c.transform;
		Component[] components = c.gameObject.GetComponents(typeof(Component));
		uint[] array = ComponentUtils.kHashBits;
		int siblingIndex = transform.GetSiblingIndex();
		int num = components.Length;
		int num2 = 0;
		while (num2 < num && c != components[num2])
		{
			num2++;
		}
		int num3 = StaticHash.Compute(k + 2, 1);
		int num4 = StaticHash.Compute(siblingIndex + 4, num3);
		int num5 = StaticHash.Compute(num + 8, num4);
		int num6 = StaticHash.Compute(num2 + 16, num5);
		array[0] = (uint)num3;
		array[1] = (uint)num4;
		array[2] = (uint)num5;
		array[3] = (uint)num6;
		SRand srand = new SRand(StaticHash.Compute(num3, num4, num5, num6));
		srand.Shuffle<uint>(array);
		Hash128 result = new Hash128(array[0], array[1], array[2], array[3]);
		Hash128 hash = Hash128.Compute(c.GetType().FullName);
		Hash128 hash2 = TransformUtils.ComputePathHash(transform);
		Hash128 hash3 = transform.localToWorldMatrix.QuantizedHash128();
		HashUtilities.AppendHash(ref hash, ref result);
		HashUtilities.AppendHash(ref hash2, ref result);
		HashUtilities.AppendHash(ref hash3, ref result);
		return result;
	}

	// Token: 0x040036CD RID: 14029
	private static readonly uint[] kHashBits = new uint[4];
}
