using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A8 RID: 168
public class BlinkingText : MonoBehaviour
{
	// Token: 0x0600044C RID: 1100 RVA: 0x0003245D File Offset: 0x0003065D
	private void Awake()
	{
		this.textComponent = base.GetComponent<Text>();
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0007AC48 File Offset: 0x00078E48
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

	// Token: 0x040004FF RID: 1279
	public float cycleTime;

	// Token: 0x04000500 RID: 1280
	public float dutyCycle;

	// Token: 0x04000501 RID: 1281
	private bool isOn;

	// Token: 0x04000502 RID: 1282
	private float lastTime;

	// Token: 0x04000503 RID: 1283
	private Text textComponent;
}
