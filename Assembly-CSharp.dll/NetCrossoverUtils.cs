using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200027A RID: 634
public static class NetCrossoverUtils
{
	// Token: 0x06000EDD RID: 3805 RVA: 0x00039756 File Offset: 0x00037956
	public static void Prewarm()
	{
		NetCrossoverUtils.FixedBuffer = new byte[2048];
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x000A58D4 File Offset: 0x000A3AD4
	public static void WriteNetDataToBuffer<T>(this T data, PhotonStream stream) where T : struct, INetworkStruct
	{
		if (stream.IsReading)
		{
			Debug.LogError("Attempted to write data to a reading stream!");
			return;
		}
		IntPtr intPtr = 0;
		try
		{
			int num = Marshal.SizeOf(typeof(T));
			intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr<T>(data, intPtr, true);
			Marshal.Copy(intPtr, NetCrossoverUtils.FixedBuffer, 0, num);
			stream.SendNext(num);
			for (int i = 0; i < num; i++)
			{
				stream.SendNext(NetCrossoverUtils.FixedBuffer[i]);
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x000A596C File Offset: 0x000A3B6C
	public static object ReadNetDataFromBuffer<T>(PhotonStream stream) where T : struct, INetworkStruct
	{
		if (stream.IsWriting)
		{
			Debug.LogError("Attmpted to read data from a writing stream!");
			return null;
		}
		IntPtr intPtr = 0;
		object result;
		try
		{
			Type typeFromHandle = typeof(T);
			int num = (int)stream.ReceiveNext();
			int num2 = Marshal.SizeOf(typeFromHandle);
			if (num != num2)
			{
				Debug.LogError(string.Format("Expected datasize {0} when reading data for type '{1}',", num2, typeFromHandle.Name) + string.Format("but {0} data is available!", num));
				result = null;
			}
			else
			{
				intPtr = Marshal.AllocHGlobal(num2);
				for (int i = 0; i < num2; i++)
				{
					NetCrossoverUtils.FixedBuffer[i] = (byte)stream.ReceiveNext();
				}
				Marshal.Copy(NetCrossoverUtils.FixedBuffer, 0, intPtr, num2);
				result = (T)((object)Marshal.PtrToStructure(intPtr, typeFromHandle));
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return result;
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x000A5A54 File Offset: 0x000A3C54
	public static void WriteNetDataToBuffer(this object data, PhotonStream stream)
	{
		if (stream.IsReading)
		{
			Debug.LogError("Attempted to write data to a reading stream!");
			return;
		}
		IntPtr intPtr = 0;
		try
		{
			int num = Marshal.SizeOf(data.GetType());
			intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(data, intPtr, true);
			Marshal.Copy(intPtr, NetCrossoverUtils.FixedBuffer, 0, num);
			stream.SendNext(num);
			for (int i = 0; i < num; i++)
			{
				stream.SendNext(NetCrossoverUtils.FixedBuffer[i]);
			}
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x000A5AE8 File Offset: 0x000A3CE8
	public static void SerializeToRPCData<T>(this RPCArgBuffer<T> argBuffer) where T : struct
	{
		IntPtr intPtr = 0;
		try
		{
			int num = Marshal.SizeOf(typeof(T));
			intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr<T>(argBuffer.Args, intPtr, true);
			Marshal.Copy(intPtr, argBuffer.Data, 0, num);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x000A5B48 File Offset: 0x000A3D48
	public static void PopulateWithRPCData<T>(this RPCArgBuffer<T> argBuffer, byte[] data) where T : struct
	{
		IntPtr intPtr = 0;
		try
		{
			int num = Marshal.SizeOf(typeof(T));
			intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(data, 0, intPtr, num);
			argBuffer.Args = Marshal.PtrToStructure<T>(intPtr);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x000A5BA4 File Offset: 0x000A3DA4
	public static Dictionary<string, SessionProperty> ToPropDict(this ExitGames.Client.Photon.Hashtable hash)
	{
		Dictionary<string, SessionProperty> dictionary = new Dictionary<string, SessionProperty>();
		foreach (DictionaryEntry dictionaryEntry in hash)
		{
			dictionary.Add((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
		}
		return dictionary;
	}

	// Token: 0x0400119A RID: 4506
	private const int MaxParameterByteLength = 2048;

	// Token: 0x0400119B RID: 4507
	private static byte[] FixedBuffer;
}
