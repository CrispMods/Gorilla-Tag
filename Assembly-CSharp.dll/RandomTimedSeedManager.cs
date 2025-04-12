using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x020001E1 RID: 481
[NetworkBehaviourWeaved(2)]
public class RandomTimedSeedManager : NetworkComponent, ITickSystemTick
{
	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00036F8D File Offset: 0x0003518D
	// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00036F94 File Offset: 0x00035194
	public static RandomTimedSeedManager instance { get; private set; }

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00036F9C File Offset: 0x0003519C
	// (set) Token: 0x06000B43 RID: 2883 RVA: 0x00036FA4 File Offset: 0x000351A4
	public int seed { get; private set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x00036FAD File Offset: 0x000351AD
	// (set) Token: 0x06000B45 RID: 2885 RVA: 0x00036FB5 File Offset: 0x000351B5
	public float currentSyncTime { get; private set; }

	// Token: 0x06000B46 RID: 2886 RVA: 0x00036FBE File Offset: 0x000351BE
	protected override void Awake()
	{
		base.Awake();
		RandomTimedSeedManager.instance = this;
		this.seed = UnityEngine.Random.Range(-1000000, -1000000);
		this.idealSyncTime = 0f;
		this.currentSyncTime = 0f;
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x00036FFD File Offset: 0x000351FD
	public void AddCallbackOnSeedChanged(Action callback)
	{
		this.callbacksOnSeedChanged.Add(callback);
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0003700B File Offset: 0x0003520B
	public void RemoveCallbackOnSeedChanged(Action callback)
	{
		this.callbacksOnSeedChanged.Remove(callback);
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0003701A File Offset: 0x0003521A
	// (set) Token: 0x06000B4A RID: 2890 RVA: 0x00037022 File Offset: 0x00035222
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x06000B4B RID: 2891 RVA: 0x00098FDC File Offset: 0x000971DC
	void ITickSystemTick.Tick()
	{
		this.currentSyncTime += Time.deltaTime;
		this.idealSyncTime += Time.deltaTime;
		if (this.idealSyncTime > 1E+09f)
		{
			this.idealSyncTime -= 1E+09f;
			this.currentSyncTime -= 1E+09f;
		}
		if (!base.GetView.AmOwner)
		{
			this.currentSyncTime = Mathf.Lerp(this.currentSyncTime, this.idealSyncTime, 0.1f);
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0003702B File Offset: 0x0003522B
	// (set) Token: 0x06000B4D RID: 2893 RVA: 0x00037055 File Offset: 0x00035255
	[Networked]
	[NetworkedWeaved(0, 2)]
	private unsafe RandomTimedSeedManager.RandomTimedSeedManagerData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing RandomTimedSeedManager.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(RandomTimedSeedManager.RandomTimedSeedManagerData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing RandomTimedSeedManager.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(RandomTimedSeedManager.RandomTimedSeedManagerData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x00037080 File Offset: 0x00035280
	public override void WriteDataFusion()
	{
		this.Data = new RandomTimedSeedManager.RandomTimedSeedManagerData(this.seed, this.currentSyncTime);
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00099068 File Offset: 0x00097268
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data.seed, this.Data.currentSyncTime);
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00037099 File Offset: 0x00035299
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		stream.SendNext(this.seed);
		stream.SendNext(this.currentSyncTime);
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00099098 File Offset: 0x00097298
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		int seedVal = (int)stream.ReceiveNext();
		float testTime = (float)stream.ReceiveNext();
		this.ReadDataShared(seedVal, testTime);
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x000990D4 File Offset: 0x000972D4
	private void ReadDataShared(int seedVal, float testTime)
	{
		this.seed = seedVal;
		if (testTime >= 0f && testTime <= 1E+09f)
		{
			if (this.idealSyncTime - testTime > 500000000f)
			{
				this.currentSyncTime = testTime;
			}
			this.idealSyncTime = testTime;
		}
		if (this.seed != this.cachedSeed && this.seed >= -1000000 && this.seed <= -1000000)
		{
			this.currentSyncTime = this.idealSyncTime;
			this.cachedSeed = this.seed;
			foreach (Action action in this.callbacksOnSeedChanged)
			{
				action();
			}
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x000370DE File Offset: 0x000352DE
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x000370F6 File Offset: 0x000352F6
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04000DA9 RID: 3497
	private List<Action> callbacksOnSeedChanged = new List<Action>();

	// Token: 0x04000DAB RID: 3499
	private float idealSyncTime;

	// Token: 0x04000DAD RID: 3501
	private int cachedSeed;

	// Token: 0x04000DAE RID: 3502
	private const int SeedMin = -1000000;

	// Token: 0x04000DAF RID: 3503
	private const int SeedMax = -1000000;

	// Token: 0x04000DB0 RID: 3504
	private const float MaxSyncTime = 1E+09f;

	// Token: 0x04000DB2 RID: 3506
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 2)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private RandomTimedSeedManager.RandomTimedSeedManagerData _Data;

	// Token: 0x020001E2 RID: 482
	[NetworkStructWeaved(2)]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	private struct RandomTimedSeedManagerData : INetworkStruct
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0003710A File Offset: 0x0003530A
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x00037118 File Offset: 0x00035318
		[Networked]
		public unsafe int seed
		{
			readonly get
			{
				return *(int*)Native.ReferenceToPointer<FixedStorage@1>(ref this._seed);
			}
			set
			{
				*(int*)Native.ReferenceToPointer<FixedStorage@1>(ref this._seed) = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00037127 File Offset: 0x00035327
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x00037135 File Offset: 0x00035335
		[Networked]
		public unsafe float currentSyncTime
		{
			readonly get
			{
				return *(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._currentSyncTime);
			}
			set
			{
				*(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._currentSyncTime) = value;
			}
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00037144 File Offset: 0x00035344
		public RandomTimedSeedManagerData(int seed, float currentSyncTime)
		{
			this.seed = seed;
			this.currentSyncTime = currentSyncTime;
		}

		// Token: 0x04000DB3 RID: 3507
		[FixedBufferProperty(typeof(int), typeof(UnityValueSurrogate@ReaderWriter@System_Int32), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@1 _seed;

		// Token: 0x04000DB4 RID: 3508
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@1 _currentSyncTime;
	}
}
