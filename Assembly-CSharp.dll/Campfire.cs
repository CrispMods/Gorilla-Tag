using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class Campfire : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001EE4 RID: 7908 RVA: 0x000EBD38 File Offset: 0x000E9F38
	private void Start()
	{
		this.lastAngleBottom = 0f;
		this.lastAngleMiddle = 0f;
		this.lastAngleTop = 0f;
		this.perlinBottom = (float)UnityEngine.Random.Range(0, 100);
		this.perlinMiddle = (float)UnityEngine.Random.Range(200, 300);
		this.perlinTop = (float)UnityEngine.Random.Range(400, 500);
		this.startingRotationBottom = this.baseFire.localEulerAngles.x;
		this.startingRotationMiddle = this.middleFire.localEulerAngles.x;
		this.startingRotationTop = this.topFire.localEulerAngles.x;
		this.tempVec = new Vector3(0f, 0f, 0f);
		this.mergedBottom = false;
		this.mergedMiddle = false;
		this.mergedTop = false;
		this.wasActive = false;
		this.lastTime = Time.time;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x00030F55 File Offset: 0x0002F155
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x000EBE24 File Offset: 0x000EA024
	public void SliceUpdate()
	{
		if (BetterDayNightManager.instance == null)
		{
			return;
		}
		if ((this.isActive[BetterDayNightManager.instance.currentTimeIndex] && BetterDayNightManager.instance.CurrentWeather() != BetterDayNightManager.WeatherType.Raining) || this.overrideDayNight == 1)
		{
			if (!this.wasActive)
			{
				this.wasActive = true;
				this.mergedBottom = false;
				this.mergedMiddle = false;
				this.mergedTop = false;
				Color.RGBToHSV(this.mat.color, out this.h, out this.s, out this.v);
				this.mat.color = Color.HSVToRGB(this.h, this.s, 1f);
			}
			this.Flap(ref this.perlinBottom, this.perlinStepBottom, ref this.lastAngleBottom, ref this.baseFire, this.bottomRange, this.baseMultiplier, ref this.mergedBottom);
			this.Flap(ref this.perlinMiddle, this.perlinStepMiddle, ref this.lastAngleMiddle, ref this.middleFire, this.middleRange, this.middleMultiplier, ref this.mergedMiddle);
			this.Flap(ref this.perlinTop, this.perlinStepTop, ref this.lastAngleTop, ref this.topFire, this.topRange, this.topMultiplier, ref this.mergedTop);
		}
		else
		{
			if (this.wasActive)
			{
				this.wasActive = false;
				this.mergedBottom = false;
				this.mergedMiddle = false;
				this.mergedTop = false;
				Color.RGBToHSV(this.mat.color, out this.h, out this.s, out this.v);
				this.mat.color = Color.HSVToRGB(this.h, this.s, 0.25f);
			}
			this.ReturnToOff(ref this.baseFire, this.startingRotationBottom, ref this.mergedBottom);
			this.ReturnToOff(ref this.middleFire, this.startingRotationMiddle, ref this.mergedMiddle);
			this.ReturnToOff(ref this.topFire, this.startingRotationTop, ref this.mergedTop);
		}
		this.lastTime = Time.time;
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000EC028 File Offset: 0x000EA228
	private void Flap(ref float perlinValue, float perlinStep, ref float lastAngle, ref Transform flameTransform, float range, float multiplier, ref bool isMerged)
	{
		perlinValue += perlinStep;
		lastAngle += (Time.time - this.lastTime) * Mathf.PerlinNoise(perlinValue, 0f);
		this.tempVec.x = range * Mathf.Sin(lastAngle * multiplier);
		if (Mathf.Abs(this.tempVec.x - flameTransform.localEulerAngles.x) > 180f)
		{
			if (this.tempVec.x > flameTransform.localEulerAngles.x)
			{
				this.tempVec.x = this.tempVec.x - 360f;
			}
			else
			{
				this.tempVec.x = this.tempVec.x + 360f;
			}
		}
		if (isMerged)
		{
			flameTransform.localEulerAngles = this.tempVec;
			return;
		}
		if (Mathf.Abs(flameTransform.localEulerAngles.x - this.tempVec.x) < 1f)
		{
			isMerged = true;
			flameTransform.localEulerAngles = this.tempVec;
			return;
		}
		this.tempVec.x = (this.tempVec.x - flameTransform.localEulerAngles.x) * this.slerp + flameTransform.localEulerAngles.x;
		flameTransform.localEulerAngles = this.tempVec;
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000EC170 File Offset: 0x000EA370
	private void ReturnToOff(ref Transform startTransform, float targetAngle, ref bool isMerged)
	{
		this.tempVec.x = targetAngle;
		if (Mathf.Abs(this.tempVec.x - startTransform.localEulerAngles.x) > 180f)
		{
			if (this.tempVec.x > startTransform.localEulerAngles.x)
			{
				this.tempVec.x = this.tempVec.x - 360f;
			}
			else
			{
				this.tempVec.x = this.tempVec.x + 360f;
			}
		}
		if (!isMerged)
		{
			if (Mathf.Abs(startTransform.localEulerAngles.x - targetAngle) < 1f)
			{
				isMerged = true;
				return;
			}
			this.tempVec.x = (this.tempVec.x - startTransform.localEulerAngles.x) * this.slerp + startTransform.localEulerAngles.x;
			startTransform.localEulerAngles = this.tempVec;
		}
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002295 RID: 8853
	public Transform baseFire;

	// Token: 0x04002296 RID: 8854
	public Transform middleFire;

	// Token: 0x04002297 RID: 8855
	public Transform topFire;

	// Token: 0x04002298 RID: 8856
	public float baseMultiplier;

	// Token: 0x04002299 RID: 8857
	public float middleMultiplier;

	// Token: 0x0400229A RID: 8858
	public float topMultiplier;

	// Token: 0x0400229B RID: 8859
	public float bottomRange;

	// Token: 0x0400229C RID: 8860
	public float middleRange;

	// Token: 0x0400229D RID: 8861
	public float topRange;

	// Token: 0x0400229E RID: 8862
	private float lastAngleBottom;

	// Token: 0x0400229F RID: 8863
	private float lastAngleMiddle;

	// Token: 0x040022A0 RID: 8864
	private float lastAngleTop;

	// Token: 0x040022A1 RID: 8865
	public float perlinStepBottom;

	// Token: 0x040022A2 RID: 8866
	public float perlinStepMiddle;

	// Token: 0x040022A3 RID: 8867
	public float perlinStepTop;

	// Token: 0x040022A4 RID: 8868
	private float perlinBottom;

	// Token: 0x040022A5 RID: 8869
	private float perlinMiddle;

	// Token: 0x040022A6 RID: 8870
	private float perlinTop;

	// Token: 0x040022A7 RID: 8871
	public float startingRotationBottom;

	// Token: 0x040022A8 RID: 8872
	public float startingRotationMiddle;

	// Token: 0x040022A9 RID: 8873
	public float startingRotationTop;

	// Token: 0x040022AA RID: 8874
	public float slerp = 0.01f;

	// Token: 0x040022AB RID: 8875
	private bool mergedBottom;

	// Token: 0x040022AC RID: 8876
	private bool mergedMiddle;

	// Token: 0x040022AD RID: 8877
	private bool mergedTop;

	// Token: 0x040022AE RID: 8878
	public string lastTimeOfDay;

	// Token: 0x040022AF RID: 8879
	public Material mat;

	// Token: 0x040022B0 RID: 8880
	private float h;

	// Token: 0x040022B1 RID: 8881
	private float s;

	// Token: 0x040022B2 RID: 8882
	private float v;

	// Token: 0x040022B3 RID: 8883
	public int overrideDayNight;

	// Token: 0x040022B4 RID: 8884
	private Vector3 tempVec;

	// Token: 0x040022B5 RID: 8885
	public bool[] isActive;

	// Token: 0x040022B6 RID: 8886
	public bool wasActive;

	// Token: 0x040022B7 RID: 8887
	private float lastTime;
}
