using System;
using System.Collections;
using UnityEngine;

namespace UnityChan
{
	// Token: 0x02000CA5 RID: 3237
	public class AutoBlink : MonoBehaviour
	{
		// Token: 0x060051B8 RID: 20920 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Awake()
		{
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00190966 File Offset: 0x0018EB66
		private void Start()
		{
			this.ResetTimer();
			base.StartCoroutine("RandomChange");
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x0019097A File Offset: 0x0018EB7A
		private void ResetTimer()
		{
			this.timeRemining = this.timeBlink;
			this.timerStarted = false;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x00190990 File Offset: 0x0018EB90
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

		// Token: 0x060051BC RID: 20924 RVA: 0x00190A04 File Offset: 0x0018EC04
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

		// Token: 0x060051BD RID: 20925 RVA: 0x00190A56 File Offset: 0x0018EC56
		private void SetCloseEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_Close);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_Close);
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x00190A7C File Offset: 0x0018EC7C
		private void SetHalfCloseEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_HalfClose);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_HalfClose);
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x00190AA2 File Offset: 0x0018ECA2
		private void SetOpenEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_Open);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_Open);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00190AC8 File Offset: 0x0018ECC8
		private IEnumerator RandomChange()
		{
			for (;;)
			{
				float num = Random.Range(0f, 1f);
				if (!this.isBlink && num > this.threshold)
				{
					this.isBlink = true;
				}
				yield return new WaitForSeconds(this.interval);
			}
			yield break;
		}

		// Token: 0x040053D2 RID: 21458
		public bool isActive = true;

		// Token: 0x040053D3 RID: 21459
		public SkinnedMeshRenderer ref_SMR_EYE_DEF;

		// Token: 0x040053D4 RID: 21460
		public SkinnedMeshRenderer ref_SMR_EL_DEF;

		// Token: 0x040053D5 RID: 21461
		public float ratio_Close = 85f;

		// Token: 0x040053D6 RID: 21462
		public float ratio_HalfClose = 20f;

		// Token: 0x040053D7 RID: 21463
		[HideInInspector]
		public float ratio_Open;

		// Token: 0x040053D8 RID: 21464
		private bool timerStarted;

		// Token: 0x040053D9 RID: 21465
		private bool isBlink;

		// Token: 0x040053DA RID: 21466
		public float timeBlink = 0.4f;

		// Token: 0x040053DB RID: 21467
		private float timeRemining;

		// Token: 0x040053DC RID: 21468
		public float threshold = 0.3f;

		// Token: 0x040053DD RID: 21469
		public float interval = 3f;

		// Token: 0x040053DE RID: 21470
		private AutoBlink.Status eyeStatus;

		// Token: 0x02000CA6 RID: 3238
		private enum Status
		{
			// Token: 0x040053E0 RID: 21472
			Close,
			// Token: 0x040053E1 RID: 21473
			HalfClose,
			// Token: 0x040053E2 RID: 21474
			Open
		}
	}
}
