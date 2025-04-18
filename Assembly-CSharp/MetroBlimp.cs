﻿using System;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class MetroBlimp : MonoBehaviour
{
	// Token: 0x06000430 RID: 1072 RVA: 0x0001940C File Offset: 0x0001760C
	private void Awake()
	{
		this._startLocalHeight = base.transform.localPosition.y;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00019424 File Offset: 0x00017624
	public void Tick()
	{
		bool flag = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f < 0.0001f;
		int num = Mathf.CeilToInt(this._numHandsOnBlimp / 2f);
		if (this._numHandsOnBlimp == 0f)
		{
			this._topStayTime = 0f;
			if (flag)
			{
				this.blimpRenderer.material.DisableKeyword("_INNER_GLOW");
			}
		}
		else
		{
			this._topStayTime += Time.deltaTime;
			if (flag)
			{
				this.blimpRenderer.material.EnableKeyword("_INNER_GLOW");
			}
		}
		Vector3 localPosition = base.transform.localPosition;
		Vector3 vector = localPosition;
		float y = vector.y;
		float num2 = this._startLocalHeight + this.descendOffset;
		float deltaTime = Time.deltaTime;
		if (num > 0)
		{
			if (y > num2)
			{
				vector += Vector3.down * (this.descendSpeed * (float)num * deltaTime);
			}
		}
		else if (y < this._startLocalHeight)
		{
			vector += Vector3.up * (this.ascendSpeed * deltaTime);
		}
		base.transform.localPosition = Vector3.Slerp(localPosition, vector, 0.5f);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00019553 File Offset: 0x00017753
	private static bool IsPlayerHand(Collider c)
	{
		return c.gameObject.IsOnLayer(UnityLayer.GorillaHand);
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00019562 File Offset: 0x00017762
	private void OnTriggerEnter(Collider other)
	{
		if (MetroBlimp.IsPlayerHand(other))
		{
			this._numHandsOnBlimp += 1f;
		}
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0001957E File Offset: 0x0001777E
	private void OnTriggerExit(Collider other)
	{
		if (MetroBlimp.IsPlayerHand(other))
		{
			this._numHandsOnBlimp -= 1f;
		}
	}

	// Token: 0x040004D1 RID: 1233
	public MetroSpotlight spotLightLeft;

	// Token: 0x040004D2 RID: 1234
	public MetroSpotlight spotLightRight;

	// Token: 0x040004D3 RID: 1235
	[Space]
	public BoxCollider topCollider;

	// Token: 0x040004D4 RID: 1236
	public Material blimpMaterial;

	// Token: 0x040004D5 RID: 1237
	public Renderer blimpRenderer;

	// Token: 0x040004D6 RID: 1238
	[Space]
	public float ascendSpeed = 1f;

	// Token: 0x040004D7 RID: 1239
	public float descendSpeed = 0.5f;

	// Token: 0x040004D8 RID: 1240
	public float descendOffset = -24.1f;

	// Token: 0x040004D9 RID: 1241
	public float descendReactionTime = 3f;

	// Token: 0x040004DA RID: 1242
	[Space]
	[NonSerialized]
	private float _startLocalHeight;

	// Token: 0x040004DB RID: 1243
	[NonSerialized]
	private float _topStayTime;

	// Token: 0x040004DC RID: 1244
	[NonSerialized]
	private float _numHandsOnBlimp;

	// Token: 0x040004DD RID: 1245
	[NonSerialized]
	private bool _lowering;

	// Token: 0x040004DE RID: 1246
	private const string _INNER_GLOW = "_INNER_GLOW";
}
