using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class DayNightResetForBaking : MonoBehaviour
{
	// Token: 0x06002782 RID: 10114 RVA: 0x0010D538 File Offset: 0x0010B738
	public void SetMaterialsForBaking()
	{
		foreach (Material material in this.dayNightManager.dayNightSupportedMaterials)
		{
			if (material != null)
			{
				material.shader = this.dayNightManager.standard;
			}
			else
			{
				Debug.LogError("a material is missing from day night supported materials in the daynightmanager! something might have gotten deleted inappropriately, or an entry should be manually removed.", base.gameObject);
			}
		}
		foreach (Material material2 in this.dayNightManager.dayNightSupportedMaterialsCutout)
		{
			if (material2 != null)
			{
				material2.shader = this.dayNightManager.standardCutout;
			}
			else
			{
				Debug.LogError("a material is missing from day night supported materials cutout in the daynightmanager! something might have gotten deleted inappropriately, or an entry should be manually removed.", base.gameObject);
			}
		}
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x0010D5DC File Offset: 0x0010B7DC
	public void SetMaterialsForGame()
	{
		foreach (Material material in this.dayNightManager.dayNightSupportedMaterials)
		{
			if (material != null)
			{
				material.shader = this.dayNightManager.gorillaUnlit;
			}
			else
			{
				Debug.LogError("a material is missing from day night supported materials in the daynightmanager! something might have gotten deleted inappropriately, or an entry should be manually removed.", base.gameObject);
			}
		}
		foreach (Material material2 in this.dayNightManager.dayNightSupportedMaterialsCutout)
		{
			if (material2 != null)
			{
				material2.shader = this.dayNightManager.gorillaUnlitCutout;
			}
			else
			{
				Debug.LogError("a material is missing from day night supported materialsc cutout in the daynightmanager! something might have gotten deleted inappropriately, or an entry should be manually removed.", base.gameObject);
			}
		}
	}

	// Token: 0x04002C98 RID: 11416
	public BetterDayNightManager dayNightManager;
}
