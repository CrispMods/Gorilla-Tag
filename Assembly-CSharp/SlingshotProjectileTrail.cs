using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200039B RID: 923
public class SlingshotProjectileTrail : MonoBehaviour
{
	// Token: 0x060015B8 RID: 5560 RVA: 0x0003EABC File Offset: 0x0003CCBC
	private void Awake()
	{
		this.initialWidthMultiplier = this.trailRenderer.widthMultiplier;
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x000C0CB0 File Offset: 0x000BEEB0
	public void AttachTrail(GameObject obj, bool blueTeam, bool redTeam)
	{
		this.followObject = obj;
		this.followXform = this.followObject.transform;
		Transform transform = base.transform;
		transform.position = this.followXform.position;
		this.initialScale = transform.localScale.x;
		transform.localScale = this.followXform.localScale;
		this.trailRenderer.widthMultiplier = this.initialWidthMultiplier * this.followXform.localScale.x;
		this.trailRenderer.Clear();
		if (blueTeam)
		{
			this.SetColor(this.blueColor);
		}
		else if (redTeam)
		{
			this.SetColor(this.orangeColor);
		}
		else
		{
			this.SetColor(this.defaultColor);
		}
		this.timeToDie = -1f;
	}

	// Token: 0x060015BA RID: 5562 RVA: 0x000C0D78 File Offset: 0x000BEF78
	protected void LateUpdate()
	{
		if (this.followObject.IsNull())
		{
			ObjectPools.instance.Destroy(base.gameObject);
			return;
		}
		base.gameObject.transform.position = this.followXform.position;
		if (!this.followObject.activeSelf && this.timeToDie < 0f)
		{
			this.timeToDie = Time.time + this.trailRenderer.time;
		}
		if (this.timeToDie > 0f && Time.time > this.timeToDie)
		{
			base.transform.localScale = Vector3.one * this.initialScale;
			ObjectPools.instance.Destroy(base.gameObject);
		}
	}

	// Token: 0x060015BB RID: 5563 RVA: 0x000C0E34 File Offset: 0x000BF034
	public void SetColor(Color color)
	{
		TrailRenderer trailRenderer = this.trailRenderer;
		this.trailRenderer.endColor = color;
		trailRenderer.startColor = color;
	}

	// Token: 0x040017F7 RID: 6135
	public TrailRenderer trailRenderer;

	// Token: 0x040017F8 RID: 6136
	public Color defaultColor = Color.white;

	// Token: 0x040017F9 RID: 6137
	public Color orangeColor = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x040017FA RID: 6138
	public Color blueColor = new Color(0f, 0.72f, 1f, 1f);

	// Token: 0x040017FB RID: 6139
	private GameObject followObject;

	// Token: 0x040017FC RID: 6140
	private Transform followXform;

	// Token: 0x040017FD RID: 6141
	private float timeToDie = -1f;

	// Token: 0x040017FE RID: 6142
	private float initialScale;

	// Token: 0x040017FF RID: 6143
	private float initialWidthMultiplier;
}
