using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002EE RID: 750
public abstract class TeleportTargetHandler : TeleportSupport
{
	// Token: 0x06001215 RID: 4629 RVA: 0x0003C51C File Offset: 0x0003A71C
	protected TeleportTargetHandler()
	{
		this._startAimAction = delegate()
		{
			base.StartCoroutine(this.TargetAimCoroutine());
		};
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x0003C54C File Offset: 0x0003A74C
	protected override void AddEventHandlers()
	{
		base.AddEventHandlers();
		base.LocomotionTeleport.EnterStateAim += this._startAimAction;
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x0003C565 File Offset: 0x0003A765
	protected override void RemoveEventHandlers()
	{
		base.RemoveEventHandlers();
		base.LocomotionTeleport.EnterStateAim -= this._startAimAction;
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x0003C57E File Offset: 0x0003A77E
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

	// Token: 0x06001219 RID: 4633 RVA: 0x0003C58D File Offset: 0x0003A78D
	protected virtual void ResetAimData()
	{
		this.AimData.Reset();
	}

	// Token: 0x0600121A RID: 4634
	protected abstract bool ConsiderTeleport(Vector3 start, ref Vector3 end);

	// Token: 0x0600121B RID: 4635 RVA: 0x000AF2DC File Offset: 0x000AD4DC
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

	// Token: 0x040013F5 RID: 5109
	[Tooltip("This bitmask controls which game object layers will be included in the targeting collision tests.")]
	public LayerMask AimCollisionLayerMask;

	// Token: 0x040013F6 RID: 5110
	protected readonly LocomotionTeleport.AimData AimData = new LocomotionTeleport.AimData();

	// Token: 0x040013F7 RID: 5111
	private readonly Action _startAimAction;

	// Token: 0x040013F8 RID: 5112
	private readonly List<Vector3> _aimPoints = new List<Vector3>();

	// Token: 0x040013F9 RID: 5113
	private const float ERROR_MARGIN = 0.1f;
}
