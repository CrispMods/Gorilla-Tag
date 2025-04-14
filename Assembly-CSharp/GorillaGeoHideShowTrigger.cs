using System;
using GorillaExtensions;
using GorillaTag.GuidedRefs;
using UnityEngine;

// Token: 0x0200055C RID: 1372
public class GorillaGeoHideShowTrigger : GorillaTriggerBox, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
{
	// Token: 0x060021AD RID: 8621 RVA: 0x000A6C70 File Offset: 0x000A4E70
	protected void Awake()
	{
		((IGuidedRefObject)this).GuidedRefInitialize();
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x000A6C78 File Offset: 0x000A4E78
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

	// Token: 0x060021AF RID: 8623 RVA: 0x000A6D32 File Offset: 0x000A4F32
	void IGuidedRefObject.GuidedRefInitialize()
	{
		GuidedRefHub.RegisterReceiverArray<GorillaGeoHideShowTrigger, GameObject>(this, "makeSureThisIsDisabled", ref this.makeSureThisIsDisabled, ref this.makeSureThisIsDisabled_gRefs);
		GuidedRefHub.RegisterReceiverArray<GorillaGeoHideShowTrigger, GameObject>(this, "makeSureThisIsEnabled", ref this.makeSureThisIsEnabled, ref this.makeSureThisIsEnabled_gRefs);
		GuidedRefHub.ReceiverFullyRegistered<GorillaGeoHideShowTrigger>(this);
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x000A6D68 File Offset: 0x000A4F68
	bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
	{
		return GuidedRefHub.TryResolveArrayItem<GorillaGeoHideShowTrigger, GameObject>(this, this.makeSureThisIsDisabled, this.makeSureThisIsDisabled_gRefs, target) || GuidedRefHub.TryResolveArrayItem<GorillaGeoHideShowTrigger, GameObject>(this, this.makeSureThisIsDisabled, this.makeSureThisIsEnabled_gRefs, target);
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x000A6D99 File Offset: 0x000A4F99
	void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
	{
		this._guidedRefsAreFullyResolved = true;
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x000A6DA2 File Offset: 0x000A4FA2
	void IGuidedRefReceiverMono.OnGuidedRefTargetDestroyed(int fieldId)
	{
		this._guidedRefsAreFullyResolved = false;
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x060021B3 RID: 8627 RVA: 0x000A6DAB File Offset: 0x000A4FAB
	// (set) Token: 0x060021B4 RID: 8628 RVA: 0x000A6DB3 File Offset: 0x000A4FB3
	int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

	// Token: 0x060021B6 RID: 8630 RVA: 0x00042E29 File Offset: 0x00041029
	Transform IGuidedRefMonoBehaviour.get_transform()
	{
		return base.transform;
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x00015AA9 File Offset: 0x00013CA9
	int IGuidedRefObject.GetInstanceID()
	{
		return base.GetInstanceID();
	}

	// Token: 0x0400254C RID: 9548
	[SerializeField]
	private GameObject[] makeSureThisIsDisabled;

	// Token: 0x0400254D RID: 9549
	[SerializeField]
	private GuidedRefReceiverArrayInfo makeSureThisIsDisabled_gRefs = new GuidedRefReceiverArrayInfo(false);

	// Token: 0x0400254E RID: 9550
	[SerializeField]
	private GameObject[] makeSureThisIsEnabled;

	// Token: 0x0400254F RID: 9551
	[SerializeField]
	private GuidedRefReceiverArrayInfo makeSureThisIsEnabled_gRefs = new GuidedRefReceiverArrayInfo(false);

	// Token: 0x04002550 RID: 9552
	private bool _guidedRefsAreFullyResolved;
}
