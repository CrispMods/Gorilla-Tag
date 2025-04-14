using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A9F RID: 2719
	public class FriendshipBracelet : MonoBehaviour
	{
		// Token: 0x060043F2 RID: 17394 RVA: 0x00142056 File Offset: 0x00140256
		protected void Awake()
		{
			this.ownerRig = base.GetComponentInParent<VRRig>();
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x00142064 File Offset: 0x00140264
		private AudioSource GetAudioSource()
		{
			if (!this.isLeftHand)
			{
				return this.ownerRig.rightHandPlayer;
			}
			return this.ownerRig.leftHandPlayer;
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x00142085 File Offset: 0x00140285
		private void OnEnable()
		{
			this.PlayAppearEffects();
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0014208D File Offset: 0x0014028D
		public void PlayAppearEffects()
		{
			this.GetAudioSource().GTPlayOneShot(this.braceletFormedSound, 1f);
			if (this.braceletFormedParticle)
			{
				this.braceletFormedParticle.Play();
			}
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x001420C0 File Offset: 0x001402C0
		private void OnDisable()
		{
			if (!this.ownerRig.gameObject.activeInHierarchy)
			{
				return;
			}
			this.GetAudioSource().GTPlayOneShot(this.braceletBrokenSound, 1f);
			if (this.braceletBrokenParticle)
			{
				this.braceletBrokenParticle.Play();
			}
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00142110 File Offset: 0x00140310
		public void UpdateBeads(List<Color> colors, int selfIndex)
		{
			int num = colors.Count - 1;
			int num2 = (this.braceletBeads.Length - num) / 2;
			for (int i = 0; i < this.braceletBeads.Length; i++)
			{
				int num3 = i - num2;
				if (num3 >= 0 && num3 < num)
				{
					this.braceletBeads[i].enabled = true;
					this.braceletBeads[i].material.color = colors[num3];
					this.braceletBananas[i].gameObject.SetActive(num3 == selfIndex);
				}
				else
				{
					this.braceletBeads[i].enabled = false;
					this.braceletBananas[i].gameObject.SetActive(false);
				}
			}
			SkinnedMeshRenderer[] array = this.braceletStrings;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].material.color = colors[colors.Count - 1];
			}
		}

		// Token: 0x04004543 RID: 17731
		[SerializeField]
		private SkinnedMeshRenderer[] braceletStrings;

		// Token: 0x04004544 RID: 17732
		[SerializeField]
		private MeshRenderer[] braceletBeads;

		// Token: 0x04004545 RID: 17733
		[SerializeField]
		private MeshRenderer[] braceletBananas;

		// Token: 0x04004546 RID: 17734
		[SerializeField]
		private bool isLeftHand;

		// Token: 0x04004547 RID: 17735
		[SerializeField]
		private AudioClip braceletFormedSound;

		// Token: 0x04004548 RID: 17736
		[SerializeField]
		private AudioClip braceletBrokenSound;

		// Token: 0x04004549 RID: 17737
		[SerializeField]
		private ParticleSystem braceletFormedParticle;

		// Token: 0x0400454A RID: 17738
		[SerializeField]
		private ParticleSystem braceletBrokenParticle;

		// Token: 0x0400454B RID: 17739
		private VRRig ownerRig;
	}
}
