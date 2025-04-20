using System;
using GorillaExtensions;
using GorillaTag.GuidedRefs;
using UnityEngine;

// Token: 0x0200056A RID: 1386
public class GorillaGeoHideShowTrigger : GorillaTriggerBox, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
{
	// Token: 0x0600220B RID: 8715 RVA: 0x0004722A File Offset: 0x0004542A
	protected void Awake()
	{
		((IGuidedRefObject)this).GuidedRefInitialize();
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000F70BC File Offset: 0x000F52BC
	public override void OnBoxTriggered()
	{
		if (!this._guidedRefsAreFullyResolved)
		{
			return;
		}
		if (this.makeSureThisIsDisabled != null)
		{
			foreach (GameObject gameObject in this.makeSureThisIsDisabled)
			{
				if (gameObject == null)
				{
					Debug.LogError("GorillaGeoHideShowTrigger: null item in makeSureThisIsDisabled. \"" + base.transform.GetPath() + "\"", this);
					return;
				}
				gameObject.SetActive(false);
			}
		}
		if (this.makeSureThisIsEnabled != null)
		{
			foreach (GameObject gameObject2 in this.makeSureThisIsEnabled)
			{
				if (gameObject2 == null)
				{
					Debug.LogError("GorillaGeoHideShowTrigger: null item in makeSureThisIsDisabled. \"" + base.transform.GetPath() + "\"", this);
					return;
				}
				gameObject2.SetActive(true);
			}
		}
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x00047232 File Offset: 0x00045432
	void IGuidedRefObject.GuidedRefInitialize()
	{
		GuidedRefHub.RegisterReceiverArray<GorillaGeoHideShowTrigger, GameObject>(this, "makeSureThisIsDisabled", ref this.makeSureThisIsDisabled, ref this.makeSureThisIsDisabled_gRefs);
		GuidedRefHub.RegisterReceiverArray<GorillaGeoHideShowTrigger, GameObject>(this, "makeSureThisIsEnabled", ref this.makeSureThisIsEnabled, ref this.makeSureThisIsEnabled_gRefs);
		GuidedRefHub.ReceiverFullyRegistered<GorillaGeoHideShowTrigger>(this);
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x00047268 File Offset: 0x00045468
	bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
	{
		return GuidedRefHub.TryResolveArrayItem<GorillaGeoHideShowTrigger, GameObject>(this, this.makeSureThisIsDisabled, this.makeSureThisIsDisabled_gRefs, target) || GuidedRefHub.TryResolveArrayItem<GorillaGeoHideShowTrigger, GameObject>(this, this.makeSureThisIsDisabled, this.makeSureThisIsEnabled_gRefs, target);
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x00047299 File Offset: 0x00045499
	void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
	{
		this._guidedRefsAreFullyResolved = true;
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000472A2 File Offset: 0x000454A2
	void IGuidedRefReceiverMono.OnGuidedRefTargetDestroyed(int fieldId)
	{
		this._guidedRefsAreFullyResolved = false;
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06002211 RID: 8721 RVA: 0x000472AB File Offset: 0x000454AB
	// (set) Token: 0x06002212 RID: 8722 RVA: 0x000472B3 File Offset: 0x000454B3
	int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

	// Token: 0x06002214 RID: 8724 RVA: 0x00039243 File Offset: 0x00037443
	Transform IGuidedRefMonoBehaviour.get_transform()
	{
		return base.transform;
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x00032CAE File Offset: 0x00030EAE
	int IGuidedRefObject.GetInstanceID()
	{
		return base.GetInstanceID();
	}

	// Token: 0x040025A4 RID: 9636
	[SerializeField]
	private GameObject[] makeSureThisIsDisabled;

	// Token: 0x040025A5 RID: 9637
	[SerializeField]
	private GuidedRefReceiverArrayInfo makeSureThisIsDisabled_gRefs = new GuidedRefReceiverArrayInfo(false);

	// Token: 0x040025A6 RID: 9638
	[SerializeField]
	private GameObject[] makeSureThisIsEnabled;

	// Token: 0x040025A7 RID: 9639
	[SerializeField]
	private GuidedRefReceiverArrayInfo makeSureThisIsEnabled_gRefs = new GuidedRefReceiverArrayInfo(false);

	// Token: 0x040025A8 RID: 9640
	private bool _guidedRefsAreFullyResolved;
}
