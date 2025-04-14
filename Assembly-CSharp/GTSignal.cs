using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public static class GTSignal
{
	// Token: 0x060023EC RID: 9196 RVA: 0x000B317C File Offset: 0x000B137C
	private static void _Emit(GTSignal.EmitMode mode, int signalID, object[] data)
	{
		object[] eventContent = GTSignal._ToEventContent(signalID, PhotonNetwork.Time, data);
		PhotonNetwork.RaiseEvent(186, eventContent, GTSignal.gTargetsToOptions[mode], GTSignal.gSendOptions);
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000B31B4 File Offset: 0x000B13B4
	private static void _Emit(int[] targetActors, int signalID, object[] data)
	{
		if (targetActors.IsNullOrEmpty<int>())
		{
			return;
		}
		GTSignal.gCustomTargetOptions.TargetActors = targetActors;
		object[] eventContent = GTSignal._ToEventContent(signalID, PhotonNetwork.Time, data);
		PhotonNetwork.RaiseEvent(186, eventContent, GTSignal.gCustomTargetOptions, GTSignal.gSendOptions);
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000B31F8 File Offset: 0x000B13F8
	private static object[] _ToEventContent(int signalID, double time, object[] data)
	{
		int num = data.Length;
		int num2 = num + 2;
		object[] array;
		if (!GTSignal.gLengthToContentArray.TryGetValue(num2, out array))
		{
			array = new object[num2];
			GTSignal.gLengthToContentArray.Add(num2, array);
		}
		array[0] = signalID;
		array[1] = time;
		for (int i = 0; i < num; i++)
		{
			array[i + 2] = data[i];
		}
		return array;
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000B3256 File Offset: 0x000B1456
	public static int ComputeID(string s)
	{
		if (!string.IsNullOrWhiteSpace(s))
		{
			return XXHash32.Compute(s.Trim(), 0U);
		}
		return 0;
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000B3270 File Offset: 0x000B1470
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
		GTSignal.gCustomTargetOptions = RaiseEventOptions.Default;
		GTSignal.gSendOptions = SendOptions.SendReliable;
		GTSignal.gSendOptions.Encrypt = true;
		GTSignal.gTargetsToOptions = new Dictionary<GTSignal.EmitMode, RaiseEventOptions>(3);
		RaiseEventOptions @default = RaiseEventOptions.Default;
		@default.Receivers = ReceiverGroup.All;
		GTSignal.gTargetsToOptions.Add(GTSignal.EmitMode.All, @default);
		RaiseEventOptions default2 = RaiseEventOptions.Default;
		default2.Receivers = ReceiverGroup.Others;
		GTSignal.gTargetsToOptions.Add(GTSignal.EmitMode.Others, default2);
		RaiseEventOptions default3 = RaiseEventOptions.Default;
		default3.Receivers = ReceiverGroup.MasterClient;
		GTSignal.gTargetsToOptions.Add(GTSignal.EmitMode.Host, default3);
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000B32F2 File Offset: 0x000B14F2
	public static void Emit(string signal, params object[] data)
	{
		GTSignal._Emit(GTSignal.EmitMode.All, GTSignal.ComputeID(signal), data);
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000B3301 File Offset: 0x000B1501
	public static void Emit(GTSignal.EmitMode mode, string signal, params object[] data)
	{
		GTSignal._Emit(mode, GTSignal.ComputeID(signal), data);
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000B3310 File Offset: 0x000B1510
	public static void Emit(int signalID, params object[] data)
	{
		GTSignal._Emit(GTSignal.EmitMode.All, signalID, data);
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000B331A File Offset: 0x000B151A
	public static void Emit(GTSignal.EmitMode mode, int signalID, params object[] data)
	{
		GTSignal._Emit(mode, signalID, data);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000B3324 File Offset: 0x000B1524
	public static void Emit(int target, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[1];
		array[0] = target;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000B333C File Offset: 0x000B153C
	public static void Emit(int target1, int target2, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[2];
		array[0] = target1;
		array[1] = target2;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000B3358 File Offset: 0x000B1558
	public static void Emit(int target1, int target2, int target3, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[3];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000B3379 File Offset: 0x000B1579
	public static void Emit(int target1, int target2, int target3, int target4, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[4];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000B339F File Offset: 0x000B159F
	public static void Emit(int target1, int target2, int target3, int target4, int target5, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[5];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		array[4] = target5;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000B33CA File Offset: 0x000B15CA
	public static void Emit(int target1, int target2, int target3, int target4, int target5, int target6, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[6];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		array[4] = target5;
		array[5] = target6;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000B33FA File Offset: 0x000B15FA
	public static void Emit(int target1, int target2, int target3, int target4, int target5, int target6, int target7, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[7];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		array[4] = target5;
		array[5] = target6;
		array[6] = target7;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000B342F File Offset: 0x000B162F
	public static void Emit(int target1, int target2, int target3, int target4, int target5, int target6, int target7, int target8, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[8];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		array[4] = target5;
		array[5] = target6;
		array[6] = target7;
		array[7] = target8;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000B3469 File Offset: 0x000B1669
	public static void Emit(int target1, int target2, int target3, int target4, int target5, int target6, int target7, int target8, int target9, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[9];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		array[4] = target5;
		array[5] = target6;
		array[6] = target7;
		array[7] = target8;
		array[8] = target9;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000B34AC File Offset: 0x000B16AC
	public static void Emit(int target1, int target2, int target3, int target4, int target5, int target6, int target7, int target8, int target9, int target10, int signalID, params object[] data)
	{
		int[] array = GTSignal.gLengthToTargetsArray[10];
		array[0] = target1;
		array[1] = target2;
		array[2] = target3;
		array[3] = target4;
		array[4] = target5;
		array[5] = target6;
		array[6] = target7;
		array[7] = target8;
		array[8] = target9;
		array[9] = target10;
		GTSignal._Emit(array, signalID, data);
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000B3500 File Offset: 0x000B1700
	// Note: this type is marked as 'beforefieldinit'.
	static GTSignal()
	{
		Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
		dictionary[1] = new object[1];
		dictionary[2] = new object[2];
		dictionary[3] = new object[3];
		dictionary[4] = new object[4];
		dictionary[5] = new object[5];
		dictionary[6] = new object[6];
		dictionary[7] = new object[7];
		dictionary[8] = new object[8];
		dictionary[9] = new object[9];
		dictionary[10] = new object[10];
		dictionary[11] = new object[11];
		dictionary[12] = new object[12];
		dictionary[13] = new object[13];
		dictionary[14] = new object[14];
		dictionary[15] = new object[15];
		dictionary[16] = new object[16];
		GTSignal.gLengthToContentArray = dictionary;
		Dictionary<int, int[]> dictionary2 = new Dictionary<int, int[]>();
		dictionary2[1] = new int[1];
		dictionary2[2] = new int[2];
		dictionary2[3] = new int[3];
		dictionary2[4] = new int[4];
		dictionary2[5] = new int[5];
		dictionary2[6] = new int[6];
		dictionary2[7] = new int[7];
		dictionary2[8] = new int[8];
		dictionary2[9] = new int[9];
		dictionary2[10] = new int[10];
		GTSignal.gLengthToTargetsArray = dictionary2;
	}

	// Token: 0x040027EB RID: 10219
	public const byte PHOTON_CODE = 186;

	// Token: 0x040027EC RID: 10220
	private static Dictionary<GTSignal.EmitMode, RaiseEventOptions> gTargetsToOptions;

	// Token: 0x040027ED RID: 10221
	private static Dictionary<int, object[]> gLengthToContentArray;

	// Token: 0x040027EE RID: 10222
	private static Dictionary<int, int[]> gLengthToTargetsArray;

	// Token: 0x040027EF RID: 10223
	private static SendOptions gSendOptions;

	// Token: 0x040027F0 RID: 10224
	private static RaiseEventOptions gCustomTargetOptions;

	// Token: 0x020005AA RID: 1450
	public enum EmitMode
	{
		// Token: 0x040027F2 RID: 10226
		None = -1,
		// Token: 0x040027F3 RID: 10227
		Others,
		// Token: 0x040027F4 RID: 10228
		Targets,
		// Token: 0x040027F5 RID: 10229
		All,
		// Token: 0x040027F6 RID: 10230
		Host
	}
}
