using System;
using System.Collections;
using UnityEngine;

namespace UnityChan
{
	// Token: 0x02000CD6 RID: 3286
	public class AutoBlink : MonoBehaviour
	{
		// Token: 0x0600531A RID: 21274 RVA: 0x00030607 File Offset: 0x0002E807
		private void Awake()
		{
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x00065DD3 File Offset: 0x00063FD3
		private void Start()
		{
			this.ResetTimer();
			base.StartCoroutine("RandomChange");
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x00065DE7 File Offset: 0x00063FE7
		private void ResetTimer()
		{
			this.timeRemining = this.timeBlink;
			this.timerStarted = false;
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x001C70CC File Offset: 0x001C52CC
		private void Update()
		{
			if (!this.timerStarted)
			{
				this.eyeStatus = AutoBlink.Status.Close;
				this.timerStarted = true;
			}
			if (this.timerStarted)
			{
				this.timeRemining -= Time.deltaTime;
				if (this.timeRemining <= 0f)
				{
					this.eyeStatus = AutoBlink.Status.Open;
					this.ResetTimer();
					return;
				}
				if (this.timeRemining <= this.timeBlink * 0.3f)
				{
					this.eyeStatus = AutoBlink.Status.HalfClose;
				}
			}
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x001C7140 File Offset: 0x001C5340
		private void LateUpdate()
		{
			if (this.isActive && this.isBlink)
			{
				switch (this.eyeStatus)
				{
				case AutoBlink.Status.Close:
					this.SetCloseEyes();
					return;
				case AutoBlink.Status.HalfClose:
					this.SetHalfCloseEyes();
					return;
				case AutoBlink.Status.Open:
					this.SetOpenEyes();
					this.isBlink = false;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x00065DFC File Offset: 0x00063FFC
		private void SetCloseEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_Close);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_Close);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x00065E22 File Offset: 0x00064022
		private void SetHalfCloseEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_HalfClose);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_HalfClose);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x00065E48 File Offset: 0x00064048
		private void SetOpenEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_Open);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_Open);
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x00065E6E File Offset: 0x0006406E
		private IEnumerator RandomChange()
		{
			for (;;)
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				if (!this.isBlink && num > this.threshold)
				{
					this.isBlink = true;
				}
				yield return new WaitForSeconds(this.interval);
			}
			yield break;
		}

		// Token: 0x040054DE RID: 21726
		public bool isActive = true;

		// Token: 0x040054DF RID: 21727
		public SkinnedMeshRenderer ref_SMR_EYE_DEF;

		// Token: 0x040054E0 RID: 21728
		public SkinnedMeshRenderer ref_SMR_EL_DEF;

		// Token: 0x040054E1 RID: 21729
		public float ratio_Close = 85f;

		// Token: 0x040054E2 RID: 21730
		public float ratio_HalfClose = 20f;

		// Token: 0x040054E3 RID: 21731
		[HideInInspector]
		public float ratio_Open;

		// Token: 0x040054E4 RID: 21732
		private bool timerStarted;

		// Token: 0x040054E5 RID: 21733
		private bool isBlink;

		// Token: 0x040054E6 RID: 21734
		public float timeBlink = 0.4f;

		// Token: 0x040054E7 RID: 21735
		private float timeRemining;

		// Token: 0x040054E8 RID: 21736
		public float threshold = 0.3f;

		// Token: 0x040054E9 RID: 21737
		public float interval = 3f;

		// Token: 0x040054EA RID: 21738
		private AutoBlink.Status eyeStatus;

		// Token: 0x02000CD7 RID: 3287
		private enum Status
		{
			// Token: 0x040054EC RID: 21740
			Close,
			// Token: 0x040054ED RID: 21741
			HalfClose,
			// Token: 0x040054EE RID: 21742
			Open
		}
	}
}
