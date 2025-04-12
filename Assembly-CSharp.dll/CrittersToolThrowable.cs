using System;
using System.Diagnostics;
using Unity.XR.CoreUtils;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CrittersToolThrowable : CrittersActor
{
	// Token: 0x06000269 RID: 617 RVA: 0x00030EF4 File Offset: 0x0002F0F4
	public override void Initialize()
	{
		base.Initialize();
		this.hasBeenGrabbedByPlayer = false;
		this.shouldDisable = false;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00030F0A File Offset: 0x0002F10A
	public override void GrabbedBy(CrittersActor grabbingActor, bool positionOverride = false, Quaternion localRotation = default(Quaternion), Vector3 localOffset = default(Vector3), bool disableGrabbing = false)
	{
		base.GrabbedBy(grabbingActor, positionOverride, localRotation, localOffset, disableGrabbing);
		this.hasBeenGrabbedByPlayer = true;
		this.OnPickedUp();
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00071E68 File Offset: 0x00070068
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

	// Token: 0x0600026C RID: 620 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnImpactCritter(CrittersPawn impactedCritter)
	{
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnPickedUp()
	{
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00071F40 File Offset: 0x00070140
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

	// Token: 0x06000270 RID: 624 RVA: 0x00071F94 File Offset: 0x00070194
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

	// Token: 0x040002E1 RID: 737
	[Header("Throwable")]
	public bool requiresPlayerGrabBeforeActivate = true;

	// Token: 0x040002E2 RID: 738
	public float requiredActivationSpeed = 2f;

	// Token: 0x040002E3 RID: 739
	public bool onlyTriggerOnDirectCritterHit;

	// Token: 0x040002E4 RID: 740
	public bool destroyOnImpact = true;

	// Token: 0x040002E5 RID: 741
	[Header("Debug")]
	[SerializeField]
	private DelayedDestroyObject debugImpactPrefab;

	// Token: 0x040002E6 RID: 742
	private bool hasBeenGrabbedByPlayer;

	// Token: 0x040002E7 RID: 743
	protected bool shouldDisable;
}
