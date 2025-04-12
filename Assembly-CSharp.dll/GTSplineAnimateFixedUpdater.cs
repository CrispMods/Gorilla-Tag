using System;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Splines;

// Token: 0x020001BD RID: 445
[NetworkBehaviourWeaved(1)]
public class GTSplineAnimateFixedUpdater : NetworkComponent
{
	// Token: 0x06000A7A RID: 2682 RVA: 0x000365B8 File Offset: 0x000347B8
	protected override void Awake()
	{
		base.Awake();
		this.splineAnimateRef.AddCallbackOnLoad(new Action(this.InitSplineAnimate));
		this.splineAnimateRef.AddCallbackOnUnload(new Action(this.ClearSplineAnimate));
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x000365EE File Offset: 0x000347EE
	private void InitSplineAnimate()
	{
		this.isSplineLoaded = this.splineAnimateRef.TryResolve<SplineAnimate>(out this.splineAnimate);
		if (this.isSplineLoaded)
		{
			this.splineAnimate.enabled = false;
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0003661B File Offset: 0x0003481B
	private void ClearSplineAnimate()
	{
		this.splineAnimate = null;
		this.isSplineLoaded = false;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x000962AC File Offset: 0x000944AC
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

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0003662B File Offset: 0x0003482B
	// (set) Token: 0x06000A7F RID: 2687 RVA: 0x00036651 File Offset: 0x00034851
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

	// Token: 0x06000A80 RID: 2688 RVA: 0x00036678 File Offset: 0x00034878
	public override void WriteDataFusion()
	{
		this.Netdata = this.progress + 1f;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0003668C File Offset: 0x0003488C
	public override void ReadDataFusion()
	{
		this.SharedReadData(this.Netdata);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0003669A File Offset: 0x0003489A
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.progress + 1f);
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x00096364 File Offset: 0x00094564
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		float incomingValue = (float)stream.ReceiveNext();
		this.SharedReadData(incomingValue);
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00096394 File Offset: 0x00094594
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

	// Token: 0x06000A86 RID: 2694 RVA: 0x000366C1 File Offset: 0x000348C1
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Netdata = this._Netdata;
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x000366D9 File Offset: 0x000348D9
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Netdata = this.Netdata;
	}

	// Token: 0x04000CCA RID: 3274
	[SerializeField]
	private XSceneRef splineAnimateRef;

	// Token: 0x04000CCB RID: 3275
	[SerializeField]
	private float Duration;

	// Token: 0x04000CCC RID: 3276
	private SplineAnimate splineAnimate;

	// Token: 0x04000CCD RID: 3277
	private bool isSplineLoaded;

	// Token: 0x04000CCE RID: 3278
	private float progress;

	// Token: 0x04000CCF RID: 3279
	private float progressLerpStart;

	// Token: 0x04000CD0 RID: 3280
	private float progressLerpEnd;

	// Token: 0x04000CD1 RID: 3281
	private const float progressLerpDuration = 1f;

	// Token: 0x04000CD2 RID: 3282
	private float progressLerpStartTime;

	// Token: 0x04000CD3 RID: 3283
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Netdata", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private float _Netdata;
}
