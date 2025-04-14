using System;
using UnityEngine;

// Token: 0x020007E2 RID: 2018
[Serializable]
public class CallLimiter
{
	// Token: 0x060031F7 RID: 12791 RVA: 0x00002050 File Offset: 0x00000250
	public CallLimiter()
	{
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x000F0410 File Offset: 0x000EE610
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

	// Token: 0x060031F9 RID: 12793 RVA: 0x000F0460 File Offset: 0x000EE660
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

	// Token: 0x060031FA RID: 12794 RVA: 0x000F0518 File Offset: 0x000EE718
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

	// Token: 0x060031FB RID: 12795 RVA: 0x000F0580 File Offset: 0x000EE780
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

	// Token: 0x04003584 RID: 13700
	protected const double k_serverMaxTime = 4294967.295;

	// Token: 0x04003585 RID: 13701
	[SerializeField]
	protected float[] callTimeHistory;

	// Token: 0x04003586 RID: 13702
	[Space]
	[SerializeField]
	protected int callHistoryLength;

	// Token: 0x04003587 RID: 13703
	[SerializeField]
	protected float timeCooldown;

	// Token: 0x04003588 RID: 13704
	[SerializeField]
	protected double maxLatency;

	// Token: 0x04003589 RID: 13705
	private int oldTimeIndex;

	// Token: 0x0400358A RID: 13706
	protected bool blockCall;

	// Token: 0x0400358B RID: 13707
	protected float blockStartTime;
}
