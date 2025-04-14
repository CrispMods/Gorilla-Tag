using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003F4 RID: 1012
public class PlantableObject : TransferrableObject
{
	// Token: 0x060018C7 RID: 6343 RVA: 0x00078DBC File Offset: 0x00076FBC
	protected override void Awake()
	{
		base.Awake();
		this.colorRShaderPropID = Shader.PropertyToID("_ColorR");
		this.colorGShaderPropID = Shader.PropertyToID("_ColorG");
		this.colorBShaderPropID = Shader.PropertyToID("_ColorB");
		this.materialPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x00078E0C File Offset: 0x0007700C
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.materialPropertyBlock.SetColor(this.colorRShaderPropID, this._colorR);
		this.flagRenderer.material = this.flagRenderer.sharedMaterial;
		this.flagRenderer.SetPropertyBlock(this.materialPropertyBlock);
		this.dippedColors = new PlantableObject.AppliedColors[20];
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x00078E6C File Offset: 0x0007706C
	private void AssureShaderStuff()
	{
		if (!this.flagRenderer)
		{
			return;
		}
		if (this.colorRShaderPropID == 0)
		{
			this.colorRShaderPropID = Shader.PropertyToID("_ColorR");
			this.colorGShaderPropID = Shader.PropertyToID("_ColorG");
			this.colorBShaderPropID = Shader.PropertyToID("_ColorB");
		}
		if (this.materialPropertyBlock == null)
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
		}
		try
		{
			this.materialPropertyBlock.SetColor(this.colorRShaderPropID, this._colorR);
			this.materialPropertyBlock.SetColor(this.colorGShaderPropID, this._colorG);
		}
		catch
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
			this.materialPropertyBlock.SetColor(this.colorRShaderPropID, this._colorR);
			this.materialPropertyBlock.SetColor(this.colorGShaderPropID, this._colorG);
		}
		this.flagRenderer.material = this.flagRenderer.sharedMaterial;
		this.flagRenderer.SetPropertyBlock(this.materialPropertyBlock);
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x060018CA RID: 6346 RVA: 0x00078F78 File Offset: 0x00077178
	// (set) Token: 0x060018CB RID: 6347 RVA: 0x00078F80 File Offset: 0x00077180
	public Color colorR
	{
		get
		{
			return this._colorR;
		}
		set
		{
			this._colorR = value;
			this.AssureShaderStuff();
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x060018CC RID: 6348 RVA: 0x00078F8F File Offset: 0x0007718F
	// (set) Token: 0x060018CD RID: 6349 RVA: 0x00078F97 File Offset: 0x00077197
	public Color colorG
	{
		get
		{
			return this._colorG;
		}
		set
		{
			this._colorG = value;
			this.AssureShaderStuff();
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x060018CE RID: 6350 RVA: 0x00078FA6 File Offset: 0x000771A6
	// (set) Token: 0x060018CF RID: 6351 RVA: 0x00078FAE File Offset: 0x000771AE
	public bool planted { get; private set; }

	// Token: 0x060018D0 RID: 6352 RVA: 0x00078FB8 File Offset: 0x000771B8
	public void SetPlanted(bool newPlanted)
	{
		if (this.planted != newPlanted)
		{
			if (newPlanted)
			{
				if (!this.rigidbodyInstance.isKinematic)
				{
					this.rigidbodyInstance.isKinematic = true;
				}
				this.respawnAtTimestamp = Time.time + this.respawnAfterDuration;
			}
			else
			{
				this.respawnAtTimestamp = 0f;
			}
			this.planted = newPlanted;
		}
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x00079010 File Offset: 0x00077210
	private void AddRed()
	{
		this.AddColor(PlantableObject.AppliedColors.Red);
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x00079019 File Offset: 0x00077219
	private void AddGreen()
	{
		this.AddColor(PlantableObject.AppliedColors.Blue);
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x00079022 File Offset: 0x00077222
	private void AddBlue()
	{
		this.AddColor(PlantableObject.AppliedColors.Green);
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x0007902B File Offset: 0x0007722B
	private void AddBlack()
	{
		this.AddColor(PlantableObject.AppliedColors.Black);
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x00079034 File Offset: 0x00077234
	public void AddColor(PlantableObject.AppliedColors color)
	{
		this.dippedColors[this.currentDipIndex] = color;
		this.currentDipIndex++;
		if (this.currentDipIndex >= this.dippedColors.Length)
		{
			this.currentDipIndex = 0;
		}
		this.UpdateDisplayedDippedColor();
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x00079070 File Offset: 0x00077270
	public void ClearColors()
	{
		for (int i = 0; i < this.dippedColors.Length; i++)
		{
			this.dippedColors[i] = PlantableObject.AppliedColors.None;
		}
		this.currentDipIndex = 0;
		this.UpdateDisplayedDippedColor();
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000790A8 File Offset: 0x000772A8
	public Color CalculateOutputColor()
	{
		Color color = Color.black;
		int num = 0;
		int num2 = 0;
		foreach (PlantableObject.AppliedColors appliedColors in this.dippedColors)
		{
			if (appliedColors == PlantableObject.AppliedColors.None)
			{
				break;
			}
			switch (appliedColors)
			{
			case PlantableObject.AppliedColors.Red:
				color += Color.red;
				num2++;
				break;
			case PlantableObject.AppliedColors.Green:
				color += Color.green;
				num2++;
				break;
			case PlantableObject.AppliedColors.Blue:
				color += Color.blue;
				num2++;
				break;
			case PlantableObject.AppliedColors.Black:
				num++;
				num2++;
				break;
			}
		}
		if (color == Color.black && num == 0)
		{
			return Color.white;
		}
		float num3 = Mathf.Max(new float[]
		{
			color.r,
			color.g,
			color.b
		});
		if (num3 == 0f)
		{
			return Color.black;
		}
		color /= num3;
		float num4 = (float)num / (float)num2;
		if (num4 > 0f)
		{
			color *= 1f - num4;
		}
		return color;
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000791B1 File Offset: 0x000773B1
	public void UpdateDisplayedDippedColor()
	{
		this.colorR = this.CalculateOutputColor();
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x000791BF File Offset: 0x000773BF
	public override void DropItem()
	{
		base.DropItem();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000791EC File Offset: 0x000773EC
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		this.itemState = (this.planted ? TransferrableObject.ItemStates.State1 : TransferrableObject.ItemStates.State0);
		if (this.respawnAtTimestamp != 0f && Time.time > this.respawnAtTimestamp)
		{
			this.respawnAtTimestamp = 0f;
			this.ResetToHome();
		}
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0007923C File Offset: 0x0007743C
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x00079266 File Offset: 0x00077466
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x00079270 File Offset: 0x00077470
	public override bool ShouldBeKinematic()
	{
		return base.ShouldBeKinematic() || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x00079288 File Offset: 0x00077488
	public override void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		base.OnOwnershipTransferred(toPlayer, fromPlayer);
		if (toPlayer == null)
		{
			return;
		}
		if (toPlayer.IsLocal && this.itemState == TransferrableObject.ItemStates.State1)
		{
			this.respawnAtTimestamp = Time.time + this.respawnAfterDuration;
		}
		Action<Color> <>9__1;
		GorillaGameManager.OnInstanceReady(delegate
		{
			VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(toPlayer);
			if (vrrig == null)
			{
				return;
			}
			VRRig vrrig2 = vrrig;
			Action<Color> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(Color color1)
				{
					this.colorG = color1;
				});
			}
			vrrig2.OnColorInitialized(action);
		});
	}

	// Token: 0x04001B87 RID: 7047
	public PlantablePoint point;

	// Token: 0x04001B88 RID: 7048
	public float respawnAfterDuration;

	// Token: 0x04001B89 RID: 7049
	private float respawnAtTimestamp;

	// Token: 0x04001B8A RID: 7050
	public SkinnedMeshRenderer flagRenderer;

	// Token: 0x04001B8B RID: 7051
	[FormerlySerializedAs("colorShaderPropID")]
	[SerializeReference]
	private int colorRShaderPropID;

	// Token: 0x04001B8C RID: 7052
	[SerializeReference]
	private int colorGShaderPropID;

	// Token: 0x04001B8D RID: 7053
	[SerializeReference]
	private int colorBShaderPropID;

	// Token: 0x04001B8E RID: 7054
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04001B8F RID: 7055
	[HideInInspector]
	[SerializeReference]
	private Color _colorR;

	// Token: 0x04001B90 RID: 7056
	[HideInInspector]
	[SerializeReference]
	private Color _colorG;

	// Token: 0x04001B92 RID: 7058
	public Transform flagTip;

	// Token: 0x04001B93 RID: 7059
	public PlantableObject.AppliedColors[] dippedColors = new PlantableObject.AppliedColors[20];

	// Token: 0x04001B94 RID: 7060
	public int currentDipIndex;

	// Token: 0x020003F5 RID: 1013
	public enum AppliedColors
	{
		// Token: 0x04001B96 RID: 7062
		None,
		// Token: 0x04001B97 RID: 7063
		Red,
		// Token: 0x04001B98 RID: 7064
		Green,
		// Token: 0x04001B99 RID: 7065
		Blue,
		// Token: 0x04001B9A RID: 7066
		Black
	}
}
