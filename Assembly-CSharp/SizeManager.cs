using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005F3 RID: 1523
public class SizeManager : MonoBehaviour
{
	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x060025E0 RID: 9696 RVA: 0x00049AE4 File Offset: 0x00047CE4
	public float currentScale
	{
		get
		{
			if (this.targetRig != null)
			{
				return this.targetRig.ScaleMultiplier;
			}
			if (this.targetPlayer != null)
			{
				return this.targetPlayer.ScaleMultiplier;
			}
			return 1f;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x060025E1 RID: 9697 RVA: 0x00049B1F File Offset: 0x00047D1F
	// (set) Token: 0x060025E2 RID: 9698 RVA: 0x0010716C File Offset: 0x0010536C
	public int currentSizeLayerMaskValue
	{
		get
		{
			if (this.targetPlayer)
			{
				return this.targetPlayer.sizeLayerMask;
			}
			if (this.targetRig)
			{
				return this.targetRig.SizeLayerMask;
			}
			return 1;
		}
		set
		{
			if (this.targetPlayer)
			{
				this.targetPlayer.sizeLayerMask = value;
				if (this.targetRig != null)
				{
					this.targetRig.SizeLayerMask = value;
					return;
				}
			}
			else if (this.targetRig)
			{
				this.targetRig.SizeLayerMask = value;
			}
		}
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x00049B54 File Offset: 0x00047D54
	private void OnDisable()
	{
		this.touchingChangers.Clear();
		this.currentSizeLayerMaskValue = 1;
		SizeManagerManager.UnregisterSM(this);
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x00049B6E File Offset: 0x00047D6E
	private void OnEnable()
	{
		SizeManagerManager.RegisterSM(this);
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x001071C8 File Offset: 0x001053C8
	private void CollectLineRenderers(GameObject obj)
	{
		this.lineRenderers = obj.GetComponentsInChildren<LineRenderer>(true);
		int num = this.lineRenderers.Length;
		foreach (LineRenderer lineRenderer in this.lineRenderers)
		{
			this.initLineScalar.Add(lineRenderer.widthMultiplier);
		}
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x00107218 File Offset: 0x00105418
	public void BuildInitialize()
	{
		this.rate = 650f;
		if (this.targetRig != null)
		{
			this.CollectLineRenderers(this.targetRig.gameObject);
		}
		else if (this.targetPlayer != null)
		{
			this.CollectLineRenderers(GorillaTagger.Instance.offlineVRRig.gameObject);
		}
		this.mainCameraTransform = Camera.main.transform;
		if (this.targetPlayer != null)
		{
			this.myType = SizeManager.SizeChangerType.LocalOffline;
		}
		else if (this.targetRig != null && !this.targetRig.isOfflineVRRig && this.targetRig.netView != null && this.targetRig.netView.Owner != NetworkSystem.Instance.LocalPlayer)
		{
			this.myType = SizeManager.SizeChangerType.OtherOnline;
		}
		else
		{
			this.myType = SizeManager.SizeChangerType.LocalOnline;
		}
		this.buildInitialized = true;
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x00049B76 File Offset: 0x00047D76
	private void Awake()
	{
		if (!this.buildInitialized)
		{
			this.BuildInitialize();
		}
		SizeManagerManager.RegisterSM(this);
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x001072FC File Offset: 0x001054FC
	public void InvokeFixedUpdate()
	{
		float num = 1f;
		SizeChanger sizeChanger = this.ControllingChanger(this.targetRig.transform);
		switch (this.myType)
		{
		case SizeManager.SizeChangerType.LocalOffline:
			num = this.ScaleFromChanger(sizeChanger, this.mainCameraTransform, Time.fixedDeltaTime);
			this.targetPlayer.SetScaleMultiplier((num == 1f) ? this.SizeOverTime(num, 0.33f, Time.fixedDeltaTime) : num);
			break;
		case SizeManager.SizeChangerType.LocalOnline:
			num = this.ScaleFromChanger(sizeChanger, this.targetRig.transform, Time.fixedDeltaTime);
			this.targetRig.ScaleMultiplier = ((num == 1f) ? this.SizeOverTime(num, 0.33f, Time.fixedDeltaTime) : num);
			break;
		case SizeManager.SizeChangerType.OtherOnline:
			num = this.ScaleFromChanger(sizeChanger, this.targetRig.transform, Time.fixedDeltaTime);
			this.targetRig.ScaleMultiplier = ((num == 1f) ? this.SizeOverTime(num, 0.33f, Time.fixedDeltaTime) : num);
			break;
		}
		if (num != this.lastScale)
		{
			for (int i = 0; i < this.lineRenderers.Length; i++)
			{
				this.lineRenderers[i].widthMultiplier = num * this.initLineScalar[i];
			}
			Vector3 scaleCenter;
			if (sizeChanger != null && sizeChanger.TryGetScaleCenterPoint(out scaleCenter))
			{
				if (this.myType == SizeManager.SizeChangerType.LocalOffline)
				{
					this.targetPlayer.ScaleAwayFromPoint(this.lastScale, num, scaleCenter);
				}
				else if (this.myType == SizeManager.SizeChangerType.LocalOnline)
				{
					GTPlayer.Instance.ScaleAwayFromPoint(this.lastScale, num, scaleCenter);
				}
			}
			if (this.myType == SizeManager.SizeChangerType.LocalOffline)
			{
				this.CheckSizeChangeEvents(num);
			}
		}
		this.lastScale = num;
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x0010749C File Offset: 0x0010569C
	private SizeChanger ControllingChanger(Transform t)
	{
		for (int i = this.touchingChangers.Count - 1; i >= 0; i--)
		{
			SizeChanger sizeChanger = this.touchingChangers[i];
			if (!(sizeChanger == null) && sizeChanger.gameObject.activeInHierarchy && (sizeChanger.SizeLayerMask & this.currentSizeLayerMaskValue) != 0 && (sizeChanger.alwaysControlWhenEntered || (sizeChanger.ClosestPoint(t.position) - t.position).magnitude < this.magnitudeThreshold))
			{
				return sizeChanger;
			}
		}
		return null;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x00107528 File Offset: 0x00105728
	private float ScaleFromChanger(SizeChanger sC, Transform t, float deltaTime)
	{
		if (sC == null)
		{
			return 1f;
		}
		SizeChanger.ChangerType changerType = sC.MyType;
		if (changerType == SizeChanger.ChangerType.Static)
		{
			return this.SizeOverTime(sC.MinScale, sC.StaticEasing, deltaTime);
		}
		if (changerType == SizeChanger.ChangerType.Continuous)
		{
			Vector3 vector = Vector3.Project(t.position - sC.StartPos.position, sC.EndPos.position - sC.StartPos.position);
			return Mathf.Clamp(sC.MaxScale - vector.magnitude / (sC.StartPos.position - sC.EndPos.position).magnitude * (sC.MaxScale - sC.MinScale), sC.MinScale, sC.MaxScale);
		}
		return 1f;
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x00049B8C File Offset: 0x00047D8C
	private float SizeOverTime(float targetSize, float easing, float deltaTime)
	{
		if (easing <= 0f || Mathf.Abs(this.targetRig.ScaleMultiplier - targetSize) < 0.05f)
		{
			return targetSize;
		}
		return Mathf.MoveTowards(this.targetRig.ScaleMultiplier, targetSize, deltaTime / easing);
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x001075FC File Offset: 0x001057FC
	private void CheckSizeChangeEvents(float newSize)
	{
		if (newSize < this.smallThreshold)
		{
			if (!this.isSmall)
			{
				this.isSmall = true;
				this.isLarge = false;
				PlayerGameEvents.MiscEvent("SizeSmall");
				return;
			}
		}
		else if (newSize > this.largeThreshold)
		{
			if (!this.isLarge)
			{
				this.isLarge = true;
				this.isSmall = false;
				PlayerGameEvents.MiscEvent("SizeLarge");
				return;
			}
		}
		else
		{
			this.isLarge = false;
			this.isSmall = false;
		}
	}

	// Token: 0x04002A14 RID: 10772
	public List<SizeChanger> touchingChangers;

	// Token: 0x04002A15 RID: 10773
	private LineRenderer[] lineRenderers;

	// Token: 0x04002A16 RID: 10774
	private List<float> initLineScalar = new List<float>();

	// Token: 0x04002A17 RID: 10775
	public VRRig targetRig;

	// Token: 0x04002A18 RID: 10776
	public GTPlayer targetPlayer;

	// Token: 0x04002A19 RID: 10777
	public float magnitudeThreshold = 0.01f;

	// Token: 0x04002A1A RID: 10778
	public float rate = 650f;

	// Token: 0x04002A1B RID: 10779
	public Transform mainCameraTransform;

	// Token: 0x04002A1C RID: 10780
	public SizeManager.SizeChangerType myType;

	// Token: 0x04002A1D RID: 10781
	public float lastScale;

	// Token: 0x04002A1E RID: 10782
	private bool buildInitialized;

	// Token: 0x04002A1F RID: 10783
	private const float returnToNormalEasing = 0.33f;

	// Token: 0x04002A20 RID: 10784
	private float smallThreshold = 0.6f;

	// Token: 0x04002A21 RID: 10785
	private float largeThreshold = 1.5f;

	// Token: 0x04002A22 RID: 10786
	private bool isSmall;

	// Token: 0x04002A23 RID: 10787
	private bool isLarge;

	// Token: 0x020005F4 RID: 1524
	public enum SizeChangerType
	{
		// Token: 0x04002A25 RID: 10789
		LocalOffline,
		// Token: 0x04002A26 RID: 10790
		LocalOnline,
		// Token: 0x04002A27 RID: 10791
		OtherOnline
	}
}
