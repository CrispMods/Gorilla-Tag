using System;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000382 RID: 898
public class PaintbrawlBalloons : MonoBehaviour
{
	// Token: 0x06001507 RID: 5383 RVA: 0x00066CC0 File Offset: 0x00064EC0
	protected void Awake()
	{
		this.matPropBlock = new MaterialPropertyBlock();
		this.renderers = new Renderer[this.balloons.Length];
		this.balloonsCachedActiveState = new bool[this.balloons.Length];
		for (int i = 0; i < this.balloons.Length; i++)
		{
			this.renderers[i] = this.balloons[i].GetComponentInChildren<Renderer>();
			this.balloonsCachedActiveState[i] = this.balloons[i].activeSelf;
		}
		this.colorShaderPropID = Shader.PropertyToID("_Color");
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x00066D4B File Offset: 0x00064F4B
	protected void OnEnable()
	{
		this.UpdateBalloonColors();
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x00066D54 File Offset: 0x00064F54
	protected void LateUpdate()
	{
		if (GorillaGameManager.instance != null && (this.bMgr != null || GorillaGameManager.instance.gameObject.GetComponent<GorillaPaintbrawlManager>() != null))
		{
			if (this.bMgr == null)
			{
				this.bMgr = GorillaGameManager.instance.gameObject.GetComponent<GorillaPaintbrawlManager>();
			}
			int playerLives = this.bMgr.GetPlayerLives(this.myRig.creator);
			for (int i = 0; i < this.balloons.Length; i++)
			{
				bool flag = playerLives >= i + 1;
				if (flag != this.balloonsCachedActiveState[i])
				{
					this.balloonsCachedActiveState[i] = flag;
					this.balloons[i].SetActive(flag);
					if (!flag)
					{
						this.PopBalloon(i);
					}
				}
			}
		}
		else if (GorillaGameManager.instance != null)
		{
			base.gameObject.SetActive(false);
		}
		this.UpdateBalloonColors();
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x00066E40 File Offset: 0x00065040
	private void PopBalloon(int i)
	{
		GameObject gameObject = ObjectPools.instance.Instantiate(this.balloonPopFXPrefab);
		gameObject.transform.position = this.balloons[i].transform.position;
		GorillaColorizableBase componentInChildren = gameObject.GetComponentInChildren<GorillaColorizableBase>();
		if (componentInChildren != null)
		{
			componentInChildren.SetColor(this.teamColor);
		}
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x00066E98 File Offset: 0x00065098
	public void UpdateBalloonColors()
	{
		if (this.bMgr != null && this.myRig.creator != null)
		{
			if (this.bMgr.OnRedTeam(this.myRig.creator))
			{
				this.teamColor = this.orangeColor;
			}
			else
			{
				this.teamColor = this.blueColor;
			}
		}
		if (this.teamColor != this.lastColor)
		{
			this.lastColor = this.teamColor;
			foreach (Renderer renderer in this.renderers)
			{
				if (renderer)
				{
					foreach (Material material in renderer.materials)
					{
						if (!(material == null))
						{
							if (material.HasProperty("_BaseColor"))
							{
								material.SetColor("_BaseColor", this.teamColor);
							}
							if (material.HasProperty("_Color"))
							{
								material.SetColor("_Color", this.teamColor);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x04001746 RID: 5958
	public VRRig myRig;

	// Token: 0x04001747 RID: 5959
	public GameObject[] balloons;

	// Token: 0x04001748 RID: 5960
	public Color orangeColor;

	// Token: 0x04001749 RID: 5961
	public Color blueColor;

	// Token: 0x0400174A RID: 5962
	public Color defaultColor;

	// Token: 0x0400174B RID: 5963
	public Color lastColor;

	// Token: 0x0400174C RID: 5964
	public GameObject balloonPopFXPrefab;

	// Token: 0x0400174D RID: 5965
	[HideInInspector]
	public GorillaPaintbrawlManager bMgr;

	// Token: 0x0400174E RID: 5966
	public Player myPlayer;

	// Token: 0x0400174F RID: 5967
	private int colorShaderPropID;

	// Token: 0x04001750 RID: 5968
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x04001751 RID: 5969
	private bool[] balloonsCachedActiveState;

	// Token: 0x04001752 RID: 5970
	private Renderer[] renderers;

	// Token: 0x04001753 RID: 5971
	private Color teamColor;
}
