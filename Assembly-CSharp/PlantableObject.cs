using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003F4 RID: 1012
public class PlantableObject : TransferrableObject
{
	// Token: 0x060018C4 RID: 6340 RVA: 0x00078A38 File Offset: 0x00076C38
	protected override void Awake()
	{
		base.Awake();
		this.colorRShaderPropID = Shader.PropertyToID("_ColorR");
		this.colorGShaderPropID = Shader.PropertyToID("_ColorG");
		this.colorBShaderPropID = Shader.PropertyToID("_ColorB");
		this.materialPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x00078A88 File Offset: 0x00076C88
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.materialPropertyBlock.SetColor(this.colorRShaderPropID, this._colorR);
		this.flagRenderer.material = this.flagRenderer.sharedMaterial;
		this.flagRenderer.SetPropertyBlock(this.materialPropertyBlock);
		this.dippedColors = new PlantableObject.AppliedColors[20];
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x00078AE8 File Offset: 0x00076CE8
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
	// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00078BF4 File Offset: 0x00076DF4
	// (set) Token: 0x060018C8 RID: 6344 RVA: 0x00078BFC File Offset: 0x00076DFC
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
	// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00078C0B File Offset: 0x00076E0B
	// (set) Token: 0x060018CA RID: 6346 RVA: 0x00078C13 File Offset: 0x00076E13
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
	// (get) Token: 0x060018CB RID: 6347 RVA: 0x00078C22 File Offset: 0x00076E22
	// (set) Token: 0x060018CC RID: 6348 RVA: 0x00078C2A File Offset: 0x00076E2A
	public bool planted { get; private set; }

	// Token: 0x060018CD RID: 6349 RVA: 0x00078C34 File Offset: 0x00076E34
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

	// Token: 0x060018CE RID: 6350 RVA: 0x00078C8C File Offset: 0x00076E8C
	private void AddRed()
	{
		this.AddColor(PlantableObject.AppliedColors.Red);
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x00078C95 File Offset: 0x00076E95
	private void AddGreen()
	{
		this.AddColor(PlantableObject.AppliedColors.Blue);
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x00078C9E File Offset: 0x00076E9E
	private void AddBlue()
	{
		this.AddColor(PlantableObject.AppliedColors.Green);
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x00078CA7 File Offset: 0x00076EA7
	private void AddBlack()
	{
		this.AddColor(PlantableObject.AppliedColors.Black);
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x00078CB0 File Offset: 0x00076EB0
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

	// Token: 0x060018D3 RID: 6355 RVA: 0x00078CEC File Offset: 0x00076EEC
	public void ClearColors()
	{
		for (int i = 0; i < this.dippedColors.Length; i++)
		{
			this.dippedColors[i] = PlantableObject.AppliedColors.None;
		}
		this.currentDipIndex = 0;
		this.UpdateDisplayedDippedColor();
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x00078D24 File Offset: 0x00076F24
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

	// Token: 0x060018D5 RID: 6357 RVA: 0x00078E2D File Offset: 0x0007702D
	public void UpdateDisplayedDippedColor()
	{
		this.colorR = this.CalculateOutputColor();
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x00078E3B File Offset: 0x0007703B
	public override void DropItem()
	{
		base.DropItem();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x00078E68 File Offset: 0x00077068
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

	// Token: 0x060018D8 RID: 6360 RVA: 0x00078EB8 File Offset: 0x000770B8
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x00078EE2 File Offset: 0x000770E2
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x00078EEC File Offset: 0x000770EC
	public override bool ShouldBeKinematic()
	{
		return base.ShouldBeKinematic() || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x00078F04 File Offset: 0x00077104
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

	// Token: 0x04001B86 RID: 7046
	public PlantablePoint point;

	// Token: 0x04001B87 RID: 7047
	public float respawnAfterDuration;

	// Token: 0x04001B88 RID: 7048
	private float respawnAtTimestamp;

	// Token: 0x04001B89 RID: 7049
	public SkinnedMeshRenderer flagRenderer;

	// Token: 0x04001B8A RID: 7050
	[FormerlySerializedAs("colorShaderPropID")]
	[SerializeReference]
	private int colorRShaderPropID;

	// Token: 0x04001B8B RID: 7051
	[SerializeReference]
	private int colorGShaderPropID;

	// Token: 0x04001B8C RID: 7052
	[SerializeReference]
	private int colorBShaderPropID;

	// Token: 0x04001B8D RID: 7053
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04001B8E RID: 7054
	[HideInInspector]
	[SerializeReference]
	private Color _colorR;

	// Token: 0x04001B8F RID: 7055
	[HideInInspector]
	[SerializeReference]
	private Color _colorG;

	// Token: 0x04001B91 RID: 7057
	public Transform flagTip;

	// Token: 0x04001B92 RID: 7058
	public PlantableObject.AppliedColors[] dippedColors = new PlantableObject.AppliedColors[20];

	// Token: 0x04001B93 RID: 7059
	public int currentDipIndex;

	// Token: 0x020003F5 RID: 1013
	public enum AppliedColors
	{
		// Token: 0x04001B95 RID: 7061
		None,
		// Token: 0x04001B96 RID: 7062
		Red,
		// Token: 0x04001B97 RID: 7063
		Green,
		// Token: 0x04001B98 RID: 7064
		Blue,
		// Token: 0x04001B99 RID: 7065
		Black
	}
}
