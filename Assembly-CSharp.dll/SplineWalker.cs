using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public class SplineWalker : MonoBehaviour, IPunObservable
{
	// Token: 0x060034AA RID: 13482 RVA: 0x00051D55 File Offset: 0x0004FF55
	private void Awake()
	{
		this._view = base.GetComponent<PhotonView>();
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x0013D184 File Offset: 0x0013B384
	private void Update()
	{
		if (this.goingForward)
		{
			this.progress += Time.deltaTime / this.duration;
			if (this.progress > 1f)
			{
				if (this.mode == SplineWalkerMode.Once)
				{
					this.progress = 1f;
				}
				else if (this.mode == SplineWalkerMode.Loop)
				{
					this.progress -= 1f;
				}
				else
				{
					this.progress = 2f - this.progress;
					this.goingForward = false;
				}
			}
		}
		else
		{
			this.progress -= Time.deltaTime / this.duration;
			if (this.progress < 0f)
			{
				this.progress = -this.progress;
				this.goingForward = true;
			}
		}
		if (this.linearSpline != null && this.walkLinearPath)
		{
			Vector3 vector = this.linearSpline.Evaluate(this.progress);
			if (this.useWorldPosition)
			{
				base.transform.position = vector;
			}
			else
			{
				base.transform.localPosition = vector;
			}
			if (this.lookForward)
			{
				base.transform.LookAt(vector + this.linearSpline.GetForwardTangent(this.progress, 0.01f));
				return;
			}
		}
		else if (this.spline != null)
		{
			Vector3 point = this.spline.GetPoint(this.progress);
			if (this.useWorldPosition)
			{
				base.transform.position = point;
			}
			else
			{
				base.transform.localPosition = point;
			}
			if (this.lookForward)
			{
				base.transform.LookAt(point + this.spline.GetDirection(this.progress));
			}
		}
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x00051D63 File Offset: 0x0004FF63
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.Serialize(ref this.progress);
	}

	// Token: 0x04003777 RID: 14199
	public BezierSpline spline;

	// Token: 0x04003778 RID: 14200
	public LinearSpline linearSpline;

	// Token: 0x04003779 RID: 14201
	public float duration;

	// Token: 0x0400377A RID: 14202
	public bool lookForward;

	// Token: 0x0400377B RID: 14203
	public SplineWalkerMode mode;

	// Token: 0x0400377C RID: 14204
	public bool walkLinearPath;

	// Token: 0x0400377D RID: 14205
	public bool useWorldPosition;

	// Token: 0x0400377E RID: 14206
	public float progress;

	// Token: 0x0400377F RID: 14207
	private bool goingForward = true;

	// Token: 0x04003780 RID: 14208
	public bool DoNetworkSync = true;

	// Token: 0x04003781 RID: 14209
	private PhotonView _view;
}
