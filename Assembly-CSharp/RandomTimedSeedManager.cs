using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x020001EC RID: 492
[NetworkBehaviourWeaved(2)]
public class RandomTimedSeedManager : NetworkComponent, ITickSystemTick
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0003824D File Offset: 0x0003644D
	// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00038254 File Offset: 0x00036454
	public static RandomTimedSeedManager instance { get; private set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0003825C File Offset: 0x0003645C
	// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00038264 File Offset: 0x00036464
	public int seed { get; private set; }

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0003826D File Offset: 0x0003646D
	// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00038275 File Offset: 0x00036475
	public float currentSyncTime { get; private set; }

	// Token: 0x06000B90 RID: 2960 RVA: 0x0003827E File Offset: 0x0003647E
	protected override void Awake()
	{
		base.Awake();
		RandomTimedSeedManager.instance = this;
		this.seed = UnityEngine.Random.Range(-1000000, -1000000);
		this.idealSyncTime = 0f;
		this.currentSyncTime = 0f;
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x000382BD File Offset: 0x000364BD
	public void AddCallbackOnSeedChanged(Action callback)
	{
		this.callbacksOnSeedChanged.Add(callback);
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x000382CB File Offset: 0x000364CB
	public void RemoveCallbackOnSeedChanged(Action callback)
	{
		this.callbacksOnSeedChanged.Remove(callback);
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000B93 RID: 2963 RVA: 0x000382DA File Offset: 0x000364DA
	// (set) Token: 0x06000B94 RID: 2964 RVA: 0x000382E2 File Offset: 0x000364E2
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x06000B95 RID: 2965 RVA: 0x0009B8D0 File Offset: 0x00099AD0
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

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000B96 RID: 2966 RVA: 0x000382EB File Offset: 0x000364EB
	// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00038315 File Offset: 0x00036515
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

	// Token: 0x06000B98 RID: 2968 RVA: 0x00038340 File Offset: 0x00036540
	public override void WriteDataFusion()
	{
		this.Data = new RandomTimedSeedManager.RandomTimedSeedManagerData(this.seed, this.currentSyncTime);
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0009B95C File Offset: 0x00099B5C
	public override void ReadDataFusion()
	{
		this.ReadDataShared(this.Data.seed, this.Data.currentSyncTime);
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00038359 File Offset: 0x00036559
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != PhotonNetwork.MasterClient)
		{
			return;
		}
		stream.SendNext(this.seed);
		stream.SendNext(this.currentSyncTime);
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0009B98C File Offset: 0x00099B8C
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

	// Token: 0x06000B9C RID: 2972 RVA: 0x0009B9C8 File Offset: 0x00099BC8
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

	// Token: 0x06000B9E RID: 2974 RVA: 0x0003839E File Offset: 0x0003659E
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x000383B6 File Offset: 0x000365B6
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04000DEE RID: 3566
	private List<Action> callbacksOnSeedChanged = new List<Action>();

	// Token: 0x04000DF0 RID: 3568
	private float idealSyncTime;

	// Token: 0x04000DF2 RID: 3570
	private int cachedSeed;

	// Token: 0x04000DF3 RID: 3571
	private const int SeedMin = -1000000;

	// Token: 0x04000DF4 RID: 3572
	private const int SeedMax = -1000000;

	// Token: 0x04000DF5 RID: 3573
	private const float MaxSyncTime = 1E+09f;

	// Token: 0x04000DF7 RID: 3575
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 2)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private RandomTimedSeedManager.RandomTimedSeedManagerData _Data;

	// Token: 0x020001ED RID: 493
	[NetworkStructWeaved(2)]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	private struct RandomTimedSeedManagerData : INetworkStruct
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x000383CA File Offset: 0x000365CA
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x000383D8 File Offset: 0x000365D8
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x000383E7 File Offset: 0x000365E7
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x000383F5 File Offset: 0x000365F5
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

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00038404 File Offset: 0x00036604
		public RandomTimedSeedManagerData(int seed, float currentSyncTime)
		{
			this.seed = seed;
			this.currentSyncTime = currentSyncTime;
		}

		// Token: 0x04000DF8 RID: 3576
		[FixedBufferProperty(typeof(int), typeof(UnityValueSurrogate@ReaderWriter@System_Int32), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@1 _seed;

		// Token: 0x04000DF9 RID: 3577
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(4)]
		private FixedStorage@1 _currentSyncTime;
	}
}
