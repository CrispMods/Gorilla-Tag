using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B18 RID: 2840
	[AddComponentMenu("")]
	public class MTAssetsMathematics : MonoBehaviour
	{
		// Token: 0x060046F8 RID: 18168 RVA: 0x00150CA8 File Offset: 0x0014EEA8
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

		// Token: 0x060046F9 RID: 18169 RVA: 0x00150CF5 File Offset: 0x0014EEF5
		public static Vector3 GetHalfPositionBetweenTwoPoints(Vector3 pointA, Vector3 pointB)
		{
			return Vector3.Lerp(pointA, pointB, 0.5f);
		}
	}
}
