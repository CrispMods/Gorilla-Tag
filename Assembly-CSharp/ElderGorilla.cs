using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class ElderGorilla : MonoBehaviour
{
	// Token: 0x06001FDA RID: 8154 RVA: 0x000F0588 File Offset: 0x000EE788
	private void Update()
	{
		if (GTPlayer.Instance == null)
		{
			return;
		}
		if (GTPlayer.Instance.inOverlay || !GTPlayer.Instance.isUserPresent)
		{
			return;
		}
		this.tHMD = GTPlayer.Instance.headCollider.transform;
		this.tLeftHand = GTPlayer.Instance.leftControllerTransform;
		this.tRightHand = GTPlayer.Instance.rightControllerTransform;
		if (Time.time - this.timeLastValidArmDist > 1f)
		{
			this.CheckHandDistance(this.tLeftHand);
			this.CheckHandDistance(this.tRightHand);
		}
		this.CheckHeight();
		this.CheckMicVolume();
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000F0628 File Offset: 0x000EE828
	private void CheckHandDistance(Transform hand)
	{
		float num = Vector3.Distance(hand.localPosition, this.tHMD.localPosition);
		if (num >= 1f)
		{
			return;
		}
		if (num >= 0.75f)
		{
			this.countValidArmDists++;
			this.timeLastValidArmDist = Time.time;
		}
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000F0678 File Offset: 0x000EE878
	private void CheckHeight()
	{
		float y = this.tHMD.localPosition.y;
		if (!this.trackingHeadHeight)
		{
			this.trackedHeadHeight = y - 0.05f;
			this.timerTrackedHeadHeight = 0f;
		}
		else if (this.trackedHeadHeight < y)
		{
			this.trackingHeadHeight = false;
		}
		if (this.trackingHeadHeight)
		{
			if (this.timerTrackedHeadHeight >= 1f)
			{
				this.savedHeadHeight = y;
				this.trackingHeadHeight = false;
				return;
			}
			this.timerTrackedHeadHeight += Time.deltaTime;
		}
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x00045A5F File Offset: 0x00043C5F
	private void CheckMicVolume()
	{
		float currentPeakAmp = GorillaTagger.Instance.myRecorder.LevelMeter.CurrentPeakAmp;
	}

	// Token: 0x040023A3 RID: 9123
	private const float MAX_HAND_DIST = 1f;

	// Token: 0x040023A4 RID: 9124
	private const float COOLDOWN_HAND_DIST = 1f;

	// Token: 0x040023A5 RID: 9125
	private const float VALID_HAND_DIST = 0.75f;

	// Token: 0x040023A6 RID: 9126
	private const float TIME_VALID_HEAD_HEIGHT = 1f;

	// Token: 0x040023A7 RID: 9127
	private Transform tHMD;

	// Token: 0x040023A8 RID: 9128
	private Transform tLeftHand;

	// Token: 0x040023A9 RID: 9129
	private Transform tRightHand;

	// Token: 0x040023AA RID: 9130
	private int countValidArmDists;

	// Token: 0x040023AB RID: 9131
	private float timeLastValidArmDist;

	// Token: 0x040023AC RID: 9132
	private bool trackingHeadHeight;

	// Token: 0x040023AD RID: 9133
	private float trackedHeadHeight;

	// Token: 0x040023AE RID: 9134
	private float timerTrackedHeadHeight;

	// Token: 0x040023AF RID: 9135
	private float savedHeadHeight = 1.5f;
}
