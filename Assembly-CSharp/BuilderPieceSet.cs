using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004E3 RID: 1251
[CreateAssetMenu(fileName = "BuilderPieceSet01", menuName = "Gorilla Tag/Builder/PieceSet", order = 0)]
public class BuilderPieceSet : ScriptableObject
{
	// Token: 0x06001E97 RID: 7831 RVA: 0x00044CC8 File Offset: 0x00042EC8
	public int GetIntIdentifier()
	{
		return this.playfabID.GetStaticHash();
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000E9580 File Offset: 0x000E7780
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

	// Token: 0x040021D8 RID: 8664
	[Tooltip("Display Name")]
	public string setName;

	// Token: 0x040021D9 RID: 8665
	public GameObject displayModel;

	// Token: 0x040021DA RID: 8666
	[FormerlySerializedAs("uniqueId")]
	[Tooltip("Playfab ID")]
	public string playfabID;

	// Token: 0x040021DB RID: 8667
	public string materialId;

	// Token: 0x040021DC RID: 8668
	public bool isScheduled;

	// Token: 0x040021DD RID: 8669
	public string scheduledDate = "1/1/0001 00:00:00";

	// Token: 0x040021DE RID: 8670
	public List<BuilderPieceSet.BuilderPieceSubset> subsets;

	// Token: 0x020004E4 RID: 1252
	public enum BuilderPieceCategory
	{
		// Token: 0x040021E0 RID: 8672
		FLAT,
		// Token: 0x040021E1 RID: 8673
		TALL,
		// Token: 0x040021E2 RID: 8674
		HALF_HEIGHT,
		// Token: 0x040021E3 RID: 8675
		BEAM,
		// Token: 0x040021E4 RID: 8676
		SLOPE,
		// Token: 0x040021E5 RID: 8677
		OVERSIZED,
		// Token: 0x040021E6 RID: 8678
		SPECIAL_DISPLAY,
		// Token: 0x040021E7 RID: 8679
		FUNCTIONAL = 18,
		// Token: 0x040021E8 RID: 8680
		DECORATIVE,
		// Token: 0x040021E9 RID: 8681
		MISC
	}

	// Token: 0x020004E5 RID: 1253
	[Serializable]
	public class BuilderPieceSubset
	{
		// Token: 0x040021EA RID: 8682
		public string subsetName;

		// Token: 0x040021EB RID: 8683
		public BuilderPieceSet.BuilderPieceCategory pieceCategory;

		// Token: 0x040021EC RID: 8684
		public List<BuilderPieceSet.PieceInfo> pieceInfos;
	}

	// Token: 0x020004E6 RID: 1254
	[Serializable]
	public struct PieceInfo
	{
		// Token: 0x040021ED RID: 8685
		public BuilderPiece piecePrefab;

		// Token: 0x040021EE RID: 8686
		public bool overrideSetMaterial;

		// Token: 0x040021EF RID: 8687
		public string[] pieceMaterialTypes;
	}
}
