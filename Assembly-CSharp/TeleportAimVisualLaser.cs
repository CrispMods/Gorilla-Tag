using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class TeleportAimVisualLaser : TeleportSupport
{
	// Token: 0x060011C0 RID: 4544 RVA: 0x0003C0C4 File Offset: 0x0003A2C4
	public TeleportAimVisualLaser()
	{
		this._enterAimStateAction = new Action(this.EnterAimState);
		this._exitAimStateAction = new Action(this.ExitAimState);
		this._updateAimDataAction = new Action<LocomotionTeleport.AimData>(this.UpdateAimData);
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x0003C102 File Offset: 0x0003A302
	private void EnterAimState()
	{
		this._lineRenderer.gameObject.SetActive(true);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0003C115 File Offset: 0x0003A315
	private void ExitAimState()
	{
		this._lineRenderer.gameObject.SetActive(false);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x0003C128 File Offset: 0x0003A328
	private void Awake()
	{
		this.LaserPrefab.gameObject.SetActive(false);
		this._lineRenderer = UnityEngine.Object.Instantiate<LineRenderer>(this.LaserPrefab);
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x0003C14C File Offset: 0x0003A34C
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._enterAimStateAction;
		base.LocomotionTeleport.ExitStateAim += this._exitAimStateAction;
		base.LocomotionTeleport.UpdateAimData += this._updateAimDataAction;
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x0003C187 File Offset: 0x0003A387
	protected override void RemoveEventHandlers()
	{
		base.LocomotionTeleport.EnterStateAim -= this._enterAimStateAction;
		base.LocomotionTeleport.ExitStateAim -= this._exitAimStateAction;
		base.LocomotionTeleport.UpdateAimData -= this._updateAimDataAction;
		base.RemoveEventHandlers();
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x000AE7C0 File Offset: 0x000AC9C0
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

	// Token: 0x040013A8 RID: 5032
	[Tooltip("This prefab will be instantiated when the aim visual is awakened, and will be set active when the user is aiming, and deactivated when they are done aiming.")]
	public LineRenderer LaserPrefab;

	// Token: 0x040013A9 RID: 5033
	private readonly Action _enterAimStateAction;

	// Token: 0x040013AA RID: 5034
	private readonly Action _exitAimStateAction;

	// Token: 0x040013AB RID: 5035
	private readonly Action<LocomotionTeleport.AimData> _updateAimDataAction;

	// Token: 0x040013AC RID: 5036
	private LineRenderer _lineRenderer;

	// Token: 0x040013AD RID: 5037
	private Vector3[] _linePoints;
}
