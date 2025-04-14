using System;
using System.Collections;
using UnityEngine;

namespace UnityChan
{
	// Token: 0x02000CA8 RID: 3240
	public class AutoBlink : MonoBehaviour
	{
		// Token: 0x060051C4 RID: 20932 RVA: 0x000023F4 File Offset: 0x000005F4
		private void Awake()
		{
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x00190F2E File Offset: 0x0018F12E
		private void Start()
		{
			this.ResetTimer();
			base.StartCoroutine("RandomChange");
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x00190F42 File Offset: 0x0018F142
		private void ResetTimer()
		{
			this.timeRemining = this.timeBlink;
			this.timerStarted = false;
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x00190F58 File Offset: 0x0018F158
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

		// Token: 0x060051C8 RID: 20936 RVA: 0x00190FCC File Offset: 0x0018F1CC
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

		// Token: 0x060051C9 RID: 20937 RVA: 0x0019101E File Offset: 0x0018F21E
		private void SetCloseEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_Close);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_Close);
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00191044 File Offset: 0x0018F244
		private void SetHalfCloseEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_HalfClose);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_HalfClose);
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x0019106A File Offset: 0x0018F26A
		private void SetOpenEyes()
		{
			this.ref_SMR_EYE_DEF.SetBlendShapeWeight(6, this.ratio_Open);
			this.ref_SMR_EL_DEF.SetBlendShapeWeight(6, this.ratio_Open);
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00191090 File Offset: 0x0018F290
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

		// Token: 0x040053E4 RID: 21476
		public bool isActive = true;

		// Token: 0x040053E5 RID: 21477
		public SkinnedMeshRenderer ref_SMR_EYE_DEF;

		// Token: 0x040053E6 RID: 21478
		public SkinnedMeshRenderer ref_SMR_EL_DEF;

		// Token: 0x040053E7 RID: 21479
		public float ratio_Close = 85f;

		// Token: 0x040053E8 RID: 21480
		public float ratio_HalfClose = 20f;

		// Token: 0x040053E9 RID: 21481
		[HideInInspector]
		public float ratio_Open;

		// Token: 0x040053EA RID: 21482
		private bool timerStarted;

		// Token: 0x040053EB RID: 21483
		private bool isBlink;

		// Token: 0x040053EC RID: 21484
		public float timeBlink = 0.4f;

		// Token: 0x040053ED RID: 21485
		private float timeRemining;

		// Token: 0x040053EE RID: 21486
		public float threshold = 0.3f;

		// Token: 0x040053EF RID: 21487
		public float interval = 3f;

		// Token: 0x040053F0 RID: 21488
		private AutoBlink.Status eyeStatus;

		// Token: 0x02000CA9 RID: 3241
		private enum Status
		{
			// Token: 0x040053F2 RID: 21490
			Close,
			// Token: 0x040053F3 RID: 21491
			HalfClose,
			// Token: 0x040053F4 RID: 21492
			Open
		}
	}
}
