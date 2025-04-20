using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B45 RID: 2885
	[AddComponentMenu("")]
	public class MTAssetsMathematics : MonoBehaviour
	{
		// Token: 0x06004841 RID: 18497 RVA: 0x0018D488 File Offset: 0x0018B688
		public static List<T> RandomizeThisList<T>(List<T> list)
		{
			int count = list.Count;
			int num = count - 1;
			for (int i = 0; i < num; i++)
			{
				int index = UnityEngine.Random.Range(i, count);
				T value = list[i];
				list[i] = list[index];
				list[index] = value;
			}
			return list;
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x0005F020 File Offset: 0x0005D220
		public static Vector3 GetHalfPositionBetweenTwoPoints(Vector3 pointA, Vector3 pointB)
		{
			return Vector3.Lerp(pointA, pointB, 0.5f);
		}
	}
}
