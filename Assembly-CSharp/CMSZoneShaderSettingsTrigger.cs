using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTag.Rendering;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using UnityEngine;

// Token: 0x02000663 RID: 1635
public class CMSZoneShaderSettingsTrigger : MonoBehaviour
{
	// Token: 0x06002874 RID: 10356 RVA: 0x0004B7F5 File Offset: 0x000499F5
	public void OnEnable()
	{
		if (this.activateOnEnable)
		{
			this.ActivateShaderSettings();
		}
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x00111544 File Offset: 0x0010F744
	public void CopySettings(ZoneShaderTriggerSettings triggerSettings)
	{
		base.gameObject.layer = UnityLayer.GorillaBoundary.ToLayerIndex();
		this.activateOnEnable = triggerSettings.activateOnEnable;
		if (triggerSettings.activationType == ZoneShaderTriggerSettings.ActivationType.ActivateCustomMapDefaults)
		{
			this.activateCustomMapDefaults = true;
			return;
		}
		GameObject zoneShaderSettingsObject = triggerSettings.zoneShaderSettingsObject;
		if (zoneShaderSettingsObject.IsNotNull())
		{
			this.shaderSettingsObject = zoneShaderSettingsObject;
		}
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x0004B805 File Offset: 0x00049A05
	public void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.bodyCollider)
		{
			this.ActivateShaderSettings();
		}
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x00111598 File Offset: 0x0010F798
	private void ActivateShaderSettings()
	{
		if (this.activateCustomMapDefaults)
		{
			CustomMapManager.ActivateDefaultZoneShaderSettings();
			return;
		}
		if (this.shaderSettingsObject.IsNotNull())
		{
			ZoneShaderSettings component = this.shaderSettingsObject.GetComponent<ZoneShaderSettings>();
			if (component.IsNotNull())
			{
				component.BecomeActiveInstance(false);
			}
		}
	}

	// Token: 0x04002DD4 RID: 11732
	public GameObject shaderSettingsObject;

	// Token: 0x04002DD5 RID: 11733
	public bool activateCustomMapDefaults;

	// Token: 0x04002DD6 RID: 11734
	public bool activateOnEnable;
}
