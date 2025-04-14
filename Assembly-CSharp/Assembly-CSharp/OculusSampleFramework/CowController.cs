using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A68 RID: 2664
	public class CowController : MonoBehaviour
	{
		// Token: 0x06004260 RID: 16992 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Start()
		{
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x00139608 File Offset: 0x00137808
		public void PlayMooSound()
		{
			this._mooCowAudioSource.timeSamples = 0;
			this._mooCowAudioSource.GTPlay();
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x00139621 File Offset: 0x00137821
		public void GoMooCowGo()
		{
			this._cowAnimation.Rewind();
			this._cowAnimation.Play();
		}

		// Token: 0x04004359 RID: 17241
		[SerializeField]
		private Animation _cowAnimation;

		// Token: 0x0400435A RID: 17242
		[SerializeField]
		private AudioSource _mooCowAudioSource;
	}
}
