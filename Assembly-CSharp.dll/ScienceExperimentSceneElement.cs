using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class ScienceExperimentSceneElement : MonoBehaviour, ITickSystemPost
{
	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x0600276F RID: 10095 RVA: 0x00049FA8 File Offset: 0x000481A8
	// (set) Token: 0x06002770 RID: 10096 RVA: 0x00049FB0 File Offset: 0x000481B0
	bool ITickSystemPost.PostTickRunning { get; set; }

	// Token: 0x06002771 RID: 10097 RVA: 0x0010A920 File Offset: 0x00108B20
	void ITickSystemPost.PostTick()
	{
		base.transform.position = this.followElement.position;
		base.transform.rotation = this.followElement.rotation;
		base.transform.localScale = this.followElement.localScale;
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x00049FB9 File Offset: 0x000481B9
	private void Start()
	{
		this.followElement = ScienceExperimentManager.instance.GetElement(this.elementID);
		TickSystem<object>.AddPostTickCallback(this);
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x00049FD9 File Offset: 0x000481D9
	private void OnDestroy()
	{
		TickSystem<object>.RemovePostTickCallback(this);
	}

	// Token: 0x04002B33 RID: 11059
	public ScienceExperimentElementID elementID;

	// Token: 0x04002B34 RID: 11060
	private Transform followElement;
}
