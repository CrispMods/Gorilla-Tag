using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class Campfire : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001F3A RID: 7994 RVA: 0x000EEA74 File Offset: 0x000ECC74
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

	// Token: 0x06001F3B RID: 7995 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x000EEB60 File Offset: 0x000ECD60
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

	// Token: 0x06001F3E RID: 7998 RVA: 0x000EED64 File Offset: 0x000ECF64
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

	// Token: 0x06001F3F RID: 7999 RVA: 0x000EEEAC File Offset: 0x000ED0AC
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

	// Token: 0x06001F41 RID: 8001 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040022E7 RID: 8935
	public Transform baseFire;

	// Token: 0x040022E8 RID: 8936
	public Transform middleFire;

	// Token: 0x040022E9 RID: 8937
	public Transform topFire;

	// Token: 0x040022EA RID: 8938
	public float baseMultiplier;

	// Token: 0x040022EB RID: 8939
	public float middleMultiplier;

	// Token: 0x040022EC RID: 8940
	public float topMultiplier;

	// Token: 0x040022ED RID: 8941
	public float bottomRange;

	// Token: 0x040022EE RID: 8942
	public float middleRange;

	// Token: 0x040022EF RID: 8943
	public float topRange;

	// Token: 0x040022F0 RID: 8944
	private float lastAngleBottom;

	// Token: 0x040022F1 RID: 8945
	private float lastAngleMiddle;

	// Token: 0x040022F2 RID: 8946
	private float lastAngleTop;

	// Token: 0x040022F3 RID: 8947
	public float perlinStepBottom;

	// Token: 0x040022F4 RID: 8948
	public float perlinStepMiddle;

	// Token: 0x040022F5 RID: 8949
	public float perlinStepTop;

	// Token: 0x040022F6 RID: 8950
	private float perlinBottom;

	// Token: 0x040022F7 RID: 8951
	private float perlinMiddle;

	// Token: 0x040022F8 RID: 8952
	private float perlinTop;

	// Token: 0x040022F9 RID: 8953
	public float startingRotationBottom;

	// Token: 0x040022FA RID: 8954
	public float startingRotationMiddle;

	// Token: 0x040022FB RID: 8955
	public float startingRotationTop;

	// Token: 0x040022FC RID: 8956
	public float slerp = 0.01f;

	// Token: 0x040022FD RID: 8957
	private bool mergedBottom;

	// Token: 0x040022FE RID: 8958
	private bool mergedMiddle;

	// Token: 0x040022FF RID: 8959
	private bool mergedTop;

	// Token: 0x04002300 RID: 8960
	public string lastTimeOfDay;

	// Token: 0x04002301 RID: 8961
	public Material mat;

	// Token: 0x04002302 RID: 8962
	private float h;

	// Token: 0x04002303 RID: 8963
	private float s;

	// Token: 0x04002304 RID: 8964
	private float v;

	// Token: 0x04002305 RID: 8965
	public int overrideDayNight;

	// Token: 0x04002306 RID: 8966
	private Vector3 tempVec;

	// Token: 0x04002307 RID: 8967
	public bool[] isActive;

	// Token: 0x04002308 RID: 8968
	public bool wasActive;

	// Token: 0x04002309 RID: 8969
	private float lastTime;
}
