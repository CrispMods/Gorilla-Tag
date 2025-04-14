using System;
using GorillaTag.Rendering;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class GorillaTriggerBoxShaderSettings : GorillaTriggerBox
{
	// Token: 0x06001BDF RID: 7135 RVA: 0x00087D23 File Offset: 0x00085F23
	private void Awake()
	{
		if (this.sameSceneSettingsRef != null)
		{
			this.settings = this.sameSceneSettingsRef;
			return;
		}
		this.settingsRef.TryResolve<ZoneShaderSettings>(out this.settings);
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00087D54 File Offset: 0x00085F54
	public override void OnBoxTriggered()
	{
		if (this.settings == null)
		{
			if (this.sameSceneSettingsRef != null)
			{
				this.settings = this.sameSceneSettingsRef;
			}
			else
			{
				this.settingsRef.TryResolve<ZoneShaderSettings>(out this.settings);
			}
		}
		if (this.settings != null)
		{
			this.settings.BecomeActiveInstance(false);
			return;
		}
		ZoneShaderSettings.ActivateDefaultSettings();
	}

	// Token: 0x04001EEB RID: 7915
	[SerializeField]
	private XSceneRef settingsRef;

	// Token: 0x04001EEC RID: 7916
	[SerializeField]
	private ZoneShaderSettings sameSceneSettingsRef;

	// Token: 0x04001EED RID: 7917
	private ZoneShaderSettings settings;
}
