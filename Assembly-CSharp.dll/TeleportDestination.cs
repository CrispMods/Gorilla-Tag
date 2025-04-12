using System;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class TeleportDestination : MonoBehaviour
{
	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600117E RID: 4478 RVA: 0x0003AF02 File Offset: 0x00039102
	// (set) Token: 0x0600117F RID: 4479 RVA: 0x0003AF0A File Offset: 0x0003910A
	public bool IsValidDestination { get; private set; }

	// Token: 0x06001180 RID: 4480 RVA: 0x0003AF13 File Offset: 0x00039113
	private TeleportDestination()
	{
		this._updateTeleportDestinationAction = new Action<bool, Vector3?, Quaternion?, Quaternion?>(this.UpdateTeleportDestination);
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x000ABF98 File Offset: 0x000AA198
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

	// Token: 0x06001182 RID: 4482 RVA: 0x0003AF2E File Offset: 0x0003912E
	private void TryDisableEventHandlers()
	{
		if (!this._eventsActive)
		{
			return;
		}
		this.LocomotionTeleport.UpdateTeleportDestination -= this._updateTeleportDestinationAction;
		this._eventsActive = false;
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x0003AF51 File Offset: 0x00039151
	public void OnDisable()
	{
		this.TryDisableEventHandlers();
	}

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06001184 RID: 4484 RVA: 0x000ABFF0 File Offset: 0x000AA1F0
	// (remove) Token: 0x06001185 RID: 4485 RVA: 0x000AC028 File Offset: 0x000AA228
	public event Action<TeleportDestination> Deactivated;

	// Token: 0x06001186 RID: 4486 RVA: 0x0003AF59 File Offset: 0x00039159
	public void OnDeactivated()
	{
		if (this.Deactivated != null)
		{
			this.Deactivated(this);
			return;
		}
		this.Recycle();
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x0003AF76 File Offset: 0x00039176
	public void Recycle()
	{
		this.LocomotionTeleport.RecycleTeleportDestination(this);
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x000AC060 File Offset: 0x000AA260
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

	// Token: 0x04001368 RID: 4968
	[Tooltip("If the target handler provides a target position, this transform will be moved to that position and it's game object enabled. A target position being provided does not mean the position is valid, only that the aim handler found something to test as a destination.")]
	public Transform PositionIndicator;

	// Token: 0x04001369 RID: 4969
	[Tooltip("This transform will be rotated to match the rotation of the aiming target. Simple teleport destinations should assign this to the object containing this component. More complex teleport destinations might assign this to a sub-object that is used to indicate the landing orientation independently from the rest of the destination indicator, such as when world space effects are required. This will typically be a child of the PositionIndicator.")]
	public Transform OrientationIndicator;

	// Token: 0x0400136A RID: 4970
	[Tooltip("After the player teleports, the character controller will have it's rotation set to this value. It is different from the OrientationIndicator transform.rotation in order to support both head-relative and forward-facing teleport modes (See TeleportOrientationHandlerThumbstick.cs).")]
	public Quaternion LandingRotation;

	// Token: 0x0400136B RID: 4971
	[NonSerialized]
	public LocomotionTeleport LocomotionTeleport;

	// Token: 0x0400136C RID: 4972
	[NonSerialized]
	public LocomotionTeleport.States TeleportState;

	// Token: 0x0400136D RID: 4973
	private readonly Action<bool, Vector3?, Quaternion?, Quaternion?> _updateTeleportDestinationAction;

	// Token: 0x0400136E RID: 4974
	private bool _eventsActive;
}
