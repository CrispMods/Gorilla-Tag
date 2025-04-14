using System;
using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CrittersToolThrowable : CrittersActor
{
	// Token: 0x06000267 RID: 615 RVA: 0x0000F6AA File Offset: 0x0000D8AA
	public override void Initialize()
	{
		base.Initialize();
		this.hasBeenGrabbedByPlayer = false;
		this.shouldDisable = false;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
	public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
		this.hasBeenGrabbedByPlayer = true;
		this.OnPickedUp();
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000F6DC File Offset: 0x0000D8DC
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
		if (this.requiredActivationSpeed > 0f && this.rb.velocity.sqrMagnitude < this.requiredActivationSpeed * this.requiredActivationSpeed)
		{
			return;
		}
		if (this.onlyTriggerOnDirectCritterHit)
		{
			CrittersPawn component = collision.gameObject.GetComponent<CrittersPawn>();
			if (component != null && component.isActiveAndEnabled)
			{
				this.OnImpactCritter(component);
			}
		}
		else
		{
			Vector3 point = collision.contacts[0].point;
			Vector3 normal = collision.contacts[0].normal;
			this.OnImpact(point, normal);
		}
		if (this.destroyOnImpact)
		{
			this.shouldDisable = true;
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
	}

	// Token: 0x0600026B RID: 619 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnImpactCritter(CrittersPawn impactedCritter)
	{
	}

	// Token: 0x0600026C RID: 620 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnPickedUp()
	{
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
	[Conditional("DRAW_DEBUG")]
	protected void ShowDebugVisualization(Vector3 position, float scale, float duration = 0f)
	{
		if (!this.debugImpactPrefab)
		{
			return;
		}
		DelayedDestroyObject delayedDestroyObject = Object.Instantiate<DelayedDestroyObject>(this.debugImpactPrefab, position, Quaternion.identity);
		delayedDestroyObject.transform.localScale *= scale;
		if (duration != 0f)
		{
			delayedDestroyObject.lifetime = duration;
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000F808 File Offset: 0x0000DA08
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

	// Token: 0x040002E0 RID: 736
	[Header("Throwable")]
	public bool requiresPlayerGrabBeforeActivate = true;

	// Token: 0x040002E1 RID: 737
	public float requiredActivationSpeed = 2f;

	// Token: 0x040002E2 RID: 738
	public bool onlyTriggerOnDirectCritterHit;

	// Token: 0x040002E3 RID: 739
	public bool destroyOnImpact = true;

	// Token: 0x040002E4 RID: 740
	[Header("Debug")]
	[SerializeField]
	private DelayedDestroyObject debugImpactPrefab;

	// Token: 0x040002E5 RID: 741
	private bool hasBeenGrabbedByPlayer;

	// Token: 0x040002E6 RID: 742
	protected bool shouldDisable;
}
