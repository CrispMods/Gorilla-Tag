using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003FF RID: 1023
public class PlantableObject : TransferrableObject
{
	// Token: 0x06001911 RID: 6417 RVA: 0x000CEAEC File Offset: 0x000CCCEC
	protected override void Awake()
	{
		base.Awake();
		this.colorRShaderPropID = Shader.PropertyToID("_ColorR");
		this.colorGShaderPropID = Shader.PropertyToID("_ColorG");
		this.colorBShaderPropID = Shader.PropertyToID("_ColorB");
		this.materialPropertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x000CEB3C File Offset: 0x000CCD3C
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.materialPropertyBlock.SetColor(this.colorRShaderPropID, this._colorR);
		this.flagRenderer.material = this.flagRenderer.sharedMaterial;
		this.flagRenderer.SetPropertyBlock(this.materialPropertyBlock);
		this.dippedColors = new PlantableObject.AppliedColors[20];
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x000CEB9C File Offset: 0x000CCD9C
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

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06001914 RID: 6420 RVA: 0x00040F84 File Offset: 0x0003F184
	// (set) Token: 0x06001915 RID: 6421 RVA: 0x00040F8C File Offset: 0x0003F18C
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

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06001916 RID: 6422 RVA: 0x00040F9B File Offset: 0x0003F19B
	// (set) Token: 0x06001917 RID: 6423 RVA: 0x00040FA3 File Offset: 0x0003F1A3
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

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06001918 RID: 6424 RVA: 0x00040FB2 File Offset: 0x0003F1B2
	// (set) Token: 0x06001919 RID: 6425 RVA: 0x00040FBA File Offset: 0x0003F1BA
	public bool planted { get; private set; }

	// Token: 0x0600191A RID: 6426 RVA: 0x000CECA8 File Offset: 0x000CCEA8
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

	// Token: 0x0600191B RID: 6427 RVA: 0x00040FC3 File Offset: 0x0003F1C3
	private void AddRed()
	{
		this.AddColor(PlantableObject.AppliedColors.Red);
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x00040FCC File Offset: 0x0003F1CC
	private void AddGreen()
	{
		this.AddColor(PlantableObject.AppliedColors.Blue);
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x00040FD5 File Offset: 0x0003F1D5
	private void AddBlue()
	{
		this.AddColor(PlantableObject.AppliedColors.Green);
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x00040FDE File Offset: 0x0003F1DE
	private void AddBlack()
	{
		this.AddColor(PlantableObject.AppliedColors.Black);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x00040FE7 File Offset: 0x0003F1E7
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

	// Token: 0x06001920 RID: 6432 RVA: 0x000CED00 File Offset: 0x000CCF00
	public void ClearColors()
	{
		for (int i = 0; i < this.dippedColors.Length; i++)
		{
			this.dippedColors[i] = PlantableObject.AppliedColors.None;
		}
		this.currentDipIndex = 0;
		this.UpdateDisplayedDippedColor();
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000CED38 File Offset: 0x000CCF38
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

	// Token: 0x06001922 RID: 6434 RVA: 0x00041022 File Offset: 0x0003F222
	public void UpdateDisplayedDippedColor()
	{
		this.colorR = this.CalculateOutputColor();
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x00041030 File Offset: 0x0003F230
	public override void DropItem()
	{
		base.DropItem();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x000CEE44 File Offset: 0x000CD044
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

	// Token: 0x06001925 RID: 6437 RVA: 0x0004105A File Offset: 0x0003F25A
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x00041084 File Offset: 0x0003F284
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x0004108E File Offset: 0x0003F28E
	public override bool ShouldBeKinematic()
	{
		return base.ShouldBeKinematic() || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x000CEE94 File Offset: 0x000CD094
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

	// Token: 0x04001BCF RID: 7119
	public PlantablePoint point;

	// Token: 0x04001BD0 RID: 7120
	public float respawnAfterDuration;

	// Token: 0x04001BD1 RID: 7121
	private float respawnAtTimestamp;

	// Token: 0x04001BD2 RID: 7122
	public SkinnedMeshRenderer flagRenderer;

	// Token: 0x04001BD3 RID: 7123
	[FormerlySerializedAs("colorShaderPropID")]
	[SerializeReference]
	private int colorRShaderPropID;

	// Token: 0x04001BD4 RID: 7124
	[SerializeReference]
	private int colorGShaderPropID;

	// Token: 0x04001BD5 RID: 7125
	[SerializeReference]
	private int colorBShaderPropID;

	// Token: 0x04001BD6 RID: 7126
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04001BD7 RID: 7127
	[HideInInspector]
	[SerializeReference]
	private Color _colorR;

	// Token: 0x04001BD8 RID: 7128
	[HideInInspector]
	[SerializeReference]
	private Color _colorG;

	// Token: 0x04001BDA RID: 7130
	public Transform flagTip;

	// Token: 0x04001BDB RID: 7131
	public PlantableObject.AppliedColors[] dippedColors = new PlantableObject.AppliedColors[20];

	// Token: 0x04001BDC RID: 7132
	public int currentDipIndex;

	// Token: 0x02000400 RID: 1024
	public enum AppliedColors
	{
		// Token: 0x04001BDE RID: 7134
		None,
		// Token: 0x04001BDF RID: 7135
		Red,
		// Token: 0x04001BE0 RID: 7136
		Green,
		// Token: 0x04001BE1 RID: 7137
		Blue,
		// Token: 0x04001BE2 RID: 7138
		Black
	}
}
