using System;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class TeleportOrientationHandlerHMD : TeleportOrientationHandler
{
	// Token: 0x06001202 RID: 4610 RVA: 0x0003C42B File Offset: 0x0003A62B
	protected override void InitializeTeleportDestination()
	{
		this._initialRotation = Quaternion.identity;
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x000AEF40 File Offset: 0x000AD140
	protected override void UpdateTeleportDestination()
	{
		if (this.AimData.Destination != null && (this.UpdateOrientationDuringAim || base.LocomotionTeleport.CurrentState == LocomotionTeleport.States.PreTeleport))
		{
			Transform centerEyeAnchor = base.LocomotionTeleport.LocomotionController.CameraRig.centerEyeAnchor;
			Vector3 valueOrDefault = this.AimData.Destination.GetValueOrDefault();
			Plane plane = new Plane(Vector3.up, valueOrDefault);
			float d;
			if (plane.Raycast(new Ray(centerEyeAnchor.position, centerEyeAnchor.forward), out d))
			{
				Vector3 vector = centerEyeAnchor.position + centerEyeAnchor.forward * d - valueOrDefault;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude > this.AimDistanceThreshold)
				{
					vector.Normalize();
					Quaternion quaternion = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z), Vector3.up);
					this._initialRotation = quaternion;
					if (this.AimDistanceMaxRange > 0f && magnitude > this.AimDistanceMaxRange)
					{
						this.AimData.TargetValid = false;
					}
					base.LocomotionTeleport.OnUpdateTeleportDestination(this.AimData.TargetValid, this.AimData.Destination, new Quaternion?(quaternion), new Quaternion?(base.GetLandingOrientation(this.OrientationMode, quaternion)));
					return;
				}
			}
		}
		base.LocomotionTeleport.OnUpdateTeleportDestination(this.AimData.TargetValid, this.AimData.Destination, new Quaternion?(this._initialRotation), new Quaternion?(base.GetLandingOrientation(this.OrientationMode, this._initialRotation)));
	}

	// Token: 0x040013E3 RID: 5091
	[Tooltip("HeadRelative=Character will orient to match the arrow. ForwardFacing=When user orients to match the arrow, they will be facing the sensors.")]
	public TeleportOrientationHandler.OrientationModes OrientationMode;

	// Token: 0x040013E4 RID: 5092
	[Tooltip("Should the destination orientation be updated during the aim state in addition to the PreTeleport state?")]
	public bool UpdateOrientationDuringAim;

	// Token: 0x040013E5 RID: 5093
	[Tooltip("How far from the destination must the HMD be pointing before using it for orientation")]
	public float AimDistanceThreshold;

	// Token: 0x040013E6 RID: 5094
	[Tooltip("How far from the destination must the HMD be pointing before rejecting the teleport")]
	public float AimDistanceMaxRange;

	// Token: 0x040013E7 RID: 5095
	private Quaternion _initialRotation;
}
