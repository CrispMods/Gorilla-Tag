using System;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class LaserPointer : OVRCursor
{
	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x060010EA RID: 4330 RVA: 0x00051B4F File Offset: 0x0004FD4F
	// (set) Token: 0x060010E9 RID: 4329 RVA: 0x00051B1B File Offset: 0x0004FD1B
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

	// Token: 0x060010EB RID: 4331 RVA: 0x00051B57 File Offset: 0x0004FD57
	private void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x00051B65 File Offset: 0x0004FD65
	private void Start()
	{
		if (this.cursorVisual)
		{
			this.cursorVisual.SetActive(false);
		}
		OVRManager.InputFocusAcquired += this.OnInputFocusAcquired;
		OVRManager.InputFocusLost += this.OnInputFocusLost;
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x00051BA2 File Offset: 0x0004FDA2
	public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
	{
		this._startPoint = start;
		this._endPoint = dest;
		this._hitTarget = true;
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x00051BB9 File Offset: 0x0004FDB9
	public override void SetCursorRay(Transform t)
	{
		this._startPoint = t.position;
		this._forward = t.forward;
		this._hitTarget = false;
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x00051BDC File Offset: 0x0004FDDC
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

	// Token: 0x060010F0 RID: 4336 RVA: 0x00051CC4 File Offset: 0x0004FEC4
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

	// Token: 0x060010F1 RID: 4337 RVA: 0x00051D5C File Offset: 0x0004FF5C
	private void OnDisable()
	{
		if (this.cursorVisual)
		{
			this.cursorVisual.SetActive(false);
		}
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00051D77 File Offset: 0x0004FF77
	public void OnInputFocusLost()
	{
		if (base.gameObject && base.gameObject.activeInHierarchy)
		{
			this.m_restoreOnInputAcquired = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00051DA6 File Offset: 0x0004FFA6
	public void OnInputFocusAcquired()
	{
		if (this.m_restoreOnInputAcquired && base.gameObject)
		{
			this.m_restoreOnInputAcquired = false;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x00051DD0 File Offset: 0x0004FFD0
	private void OnDestroy()
	{
		OVRManager.InputFocusAcquired -= this.OnInputFocusAcquired;
		OVRManager.InputFocusLost -= this.OnInputFocusLost;
	}

	// Token: 0x040012E8 RID: 4840
	public GameObject cursorVisual;

	// Token: 0x040012E9 RID: 4841
	public float maxLength = 10f;

	// Token: 0x040012EA RID: 4842
	private LaserPointer.LaserBeamBehavior _laserBeamBehavior;

	// Token: 0x040012EB RID: 4843
	private bool m_restoreOnInputAcquired;

	// Token: 0x040012EC RID: 4844
	private Vector3 _startPoint;

	// Token: 0x040012ED RID: 4845
	private Vector3 _forward;

	// Token: 0x040012EE RID: 4846
	private Vector3 _endPoint;

	// Token: 0x040012EF RID: 4847
	private bool _hitTarget;

	// Token: 0x040012F0 RID: 4848
	private LineRenderer lineRenderer;

	// Token: 0x020002BF RID: 703
	public enum LaserBeamBehavior
	{
		// Token: 0x040012F2 RID: 4850
		On,
		// Token: 0x040012F3 RID: 4851
		Off,
		// Token: 0x040012F4 RID: 4852
		OnWhenHitTarget
	}
}
