using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class GravityOverrideVolume : MonoBehaviour
{
	// Token: 0x06001BEC RID: 7148 RVA: 0x000423AD File Offset: 0x000405AD
	private void OnEnable()
	{
		if (this.triggerEvents != null)
		{
			this.triggerEvents.CompositeTriggerEnter += this.OnColliderEnteredVolume;
			this.triggerEvents.CompositeTriggerExit += this.OnColliderExitedVolume;
		}
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x000423EB File Offset: 0x000405EB
	private void OnDisable()
	{
		if (this.triggerEvents != null)
		{
			this.triggerEvents.CompositeTriggerEnter -= this.OnColliderEnteredVolume;
			this.triggerEvents.CompositeTriggerExit -= this.OnColliderExitedVolume;
		}
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x000D9064 File Offset: 0x000D7264
	private void OnColliderEnteredVolume(Collider collider)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && collider == instance.headCollider)
		{
			instance.SetGravityOverride(this, new Action<GTPlayer>(this.GravityOverrideFunction));
		}
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x000D90A4 File Offset: 0x000D72A4
	private void OnColliderExitedVolume(Collider collider)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && collider == instance.headCollider)
		{
			instance.UnsetGravityOverride(this);
		}
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x000D90D8 File Offset: 0x000D72D8
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

	// Token: 0x04001EFB RID: 7931
	[SerializeField]
	private GravityOverrideVolume.GravityType gravityType;

	// Token: 0x04001EFC RID: 7932
	[SerializeField]
	private float strength = 9.8f;

	// Token: 0x04001EFD RID: 7933
	[SerializeField]
	[Tooltip("In Radial: the center point of gravity, In Directional: the forward vector of this transform defines the direction")]
	private Transform referenceTransform;

	// Token: 0x04001EFE RID: 7934
	[SerializeField]
	private CompositeTriggerEvents triggerEvents;

	// Token: 0x02000480 RID: 1152
	public enum GravityType
	{
		// Token: 0x04001F00 RID: 7936
		Directional,
		// Token: 0x04001F01 RID: 7937
		Radial
	}
}
