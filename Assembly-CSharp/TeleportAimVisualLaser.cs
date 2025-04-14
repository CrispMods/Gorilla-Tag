using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class TeleportAimVisualLaser : TeleportSupport
{
	// Token: 0x06001174 RID: 4468 RVA: 0x000536EF File Offset: 0x000518EF
	public TeleportAimVisualLaser()
	{
		this._enterAimStateAction = new Action(this.EnterAimState);
		this._exitAimStateAction = new Action(this.ExitAimState);
		this._updateAimDataAction = new Action<LocomotionTeleport.AimData>(this.UpdateAimData);
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x0005372D File Offset: 0x0005192D
	private void EnterAimState()
	{
		this._lineRenderer.gameObject.SetActive(true);
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x00053740 File Offset: 0x00051940
	private void ExitAimState()
	{
		this._lineRenderer.gameObject.SetActive(false);
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00053753 File Offset: 0x00051953
	private void Awake()
	{
		this.LaserPrefab.gameObject.SetActive(false);
		this._lineRenderer = Object.Instantiate<LineRenderer>(this.LaserPrefab);
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00053777 File Offset: 0x00051977
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._enterAimStateAction;
		base.LocomotionTeleport.ExitStateAim += this._exitAimStateAction;
		base.LocomotionTeleport.UpdateAimData += this._updateAimDataAction;
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x000537B2 File Offset: 0x000519B2
	protected override void RemoveEventHandlers()
	{
		base.LocomotionTeleport.EnterStateAim -= this._enterAimStateAction;
		base.LocomotionTeleport.ExitStateAim -= this._exitAimStateAction;
		base.LocomotionTeleport.UpdateAimData -= this._updateAimDataAction;
		base.RemoveEventHandlers();
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x000537F0 File Offset: 0x000519F0
	private void UpdateAimData(LocomotionTeleport.AimData obj)
	{
		this._lineRenderer.sharedMaterial.color = (obj.TargetValid ? Color.green : Color.red);
		List<Vector3> points = obj.Points;
		this._lineRenderer.positionCount = points.Count;
		for (int i = 0; i < points.Count; i++)
		{
			this._lineRenderer.SetPosition(i, points[i]);
		}
	}

	// Token: 0x04001360 RID: 4960
	[Tooltip("This prefab will be instantiated when the aim visual is awakened, and will be set active when the user is aiming, and deactivated when they are done aiming.")]
	public LineRenderer LaserPrefab;

	// Token: 0x04001361 RID: 4961
	private readonly Action _enterAimStateAction;

	// Token: 0x04001362 RID: 4962
	private readonly Action _exitAimStateAction;

	// Token: 0x04001363 RID: 4963
	private readonly Action<LocomotionTeleport.AimData> _updateAimDataAction;

	// Token: 0x04001364 RID: 4964
	private LineRenderer _lineRenderer;

	// Token: 0x04001365 RID: 4965
	private Vector3[] _linePoints;
}
