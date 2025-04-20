using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class RotationSoundPlayer : MonoBehaviour
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x0008357C File Offset: 0x0008177C
	private void Awake()
	{
		List<Transform> list = new List<Transform>(this.transforms);
		list.RemoveAll((Transform xform) => xform == null);
		this.transforms = list.ToArray();
		this.initialUpAxis = new Vector3[this.transforms.Length];
		this.lastUpAxis = new Vector3[this.transforms.Length];
		this.lastRotationSpeeds = new float[this.transforms.Length];
		for (int i = 0; i < this.transforms.Length; i++)
		{
			this.initialUpAxis[i] = this.transforms[i].localRotation * Vector3.up;
			this.lastUpAxis[i] = this.initialUpAxis[i];
			this.lastRotationSpeeds[i] = 0f;
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0008365C File Offset: 0x0008185C
	private void Update()
	{
		this.cooldownTimer -= Time.deltaTime;
		for (int i = 0; i < this.transforms.Length; i++)
		{
			Vector3 vector = this.transforms[i].localRotation * Vector3.up;
			float num = Vector3.Angle(vector, this.initialUpAxis[i]);
			float num2 = Vector3.Angle(vector, this.lastUpAxis[i]);
			float deltaTime = Time.deltaTime;
			float num3 = num2 / deltaTime;
			if (this.cooldownTimer <= 0f && num > this.rotationAmountThreshold && num3 > this.rotationSpeedThreshold && !this.soundBankPlayer.isPlaying)
			{
				this.cooldownTimer = this.cooldown;
				this.soundBankPlayer.Play();
			}
			this.lastUpAxis[i] = vector;
			this.lastRotationSpeeds[i] = num3;
		}
	}

	// Token: 0x040006B8 RID: 1720
	[Tooltip("Transforms that will make a noise when they rotate.")]
	[SerializeField]
	private Transform[] transforms;

	// Token: 0x040006B9 RID: 1721
	[SerializeField]
	private SoundBankPlayer soundBankPlayer;

	// Token: 0x040006BA RID: 1722
	[Tooltip("How much the transform must rotate from it's initial rotation before a sound is played.")]
	private float rotationAmountThreshold = 30f;

	// Token: 0x040006BB RID: 1723
	[Tooltip("How fast the transform must rotate before a sound is played.")]
	private float rotationSpeedThreshold = 45f;

	// Token: 0x040006BC RID: 1724
	private float cooldown = 0.6f;

	// Token: 0x040006BD RID: 1725
	private float cooldownTimer;

	// Token: 0x040006BE RID: 1726
	private Vector3[] initialUpAxis;

	// Token: 0x040006BF RID: 1727
	private Vector3[] lastUpAxis;

	// Token: 0x040006C0 RID: 1728
	private float[] lastRotationSpeeds;
}
