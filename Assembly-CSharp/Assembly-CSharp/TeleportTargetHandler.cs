using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public abstract class TeleportTargetHandler : TeleportSupport
{
	// Token: 0x060011CC RID: 4556 RVA: 0x000549EA File Offset: 0x00052BEA
	protected TeleportTargetHandler()
	{
		this._startAimAction = delegate()
		{
			base.StartCoroutine(this.TargetAimCoroutine());
		};
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x00054A1A File Offset: 0x00052C1A
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._startAimAction;
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00054A33 File Offset: 0x00052C33
	protected override void RemoveEventHandlers()
	{
		base.RemoveEventHandlers();
		base.LocomotionTeleport.EnterStateAim -= this._startAimAction;
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x00054A4C File Offset: 0x00052C4C
	private IEnumerator TargetAimCoroutine()
	{
		while (base.LocomotionTeleport.CurrentState == LocomotionTeleport.States.Aim)
		{
			this.ResetAimData();
			Vector3 start = base.LocomotionTeleport.transform.position;
			this._aimPoints.Clear();
			base.LocomotionTeleport.AimHandler.GetPoints(this._aimPoints);
			for (int i = 0; i < this._aimPoints.Count; i++)
			{
				Vector3 vector = this._aimPoints[i];
				this.AimData.TargetValid = this.ConsiderTeleport(start, ref vector);
				this.AimData.Points.Add(vector);
				if (this.AimData.TargetValid)
				{
					this.AimData.Destination = this.ConsiderDestination(vector);
					this.AimData.TargetValid = (this.AimData.Destination != null);
					break;
				}
				start = this._aimPoints[i];
			}
			base.LocomotionTeleport.OnUpdateAimData(this.AimData);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x00054A5B File Offset: 0x00052C5B
	protected virtual void ResetAimData()
	{
		this.AimData.Reset();
	}

	// Token: 0x060011D1 RID: 4561
	protected abstract bool ConsiderTeleport(Vector3 start, ref Vector3 end);

	// Token: 0x060011D2 RID: 4562 RVA: 0x00054A68 File Offset: 0x00052C68
	public virtual Vector3? ConsiderDestination(Vector3 location)
	{
		CapsuleCollider characterController = base.LocomotionTeleport.LocomotionController.CharacterController;
		float num = characterController.radius - 0.1f;
		Vector3 vector = location;
		vector.y += num + 0.1f;
		Vector3 end = vector;
		end.y += characterController.height - 0.1f;
		if (Physics.CheckCapsule(vector, end, num, this.AimCollisionLayerMask, QueryTriggerInteraction.Ignore))
		{
			return null;
		}
		return new Vector3?(location);
	}

	// Token: 0x040013AE RID: 5038
	[Tooltip("This bitmask controls which game object layers will be included in the targeting collision tests.")]
	public LayerMask AimCollisionLayerMask;

	// Token: 0x040013AF RID: 5039
	protected readonly LocomotionTeleport.AimData AimData = new LocomotionTeleport.AimData();

	// Token: 0x040013B0 RID: 5040
	private readonly Action _startAimAction;

	// Token: 0x040013B1 RID: 5041
	private readonly List<Vector3> _aimPoints = new List<Vector3>();

	// Token: 0x040013B2 RID: 5042
	private const float ERROR_MARGIN = 0.1f;
}
