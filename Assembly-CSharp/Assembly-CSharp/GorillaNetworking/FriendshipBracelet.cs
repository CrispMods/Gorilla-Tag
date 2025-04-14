using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AA2 RID: 2722
	public class FriendshipBracelet : MonoBehaviour
	{
		// Token: 0x060043FE RID: 17406 RVA: 0x0014261E File Offset: 0x0014081E
		protected void Awake()
		{
			this.ownerRig = base.GetComponentInParent<VRRig>();
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0014262C File Offset: 0x0014082C
		private AudioSource GetAudioSource()
		{
			if (!this.isLeftHand)
			{
				return this.ownerRig.rightHandPlayer;
			}
			return this.ownerRig.leftHandPlayer;
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0014264D File Offset: 0x0014084D
		private void OnEnable()
		{
			this.PlayAppearEffects();
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x00142655 File Offset: 0x00140855
		public void PlayAppearEffects()
		{
			this.GetAudioSource().GTPlayOneShot(this.braceletFormedSound, 1f);
			if (this.braceletFormedParticle)
			{
				this.braceletFormedParticle.Play();
			}
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x00142688 File Offset: 0x00140888
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

		// Token: 0x06004403 RID: 17411 RVA: 0x001426D8 File Offset: 0x001408D8
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

		// Token: 0x04004555 RID: 17749
		[SerializeField]
		private SkinnedMeshRenderer[] braceletStrings;

		// Token: 0x04004556 RID: 17750
		[SerializeField]
		private MeshRenderer[] braceletBeads;

		// Token: 0x04004557 RID: 17751
		[SerializeField]
		private MeshRenderer[] braceletBananas;

		// Token: 0x04004558 RID: 17752
		[SerializeField]
		private bool isLeftHand;

		// Token: 0x04004559 RID: 17753
		[SerializeField]
		private AudioClip braceletFormedSound;

		// Token: 0x0400455A RID: 17754
		[SerializeField]
		private AudioClip braceletBrokenSound;

		// Token: 0x0400455B RID: 17755
		[SerializeField]
		private ParticleSystem braceletFormedParticle;

		// Token: 0x0400455C RID: 17756
		[SerializeField]
		private ParticleSystem braceletBrokenParticle;

		// Token: 0x0400455D RID: 17757
		private VRRig ownerRig;
	}
}
