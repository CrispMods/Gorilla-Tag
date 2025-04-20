using System;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class SampleUI : MonoBehaviour
{
	// Token: 0x0600129A RID: 4762 RVA: 0x0003CC0A File Offset: 0x0003AE0A
	private void Start()
	{
		DebugUIBuilder.instance.AddLabel("Enable Firebase in your project before running this sample", 1);
		DebugUIBuilder.instance.Show();
		this.inMenu = true;
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x000B1A78 File Offset: 0x000AFC78
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Active) || OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Active))
		{
			if (this.inMenu)
			{
				DebugUIBuilder.instance.Hide();
			}
			else
			{
				DebugUIBuilder.instance.Show();
			}
			this.inMenu = !this.inMenu;
		}
	}

	// Token: 0x04001487 RID: 5255
	private RectTransform collectionButton;

	// Token: 0x04001488 RID: 5256
	private RectTransform inputText;

	// Token: 0x04001489 RID: 5257
	private RectTransform valueText;

	// Token: 0x0400148A RID: 5258
	private bool inMenu;
}
