using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class TransformFollowXScene : MonoBehaviour
{
	// Token: 0x06002804 RID: 10244 RVA: 0x000C3FCD File Offset: 0x000C21CD
	private void Awake()
	{
		this.prevPos = base.transform.position;
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000C3FE0 File Offset: 0x000C21E0
	private void Start()
	{
		this.refToFollow.TryResolve<Transform>(out this.transformToFollow);
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000C3FF4 File Offset: 0x000C21F4
	private void LateUpdate()
	{
		this.prevPos = base.transform.position;
		base.transform.rotation = this.transformToFollow.rotation;
		base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
	}

	// Token: 0x04002BEC RID: 11244
	public XSceneRef refToFollow;

	// Token: 0x04002BED RID: 11245
	private Transform transformToFollow;

	// Token: 0x04002BEE RID: 11246
	public Vector3 offset;

	// Token: 0x04002BEF RID: 11247
	public Vector3 prevPos;
}
