using System;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Splines;

// Token: 0x020001C8 RID: 456
[NetworkBehaviourWeaved(1)]
public class GTSplineAnimateFixedUpdater : NetworkComponent
{
	// Token: 0x06000AC4 RID: 2756 RVA: 0x00037878 File Offset: 0x00035A78
	protected override void Awake()
	{
		base.Awake();
		this.splineAnimateRef.AddCallbackOnLoad(new Action(this.InitSplineAnimate));
		this.splineAnimateRef.AddCallbackOnUnload(new Action(this.ClearSplineAnimate));
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x000378AE File Offset: 0x00035AAE
	private void InitSplineAnimate()
	{
		this.isSplineLoaded = this.splineAnimateRef.TryResolve<SplineAnimate>(out this.splineAnimate);
		if (this.isSplineLoaded)
		{
			this.splineAnimate.enabled = false;
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x000378DB File Offset: 0x00035ADB
	private void ClearSplineAnimate()
	{
		this.splineAnimate = null;
		this.isSplineLoaded = false;
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00098BA0 File Offset: 0x00096DA0
	private void FixedUpdate()
	{
		if (!base.IsMine && this.progressLerpStartTime + 1f > Time.time)
		{
			if (this.isSplineLoaded)
			{
				this.progress = Mathf.Lerp(this.progressLerpStart, this.progressLerpEnd, (Time.time - this.progressLerpStartTime) / 1f) % this.Duration;
				this.splineAnimate.NormalizedTime = this.progress / this.Duration;
				return;
			}
		}
		else
		{
			this.progress = (this.progress + Time.fixedDeltaTime) % this.Duration;
			if (this.isSplineLoaded)
			{
				this.splineAnimate.NormalizedTime = this.progress / this.Duration;
			}
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x000378EB File Offset: 0x00035AEB
	// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x00037911 File Offset: 0x00035B11
	[Networked]
	[NetworkedWeaved(0, 1)]
	public unsafe float Netdata
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GTSplineAnimateFixedUpdater.Netdata. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(float*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GTSplineAnimateFixedUpdater.Netdata. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(float*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00037938 File Offset: 0x00035B38
	public override void WriteDataFusion()
	{
		this.Netdata = this.progress + 1f;
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0003794C File Offset: 0x00035B4C
	public override void ReadDataFusion()
	{
		this.SharedReadData(this.Netdata);
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0003795A File Offset: 0x00035B5A
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.progress + 1f);
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00098C58 File Offset: 0x00096E58
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		float incomingValue = (float)stream.ReceiveNext();
		this.SharedReadData(incomingValue);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00098C88 File Offset: 0x00096E88
	private void SharedReadData(float incomingValue)
	{
		if (float.IsNaN(incomingValue) || incomingValue > this.Duration + 1f || incomingValue < 0f)
		{
			return;
		}
		this.progressLerpEnd = incomingValue;
		if (this.progressLerpEnd < this.progress)
		{
			if (this.progress < this.Duration)
			{
				this.progressLerpEnd += this.Duration;
			}
			else
			{
				this.progress -= this.Duration;
			}
		}
		this.progressLerpStart = this.progress;
		this.progressLerpStartTime = Time.time;
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00037981 File Offset: 0x00035B81
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Netdata = this._Netdata;
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00037999 File Offset: 0x00035B99
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Netdata = this.Netdata;
	}

	// Token: 0x04000D0F RID: 3343
	[SerializeField]
	private XSceneRef splineAnimateRef;

	// Token: 0x04000D10 RID: 3344
	[SerializeField]
	private float Duration;

	// Token: 0x04000D11 RID: 3345
	private SplineAnimate splineAnimate;

	// Token: 0x04000D12 RID: 3346
	private bool isSplineLoaded;

	// Token: 0x04000D13 RID: 3347
	private float progress;

	// Token: 0x04000D14 RID: 3348
	private float progressLerpStart;

	// Token: 0x04000D15 RID: 3349
	private float progressLerpEnd;

	// Token: 0x04000D16 RID: 3350
	private const float progressLerpDuration = 1f;

	// Token: 0x04000D17 RID: 3351
	private float progressLerpStartTime;

	// Token: 0x04000D18 RID: 3352
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Netdata", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private float _Netdata;
}
