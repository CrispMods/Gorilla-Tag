using System;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class SampleUI : MonoBehaviour
{
	// Token: 0x0600124E RID: 4686 RVA: 0x00057554 File Offset: 0x00055754
	private void Start()
	{
		DebugUIBuilder.instance.AddLabel("Enable Firebase in your project before running this sample", 1);
		DebugUIBuilder.instance.Show();
		this.inMenu = true;
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x00057578 File Offset: 0x00055778
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

	// Token: 0x0400143F RID: 5183
	private RectTransform collectionButton;

	// Token: 0x04001440 RID: 5184
	private RectTransform inputText;

	// Token: 0x04001441 RID: 5185
	private RectTransform valueText;

	// Token: 0x04001442 RID: 5186
	private bool inMenu;
}
