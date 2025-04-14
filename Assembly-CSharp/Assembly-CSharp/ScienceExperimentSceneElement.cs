using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class ScienceExperimentSceneElement : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x0600276F RID: 10095 RVA: 0x000C181D File Offset: 0x000BFA1D
	// (set) Token: 0x06002770 RID: 10096 RVA: 0x000C1825 File Offset: 0x000BFA25
	bool ITickSystemPost.PostTickRunning { get; set; }

	// Token: 0x06002771 RID: 10097 RVA: 0x000C1830 File Offset: 0x000BFA30
	void ITickSystemPost.PostTick()
	{
		base.transform.position = this.followElement.position;
		base.transform.rotation = this.followElement.rotation;
		base.transform.localScale = this.followElement.localScale;
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x000C187F File Offset: 0x000BFA7F
	private void Start()
	{
		this.followElement = ScienceExperimentManager.instance.GetElement(this.elementID);
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x000C189F File Offset: 0x000BFA9F
	private void OnDestroy()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x04002B33 RID: 11059
	public ScienceExperimentElementID elementID;

	// Token: 0x04002B34 RID: 11060
	private Transform followElement;
}
