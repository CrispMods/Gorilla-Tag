using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class GravityOverrideVolume : MonoBehaviour
{
	// Token: 0x06001BE9 RID: 7145 RVA: 0x00087F60 File Offset: 0x00086160
	private void OnEnable()
	{
		if (this.triggerEvents != null)
		{
			this.triggerEvents.CompositeTriggerEnter += this.OnColliderEnteredVolume;
			this.triggerEvents.CompositeTriggerExit += this.OnColliderExitedVolume;
		}
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00087F9E File Offset: 0x0008619E
	private void OnDisable()
	{
		if (this.triggerEvents != null)
		{
			this.triggerEvents.CompositeTriggerEnter -= this.OnColliderEnteredVolume;
			this.triggerEvents.CompositeTriggerExit -= this.OnColliderExitedVolume;
		}
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00087FDC File Offset: 0x000861DC
	private void OnColliderEnteredVolume(Collider collider)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && collider == instance.headCollider)
		{
			instance.SetGravityOverride(this, new Action<GTPlayer>(this.GravityOverrideFunction));
		}
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x0008801C File Offset: 0x0008621C
	private void OnColliderExitedVolume(Collider collider)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && collider == instance.headCollider)
		{
			instance.UnsetGravityOverride(this);
		}
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x00088050 File Offset: 0x00086250
	public void GravityOverrideFunction(GTPlayer player)
	{
		GravityOverrideVolume.GravityType gravityType = this.gravityType;
		if (gravityType == GravityOverrideVolume.GravityType.Directional)
		{
			Vector3 forward = this.referenceTransform.forward;
			player.AddForce(forward * this.strength, ForceMode.Acceleration);
			return;
		}
		if (gravityType != GravityOverrideVolume.GravityType.Radial)
		{
			return;
		}
		Vector3 normalized = (this.referenceTransform.position - player.headCollider.transform.position).normalized;
		player.AddForce(normalized * this.strength, ForceMode.Acceleration);
	}

	// Token: 0x04001EFA RID: 7930
	[SerializeField]
	private GravityOverrideVolume.GravityType gravityType;

	// Token: 0x04001EFB RID: 7931
	[SerializeField]
	private float strength = 9.8f;

	// Token: 0x04001EFC RID: 7932
	[SerializeField]
	[Tooltip("In Radial: the center point of gravity, In Directional: the forward vector of this transform defines the direction")]
	private Transform referenceTransform;

	// Token: 0x04001EFD RID: 7933
	[SerializeField]
	private CompositeTriggerEvents triggerEvents;

	// Token: 0x02000480 RID: 1152
	public enum GravityType
	{
		// Token: 0x04001EFF RID: 7935
		Directional,
		// Token: 0x04001F00 RID: 7936
		Radial
	}
}
