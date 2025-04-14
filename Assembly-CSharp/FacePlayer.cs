using System;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class FacePlayer : MonoBehaviour
{
	// Token: 0x06001B68 RID: 7016 RVA: 0x00086E60 File Offset: 0x00085060
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - GorillaTagger.Instance.headCollider.transform.position) * Quaternion.AngleAxis(-90f, Vector3.up);
	}
}
