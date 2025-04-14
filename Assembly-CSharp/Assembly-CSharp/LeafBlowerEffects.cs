using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class LeafBlowerEffects : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x00031431 File Offset: 0x0002F631
	// (set) Token: 0x06000914 RID: 2324 RVA: 0x00031439 File Offset: 0x0002F639
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x00031442 File Offset: 0x0002F642
	// (set) Token: 0x06000916 RID: 2326 RVA: 0x0003144A File Offset: 0x0002F64A
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000917 RID: 2327 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00031454 File Offset: 0x0002F654
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.headToleranceAngleCos = Mathf.Cos(0.017453292f * this.headToleranceAngle);
		this.squareHitAngleCos = Mathf.Cos(0.017453292f * this.squareHitAngle);
		this.fan = rig.cosmeticReferences.Get(this.fanRef).GetComponent<CosmeticFan>();
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000314AB File Offset: 0x0002F6AB
	public void StartFan()
	{
		this.fan.Run();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000314B8 File Offset: 0x0002F6B8
	public void StopFan()
	{
		this.fan.Stop();
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x000314C5 File Offset: 0x0002F6C5
	public void UpdateEffects()
	{
		this.ProjectParticles();
		this.BlowFaces();
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x000314D4 File Offset: 0x0002F6D4
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

	// Token: 0x0600091D RID: 2333 RVA: 0x00031686 File Offset: 0x0002F886
	public void StopEffects()
	{
		this.angledHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		this.squareHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x000316A4 File Offset: 0x0002F8A4
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

	// Token: 0x0600091F RID: 2335 RVA: 0x0003173C File Offset: 0x0002F93C
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

	// Token: 0x04000B0E RID: 2830
	[SerializeField]
	private GameObject gunBarrel;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private float projectionRange;

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private float projectionWidth;

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private float headToleranceAngle;

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private LayerMask raycastLayers;

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private ParticleSystem angledHitParticleSystem;

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private ParticleSystem squareHitParticleSystem;

	// Token: 0x04000B15 RID: 2837
	[SerializeField]
	private float squareHitAngle;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private CosmeticRefID fanRef;

	// Token: 0x04000B17 RID: 2839
	private float headToleranceAngleCos;

	// Token: 0x04000B18 RID: 2840
	private float squareHitAngleCos;

	// Token: 0x04000B19 RID: 2841
	private CosmeticFan fan;
}
