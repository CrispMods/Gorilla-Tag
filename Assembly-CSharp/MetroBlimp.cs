using System;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class MetroBlimp : MonoBehaviour
{
	// Token: 0x0600046C RID: 1132 RVA: 0x00033538 File Offset: 0x00031738
	private void Awake()
	{
		this._startLocalHeight = base.transform.localPosition.y;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0007CF6C File Offset: 0x0007B16C
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

	// Token: 0x0600046E RID: 1134 RVA: 0x00033550 File Offset: 0x00031750
	private static bool IsPlayerHand(Collider c)
	{
		return c.gameObject.IsOnLayer(UnityLayer.GorillaHand);
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0003355F File Offset: 0x0003175F
	private void OnTriggerEnter(Collider other)
	{
		if (MetroBlimp.IsPlayerHand(other))
		{
			this._numHandsOnBlimp += 1f;
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0003357B File Offset: 0x0003177B
	private void OnTriggerExit(Collider other)
	{
		if (MetroBlimp.IsPlayerHand(other))
		{
			this._numHandsOnBlimp -= 1f;
		}
	}

	// Token: 0x04000511 RID: 1297
	public MetroSpotlight spotLightLeft;

	// Token: 0x04000512 RID: 1298
	public MetroSpotlight spotLightRight;

	// Token: 0x04000513 RID: 1299
	[Space]
	public BoxCollider topCollider;

	// Token: 0x04000514 RID: 1300
	public Material blimpMaterial;

	// Token: 0x04000515 RID: 1301
	public Renderer blimpRenderer;

	// Token: 0x04000516 RID: 1302
	[Space]
	public float ascendSpeed = 1f;

	// Token: 0x04000517 RID: 1303
	public float descendSpeed = 0.5f;

	// Token: 0x04000518 RID: 1304
	public float descendOffset = -24.1f;

	// Token: 0x04000519 RID: 1305
	public float descendReactionTime = 3f;

	// Token: 0x0400051A RID: 1306
	[Space]
	[NonSerialized]
	private float _startLocalHeight;

	// Token: 0x0400051B RID: 1307
	[NonSerialized]
	private float _topStayTime;

	// Token: 0x0400051C RID: 1308
	[NonSerialized]
	private float _numHandsOnBlimp;

	// Token: 0x0400051D RID: 1309
	[NonSerialized]
	private bool _lowering;

	// Token: 0x0400051E RID: 1310
	private const string _INNER_GLOW = "_INNER_GLOW";
}
