using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B1B RID: 2843
	[AddComponentMenu("")]
	public class MTAssetsMathematics : MonoBehaviour
	{
		// Token: 0x06004704 RID: 18180 RVA: 0x00151270 File Offset: 0x0014F470
		public static List<T> RandomizeThisList<T>(List<T> list)
		{
			int count = list.Count;
			int num = count - 1;
			for (int i = 0; i < num; i++)
			{
				int index = Random.Range(i, count);
				T value = list[i];
				list[i] = list[index];
				list[index] = value;
			}
			return list;
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x001512BD File Offset: 0x0014F4BD
		public static Vector3 GetHalfPositionBetweenTwoPoints(Vector3 pointA, Vector3 pointB)
		{
			return Vector3.Lerp(pointA, pointB, 0.5f);
		}
	}
}
