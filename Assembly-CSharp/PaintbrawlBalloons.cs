using System;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200038D RID: 909
public class PaintbrawlBalloons : MonoBehaviour
{
	// Token: 0x06001553 RID: 5459 RVA: 0x000BF6E8 File Offset: 0x000BD8E8
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

	// Token: 0x06001554 RID: 5460 RVA: 0x0003E67C File Offset: 0x0003C87C
	protected void OnEnable()
	{
		this.UpdateBalloonColors();
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000BF774 File Offset: 0x000BD974
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

	// Token: 0x06001556 RID: 5462 RVA: 0x000BF860 File Offset: 0x000BDA60
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

	// Token: 0x06001557 RID: 5463 RVA: 0x000BF8B8 File Offset: 0x000BDAB8
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

	// Token: 0x0400178E RID: 6030
	public VRRig myRig;

	// Token: 0x0400178F RID: 6031
	public GameObject[] balloons;

	// Token: 0x04001790 RID: 6032
	public Color orangeColor;

	// Token: 0x04001791 RID: 6033
	public Color blueColor;

	// Token: 0x04001792 RID: 6034
	public Color defaultColor;

	// Token: 0x04001793 RID: 6035
	public Color lastColor;

	// Token: 0x04001794 RID: 6036
	public GameObject balloonPopFXPrefab;

	// Token: 0x04001795 RID: 6037
	[HideInInspector]
	public GorillaPaintbrawlManager bMgr;

	// Token: 0x04001796 RID: 6038
	public Player myPlayer;

	// Token: 0x04001797 RID: 6039
	private int colorShaderPropID;

	// Token: 0x04001798 RID: 6040
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x04001799 RID: 6041
	private bool[] balloonsCachedActiveState;

	// Token: 0x0400179A RID: 6042
	private Renderer[] renderers;

	// Token: 0x0400179B RID: 6043
	private Color teamColor;
}
