using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B2 RID: 178
public class BlinkingText : MonoBehaviour
{
	// Token: 0x06000486 RID: 1158 RVA: 0x00033664 File Offset: 0x00031864
	private void Awake()
	{
		this.textComponent = base.GetComponent<Text>();
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0007D4A4 File Offset: 0x0007B6A4
	private void Update()
	{
		if (this.isOn && Time.time > this.lastTime + this.cycleTime * this.dutyCycle)
		{
			this.isOn = false;
			this.textComponent.enabled = false;
			return;
		}
		if (!this.isOn && Time.time > this.lastTime + this.cycleTime)
		{
			this.lastTime = Time.time;
			this.isOn = true;
			this.textComponent.enabled = true;
		}
	}

	// Token: 0x0400053E RID: 1342
	public float cycleTime;

	// Token: 0x0400053F RID: 1343
	public float dutyCycle;

	// Token: 0x04000540 RID: 1344
	private bool isOn;

	// Token: 0x04000541 RID: 1345
	private float lastTime;

	// Token: 0x04000542 RID: 1346
	private Text textComponent;
}
