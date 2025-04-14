using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class PlantableFlagManager : MonoBehaviourPun, IPunObservable
{
	// Token: 0x06000ADE RID: 2782 RVA: 0x0003AD88 File Offset: 0x00038F88
	public void ResetMyFlags()
	{
		foreach (PlantableObject plantableObject in this.flags)
		{
			if (plantableObject.IsMyItem())
			{
				if (plantableObject.currentState != TransferrableObject.PositionState.Dropped)
				{
					plantableObject.DropItem();
				}
				plantableObject.ResetToHome();
			}
		}
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0003ADD0 File Offset: 0x00038FD0
	public void ResetAllFlags()
	{
		foreach (PlantableObject plantableObject in this.flags)
		{
			if (!plantableObject.IsMyItem())
			{
				plantableObject.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().RequestOwnershipImmediately(delegate
				{
				});
			}
			if (plantableObject.currentState != TransferrableObject.PositionState.Dropped)
			{
				plantableObject.DropItem();
			}
			plantableObject.ResetToHome();
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0003AE48 File Offset: 0x00039048
	public void RainbowifyAllFlags(float saturation = 1f, float value = 1f)
	{
		Color red = Color.red;
		for (int i = 0; i < this.flags.Length; i++)
		{
			Color colorR = Color.HSVToRGB((float)i / (float)this.flags.Length, saturation, value);
			PlantableObject plantableObject = this.flags[i];
			if (plantableObject)
			{
				plantableObject.colorR = colorR;
				plantableObject.colorG = Color.black;
			}
		}
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0003AEA8 File Offset: 0x000390A8
	public void Awake()
	{
		this.mode = new FlagCauldronColorer.ColorMode[this.flags.Length];
		this.flagColors = new PlantableObject.AppliedColors[this.flags.Length][];
		for (int i = 0; i < this.flags.Length; i++)
		{
			this.flagColors[i] = new PlantableObject.AppliedColors[20];
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0003AF00 File Offset: 0x00039100
	public void Update()
	{
		if (this.mode == null)
		{
			this.mode = new FlagCauldronColorer.ColorMode[this.flags.Length];
		}
		if (this.flagColors == null)
		{
			this.flagColors = new PlantableObject.AppliedColors[this.flags.Length][];
			for (int i = 0; i < this.flags.Length; i++)
			{
				this.flagColors[i] = new PlantableObject.AppliedColors[20];
			}
		}
		for (int j = 0; j < this.flags.Length; j++)
		{
			PlantableObject plantableObject = this.flags[j];
			if (plantableObject.IsMyItem())
			{
				Vector3.SqrMagnitude(plantableObject.flagTip.position - base.transform.position);
			}
		}
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0003AFB0 File Offset: 0x000391B0
	[PunRPC]
	public void UpdateFlagColorRPC(int flagIndex, int colorIndex, PhotonMessageInfo info)
	{
		PlantableObject plantableObject = this.flags[flagIndex];
		if (colorIndex == 0)
		{
			plantableObject.ClearColors();
			return;
		}
		plantableObject.AddColor((PlantableObject.AppliedColors)colorIndex);
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0003AFDC File Offset: 0x000391DC
	public void UpdateFlagColors()
	{
		for (int i = 0; i < this.flagColors.Length; i++)
		{
			PlantableObject.AppliedColors[] array = this.flagColors[i];
			PlantableObject plantableObject = this.flags[i];
			if (!plantableObject.IsMyItem() && array.Length <= 20)
			{
				plantableObject.dippedColors = array;
				plantableObject.UpdateDisplayedDippedColor();
			}
		}
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0003B02C File Offset: 0x0003922C
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			for (int i = 0; i < this.flagColors.Length; i++)
			{
				for (int j = 0; j < 20; j++)
				{
					stream.SendNext((int)this.flagColors[i][j]);
				}
			}
			return;
		}
		for (int k = 0; k < this.flagColors.Length; k++)
		{
			for (int l = 0; l < 20; l++)
			{
				this.flagColors[k][l] = (PlantableObject.AppliedColors)stream.ReceiveNext();
			}
		}
		this.UpdateFlagColors();
	}

	// Token: 0x04000D4B RID: 3403
	public PlantableObject[] flags;

	// Token: 0x04000D4C RID: 3404
	public FlagCauldronColorer[] cauldrons;

	// Token: 0x04000D4D RID: 3405
	public FlagCauldronColorer.ColorMode[] mode;

	// Token: 0x04000D4E RID: 3406
	public PlantableObject.AppliedColors[][] flagColors;
}
