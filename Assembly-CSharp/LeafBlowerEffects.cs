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
	// (get) Token: 0x06000911 RID: 2321 RVA: 0x0003110D File Offset: 0x0002F30D
	// (set) Token: 0x06000912 RID: 2322 RVA: 0x00031115 File Offset: 0x0002F315
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x0003111E File Offset: 0x0002F31E
	// (set) Token: 0x06000914 RID: 2324 RVA: 0x00031126 File Offset: 0x0002F326
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000915 RID: 2325 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00031130 File Offset: 0x0002F330
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.headToleranceAngleCos = Mathf.Cos(0.017453292f * this.headToleranceAngle);
		this.squareHitAngleCos = Mathf.Cos(0.017453292f * this.squareHitAngle);
		this.fan = rig.cosmeticReferences.Get(this.fanRef).GetComponent<CosmeticFan>();
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00031187 File Offset: 0x0002F387
	public void StartFan()
	{
		this.fan.Run();
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00031194 File Offset: 0x0002F394
	public void StopFan()
	{
		this.fan.Stop();
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000311A1 File Offset: 0x0002F3A1
	public void UpdateEffects()
	{
		this.ProjectParticles();
		this.BlowFaces();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000311B0 File Offset: 0x0002F3B0
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

	// Token: 0x0600091B RID: 2331 RVA: 0x00031362 File Offset: 0x0002F562
	public void StopEffects()
	{
		this.angledHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		this.squareHitParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00031380 File Offset: 0x0002F580
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

	// Token: 0x0600091D RID: 2333 RVA: 0x00031418 File Offset: 0x0002F618
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

	// Token: 0x04000B0D RID: 2829
	[SerializeField]
	private GameObject gunBarrel;

	// Token: 0x04000B0E RID: 2830
	[SerializeField]
	private float projectionRange;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private float projectionWidth;

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private float headToleranceAngle;

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private LayerMask raycastLayers;

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private ParticleSystem angledHitParticleSystem;

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private ParticleSystem squareHitParticleSystem;

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private float squareHitAngle;

	// Token: 0x04000B15 RID: 2837
	[SerializeField]
	private CosmeticRefID fanRef;

	// Token: 0x04000B16 RID: 2838
	private float headToleranceAngleCos;

	// Token: 0x04000B17 RID: 2839
	private float squareHitAngleCos;

	// Token: 0x04000B18 RID: 2840
	private CosmeticFan fan;
}
