using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000859 RID: 2137
public static class ComponentUtils
{
	// Token: 0x06003423 RID: 13347 RVA: 0x0013D314 File Offset: 0x0013B514
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

	// Token: 0x06003424 RID: 13348 RVA: 0x0005245A File Offset: 0x0005065A
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

	// Token: 0x06003425 RID: 13349 RVA: 0x00052493 File Offset: 0x00050693
	public static T AddComponent<T>(this Component c) where T : Component
	{
		return c.gameObject.AddComponent<T>();
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x000524A0 File Offset: 0x000506A0
	public static void GetOrAddComponent<T>(this Component c, out T result) where T : Component
	{
		if (!c.TryGetComponent<T>(out result))
		{
			result = c.gameObject.AddComponent<T>();
		}
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000524BC File Offset: 0x000506BC
	public static bool GetComponentAndSetFieldIfNullElseLogAndDisable<T>(this Behaviour c, ref T fieldRef, string fieldName, string fieldTypeName, string msgSuffix = "Disabling.", [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : Component
	{
		if (c.GetComponentAndSetFieldIfNullElseLog(ref fieldRef, fieldName, fieldTypeName, msgSuffix, caller))
		{
			return true;
		}
		c.enabled = false;
		return false;
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x0013D368 File Offset: 0x0013B568
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

	// Token: 0x06003429 RID: 13353 RVA: 0x000524D7 File Offset: 0x000506D7
	public static bool DisableIfNull<T>(this Behaviour c, T fieldRef, string fieldName, string fieldTypeName, [CallerMemberName] string caller = "__UNKNOWN_CALLER__") where T : UnityEngine.Object
	{
		if (fieldRef != null)
		{
			return true;
		}
		c.enabled = false;
		return false;
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x000524F1 File Offset: 0x000506F1
	public static Hash128 ComputeStaticHash128(Component c, string k)
	{
		return ComponentUtils.ComputeStaticHash128(c, StaticHash.Compute(k));
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x0013D3FC File Offset: 0x0013B5FC
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

	// Token: 0x04003777 RID: 14199
	private static readonly uint[] kHashBits = new uint[4];
}
