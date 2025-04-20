using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class PlantableFlagManager : MonoBehaviourPun, IPunObservable
{
	// Token: 0x06000B2A RID: 2858 RVA: 0x0009A240 File Offset: 0x00098440
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

	// Token: 0x06000B2B RID: 2859 RVA: 0x0009A288 File Offset: 0x00098488
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

	// Token: 0x06000B2C RID: 2860 RVA: 0x0009A300 File Offset: 0x00098500
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

	// Token: 0x06000B2D RID: 2861 RVA: 0x0009A360 File Offset: 0x00098560
	public void Awake()
	{
		this.mode = new FlagCauldronColorer.ColorMode[this.flags.Length];
		this.flagColors = new PlantableObject.AppliedColors[this.flags.Length][];
		for (int i = 0; i < this.flags.Length; i++)
		{
			this.flagColors[i] = new PlantableObject.AppliedColors[20];
		}
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0009A3B8 File Offset: 0x000985B8
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

	// Token: 0x06000B2F RID: 2863 RVA: 0x0009A468 File Offset: 0x00098668
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

	// Token: 0x06000B30 RID: 2864 RVA: 0x0009A494 File Offset: 0x00098694
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

	// Token: 0x06000B31 RID: 2865 RVA: 0x0009A4E4 File Offset: 0x000986E4
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

	// Token: 0x04000D91 RID: 3473
	public PlantableObject[] flags;

	// Token: 0x04000D92 RID: 3474
	public FlagCauldronColorer[] cauldrons;

	// Token: 0x04000D93 RID: 3475
	public FlagCauldronColorer.ColorMode[] mode;

	// Token: 0x04000D94 RID: 3476
	public PlantableObject.AppliedColors[][] flagColors;
}
