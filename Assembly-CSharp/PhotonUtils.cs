using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x020007D6 RID: 2006
public static class PhotonUtils
{
	// Token: 0x060031A7 RID: 12711 RVA: 0x000EED24 File Offset: 0x000ECF24
	public static void ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9, out T10 arg10, out T11 arg11, out T12 arg12)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
		arg7 = (T7)((object)args[startIndex + 6]);
		arg8 = (T8)((object)args[startIndex + 7]);
		arg9 = (T9)((object)args[startIndex + 8]);
		arg10 = (T10)((object)args[startIndex + 9]);
		arg11 = (T11)((object)args[startIndex + 10]);
		arg12 = (T12)((object)args[startIndex + 11]);
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x000EEDFC File Offset: 0x000ECFFC
	public static void ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9, out T10 arg10, out T11 arg11)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
		arg7 = (T7)((object)args[startIndex + 6]);
		arg8 = (T8)((object)args[startIndex + 7]);
		arg9 = (T9)((object)args[startIndex + 8]);
		arg10 = (T10)((object)args[startIndex + 9]);
		arg11 = (T11)((object)args[startIndex + 10]);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x000EEEC4 File Offset: 0x000ED0C4
	public static void ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9, out T10 arg10)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
		arg7 = (T7)((object)args[startIndex + 6]);
		arg8 = (T8)((object)args[startIndex + 7]);
		arg9 = (T9)((object)args[startIndex + 8]);
		arg10 = (T10)((object)args[startIndex + 9]);
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x000EEF78 File Offset: 0x000ED178
	public static void ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
		arg7 = (T7)((object)args[startIndex + 6]);
		arg8 = (T8)((object)args[startIndex + 7]);
		arg9 = (T9)((object)args[startIndex + 8]);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x000EF01C File Offset: 0x000ED21C
	public static void ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
		arg7 = (T7)((object)args[startIndex + 6]);
		arg8 = (T8)((object)args[startIndex + 7]);
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x000EF0B0 File Offset: 0x000ED2B0
	public static void ParseArgs<T1, T2, T3, T4, T5, T6, T7>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
		arg7 = (T7)((object)args[startIndex + 6]);
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x000EF130 File Offset: 0x000ED330
	public static void ParseArgs<T1, T2, T3, T4, T5, T6>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x000EF1A0 File Offset: 0x000ED3A0
	public static void ParseArgs<T1, T2, T3, T4, T5>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x000EF200 File Offset: 0x000ED400
	public static void ParseArgs<T1, T2, T3, T4>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x000EF24D File Offset: 0x000ED44D
	public static void ParseArgs<T1, T2, T3>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x000EF27E File Offset: 0x000ED47E
	public static void ParseArgs<T1, T2>(this object[] args, int startIndex, out T1 arg1, out T2 arg2)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x000EF29E File Offset: 0x000ED49E
	public static void ParseArgs<T1>(this object[] args, int startIndex, out T1 arg1)
	{
		arg1 = (T1)((object)args[startIndex]);
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x000EF2B0 File Offset: 0x000ED4B0
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9, out T10 arg10, out T11 arg11, out T12 arg12)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		arg7 = default(T7);
		arg8 = default(T8);
		arg9 = default(T9);
		arg10 = default(T10);
		arg11 = default(T11);
		arg12 = default(T12);
		if (args == null || args.Length < startIndex + 12)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6, out arg7, out arg8, out arg9, out arg10, out arg11, out arg12);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x000EF364 File Offset: 0x000ED564
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9, out T10 arg10, out T11 arg11)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		arg7 = default(T7);
		arg8 = default(T8);
		arg9 = default(T9);
		arg10 = default(T10);
		arg11 = default(T11);
		if (args == null || args.Length < startIndex + 11)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6, out arg7, out arg8, out arg9, out arg10, out arg11);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x000EF40C File Offset: 0x000ED60C
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9, out T10 arg10)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		arg7 = default(T7);
		arg8 = default(T8);
		arg9 = default(T9);
		arg10 = default(T10);
		if (args == null || args.Length < startIndex + 10)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6, out arg7, out arg8, out arg9, out arg10);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x000EF4AC File Offset: 0x000ED6AC
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8, out T9 arg9)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		arg7 = default(T7);
		arg8 = default(T8);
		arg9 = default(T9);
		if (args == null || args.Length < startIndex + 9)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6, out arg7, out arg8, out arg9);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x000EF540 File Offset: 0x000ED740
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6, T7, T8>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7, out T8 arg8)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		arg7 = default(T7);
		arg8 = default(T8);
		if (args == null || args.Length < startIndex + 8)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6, out arg7, out arg8);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x000EF5C8 File Offset: 0x000ED7C8
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6, T7>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6, out T7 arg7)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		arg7 = default(T7);
		if (args == null || args.Length < startIndex + 7)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6, out arg7);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000EF648 File Offset: 0x000ED848
	public static bool TryParseArgs<T1, T2, T3, T4, T5, T6>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		arg6 = default(T6);
		if (args == null || args.Length < startIndex + 6)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5, out arg6);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x000EF6BC File Offset: 0x000ED8BC
	public static bool TryParseArgs<T1, T2, T3, T4, T5>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		arg5 = default(T5);
		if (args == null || args.Length < startIndex + 5)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4, out arg5);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x000EF728 File Offset: 0x000ED928
	public static bool TryParseArgs<T1, T2, T3, T4>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		arg4 = default(T4);
		if (args == null || args.Length < startIndex + 4)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3, out arg4);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x000EF788 File Offset: 0x000ED988
	public static bool TryParseArgs<T1, T2, T3>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		arg3 = default(T3);
		if (args == null || args.Length < startIndex + 3)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2, out arg3);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031BD RID: 12733 RVA: 0x000EF7E0 File Offset: 0x000ED9E0
	public static bool TryParseArgs<T1, T2>(this object[] args, int startIndex, out T1 arg1, out T2 arg2)
	{
		arg1 = default(T1);
		arg2 = default(T2);
		if (args == null || args.Length < startIndex + 2)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1, out arg2);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x000EF82C File Offset: 0x000EDA2C
	public static bool TryParseArgs<T1>(this object[] args, int startIndex, out T1 arg1)
	{
		arg1 = default(T1);
		if (args == null || args.Length < startIndex + 1)
		{
			return false;
		}
		try
		{
			args.ParseArgs(startIndex, out arg1);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x000EF870 File Offset: 0x000EDA70
	public static ref readonly T[] FetchDelegatesNonAlloc<T>(T @delegate) where T : MulticastDelegate
	{
		if (@delegate == null)
		{
			return PhotonUtils.EmptyArray<T>.Ref();
		}
		return @delegate.GetInvocationListUnsafe<T>();
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000EF88C File Offset: 0x000EDA8C
	public static object[] FetchScratchArray(int size)
	{
		if (size < 0)
		{
			throw new Exception("Size cannot be less than 0.");
		}
		object[] array;
		if (!PhotonUtils.gLengthToArgsArray.TryGetValue(size, out array))
		{
			array = new object[size];
			PhotonUtils.gLengthToArgsArray.Add(size, array);
		}
		return array;
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000EF8CC File Offset: 0x000EDACC
	public static NetPlayer GetNetPlayer(int actorNumber)
	{
		NetworkSystem networkSystem;
		if (!PhotonUtils.TryGetNetSystem(out networkSystem))
		{
			return null;
		}
		return networkSystem.GetPlayer(actorNumber);
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000EF8EC File Offset: 0x000EDAEC
	public static int LocalActorNumber
	{
		get
		{
			NetPlayer localNetPlayer = PhotonUtils.LocalNetPlayer;
			if (localNetPlayer == null)
			{
				return -1;
			}
			return localNetPlayer.ActorNumber;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x060031C3 RID: 12739 RVA: 0x000EF90C File Offset: 0x000EDB0C
	public static NetPlayer LocalNetPlayer
	{
		get
		{
			if (PhotonUtils.gLocalNetPlayer != null)
			{
				return PhotonUtils.gLocalNetPlayer;
			}
			NetworkSystem networkSystem;
			if (PhotonUtils.TryGetNetSystem(out networkSystem))
			{
				PhotonUtils.gLocalNetPlayer = networkSystem.GetLocalPlayer();
			}
			return PhotonUtils.gLocalNetPlayer;
		}
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000EF93F File Offset: 0x000EDB3F
	private static bool TryGetNetSystem(out NetworkSystem ns)
	{
		if (!PhotonUtils.gNetSystem)
		{
			PhotonUtils.gNetSystem = NetworkSystem.Instance;
		}
		if (!PhotonUtils.gNetSystem)
		{
			ns = null;
			return false;
		}
		ns = PhotonUtils.gNetSystem;
		return true;
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x000EF970 File Offset: 0x000EDB70
	static PhotonUtils()
	{
		for (int i = 0; i <= 16; i++)
		{
			PhotonUtils.gLengthToArgsArray.Add(i, new object[i]);
		}
	}

	// Token: 0x04003557 RID: 13655
	private static NetworkSystem gNetSystem;

	// Token: 0x04003558 RID: 13656
	private static NetPlayer gLocalNetPlayer;

	// Token: 0x04003559 RID: 13657
	private static readonly Dictionary<int, object[]> gLengthToArgsArray = new Dictionary<int, object[]>(16);

	// Token: 0x0400355A RID: 13658
	private const int ARG_ARRAYS = 16;

	// Token: 0x020007D7 RID: 2007
	private static class EmptyArray<T>
	{
		// Token: 0x060031C6 RID: 12742 RVA: 0x000EF9A7 File Offset: 0x000EDBA7
		public static ref readonly T[] Ref()
		{
			return ref PhotonUtils.EmptyArray<T>.gEmpty;
		}

		// Token: 0x0400355B RID: 13659
		private static readonly T[] gEmpty = Array.Empty<T>();
	}

	// Token: 0x020007D8 RID: 2008
	public static class CustomTypes
	{
		// Token: 0x060031C8 RID: 12744 RVA: 0x000EF9BA File Offset: 0x000EDBBA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitOnLoad()
		{
			PhotonPeer.RegisterType(typeof(Color32), 67, new SerializeMethod(PhotonUtils.CustomTypes.SerializeColor32), new DeserializeMethod(PhotonUtils.CustomTypes.DeserializeColor32));
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000EF9E6 File Offset: 0x000EDBE6
		public static byte[] SerializeColor32(object value)
		{
			return PhotonUtils.CustomTypes.CastToBytes<Color32>((Color32)value);
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000EF9F3 File Offset: 0x000EDBF3
		public static object DeserializeColor32(byte[] data)
		{
			return PhotonUtils.CustomTypes.CastToStruct<Color32>(data);
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x000EFA00 File Offset: 0x000EDC00
		private static T CastToStruct<T>(byte[] bytes) where T : struct
		{
			GCHandle gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			T result = Marshal.PtrToStructure<T>(gchandle.AddrOfPinnedObject());
			gchandle.Free();
			return result;
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x000EFA28 File Offset: 0x000EDC28
		private static byte[] CastToBytes<T>(T data) where T : struct
		{
			byte[] array = new byte[Marshal.SizeOf<T>()];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr ptr = gchandle.AddrOfPinnedObject();
			Marshal.StructureToPtr<T>(data, ptr, true);
			gchandle.Free();
			return array;
		}

		// Token: 0x0400355C RID: 13660
		private const short LEN_C32 = 4;
	}
}
