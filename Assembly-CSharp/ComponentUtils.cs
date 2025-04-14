using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200083F RID: 2111
public static class ComponentUtils
{
	// Token: 0x06003368 RID: 13160 RVA: 0x000F5828 File Offset: 0x000F3A28
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

	// Token: 0x06003369 RID: 13161 RVA: 0x000F587B File Offset: 0x000F3A7B
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

	// Token: 0x0600336A RID: 13162 RVA: 0x000F58B4 File Offset: 0x000F3AB4
	public static T AddComponent<T>(this Component c) where T : Component
	{
		return c.gameObject.AddComponent<T>();
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x000F58C1 File Offset: 0x000F3AC1
	public static void GetOrAddComponent<T>(this Component c, out T result) where T : Component
	{
		if (!c.TryGetComponent<T>(out result))
		{
			result = c.gameObject.AddComponent<T>();
		}
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x000F58DD File Offset: 0x000F3ADD
	public static bool GetComponentAndSetFieldIfNullElseLogAndDisable<T>(this Behaviour c, ref T fieldRef, string fieldName, string fieldTypeName, string msgSuffix = "Disabling.", [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : Component
	{
		if (c.GetComponentAndSetFieldIfNullElseLog(ref fieldRef, fieldName, fieldTypeName, msgSuffix, caller))
		{
			return true;
		}
		c.enabled = false;
		return false;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x000F58F8 File Offset: 0x000F3AF8
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

	// Token: 0x0600336E RID: 13166 RVA: 0x000F5989 File Offset: 0x000F3B89
	public static bool DisableIfNull<T>(this Behaviour c, T fieldRef, string fieldName, string fieldTypeName, [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : Object
	{
		if (fieldRef != null)
		{
			return true;
		}
		c.enabled = false;
		return false;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x000F59A3 File Offset: 0x000F3BA3
	public static Hash128 ComputeStaticHash128(Component c, string k)
	{
		return ComponentUtils.ComputeStaticHash128(c, StaticHash.Compute(k));
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x000F59B4 File Offset: 0x000F3BB4
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

	// Token: 0x040036BB RID: 14011
	private static readonly uint[] kHashBits = new uint[4];
}
