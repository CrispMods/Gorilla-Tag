using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class TransformFollowXScene : MonoBehaviour
{
	// Token: 0x0600272F RID: 10031 RVA: 0x0004AC36 File Offset: 0x00048E36
	private void Awake()
	{
		this.prevPos = base.transform.position;
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x0004AC49 File Offset: 0x00048E49
	private void Start()
	{
		this.refToFollow.TryResolve<Transform>(out this.transformToFollow);
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x0010B284 File Offset: 0x00109484
	private void LateUpdate()
	{
		this.prevPos = base.transform.position;
		base.transform.rotation = this.transformToFollow.rotation;
		base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
	}

	// Token: 0x04002B52 RID: 11090
	public XSceneRef refToFollow;

	// Token: 0x04002B53 RID: 11091
	private Transform transformToFollow;

	// Token: 0x04002B54 RID: 11092
	public Vector3 offset;

	// Token: 0x04002B55 RID: 11093
	public Vector3 prevPos;
}
