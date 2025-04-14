using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000542 RID: 1346
internal class RPCUtil
{
	// Token: 0x060020C2 RID: 8386 RVA: 0x000A3F84 File Offset: 0x000A2184
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

	// Token: 0x060020C3 RID: 8387 RVA: 0x000A3FE5 File Offset: 0x000A21E5
	public static bool SafeValue(float v)
	{
		return !float.IsNaN(v) && float.IsFinite(v);
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000A3FF7 File Offset: 0x000A21F7
	public static bool SafeValue(float v, float min, float max)
	{
		return RPCUtil.SafeValue(v) && v <= max && v >= min;
	}

	// Token: 0x040024A4 RID: 9380
	private static Dictionary<RPCUtil.RPCCallID, float> RPCCallLog = new Dictionary<RPCUtil.RPCCallID, float>();

	// Token: 0x02000543 RID: 1347
	private struct RPCCallID : IEquatable<RPCUtil.RPCCallID>
	{
		// Token: 0x060020C7 RID: 8391 RVA: 0x000A401C File Offset: 0x000A221C
		public RPCCallID(string nameOfFunction, int senderId)
		{
			this._senderID = senderId;
			this._nameOfFunction = nameOfFunction;
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x000A402C File Offset: 0x000A222C
		public readonly int SenderID
		{
			get
			{
				return this._senderID;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000A4034 File Offset: 0x000A2234
		public readonly string NameOfFunction
		{
			get
			{
				return this._nameOfFunction;
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000A403C File Offset: 0x000A223C
		bool IEquatable<RPCUtil.RPCCallID>.Equals(RPCUtil.RPCCallID other)
		{
			return other.NameOfFunction.Equals(this.NameOfFunction) && other.SenderID.Equals(this.SenderID);
		}

		// Token: 0x040024A5 RID: 9381
		private int _senderID;

		// Token: 0x040024A6 RID: 9382
		private string _nameOfFunction;
	}
}
