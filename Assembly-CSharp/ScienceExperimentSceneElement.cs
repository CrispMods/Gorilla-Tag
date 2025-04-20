using System;
using GorillaTag;
using UnityEngine;

// Token: 0x0200060F RID: 1551
public class ScienceExperimentSceneElement : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06002692 RID: 9874 RVA: 0x0004A53D File Offset: 0x0004873D
	// (set) Token: 0x06002693 RID: 9875 RVA: 0x0004A545 File Offset: 0x00048745
	bool ITickSystemPost.PostTickRunning { get; set; }

	// Token: 0x06002694 RID: 9876 RVA: 0x00108D0C File Offset: 0x00106F0C
	void ITickSystemPost.PostTick()
	{
		base.transform.position = this.followElement.position;
		base.transform.rotation = this.followElement.rotation;
		base.transform.localScale = this.followElement.localScale;
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x0004A54E File Offset: 0x0004874E
	private void Start()
	{
		this.followElement = ScienceExperimentManager.instance.GetElement(this.elementID);
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x0004A56E File Offset: 0x0004876E
	private void OnDestroy()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x04002A93 RID: 10899
	public ScienceExperimentElementID elementID;

	// Token: 0x04002A94 RID: 10900
	private Transform followElement;
}
