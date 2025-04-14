using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class ElderGorilla : MonoBehaviour
{
	// Token: 0x06001F81 RID: 8065 RVA: 0x0009EA4C File Offset: 0x0009CC4C
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

	// Token: 0x06001F82 RID: 8066 RVA: 0x0009EAEC File Offset: 0x0009CCEC
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

	// Token: 0x06001F83 RID: 8067 RVA: 0x0009EB3C File Offset: 0x0009CD3C
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

	// Token: 0x06001F84 RID: 8068 RVA: 0x0009EBC2 File Offset: 0x0009CDC2
	private void CheckMicVolume()
	{
		float currentPeakAmp = GorillaTagger.Instance.myRecorder.LevelMeter.CurrentPeakAmp;
	}

	// Token: 0x04002350 RID: 9040
	private const float MAX_HAND_DIST = 1f;

	// Token: 0x04002351 RID: 9041
	private const float COOLDOWN_HAND_DIST = 1f;

	// Token: 0x04002352 RID: 9042
	private const float VALID_HAND_DIST = 0.75f;

	// Token: 0x04002353 RID: 9043
	private const float TIME_VALID_HEAD_HEIGHT = 1f;

	// Token: 0x04002354 RID: 9044
	private Transform tHMD;

	// Token: 0x04002355 RID: 9045
	private Transform tLeftHand;

	// Token: 0x04002356 RID: 9046
	private Transform tRightHand;

	// Token: 0x04002357 RID: 9047
	private int countValidArmDists;

	// Token: 0x04002358 RID: 9048
	private float timeLastValidArmDist;

	// Token: 0x04002359 RID: 9049
	private bool trackingHeadHeight;

	// Token: 0x0400235A RID: 9050
	private float trackedHeadHeight;

	// Token: 0x0400235B RID: 9051
	private float timerTrackedHeadHeight;

	// Token: 0x0400235C RID: 9052
	private float savedHeadHeight = 1.5f;
}
