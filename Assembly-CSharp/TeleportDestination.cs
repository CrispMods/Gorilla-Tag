using System;
using UnityEngine;

// Token: 0x020002DE RID: 734
public class TeleportDestination : MonoBehaviour
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060011C7 RID: 4551 RVA: 0x0003C1C2 File Offset: 0x0003A3C2
	// (set) Token: 0x060011C8 RID: 4552 RVA: 0x0003C1CA File Offset: 0x0003A3CA
	public bool IsValidDestination { get; private set; }

	// Token: 0x060011C9 RID: 4553 RVA: 0x0003C1D3 File Offset: 0x0003A3D3
	private TeleportDestination()
	{
		this._updateTeleportDestinationAction = new Action<bool, Vector3?, Quaternion?, Quaternion?>(this.UpdateTeleportDestination);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x000AE830 File Offset: 0x000ACA30
	public void OnEnable()
	{
		this.PositionIndicator.gameObject.SetActive(false);
		if (this.OrientationIndicator != null)
		{
			this.OrientationIndicator.gameObject.SetActive(false);
		}
		this.LocomotionTeleport.UpdateTeleportDestination += this._updateTeleportDestinationAction;
		this._eventsActive = true;
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0003C1EE File Offset: 0x0003A3EE
	private void TryDisableEventHandlers()
	{
		if (!this._eventsActive)
		{
			return;
		}
		this.LocomotionTeleport.UpdateTeleportDestination -= this._updateTeleportDestinationAction;
		this._eventsActive = false;
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0003C211 File Offset: 0x0003A411
	public void OnDisable()
	{
		this.TryDisableEventHandlers();
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060011CD RID: 4557 RVA: 0x000AE888 File Offset: 0x000ACA88
	// (remove) Token: 0x060011CE RID: 4558 RVA: 0x000AE8C0 File Offset: 0x000ACAC0
	public event Action<TeleportDestination> Deactivated;

	// Token: 0x060011CF RID: 4559 RVA: 0x0003C219 File Offset: 0x0003A419
	public void OnDeactivated()
	{
		if (this.Deactivated != null)
		{
			this.Deactivated(this);
			return;
		}
		this.Recycle();
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x0003C236 File Offset: 0x0003A436
	public void Recycle()
	{
		this.LocomotionTeleport.RecycleTeleportDestination(this);
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x000AE8F8 File Offset: 0x000ACAF8
	public virtual void UpdateTeleportDestination(bool isValidDestination, Vector3? position, Quaternion? rotation, Quaternion? landingRotation)
	{
		this.IsValidDestination = isValidDestination;
		this.LandingRotation = landingRotation.GetValueOrDefault();
		GameObject gameObject = this.PositionIndicator.gameObject;
		bool activeInHierarchy = gameObject.activeInHierarchy;
		if (position == null)
		{
			if (activeInHierarchy)
			{
				gameObject.SetActive(false);
			}
			return;
		}
		if (!activeInHierarchy)
		{
			gameObject.SetActive(true);
		}
		base.transform.position = position.GetValueOrDefault();
		if (this.OrientationIndicator == null)
		{
			if (rotation != null)
			{
				base.transform.rotation = rotation.GetValueOrDefault();
			}
			return;
		}
		GameObject gameObject2 = this.OrientationIndicator.gameObject;
		bool activeInHierarchy2 = gameObject2.activeInHierarchy;
		if (rotation == null)
		{
			if (activeInHierarchy2)
			{
				gameObject2.SetActive(false);
			}
			return;
		}
		this.OrientationIndicator.rotation = rotation.GetValueOrDefault();
		if (!activeInHierarchy2)
		{
			gameObject2.SetActive(true);
		}
	}

	// Token: 0x040013AF RID: 5039
	[Tooltip("If the target handler provides a target position, this transform will be moved to that position and it's game object enabled. A target position being provided does not mean the position is valid, only that the aim handler found something to test as a destination.")]
	public Transform PositionIndicator;

	// Token: 0x040013B0 RID: 5040
	[Tooltip("This transform will be rotated to match the rotation of the aiming target. Simple teleport destinations should assign this to the object containing this component. More complex teleport destinations might assign this to a sub-object that is used to indicate the landing orientation independently from the rest of the destination indicator, such as when world space effects are required. This will typically be a child of the PositionIndicator.")]
	public Transform OrientationIndicator;

	// Token: 0x040013B1 RID: 5041
	[Tooltip("After the player teleports, the character controller will have it's rotation set to this value. It is different from the OrientationIndicator transform.rotation in order to support both head-relative and forward-facing teleport modes (See TeleportOrientationHandlerThumbstick.cs).")]
	public Quaternion LandingRotation;

	// Token: 0x040013B2 RID: 5042
	[NonSerialized]
	public LocomotionTeleport LocomotionTeleport;

	// Token: 0x040013B3 RID: 5043
	[NonSerialized]
	public LocomotionTeleport.States TeleportState;

	// Token: 0x040013B4 RID: 5044
	private readonly Action<bool, Vector3?, Quaternion?, Quaternion?> _updateTeleportDestinationAction;

	// Token: 0x040013B5 RID: 5045
	private bool _eventsActive;
}
