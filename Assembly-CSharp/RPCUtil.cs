using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000550 RID: 1360
internal class RPCUtil
{
	// Token: 0x06002120 RID: 8480 RVA: 0x000F4D38 File Offset: 0x000F2F38
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

	// Token: 0x06002121 RID: 8481 RVA: 0x000468BF File Offset: 0x00044ABF
	public static bool SafeValue(float v)
	{
		return !float.IsNaN(v) && float.IsFinite(v);
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000468D1 File Offset: 0x00044AD1
	public static bool SafeValue(float v, float min, float max)
	{
		return RPCUtil.SafeValue(v) && v <= max && v >= min;
	}

	// Token: 0x040024FC RID: 9468
	private static Dictionary<RPCUtil.RPCCallID, float> RPCCallLog = new Dictionary<RPCUtil.RPCCallID, float>();

	// Token: 0x02000551 RID: 1361
	private struct RPCCallID : IEquatable<RPCUtil.RPCCallID>
	{
		// Token: 0x06002125 RID: 8485 RVA: 0x000468F6 File Offset: 0x00044AF6
		public RPCCallID(string nameOfFunction, int senderId)
		{
			this._senderID = senderId;
			this._nameOfFunction = nameOfFunction;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06002126 RID: 8486 RVA: 0x00046906 File Offset: 0x00044B06
		public readonly int SenderID
		{
			get
			{
				return this._senderID;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06002127 RID: 8487 RVA: 0x0004690E File Offset: 0x00044B0E
		public readonly string NameOfFunction
		{
			get
			{
				return this._nameOfFunction;
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000F4D9C File Offset: 0x000F2F9C
		bool IEquatable<RPCUtil.RPCCallID>.Equals(RPCUtil.RPCCallID other)
		{
			return other.NameOfFunction.Equals(this.NameOfFunction) && other.SenderID.Equals(this.SenderID);
		}

		// Token: 0x040024FD RID: 9469
		private int _senderID;

		// Token: 0x040024FE RID: 9470
		private string _nameOfFunction;
	}
}
