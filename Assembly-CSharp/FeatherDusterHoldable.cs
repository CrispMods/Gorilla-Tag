using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class FeatherDusterHoldable : MonoBehaviour
{
	// Token: 0x0600057D RID: 1405 RVA: 0x000206E7 File Offset: 0x0001E8E7
	protected void Awake()
	{
		this.timeSinceLastSound = this.soundCooldown;
		this.emissionModule = this.particleFx.emission;
		this.initialRateOverTime = this.emissionModule.rateOverTimeMultiplier;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00020717 File Offset: 0x0001E917
	protected void OnEnable()
	{
		this.lastWorldPos = base.transform.position;
		this.emissionModule.rateOverTimeMultiplier = 0f;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0002073C File Offset: 0x0001E93C
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

	// Token: 0x0400066C RID: 1644
	public LayerMask collisionLayer;

	// Token: 0x0400066D RID: 1645
	public float overlapSphereRadius = 0.08f;

	// Token: 0x0400066E RID: 1646
	[Tooltip("Collision is not tested until this speed requirement is met.")]
	private float collideMinSpeed = 1f;

	// Token: 0x0400066F RID: 1647
	public ParticleSystem particleFx;

	// Token: 0x04000670 RID: 1648
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x04000671 RID: 1649
	private float soundCooldown = 0.8f;

	// Token: 0x04000672 RID: 1650
	private ParticleSystem.EmissionModule emissionModule;

	// Token: 0x04000673 RID: 1651
	private float initialRateOverTime;

	// Token: 0x04000674 RID: 1652
	private float timeSinceLastSound;

	// Token: 0x04000675 RID: 1653
	private Vector3 lastWorldPos;

	// Token: 0x04000676 RID: 1654
	private Collider[] colliderResult = new Collider[1];
}
