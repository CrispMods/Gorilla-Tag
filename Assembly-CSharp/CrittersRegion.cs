using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class CrittersRegion : MonoBehaviour
{
	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600024A RID: 586 RVA: 0x0000EFEB File Offset: 0x0000D1EB
	public int CritterCount
	{
		get
		{
			return this._critters.Count;
		}
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
	public void AddCritter(CrittersPawn pawn)
	{
		this._critters.Add(pawn);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000F006 File Offset: 0x0000D206
	public void RemoveCritter(CrittersPawn pawn)
	{
		this._critters.Remove(pawn);
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000F018 File Offset: 0x0000D218
	public Vector3 GetSpawnPoint()
	{
		float num = this.scale / 2f;
		float num2 = base.transform.lossyScale.y * this.scale;
		Vector3 vector = base.transform.TransformPoint(new Vector3(Random.Range(-num, num), num, Random.Range(-num, num)));
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, -base.transform.up, out raycastHit, num2, -1, QueryTriggerInteraction.Ignore))
		{
			Debug.DrawLine(vector, raycastHit.point, Color.green, 5f);
			return raycastHit.point;
		}
		Debug.DrawLine(vector, vector - base.transform.up * num2, Color.red, 5f);
		return vector;
	}

	// Token: 0x040002CB RID: 715
	public CrittersBiome Biome = CrittersBiome.Any;

	// Token: 0x040002CC RID: 716
	public int maxCritters = 10;

	// Token: 0x040002CD RID: 717
	public float scale = 10f;

	// Token: 0x040002CE RID: 718
	public List<CrittersPawn> _critters = new List<CrittersPawn>();
}
