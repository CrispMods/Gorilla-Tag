using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class SlingshotProjectileTrail : MonoBehaviour
{
	// Token: 0x0600156F RID: 5487 RVA: 0x0003D7FC File Offset: 0x0003B9FC
	private void Awake()
	{
		this.initialWidthMultiplier = this.trailRenderer.widthMultiplier;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x000BE478 File Offset: 0x000BC678
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

	// Token: 0x06001571 RID: 5489 RVA: 0x000BE540 File Offset: 0x000BC740
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

	// Token: 0x06001572 RID: 5490 RVA: 0x000BE5FC File Offset: 0x000BC7FC
	public void SetColor(Color color)
	{
		TrailRenderer trailRenderer = this.trailRenderer;
		this.trailRenderer.endColor = color;
		trailRenderer.startColor = color;
	}

	// Token: 0x040017B1 RID: 6065
	public TrailRenderer trailRenderer;

	// Token: 0x040017B2 RID: 6066
	public Color defaultColor = Color.white;

	// Token: 0x040017B3 RID: 6067
	public Color orangeColor = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x040017B4 RID: 6068
	public Color blueColor = new Color(0f, 0.72f, 1f, 1f);

	// Token: 0x040017B5 RID: 6069
	private GameObject followObject;

	// Token: 0x040017B6 RID: 6070
	private Transform followXform;

	// Token: 0x040017B7 RID: 6071
	private float timeToDie = -1f;

	// Token: 0x040017B8 RID: 6072
	private float initialScale;

	// Token: 0x040017B9 RID: 6073
	private float initialWidthMultiplier;
}
