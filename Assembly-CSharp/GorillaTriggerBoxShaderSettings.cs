using System;
using GorillaTag.Rendering;
using UnityEngine;

// Token: 0x02000488 RID: 1160
public class GorillaTriggerBoxShaderSettings : GorillaTriggerBox
{
	// Token: 0x06001C33 RID: 7219 RVA: 0x00043650 File Offset: 0x00041850
	private void Awake()
	{
		if (this.sameSceneSettingsRef != null)
		{
			this.settings = this.sameSceneSettingsRef;
			return;
		}
		this.settingsRef.TryResolve<ZoneShaderSettings>(out this.settings);
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000DBB6C File Offset: 0x000D9D6C
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

	// Token: 0x04001F3A RID: 7994
	[SerializeField]
	private XSceneRef settingsRef;

	// Token: 0x04001F3B RID: 7995
	[SerializeField]
	private ZoneShaderSettings sameSceneSettingsRef;

	// Token: 0x04001F3C RID: 7996
	private ZoneShaderSettings settings;
}
