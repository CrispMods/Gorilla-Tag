using System;
using UnityEngine;

// Token: 0x02000468 RID: 1128
public class FacePlayer : MonoBehaviour
{
	// Token: 0x06001BBC RID: 7100 RVA: 0x000DB36C File Offset: 0x000D956C
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - GorillaTagger.Instance.headCollider.transform.position) * Quaternion.AngleAxis(-90f, Vector3.up);
	}
}
