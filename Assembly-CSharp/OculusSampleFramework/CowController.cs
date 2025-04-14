using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A65 RID: 2661
	public class CowController : MonoBehaviour
	{
		// Token: 0x06004254 RID: 16980 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Start()
		{
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x00139040 File Offset: 0x00137240
		public void PlayMooSound()
		{
			this._mooCowAudioSource.timeSamples = 0;
			this._mooCowAudioSource.GTPlay();
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00139059 File Offset: 0x00137259
		public void GoMooCowGo()
		{
			this._cowAnimation.Rewind();
			this._cowAnimation.Play();
		}

		// Token: 0x04004347 RID: 17223
		[SerializeField]
		private Animation _cowAnimation;

		// Token: 0x04004348 RID: 17224
		[SerializeField]
		private AudioSource _mooCowAudioSource;
	}
}
