using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class LeafBlowerEffects : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x0600095E RID: 2398 RVA: 0x000369C6 File Offset: 0x00034BC6
	// (set) Token: 0x0600095F RID: 2399 RVA: 0x000369CE File Offset: 0x00034BCE
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000960 RID: 2400 RVA: 0x000369D7 File Offset: 0x00034BD7
	// (set) Token: 0x06000961 RID: 2401 RVA: 0x000369DF File Offset: 0x00034BDF
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000962 RID: 2402 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x000919B8 File Offset: 0x0008FBB8
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.headToleranceAngleCos = Mathf.Cos(0.017453292f * this.headToleranceAngle);
		this.squareHitAngleCos = Mathf.Cos(0.017453292f * this.squareHitAngle);
		this.fan = rig.cosmeticReferences.Get(this.fanRef).GetComponent<CosmeticFan>();
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x000369E8 File Offset: 0x00034BE8
	public void StartFan()
	{
		this.fan.Run();
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x000369F5 File Offset: 0x00034BF5
	public void StopFan()
	{
		this.fan.Stop();
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00036A02 File Offset: 0x00034C02
	public void UpdateEffects()
	{
		this.ProjectParticles();
		this.BlowFaces();
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00091A10 File Offset: 0x0008FC10
	public void ProjectParticles()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.gunBarrel.transform.position, this.gunBarrel.transform.forward, out raycastHit, this.projectionRange, this.raycastLayers))
		{
			SpawnOnEnter component = raycastHit.collider.GetComponent<SpawnOnEnter>();
			if (component != null)
			{
				component.OnTriggerEnter(raycastHit.collider);
			}
			if (Vector3.Dot(raycastHit.normal, this.gunBarrel.transform.forward) < -this.squareHitAngleCos)
			{
				this.squareHitParticleSystem.transform.position = raycastHit.point;
				this.squareHitParticleSystem.transform.rotation = Quaternion.LookRotation(raycastHit.normal, this.gunBarrel.transform.forward);
				if (this.angledHitParticleSystem != this.squareHitParticleSystem && this.angledHitParticleSystem.isPlaying)
				{
					this.angledHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
				if (!this.squareHitParticleSystem.isPlaying)
				{
					this.squareHitParticleSystem.Play(true);
					return;
				}
			}
			else
			{
				this.angledHitParticleSystem.transform.position = raycastHit.point;
				this.angledHitParticleSystem.transform.rotation = Quaternion.LookRotation(raycastHit.normal, this.gunBarrel.transform.forward);
				if (this.angledHitParticleSystem != this.squareHitParticleSystem && this.squareHitParticleSystem.isPlaying)
				{
					this.squareHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				}
				if (!this.angledHitParticleSystem.isPlaying)
				{
					this.angledHitParticleSystem.Play(true);
					return;
				}
			}
		}
		else
		{
			this.StopEffects();
		}
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00036A10 File Offset: 0x00034C10
	public void StopEffects()
	{
		this.angledHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		this.squareHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00091BC4 File Offset: 0x0008FDC4
	public void BlowFaces()
	{
		Vector3 position = this.gunBarrel.transform.position;
		Vector3 forward = this.gunBarrel.transform.forward;
		if (NetworkSystem.Instance.InRoom)
		{
			using (List<VRRig>.Enumerator enumerator = GorillaParent.instance.vrrigs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VRRig rig = enumerator.Current;
					this.TryBlowFace(rig, position, forward);
				}
				return;
			}
		}
		this.TryBlowFace(VRRig.LocalRig, position, forward);
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00091C5C File Offset: 0x0008FE5C
	private void TryBlowFace(VRRig rig, Vector3 origin, Vector3 directionNormalized)
	{
		Transform rigTarget = rig.head.rigTarget;
		Vector3 vector = rigTarget.position - origin;
		float num = Vector3.Dot(vector, directionNormalized);
		if (num < 0f || num > this.projectionRange)
		{
			return;
		}
		if ((vector - num * directionNormalized).IsLongerThan(this.projectionWidth))
		{
			return;
		}
		if (Vector3.Dot(-rigTarget.forward, vector.normalized) < this.headToleranceAngleCos)
		{
			return;
		}
		rig.GetComponent<GorillaMouthFlap>().EnableLeafBlower();
	}

	// Token: 0x04000B54 RID: 2900
	[SerializeField]
	private GameObject gunBarrel;

	// Token: 0x04000B55 RID: 2901
	[SerializeField]
	private float projectionRange;

	// Token: 0x04000B56 RID: 2902
	[SerializeField]
	private float projectionWidth;

	// Token: 0x04000B57 RID: 2903
	[SerializeField]
	private float headToleranceAngle;

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	private LayerMask raycastLayers;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private ParticleSystem angledHitParticleSystem;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private ParticleSystem squareHitParticleSystem;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private float squareHitAngle;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private CosmeticRefID fanRef;

	// Token: 0x04000B5D RID: 2909
	private float headToleranceAngleCos;

	// Token: 0x04000B5E RID: 2910
	private float squareHitAngleCos;

	// Token: 0x04000B5F RID: 2911
	private CosmeticFan fan;
}
