using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x020007D7 RID: 2007
public static class PhotonUtils
{
	// Token: 0x060031AF RID: 12719 RVA: 0x00132020 File Offset: 0x00130220
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

	// Token: 0x060031B0 RID: 12720 RVA: 0x001320F8 File Offset: 0x001302F8
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

	// Token: 0x060031B1 RID: 12721 RVA: 0x001321C0 File Offset: 0x001303C0
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

	// Token: 0x060031B2 RID: 12722 RVA: 0x00132274 File Offset: 0x00130474
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

	// Token: 0x060031B3 RID: 12723 RVA: 0x00132318 File Offset: 0x00130518
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

	// Token: 0x060031B4 RID: 12724 RVA: 0x001323AC File Offset: 0x001305AC
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

	// Token: 0x060031B5 RID: 12725 RVA: 0x0013242C File Offset: 0x0013062C
	public static void ParseArgs<T1, T2, T3, T4, T5, T6>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5, out T6 arg6)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
		arg6 = (T6)((object)args[startIndex + 5]);
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x0013249C File Offset: 0x0013069C
	public static void ParseArgs<T1, T2, T3, T4, T5>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4, out T5 arg5)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
		arg5 = (T5)((object)args[startIndex + 4]);
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x001324FC File Offset: 0x001306FC
	public static void ParseArgs<T1, T2, T3, T4>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3, out T4 arg4)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
		arg4 = (T4)((object)args[startIndex + 3]);
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x0005018E File Offset: 0x0004E38E
	public static void ParseArgs<T1, T2, T3>(this object[] args, int startIndex, out T1 arg1, out T2 arg2, out T3 arg3)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
		arg3 = (T3)((object)args[startIndex + 2]);
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000501BF File Offset: 0x0004E3BF
	public static void ParseArgs<T1, T2>(this object[] args, int startIndex, out T1 arg1, out T2 arg2)
	{
		arg1 = (T1)((object)args[startIndex]);
		arg2 = (T2)((object)args[startIndex + 1]);
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x000501DF File Offset: 0x0004E3DF
	public static void ParseArgs<T1>(this object[] args, int startIndex, out T1 arg1)
	{
		arg1 = (T1)((object)args[startIndex]);
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x0013254C File Offset: 0x0013074C
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

	// Token: 0x060031BC RID: 12732 RVA: 0x00132600 File Offset: 0x00130800
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

	// Token: 0x060031BD RID: 12733 RVA: 0x001326A8 File Offset: 0x001308A8
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

	// Token: 0x060031BE RID: 12734 RVA: 0x00132748 File Offset: 0x00130948
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

	// Token: 0x060031BF RID: 12735 RVA: 0x001327DC File Offset: 0x001309DC
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

	// Token: 0x060031C0 RID: 12736 RVA: 0x00132864 File Offset: 0x00130A64
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

	// Token: 0x060031C1 RID: 12737 RVA: 0x001328E4 File Offset: 0x00130AE4
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

	// Token: 0x060031C2 RID: 12738 RVA: 0x00132958 File Offset: 0x00130B58
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

	// Token: 0x060031C3 RID: 12739 RVA: 0x001329C4 File Offset: 0x00130BC4
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

	// Token: 0x060031C4 RID: 12740 RVA: 0x00132A24 File Offset: 0x00130C24
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

	// Token: 0x060031C5 RID: 12741 RVA: 0x00132A7C File Offset: 0x00130C7C
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

	// Token: 0x060031C6 RID: 12742 RVA: 0x00132AC8 File Offset: 0x00130CC8
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

	// Token: 0x060031C7 RID: 12743 RVA: 0x000501EF File Offset: 0x0004E3EF
	public static ref readonly T[] FetchDelegatesNonAlloc<T>(T @delegate) where T : MulticastDelegate
	{
		if (@delegate == null)
		{
			return PhotonUtils.EmptyArray<T>.Ref();
		}
		return @delegate.GetInvocationListUnsafe<T>();
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x00132B0C File Offset: 0x00130D0C
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

	// Token: 0x060031C9 RID: 12745 RVA: 0x00132B4C File Offset: 0x00130D4C
	public static NetPlayer GetNetPlayer(int actorNumber)
	{
		NetworkSystem networkSystem;
		if (!PhotonUtils.TryGetNetSystem(out networkSystem))
		{
			return null;
		}
		return networkSystem.GetPlayer(actorNumber);
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x060031CA RID: 12746 RVA: 0x00132B6C File Offset: 0x00130D6C
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

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x060031CB RID: 12747 RVA: 0x00132B8C File Offset: 0x00130D8C
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

	// Token: 0x060031CC RID: 12748 RVA: 0x0005020B File Offset: 0x0004E40B
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

	// Token: 0x060031CD RID: 12749 RVA: 0x00132BC0 File Offset: 0x00130DC0
	static PhotonUtils()
	{
		for (int i = 0; i <= 16; i++)
		{
			PhotonUtils.gLengthToArgsArray.Add(i, new object[i]);
		}
	}

	// Token: 0x0400355D RID: 13661
	private static NetworkSystem gNetSystem;

	// Token: 0x0400355E RID: 13662
	private static NetPlayer gLocalNetPlayer;

	// Token: 0x0400355F RID: 13663
	private static readonly Dictionary<int, object[]> gLengthToArgsArray = new Dictionary<int, object[]>(16);

	// Token: 0x04003560 RID: 13664
	private const int ARG_ARRAYS = 16;

	// Token: 0x020007D8 RID: 2008
	private static class EmptyArray<T>
	{
		// Token: 0x060031CE RID: 12750 RVA: 0x0005023C File Offset: 0x0004E43C
		public static ref readonly T[] Ref()
		{
			return ref PhotonUtils.EmptyArray<T>.gEmpty;
		}

		// Token: 0x04003561 RID: 13665
		private static readonly T[] gEmpty = Array.Empty<T>();
	}

	// Token: 0x020007D9 RID: 2009
	public static class CustomTypes
	{
		// Token: 0x060031D0 RID: 12752 RVA: 0x0005024F File Offset: 0x0004E44F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitOnLoad()
		{
			PhotonPeer.RegisterType(typeof(Color32), 67, new SerializeMethod(PhotonUtils.CustomTypes.SerializeColor32), new DeserializeMethod(PhotonUtils.CustomTypes.DeserializeColor32));
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x0005027B File Offset: 0x0004E47B
		public static byte[] SerializeColor32(object value)
		{
			return PhotonUtils.CustomTypes.CastToBytes<Color32>((Color32)value);
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x00050288 File Offset: 0x0004E488
		public static object DeserializeColor32(byte[] data)
		{
			return PhotonUtils.CustomTypes.CastToStruct<Color32>(data);
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x00132BF8 File Offset: 0x00130DF8
		private static T CastToStruct<T>(byte[] bytes) where T : struct
		{
			GCHandle gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			T result = Marshal.PtrToStructure<T>(gchandle.AddrOfPinnedObject());
			gchandle.Free();
			return result;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x00132C20 File Offset: 0x00130E20
		private static byte[] CastToBytes<T>(T data) where T : struct
		{
			byte[] array = new byte[Marshal.SizeOf<T>()];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr ptr = gchandle.AddrOfPinnedObject();
			Marshal.StructureToPtr<T>(data, ptr, true);
			gchandle.Free();
			return array;
		}

		// Token: 0x04003562 RID: 13666
		private const short LEN_C32 = 4;
	}
}
