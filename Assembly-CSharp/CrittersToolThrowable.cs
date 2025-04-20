using System;
using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class CrittersToolThrowable : CrittersActor
{
	// Token: 0x06000294 RID: 660 RVA: 0x00032036 File Offset: 0x00030236
	public override void Initialize()
	{
		base.Initialize();
		this.hasBeenGrabbedByPlayer = false;
		this.shouldDisable = false;
		this.hasTriggeredSinceLastGrab = false;
		this._sqrActivationSpeed = this.requiredActivationSpeed * this.requiredActivationSpeed;
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00032066 File Offset: 0x00030266
	public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
		this.hasBeenGrabbedByPlayer = true;
		this.hasTriggeredSinceLastGrab = false;
		this.OnPickedUp();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00074444 File Offset: 0x00072644
	public void OnCollisionEnter(Collision collision)
	{
		if (CrittersManager.instance.containerLayer.Contains(collision.gameObject.layer))
		{
			return;
		}
		if (this.requiresPlayerGrabBeforeActivate && !this.hasBeenGrabbedByPlayer)
		{
			return;
		}
		if (this._sqrActivationSpeed > 0f && collision.relativeVelocity.sqrMagnitude < this._sqrActivationSpeed)
		{
			return;
		}
		if (this.onlyTriggerOncePerGrab && this.hasTriggeredSinceLastGrab)
		{
			return;
		}
		if (this.onlyTriggerOnDirectCritterHit)
		{
			CrittersPawn component = collision.gameObject.GetComponent<CrittersPawn>();
			if (component != null && component.isActiveAndEnabled)
			{
				this.hasTriggeredSinceLastGrab = true;
				this.OnImpactCritter(component);
			}
		}
		else
		{
			Vector3 point = collision.contacts[0].point;
			Vector3 normal = collision.contacts[0].normal;
			this.hasTriggeredSinceLastGrab = true;
			this.OnImpact(point, normal);
		}
		if (this.destroyOnImpact)
		{
			this.shouldDisable = true;
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnImpactCritter(CrittersPawn impactedCritter)
	{
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnPickedUp()
	{
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00074530 File Offset: 0x00072730
	[Conditional("DRAW_DEBUG")]
	protected void ShowDebugVisualization(Vector3 position, float scale, float duration = 0f)
	{
		if (!this.debugImpactPrefab)
		{
			return;
		}
		DelayedDestroyObject delayedDestroyObject = UnityEngine.Object.Instantiate<DelayedDestroyObject>(this.debugImpactPrefab, position, Quaternion.identity);
		delayedDestroyObject.transform.localScale *= scale;
		if (duration != 0f)
		{
			delayedDestroyObject.lifetime = duration;
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00074584 File Offset: 0x00072784
	public override bool ProcessLocal()
	{
		bool result = base.ProcessLocal();
		if (this.shouldDisable)
		{
			base.gameObject.SetActive(false);
			return true;
		}
		return result;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000745B0 File Offset: 0x000727B0
	public override void TogglePhysics(bool enable)
	{
		if (enable)
		{
			this.rb.isKinematic = false;
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			return;
		}
		this.rb.isKinematic = true;
		this.rb.interpolation = RigidbodyInterpolation.None;
		this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}

	// Token: 0x0400030F RID: 783
	[Header("Throwable")]
	public bool requiresPlayerGrabBeforeActivate = true;

	// Token: 0x04000310 RID: 784
	public float requiredActivationSpeed = 2f;

	// Token: 0x04000311 RID: 785
	public bool onlyTriggerOnDirectCritterHit;

	// Token: 0x04000312 RID: 786
	public bool destroyOnImpact = true;

	// Token: 0x04000313 RID: 787
	public bool onlyTriggerOncePerGrab = true;

	// Token: 0x04000314 RID: 788
	[Header("Debug")]
	[SerializeField]
	private DelayedDestroyObject debugImpactPrefab;

	// Token: 0x04000315 RID: 789
	private bool hasBeenGrabbedByPlayer;

	// Token: 0x04000316 RID: 790
	protected bool shouldDisable;

	// Token: 0x04000317 RID: 791
	private bool hasTriggeredSinceLastGrab;

	// Token: 0x04000318 RID: 792
	private float _sqrActivationSpeed;
}
