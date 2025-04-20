using System;
using UnityEngine;

// Token: 0x020007FC RID: 2044
[Serializable]
public class CallLimiter
{
	// Token: 0x060032AD RID: 12973 RVA: 0x00030490 File Offset: 0x0002E690
	public CallLimiter()
	{
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x001387A8 File Offset: 0x001369A8
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

	// Token: 0x060032AF RID: 12975 RVA: 0x001387F8 File Offset: 0x001369F8
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

	// Token: 0x060032B0 RID: 12976 RVA: 0x001388B0 File Offset: 0x00136AB0
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

	// Token: 0x060032B1 RID: 12977 RVA: 0x00138918 File Offset: 0x00136B18
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

	// Token: 0x0400363A RID: 13882
	protected const double k_serverMaxTime = 4294967.295;

	// Token: 0x0400363B RID: 13883
	[SerializeField]
	protected float[] callTimeHistory;

	// Token: 0x0400363C RID: 13884
	[Space]
	[SerializeField]
	protected int callHistoryLength;

	// Token: 0x0400363D RID: 13885
	[SerializeField]
	protected float timeCooldown;

	// Token: 0x0400363E RID: 13886
	[SerializeField]
	protected double maxLatency;

	// Token: 0x0400363F RID: 13887
	private int oldTimeIndex;

	// Token: 0x04003640 RID: 13888
	protected bool blockCall;

	// Token: 0x04003641 RID: 13889
	protected float blockStartTime;
}
