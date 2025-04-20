using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class GravityOverrideVolume : MonoBehaviour
{
	// Token: 0x06001C3D RID: 7229 RVA: 0x000436E6 File Offset: 0x000418E6
	private void OnEnable()
	{
		if (this.triggerEvents != null)
		{
			this.triggerEvents.CompositeTriggerEnter += this.OnColliderEnteredVolume;
			this.triggerEvents.CompositeTriggerExit += this.OnColliderExitedVolume;
		}
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x00043724 File Offset: 0x00041924
	private void OnDisable()
	{
		if (this.triggerEvents != null)
		{
			this.triggerEvents.CompositeTriggerEnter -= this.OnColliderEnteredVolume;
			this.triggerEvents.CompositeTriggerExit -= this.OnColliderExitedVolume;
		}
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x000DBD14 File Offset: 0x000D9F14
	private void OnColliderEnteredVolume(Collider collider)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && collider == instance.headCollider)
		{
			instance.SetGravityOverride(this, new Action<GTPlayer>(this.GravityOverrideFunction));
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x000DBD54 File Offset: 0x000D9F54
	private void OnColliderExitedVolume(Collider collider)
	{
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null && collider == instance.headCollider)
		{
			instance.UnsetGravityOverride(this);
		}
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x000DBD88 File Offset: 0x000D9F88
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

	// Token: 0x04001F49 RID: 8009
	[SerializeField]
	private GravityOverrideVolume.GravityType gravityType;

	// Token: 0x04001F4A RID: 8010
	[SerializeField]
	private float strength = 9.8f;

	// Token: 0x04001F4B RID: 8011
	[SerializeField]
	[Tooltip("In Radial: the center point of gravity, In Directional: the forward vector of this transform defines the direction")]
	private Transform referenceTransform;

	// Token: 0x04001F4C RID: 8012
	[SerializeField]
	private CompositeTriggerEvents triggerEvents;

	// Token: 0x0200048C RID: 1164
	public enum GravityType
	{
		// Token: 0x04001F4E RID: 8014
		Directional,
		// Token: 0x04001F4F RID: 8015
		Radial
	}
}
