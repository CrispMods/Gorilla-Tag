using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000ACC RID: 2764
	public class FriendshipBracelet : MonoBehaviour
	{
		// Token: 0x06004537 RID: 17719 RVA: 0x0005D1AF File Offset: 0x0005B3AF
		protected void Awake()
		{
			this.ownerRig = base.GetComponentInParent<VRRig>();
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x0005D1BD File Offset: 0x0005B3BD
		private AudioSource GetAudioSource()
		{
			if (!this.isLeftHand)
			{
				return this.ownerRig.rightHandPlayer;
			}
			return this.ownerRig.leftHandPlayer;
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x0005D1DE File Offset: 0x0005B3DE
		private void OnEnable()
		{
			this.PlayAppearEffects();
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x0005D1E6 File Offset: 0x0005B3E6
		public void PlayAppearEffects()
		{
			this.GetAudioSource().GTPlayOneShot(this.braceletFormedSound, 1f);
			if (this.braceletFormedParticle)
			{
				this.braceletFormedParticle.Play();
			}
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00180640 File Offset: 0x0017E840
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

		// Token: 0x0600453C RID: 17724 RVA: 0x00180690 File Offset: 0x0017E890
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

		// Token: 0x0400463A RID: 17978
		[SerializeField]
		private SkinnedMeshRenderer[] braceletStrings;

		// Token: 0x0400463B RID: 17979
		[SerializeField]
		private MeshRenderer[] braceletBeads;

		// Token: 0x0400463C RID: 17980
		[SerializeField]
		private MeshRenderer[] braceletBananas;

		// Token: 0x0400463D RID: 17981
		[SerializeField]
		private bool isLeftHand;

		// Token: 0x0400463E RID: 17982
		[SerializeField]
		private AudioClip braceletFormedSound;

		// Token: 0x0400463F RID: 17983
		[SerializeField]
		private AudioClip braceletBrokenSound;

		// Token: 0x04004640 RID: 17984
		[SerializeField]
		private ParticleSystem braceletFormedParticle;

		// Token: 0x04004641 RID: 17985
		[SerializeField]
		private ParticleSystem braceletBrokenParticle;

		// Token: 0x04004642 RID: 17986
		private VRRig ownerRig;
	}
}
