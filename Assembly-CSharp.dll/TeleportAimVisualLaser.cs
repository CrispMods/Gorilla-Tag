using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D2 RID: 722
public class TeleportAimVisualLaser : TeleportSupport
{
	// Token: 0x06001177 RID: 4471 RVA: 0x0003AE04 File Offset: 0x00039004
	public TeleportAimVisualLaser()
	{
		this._enterAimStateAction = new Action(this.EnterAimState);
		this._exitAimStateAction = new Action(this.ExitAimState);
		this._updateAimDataAction = new Action<LocomotionTeleport.AimData>(this.UpdateAimData);
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x0003AE42 File Offset: 0x00039042
	private void EnterAimState()
	{
		this._lineRenderer.gameObject.SetActive(true);
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x0003AE55 File Offset: 0x00039055
	private void ExitAimState()
	{
		this._lineRenderer.gameObject.SetActive(false);
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x0003AE68 File Offset: 0x00039068
	private void Awake()
	{
		this.LaserPrefab.gameObject.SetActive(false);
		this._lineRenderer = UnityEngine.Object.Instantiate<LineRenderer>(this.LaserPrefab);
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x0003AE8C File Offset: 0x0003908C
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._enterAimStateAction;
		base.LocomotionTeleport.ExitStateAim += this._exitAimStateAction;
		base.LocomotionTeleport.UpdateAimData += this._updateAimDataAction;
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x0003AEC7 File Offset: 0x000390C7
	protected override void RemoveEventHandlers()
	{
		base.LocomotionTeleport.EnterStateAim -= this._enterAimStateAction;
		base.LocomotionTeleport.ExitStateAim -= this._exitAimStateAction;
		base.LocomotionTeleport.UpdateAimData -= this._updateAimDataAction;
		base.RemoveEventHandlers();
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x000ABF28 File Offset: 0x000AA128
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

	// Token: 0x04001361 RID: 4961
	[Tooltip("This prefab will be instantiated when the aim visual is awakened, and will be set active when the user is aiming, and deactivated when they are done aiming.")]
	public LineRenderer LaserPrefab;

	// Token: 0x04001362 RID: 4962
	private readonly Action _enterAimStateAction;

	// Token: 0x04001363 RID: 4963
	private readonly Action _exitAimStateAction;

	// Token: 0x04001364 RID: 4964
	private readonly Action<LocomotionTeleport.AimData> _updateAimDataAction;

	// Token: 0x04001365 RID: 4965
	private LineRenderer _lineRenderer;

	// Token: 0x04001366 RID: 4966
	private Vector3[] _linePoints;
}
