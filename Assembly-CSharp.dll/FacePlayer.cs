﻿using System;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class FacePlayer : MonoBehaviour
{
	// Token: 0x06001B6B RID: 7019 RVA: 0x000D86BC File Offset: 0x000D68BC
	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - GorillaTagger.Instance.headCollider.transform.position) * Quaternion.AngleAxis(-90f, Vector3.up);
	}
}
