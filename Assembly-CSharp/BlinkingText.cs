using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A8 RID: 168
public class BlinkingText : MonoBehaviour
{
	// Token: 0x0600044A RID: 1098 RVA: 0x00019A6C File Offset: 0x00017C6C
	private void Awake()
	{
		this.textComponent = base.GetComponent<Text>();
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00019A7C File Offset: 0x00017C7C
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

	// Token: 0x040004FE RID: 1278
	public float cycleTime;

	// Token: 0x040004FF RID: 1279
	public float dutyCycle;

	// Token: 0x04000500 RID: 1280
	private bool isOn;

	// Token: 0x04000501 RID: 1281
	private float lastTime;

	// Token: 0x04000502 RID: 1282
	private Text textComponent;
}
