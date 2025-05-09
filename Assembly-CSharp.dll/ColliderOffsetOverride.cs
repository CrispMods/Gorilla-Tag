﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class ColliderOffsetOverride : MonoBehaviour
{
	// Token: 0x0600008E RID: 142 RVA: 0x00068434 File Offset: 0x00066634
	private void Awake()
	{
		if (this.autoSearch)
		{
			this.FindColliders();
		}
		foreach (Collider collider in this.colliders)
		{
			if (collider != null)
			{
				collider.contactOffset = 0.01f * this.targetScale;
			}
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x000684AC File Offset: 0x000666AC
	public void FindColliders()
	{
		foreach (Collider item in base.gameObject.GetComponents<Collider>().ToList<Collider>())
		{
			if (!this.colliders.Contains(item))
			{
				this.colliders.Add(item);
			}
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x0006851C File Offset: 0x0006671C
	public void FindCollidersRecursively()
	{
		foreach (Collider item in base.gameObject.GetComponentsInChildren<Collider>().ToList<Collider>())
		{
			if (!this.colliders.Contains(item))
			{
				this.colliders.Add(item);
			}
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0002FB8F File Offset: 0x0002DD8F
	private void AutoDisabled()
	{
		this.autoSearch = true;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0002FB98 File Offset: 0x0002DD98
	private void AutoEnabled()
	{
		this.autoSearch = false;
	}

	// Token: 0x040000AF RID: 175
	public List<Collider> colliders;

	// Token: 0x040000B0 RID: 176
	[HideInInspector]
	public bool autoSearch;

	// Token: 0x040000B1 RID: 177
	public float targetScale = 1f;
}
