using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005E6 RID: 1510
public class SizeManager : MonoBehaviour
{
	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06002586 RID: 9606 RVA: 0x000B9870 File Offset: 0x000B7A70
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

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06002587 RID: 9607 RVA: 0x000B98AB File Offset: 0x000B7AAB
	// (set) Token: 0x06002588 RID: 9608 RVA: 0x000B98E0 File Offset: 0x000B7AE0
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

	// Token: 0x06002589 RID: 9609 RVA: 0x000B993A File Offset: 0x000B7B3A
	private void OnDisable()
	{
		this.touchingChangers.Clear();
		this.currentSizeLayerMaskValue = 1;
		SizeManagerManager.UnregisterSM(this);
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000B9954 File Offset: 0x000B7B54
	private void OnEnable()
	{
		SizeManagerManager.RegisterSM(this);
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x000B995C File Offset: 0x000B7B5C
	private void CollectLineRenderers(GameObject obj)
	{
		this.lineRenderers = obj.GetComponentsInChildren<LineRenderer>(true);
		int num = this.lineRenderers.Length;
		foreach (LineRenderer lineRenderer in this.lineRenderers)
		{
			this.initLineScalar.Add(lineRenderer.widthMultiplier);
		}
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x000B99AC File Offset: 0x000B7BAC
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

	// Token: 0x0600258D RID: 9613 RVA: 0x000B9A90 File Offset: 0x000B7C90
	private void Awake()
	{
		if (!this.buildInitialized)
		{
			this.BuildInitialize();
		}
		SizeManagerManager.RegisterSM(this);
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x000B9AA8 File Offset: 0x000B7CA8
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

	// Token: 0x0600258F RID: 9615 RVA: 0x000B9C48 File Offset: 0x000B7E48
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

	// Token: 0x06002590 RID: 9616 RVA: 0x000B9CD4 File Offset: 0x000B7ED4
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

	// Token: 0x06002591 RID: 9617 RVA: 0x000B9DA6 File Offset: 0x000B7FA6
	private float SizeOverTime(float targetSize, float easing, float deltaTime)
	{
		if (easing <= 0f || Mathf.Abs(this.targetRig.ScaleMultiplier - targetSize) < 0.05f)
		{
			return targetSize;
		}
		return Mathf.MoveTowards(this.targetRig.ScaleMultiplier, targetSize, deltaTime / easing);
	}

	// Token: 0x06002592 RID: 9618 RVA: 0x000B9DE0 File Offset: 0x000B7FE0
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

	// Token: 0x040029BB RID: 10683
	public List<SizeChanger> touchingChangers;

	// Token: 0x040029BC RID: 10684
	private LineRenderer[] lineRenderers;

	// Token: 0x040029BD RID: 10685
	private List<float> initLineScalar = new List<float>();

	// Token: 0x040029BE RID: 10686
	public VRRig targetRig;

	// Token: 0x040029BF RID: 10687
	public GTPlayer targetPlayer;

	// Token: 0x040029C0 RID: 10688
	public float magnitudeThreshold = 0.01f;

	// Token: 0x040029C1 RID: 10689
	public float rate = 650f;

	// Token: 0x040029C2 RID: 10690
	public Transform mainCameraTransform;

	// Token: 0x040029C3 RID: 10691
	public SizeManager.SizeChangerType myType;

	// Token: 0x040029C4 RID: 10692
	public float lastScale;

	// Token: 0x040029C5 RID: 10693
	private bool buildInitialized;

	// Token: 0x040029C6 RID: 10694
	private const float returnToNormalEasing = 0.33f;

	// Token: 0x040029C7 RID: 10695
	private float smallThreshold = 0.6f;

	// Token: 0x040029C8 RID: 10696
	private float largeThreshold = 1.5f;

	// Token: 0x040029C9 RID: 10697
	private bool isSmall;

	// Token: 0x040029CA RID: 10698
	private bool isLarge;

	// Token: 0x020005E7 RID: 1511
	public enum SizeChangerType
	{
		// Token: 0x040029CC RID: 10700
		LocalOffline,
		// Token: 0x040029CD RID: 10701
		LocalOnline,
		// Token: 0x040029CE RID: 10702
		OtherOnline
	}
}
