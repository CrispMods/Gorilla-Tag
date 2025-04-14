using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
public class SizeManager : MonoBehaviour
{
	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x0600257E RID: 9598 RVA: 0x000B93F0 File Offset: 0x000B75F0
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

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600257F RID: 9599 RVA: 0x000B942B File Offset: 0x000B762B
	// (set) Token: 0x06002580 RID: 9600 RVA: 0x000B9460 File Offset: 0x000B7660
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

	// Token: 0x06002581 RID: 9601 RVA: 0x000B94BA File Offset: 0x000B76BA
	private void OnDisable()
	{
		this.touchingChangers.Clear();
		this.currentSizeLayerMaskValue = 1;
		SizeManagerManager.UnregisterSM(this);
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000B94D4 File Offset: 0x000B76D4
	private void OnEnable()
	{
		SizeManagerManager.RegisterSM(this);
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000B94DC File Offset: 0x000B76DC
	private void CollectLineRenderers(GameObject obj)
	{
		this.lineRenderers = obj.GetComponentsInChildren<LineRenderer>(true);
		int num = this.lineRenderers.Length;
		foreach (LineRenderer lineRenderer in this.lineRenderers)
		{
			this.initLineScalar.Add(lineRenderer.widthMultiplier);
		}
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000B952C File Offset: 0x000B772C
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

	// Token: 0x06002585 RID: 9605 RVA: 0x000B9610 File Offset: 0x000B7810
	private void Awake()
	{
		if (!this.buildInitialized)
		{
			this.BuildInitialize();
		}
		SizeManagerManager.RegisterSM(this);
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x000B9628 File Offset: 0x000B7828
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

	// Token: 0x06002587 RID: 9607 RVA: 0x000B97C8 File Offset: 0x000B79C8
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

	// Token: 0x06002588 RID: 9608 RVA: 0x000B9854 File Offset: 0x000B7A54
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

	// Token: 0x06002589 RID: 9609 RVA: 0x000B9926 File Offset: 0x000B7B26
	private float SizeOverTime(float targetSize, float easing, float deltaTime)
	{
		if (easing <= 0f || Mathf.Abs(this.targetRig.ScaleMultiplier - targetSize) < 0.05f)
		{
			return targetSize;
		}
		return Mathf.MoveTowards(this.targetRig.ScaleMultiplier, targetSize, deltaTime / easing);
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000B9960 File Offset: 0x000B7B60
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

	// Token: 0x040029B5 RID: 10677
	public List<SizeChanger> touchingChangers;

	// Token: 0x040029B6 RID: 10678
	private LineRenderer[] lineRenderers;

	// Token: 0x040029B7 RID: 10679
	private List<float> initLineScalar = new List<float>();

	// Token: 0x040029B8 RID: 10680
	public VRRig targetRig;

	// Token: 0x040029B9 RID: 10681
	public GTPlayer targetPlayer;

	// Token: 0x040029BA RID: 10682
	public float magnitudeThreshold = 0.01f;

	// Token: 0x040029BB RID: 10683
	public float rate = 650f;

	// Token: 0x040029BC RID: 10684
	public Transform mainCameraTransform;

	// Token: 0x040029BD RID: 10685
	public SizeManager.SizeChangerType myType;

	// Token: 0x040029BE RID: 10686
	public float lastScale;

	// Token: 0x040029BF RID: 10687
	private bool buildInitialized;

	// Token: 0x040029C0 RID: 10688
	private const float returnToNormalEasing = 0.33f;

	// Token: 0x040029C1 RID: 10689
	private float smallThreshold = 0.6f;

	// Token: 0x040029C2 RID: 10690
	private float largeThreshold = 1.5f;

	// Token: 0x040029C3 RID: 10691
	private bool isSmall;

	// Token: 0x040029C4 RID: 10692
	private bool isLarge;

	// Token: 0x020005E6 RID: 1510
	public enum SizeChangerType
	{
		// Token: 0x040029C6 RID: 10694
		LocalOffline,
		// Token: 0x040029C7 RID: 10695
		LocalOnline,
		// Token: 0x040029C8 RID: 10696
		OtherOnline
	}
}
