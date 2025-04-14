using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class RotationSoundPlayer : MonoBehaviour
{
	// Token: 0x06000583 RID: 1411 RVA: 0x00020B5C File Offset: 0x0001ED5C
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

	// Token: 0x06000584 RID: 1412 RVA: 0x00020C3C File Offset: 0x0001EE3C
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

	// Token: 0x04000678 RID: 1656
	[Tooltip("Transforms that will make a noise when they rotate.")]
	[SerializeField]
	private Transform[] transforms;

	// Token: 0x04000679 RID: 1657
	[SerializeField]
	private SoundBankPlayer soundBankPlayer;

	// Token: 0x0400067A RID: 1658
	[Tooltip("How much the transform must rotate from it's initial rotation before a sound is played.")]
	private float rotationAmountThreshold = 30f;

	// Token: 0x0400067B RID: 1659
	[Tooltip("How fast the transform must rotate before a sound is played.")]
	private float rotationSpeedThreshold = 45f;

	// Token: 0x0400067C RID: 1660
	private float cooldown = 0.6f;

	// Token: 0x0400067D RID: 1661
	private float cooldownTimer;

	// Token: 0x0400067E RID: 1662
	private Vector3[] initialUpAxis;

	// Token: 0x0400067F RID: 1663
	private Vector3[] lastUpAxis;

	// Token: 0x04000680 RID: 1664
	private float[] lastRotationSpeeds;
}
