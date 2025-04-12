using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000543 RID: 1347
internal class RPCUtil
{
	// Token: 0x060020CA RID: 8394 RVA: 0x000F1FB4 File Offset: 0x000F01B4
	public static bool NotSpam(string id, PhotonMessageInfoWrapped info, float delay)
	{
		RPCUtil.RPCCallID key = new RPCUtil.RPCCallID(id, info.senderID);
		if (!RPCUtil.RPCCallLog.ContainsKey(key))
		{
			RPCUtil.RPCCallLog.Add(key, Time.time);
			return true;
		}
		if (Time.time - RPCUtil.RPCCallLog[key] > delay)
		{
			RPCUtil.RPCCallLog[key] = Time.time;
			return true;
		}
		return false;
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x00045520 File Offset: 0x00043720
	public static bool SafeValue(float v)
	{
		return !float.IsNaN(v) && float.IsFinite(v);
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x00045532 File Offset: 0x00043732
	public static bool SafeValue(float v, float min, float max)
	{
		return RPCUtil.SafeValue(v) && v <= max && v >= min;
	}

	// Token: 0x040024AA RID: 9386
	private static Dictionary<RPCUtil.RPCCallID, float> RPCCallLog = new Dictionary<RPCUtil.RPCCallID, float>();

	// Token: 0x02000544 RID: 1348
	private struct RPCCallID : IEquatable<RPCUtil.RPCCallID>
	{
		// Token: 0x060020CF RID: 8399 RVA: 0x00045557 File Offset: 0x00043757
		public RPCCallID(string nameOfFunction, int senderId)
		{
			this._senderID = senderId;
			this._nameOfFunction = nameOfFunction;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060020D0 RID: 8400 RVA: 0x00045567 File Offset: 0x00043767
		public readonly int SenderID
		{
			get
			{
				return this._senderID;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x0004556F File Offset: 0x0004376F
		public readonly string NameOfFunction
		{
			get
			{
				return this._nameOfFunction;
			}
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000F2018 File Offset: 0x000F0218
		bool IEquatable<RPCUtil.RPCCallID>.Equals(RPCUtil.RPCCallID other)
		{
			return other.NameOfFunction.Equals(this.NameOfFunction) && other.SenderID.Equals(this.SenderID);
		}

		// Token: 0x040024AB RID: 9387
		private int _senderID;

		// Token: 0x040024AC RID: 9388
		private string _nameOfFunction;
	}
}
