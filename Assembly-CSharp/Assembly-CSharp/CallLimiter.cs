using System;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
[Serializable]
public class CallLimiter
{
	// Token: 0x06003203 RID: 12803 RVA: 0x00002050 File Offset: 0x00000250
	public CallLimiter()
	{
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x000F09D8 File Offset: 0x000EEBD8
	public CallLimiter(int historyLength, float coolDown, float latencyMax = 0.5f)
	{
		this.callTimeHistory = new float[historyLength];
		this.callHistoryLength = historyLength;
		for (int i = 0; i < historyLength; i++)
		{
			this.callTimeHistory[i] = float.MinValue;
		}
		this.timeCooldown = coolDown;
		this.maxLatency = (double)latencyMax;
	}

	// Token: 0x06003205 RID: 12805 RVA: 0x000F0A28 File Offset: 0x000EEC28
	public bool CheckCallServerTime(double time)
	{
		double simTime = NetworkSystem.Instance.SimTime;
		double num = this.maxLatency;
		double num2 = 4294967.295 - this.maxLatency;
		double num3;
		if (simTime > num || time < num)
		{
			if (time > simTime)
			{
				return false;
			}
			num3 = simTime - time;
		}
		else
		{
			double num4 = num2 + simTime;
			if (time > simTime && time < num4)
			{
				return false;
			}
			num3 = simTime + (4294967.295 - time);
		}
		if (num3 > this.maxLatency)
		{
			return false;
		}
		int num5 = (this.oldTimeIndex > 0) ? (this.oldTimeIndex - 1) : (this.callHistoryLength - 1);
		double num6 = (double)this.callTimeHistory[num5];
		if (num6 > num2 && time < num6)
		{
			this.Reset();
		}
		else if (time < num6)
		{
			return false;
		}
		return this.CheckCallTime((float)time);
	}

	// Token: 0x06003206 RID: 12806 RVA: 0x000F0AE0 File Offset: 0x000EECE0
	public virtual bool CheckCallTime(float time)
	{
		if (this.callTimeHistory[this.oldTimeIndex] > time)
		{
			this.blockCall = true;
			this.blockStartTime = time;
			return false;
		}
		this.callTimeHistory[this.oldTimeIndex] = time + this.timeCooldown;
		int num = this.oldTimeIndex + 1;
		this.oldTimeIndex = num;
		this.oldTimeIndex = num % this.callHistoryLength;
		this.blockCall = false;
		return true;
	}

	// Token: 0x06003207 RID: 12807 RVA: 0x000F0B48 File Offset: 0x000EED48
	public virtual void Reset()
	{
		if (this.callTimeHistory == null)
		{
			return;
		}
		for (int i = 0; i < this.callHistoryLength; i++)
		{
			this.callTimeHistory[i] = float.MinValue;
		}
		this.oldTimeIndex = 0;
		this.blockStartTime = 0f;
		this.blockCall = false;
	}

	// Token: 0x04003596 RID: 13718
	protected const double k_serverMaxTime = 4294967.295;

	// Token: 0x04003597 RID: 13719
	[SerializeField]
	protected float[] callTimeHistory;

	// Token: 0x04003598 RID: 13720
	[Space]
	[SerializeField]
	protected int callHistoryLength;

	// Token: 0x04003599 RID: 13721
	[SerializeField]
	protected float timeCooldown;

	// Token: 0x0400359A RID: 13722
	[SerializeField]
	protected double maxLatency;

	// Token: 0x0400359B RID: 13723
	private int oldTimeIndex;

	// Token: 0x0400359C RID: 13724
	protected bool blockCall;

	// Token: 0x0400359D RID: 13725
	protected float blockStartTime;
}
