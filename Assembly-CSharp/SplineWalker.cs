using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200089A RID: 2202
public class SplineWalker : MonoBehaviour, IPunObservable
{
	// Token: 0x0600356A RID: 13674 RVA: 0x00053262 File Offset: 0x00051462
	private void Awake()
	{
		this._view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x0014276C File Offset: 0x0014096C
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

	// Token: 0x0600356C RID: 13676 RVA: 0x00053270 File Offset: 0x00051470
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.Serialize(ref this.progress);
	}

	// Token: 0x04003825 RID: 14373
	public BezierSpline spline;

	// Token: 0x04003826 RID: 14374
	public LinearSpline linearSpline;

	// Token: 0x04003827 RID: 14375
	public float duration;

	// Token: 0x04003828 RID: 14376
	public bool lookForward;

	// Token: 0x04003829 RID: 14377
	public SplineWalkerMode mode;

	// Token: 0x0400382A RID: 14378
	public bool walkLinearPath;

	// Token: 0x0400382B RID: 14379
	public bool useWorldPosition;

	// Token: 0x0400382C RID: 14380
	public float progress;

	// Token: 0x0400382D RID: 14381
	private bool goingForward = true;

	// Token: 0x0400382E RID: 14382
	public bool DoNetworkSync = true;

	// Token: 0x0400382F RID: 14383
	private PhotonView _view;
}
