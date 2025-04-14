using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004D6 RID: 1238
[CreateAssetMenu(fileName = "BuilderPieceSet01", menuName = "Gorilla Tag/Builder/PieceSet", order = 0)]
public class BuilderPieceSet : ScriptableObject
{
	// Token: 0x06001E3E RID: 7742 RVA: 0x00096CD9 File Offset: 0x00094ED9
	public int GetIntIdentifier()
	{
		return this.playfabID.GetStaticHash();
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x00096CE8 File Offset: 0x00094EE8
	public DateTime GetScheduleDateTime()
	{
		if (this.isScheduled)
		{
			try
			{
				return DateTime.Parse(this.scheduledDate, CultureInfo.InvariantCulture);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}
		return DateTime.MinValue;
	}

	// Token: 0x04002185 RID: 8581
	[Tooltip("Display Name")]
	public string setName;

	// Token: 0x04002186 RID: 8582
	public GameObject displayModel;

	// Token: 0x04002187 RID: 8583
	[FormerlySerializedAs("uniqueId")]
	[Tooltip("Playfab ID")]
	public string playfabID;

	// Token: 0x04002188 RID: 8584
	public string materialId;

	// Token: 0x04002189 RID: 8585
	public bool isScheduled;

	// Token: 0x0400218A RID: 8586
	public string scheduledDate = "1/1/0001 00:00:00";

	// Token: 0x0400218B RID: 8587
	public List<BuilderPieceSet.BuilderPieceSubset> subsets;

	// Token: 0x020004D7 RID: 1239
	public enum BuilderPieceCategory
	{
		// Token: 0x0400218D RID: 8589
		FLAT,
		// Token: 0x0400218E RID: 8590
		TALL,
		// Token: 0x0400218F RID: 8591
		HALF_HEIGHT,
		// Token: 0x04002190 RID: 8592
		BEAM,
		// Token: 0x04002191 RID: 8593
		SLOPE,
		// Token: 0x04002192 RID: 8594
		OVERSIZED,
		// Token: 0x04002193 RID: 8595
		SPECIAL_DISPLAY,
		// Token: 0x04002194 RID: 8596
		FUNCTIONAL = 18,
		// Token: 0x04002195 RID: 8597
		DECORATIVE,
		// Token: 0x04002196 RID: 8598
		MISC
	}

	// Token: 0x020004D8 RID: 1240
	[Serializable]
	public class BuilderPieceSubset
	{
		// Token: 0x04002197 RID: 8599
		public string subsetName;

		// Token: 0x04002198 RID: 8600
		public BuilderPieceSet.BuilderPieceCategory pieceCategory;

		// Token: 0x04002199 RID: 8601
		public List<BuilderPieceSet.PieceInfo> pieceInfos;
	}

	// Token: 0x020004D9 RID: 1241
	[Serializable]
	public struct PieceInfo
	{
		// Token: 0x0400219A RID: 8602
		public BuilderPiece piecePrefab;

		// Token: 0x0400219B RID: 8603
		public bool overrideSetMaterial;

		// Token: 0x0400219C RID: 8604
		public string[] pieceMaterialTypes;
	}
}
