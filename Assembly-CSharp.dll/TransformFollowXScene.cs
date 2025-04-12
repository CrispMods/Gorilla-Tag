using System;
using UnityEngine;

// Token: 0x0200064F RID: 1615
public class TransformFollowXScene : MonoBehaviour
{
	// Token: 0x0600280C RID: 10252 RVA: 0x0004A6A1 File Offset: 0x000488A1
	private void Awake()
	{
		this.prevPos = base.transform.position;
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x0004A6B4 File Offset: 0x000488B4
	private void Start()
	{
		this.refToFollow.TryResolve<Transform>(out this.transformToFollow);
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x0010CE5C File Offset: 0x0010B05C
	private void LateUpdate()
	{
		this.prevPos = base.transform.position;
		base.transform.rotation = this.transformToFollow.rotation;
		base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
	}

	// Token: 0x04002BF2 RID: 11250
	public XSceneRef refToFollow;

	// Token: 0x04002BF3 RID: 11251
	private Transform transformToFollow;

	// Token: 0x04002BF4 RID: 11252
	public Vector3 offset;

	// Token: 0x04002BF5 RID: 11253
	public Vector3 prevPos;
}
