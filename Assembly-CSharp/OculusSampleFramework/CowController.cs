using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A92 RID: 2706
	public class CowController : MonoBehaviour
	{
		// Token: 0x06004399 RID: 17305 RVA: 0x00030607 File Offset: 0x0002E807
		private void Start()
		{
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x0005C149 File Offset: 0x0005A349
		public void PlayMooSound()
		{
			this._mooCowAudioSource.timeSamples = 0;
			this._mooCowAudioSource.GTPlay();
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0005C162 File Offset: 0x0005A362
		public void GoMooCowGo()
		{
			this._cowAnimation.Rewind();
			this._cowAnimation.Play();
		}

		// Token: 0x04004441 RID: 17473
		[SerializeField]
		private Animation _cowAnimation;

		// Token: 0x04004442 RID: 17474
		[SerializeField]
		private AudioSource _mooCowAudioSource;
	}
}
