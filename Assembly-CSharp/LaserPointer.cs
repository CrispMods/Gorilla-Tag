using System;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class LaserPointer : OVRCursor
{
	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06001136 RID: 4406 RVA: 0x0003BB8A File Offset: 0x00039D8A
	// (set) Token: 0x06001135 RID: 4405 RVA: 0x0003BB56 File Offset: 0x00039D56
	public LaserPointer.LaserBeamBehavior laserBeamBehavior
	{
		get
		{
			return this._laserBeamBehavior;
		}
		set
		{
			this._laserBeamBehavior = value;
			if (this.laserBeamBehavior == LaserPointer.LaserBeamBehavior.Off || this.laserBeamBehavior == LaserPointer.LaserBeamBehavior.OnWhenHitTarget)
			{
				this.lineRenderer.enabled = false;
				return;
			}
			this.lineRenderer.enabled = true;
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x0003BB92 File Offset: 0x00039D92
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x0003BBA0 File Offset: 0x00039DA0
	private void Start()
	{
		if (this.cursorVisual)
		{
			this.cursorVisual.SetActive(false);
		}
		OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
		OVRManager.InputFocusLost += this.OnInputFocusLost;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x0003BBDD File Offset: 0x00039DDD
	public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
	{
		this._startPoint = start;
		this._endPoint = dest;
		this._hitTarget = true;
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x0003BBF4 File Offset: 0x00039DF4
	public override void SetCursorRay(Transform t)
	{
		this._startPoint = t.position;
		this._forward = t.forward;
		this._hitTarget = false;
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x000AD150 File Offset: 0x000AB350
	private void LateUpdate()
	{
		this.lineRenderer.SetPosition(0, this._startPoint);
		if (this._hitTarget)
		{
			this.lineRenderer.SetPosition(1, this._endPoint);
			this.UpdateLaserBeam(this._startPoint, this._endPoint);
			if (this.cursorVisual)
			{
				this.cursorVisual.transform.position = this._endPoint;
				this.cursorVisual.SetActive(true);
				return;
			}
		}
		else
		{
			this.UpdateLaserBeam(this._startPoint, this._startPoint + this.maxLength * this._forward);
			this.lineRenderer.SetPosition(1, this._startPoint + this.maxLength * this._forward);
			if (this.cursorVisual)
			{
				this.cursorVisual.SetActive(false);
			}
		}
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x000AD238 File Offset: 0x000AB438
	private void UpdateLaserBeam(Vector3 start, Vector3 end)
	{
		if (this.laserBeamBehavior == LaserPointer.LaserBeamBehavior.Off)
		{
			return;
		}
		if (this.laserBeamBehavior == LaserPointer.LaserBeamBehavior.On)
		{
			this.lineRenderer.SetPosition(0, start);
			this.lineRenderer.SetPosition(1, end);
			return;
		}
		if (this.laserBeamBehavior == LaserPointer.LaserBeamBehavior.OnWhenHitTarget)
		{
			if (this._hitTarget)
			{
				if (!this.lineRenderer.enabled)
				{
					this.lineRenderer.enabled = true;
					this.lineRenderer.SetPosition(0, start);
					this.lineRenderer.SetPosition(1, end);
					return;
				}
			}
			else if (this.lineRenderer.enabled)
			{
				this.lineRenderer.enabled = false;
			}
		}
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x0003BC15 File Offset: 0x00039E15
	private void OnDisable()
	{
		if (this.cursorVisual)
		{
			this.cursorVisual.SetActive(false);
		}
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x0003BC30 File Offset: 0x00039E30
	public void OnInputFocusLost()
	{
		if (base.gameObject && base.gameObject.activeInHierarchy)
		{
			this.m_restoreOnInputAcquired = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x0003BC5F File Offset: 0x00039E5F
	public void OnInputFocusAcquired()
	{
		if (this.m_restoreOnInputAcquired && base.gameObject)
		{
			this.m_restoreOnInputAcquired = false;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x0003BC89 File Offset: 0x00039E89
	private void OnDestroy()
	{
		OVRManager.InputFocusAcquired -= this.OnInputFocusAcquired;
		OVRManager.InputFocusLost -= this.OnInputFocusLost;
	}

	// Token: 0x04001330 RID: 4912
	public GameObject cursorVisual;

	// Token: 0x04001331 RID: 4913
	public float maxLength = 10f;

	// Token: 0x04001332 RID: 4914
	private LaserPointer.LaserBeamBehavior _laserBeamBehavior;

	// Token: 0x04001333 RID: 4915
	private bool m_restoreOnInputAcquired;

	// Token: 0x04001334 RID: 4916
	private Vector3 _startPoint;

	// Token: 0x04001335 RID: 4917
	private Vector3 _forward;

	// Token: 0x04001336 RID: 4918
	private Vector3 _endPoint;

	// Token: 0x04001337 RID: 4919
	private bool _hitTarget;

	// Token: 0x04001338 RID: 4920
	private LineRenderer lineRenderer;

	// Token: 0x020002CA RID: 714
	public enum LaserBeamBehavior
	{
		// Token: 0x0400133A RID: 4922
		On,
		// Token: 0x0400133B RID: 4923
		Off,
		// Token: 0x0400133C RID: 4924
		OnWhenHitTarget
	}
}
