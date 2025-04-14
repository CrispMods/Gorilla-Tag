using System;
using GorillaTag.Rendering;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class GorillaTriggerBoxShaderSettings : GorillaTriggerBox
{
	// Token: 0x06001BE2 RID: 7138 RVA: 0x000880A7 File Offset: 0x000862A7
	private void Awake()
	{
		if (this.sameSceneSettingsRef != null)
		{
			this.settings = this.sameSceneSettingsRef;
			return;
		}
		this.settingsRef.TryResolve<ZoneShaderSettings>(out this.settings);
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000880D8 File Offset: 0x000862D8
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

	// Token: 0x04001EEC RID: 7916
	[SerializeField]
	private XSceneRef settingsRef;

	// Token: 0x04001EED RID: 7917
	[SerializeField]
	private ZoneShaderSettings sameSceneSettingsRef;

	// Token: 0x04001EEE RID: 7918
	private ZoneShaderSettings settings;
}
