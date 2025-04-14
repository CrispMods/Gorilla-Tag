using System;
using UnityEngine;

// Token: 0x0200069D RID: 1693
public class ZoneBasedObject : MonoBehaviour
{
	// Token: 0x06002A0D RID: 10765 RVA: 0x000D115C File Offset: 0x000CF35C
	public bool IsLocalPlayerInZone()
	{
		GTZone[] array = this.zones;
		for (int i = 0; i < array.Length; i++)
		{
			if (ZoneManagement.IsInZone(array[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x000D118C File Offset: 0x000CF38C
	public static ZoneBasedObject SelectRandomEligible(ZoneBasedObject[] objects, string overrideChoice = "")
	{
		if (overrideChoice != "")
		{
			foreach (ZoneBasedObject zoneBasedObject in objects)
			{
				if (zoneBasedObject.gameObject.name == overrideChoice)
				{
					return zoneBasedObject;
				}
			}
		}
		ZoneBasedObject result = null;
		int num = 0;
		foreach (ZoneBasedObject zoneBasedObject2 in objects)
		{
			if (zoneBasedObject2.gameObject.activeInHierarchy)
			{
				GTZone[] array = zoneBasedObject2.zones;
				for (int j = 0; j < array.Length; j++)
				{
					if (ZoneManagement.IsInZone(array[j]))
					{
						if (Random.Range(0, num) == 0)
						{
							result = zoneBasedObject2;
						}
						num++;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04002F91 RID: 12177
	public GTZone[] zones;
}
