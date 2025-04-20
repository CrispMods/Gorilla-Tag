using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class FeatherDusterHoldable : MonoBehaviour
{
	// Token: 0x060005BE RID: 1470 RVA: 0x000343D0 File Offset: 0x000325D0
	protected void Awake()
	{
		this.timeSinceLastSound = this.soundCooldown;
		this.emissionModule = this.particleFx.emission;
		this.initialRateOverTime = this.emissionModule.rateOverTimeMultiplier;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00034400 File Offset: 0x00032600
	protected void OnEnable()
	{
		this.lastWorldPos = base.transform.position;
		this.emissionModule.rateOverTimeMultiplier = 0f;
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x000834B4 File Offset: 0x000816B4
	protected void Update()
	{
		this.timeSinceLastSound += Time.deltaTime;
		Transform transform = base.transform;
		Vector3 position = transform.position;
		float num = (position - this.lastWorldPos).magnitude / Time.deltaTime;
		this.emissionModule.rateOverTimeMultiplier = 0f;
		if (num >= this.collideMinSpeed && Physics.OverlapSphereNonAlloc(position, this.overlapSphereRadius * transform.localScale.x, this.colliderResult, this.collisionLayer) > 0)
		{
			this.emissionModule.rateOverTimeMultiplier = this.initialRateOverTime;
			if (this.timeSinceLastSound >= this.soundCooldown)
			{
				this.soundBankPlayer.Play();
				this.timeSinceLastSound = 0f;
			}
		}
		this.lastWorldPos = position;
	}

	// Token: 0x040006AD RID: 1709
	public LayerMask collisionLayer;

	// Token: 0x040006AE RID: 1710
	public float overlapSphereRadius = 0.08f;

	// Token: 0x040006AF RID: 1711
	[Tooltip("Collision is not tested until this speed requirement is met.")]
	private float collideMinSpeed = 1f;

	// Token: 0x040006B0 RID: 1712
	public ParticleSystem particleFx;

	// Token: 0x040006B1 RID: 1713
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x040006B2 RID: 1714
	private float soundCooldown = 0.8f;

	// Token: 0x040006B3 RID: 1715
	private ParticleSystem.EmissionModule emissionModule;

	// Token: 0x040006B4 RID: 1716
	private float initialRateOverTime;

	// Token: 0x040006B5 RID: 1717
	private float timeSinceLastSound;

	// Token: 0x040006B6 RID: 1718
	private Vector3 lastWorldPos;

	// Token: 0x040006B7 RID: 1719
	private Collider[] colliderResult = new Collider[1];
}
