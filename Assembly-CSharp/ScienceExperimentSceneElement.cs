using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class ScienceExperimentSceneElement : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06002767 RID: 10087 RVA: 0x000C139D File Offset: 0x000BF59D
	// (set) Token: 0x06002768 RID: 10088 RVA: 0x000C13A5 File Offset: 0x000BF5A5
	bool ITickSystemPost.PostTickRunning { get; set; }

	// Token: 0x06002769 RID: 10089 RVA: 0x000C13B0 File Offset: 0x000BF5B0
	void ITickSystemPost.PostTick()
	{
		base.transform.position = this.followElement.position;
		base.transform.rotation = this.followElement.rotation;
		base.transform.localScale = this.followElement.localScale;
	}

	// Token: 0x0600276A RID: 10090 RVA: 0x000C13FF File Offset: 0x000BF5FF
	private void Start()
	{
		this.followElement = ScienceExperimentManager.instance.GetElement(this.elementID);
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x0600276B RID: 10091 RVA: 0x000C141F File Offset: 0x000BF61F
	private void OnDestroy()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x04002B2D RID: 11053
	public ScienceExperimentElementID elementID;

	// Token: 0x04002B2E RID: 11054
	private Transform followElement;
}
