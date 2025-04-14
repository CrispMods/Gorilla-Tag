using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200087E RID: 2174
public class SplineWalker : MonoBehaviour, IPunObservable
{
	// Token: 0x0600349E RID: 13470 RVA: 0x000FBA80 File Offset: 0x000F9C80
	private void Awake()
	{
		this._view = base.GetComponent<PhotonView>();
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x000FBA90 File Offset: 0x000F9C90
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

	// Token: 0x060034A0 RID: 13472 RVA: 0x000FBC3E File Offset: 0x000F9E3E
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.Serialize(ref this.progress);
	}

	// Token: 0x04003765 RID: 14181
	public BezierSpline spline;

	// Token: 0x04003766 RID: 14182
	public LinearSpline linearSpline;

	// Token: 0x04003767 RID: 14183
	public float duration;

	// Token: 0x04003768 RID: 14184
	public bool lookForward;

	// Token: 0x04003769 RID: 14185
	public SplineWalkerMode mode;

	// Token: 0x0400376A RID: 14186
	public bool walkLinearPath;

	// Token: 0x0400376B RID: 14187
	public bool useWorldPosition;

	// Token: 0x0400376C RID: 14188
	public float progress;

	// Token: 0x0400376D RID: 14189
	private bool goingForward = true;

	// Token: 0x0400376E RID: 14190
	public bool DoNetworkSync = true;

	// Token: 0x0400376F RID: 14191
	private PhotonView _view;
}
