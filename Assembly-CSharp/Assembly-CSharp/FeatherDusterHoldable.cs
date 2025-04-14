using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class FeatherDusterHoldable : MonoBehaviour
{
	// Token: 0x0600057F RID: 1407 RVA: 0x00020A0B File Offset: 0x0001EC0B
	protected void Awake()
	{
		this.timeSinceLastSound = this.soundCooldown;
		this.emissionModule = this.particleFx.emission;
		this.initialRateOverTime = this.emissionModule.rateOverTimeMultiplier;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00020A3B File Offset: 0x0001EC3B
	protected void OnEnable()
	{
		this.lastWorldPos = base.transform.position;
		this.emissionModule.rateOverTimeMultiplier = 0f;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00020A60 File Offset: 0x0001EC60
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

	// Token: 0x0400066D RID: 1645
	public LayerMask collisionLayer;

	// Token: 0x0400066E RID: 1646
	public float overlapSphereRadius = 0.08f;

	// Token: 0x0400066F RID: 1647
	[Tooltip("Collision is not tested until this speed requirement is met.")]
	private float collideMinSpeed = 1f;

	// Token: 0x04000670 RID: 1648
	public ParticleSystem particleFx;

	// Token: 0x04000671 RID: 1649
	public SoundBankPlayer soundBankPlayer;

	// Token: 0x04000672 RID: 1650
	private float soundCooldown = 0.8f;

	// Token: 0x04000673 RID: 1651
	private ParticleSystem.EmissionModule emissionModule;

	// Token: 0x04000674 RID: 1652
	private float initialRateOverTime;

	// Token: 0x04000675 RID: 1653
	private float timeSinceLastSound;

	// Token: 0x04000676 RID: 1654
	private Vector3 lastWorldPos;

	// Token: 0x04000677 RID: 1655
	private Collider[] colliderResult = new Collider[1];
}
