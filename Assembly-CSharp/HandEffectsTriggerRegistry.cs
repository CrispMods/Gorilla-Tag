using System;
using System.Collections.Generic;
using TagEffects;
using UnityEngine;

// Token: 0x0200022F RID: 559
[DefaultExecutionOrder(10000)]
public class HandEffectsTriggerRegistry : MonoBehaviour
{
	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0004307B File Offset: 0x0004127B
	// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x00043082 File Offset: 0x00041282
	public static HandEffectsTriggerRegistry Instance { get; private set; }

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0004308A File Offset: 0x0004128A
	// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x00043091 File Offset: 0x00041291
	public static bool HasInstance { get; private set; }

	// Token: 0x06000CC6 RID: 3270 RVA: 0x00043099 File Offset: 0x00041299
	public static void FindInstance()
	{
		HandEffectsTriggerRegistry.Instance = Object.FindAnyObjectByType<HandEffectsTriggerRegistry>();
		HandEffectsTriggerRegistry.HasInstance = true;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x000430AB File Offset: 0x000412AB
	private void Awake()
	{
		HandEffectsTriggerRegistry.Instance = this;
		HandEffectsTriggerRegistry.HasInstance = true;
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000430B9 File Offset: 0x000412B9
	public void Register(IHandEffectsTrigger trigger)
	{
		if (this.triggers.Count < 30)
		{
			this.triggers.Add(trigger);
		}
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x000430D6 File Offset: 0x000412D6
	public void Unregister(IHandEffectsTrigger trigger)
	{
		this.triggers.Remove(trigger);
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x000430E8 File Offset: 0x000412E8
	private void Update()
	{
		int num = 0;
		for (int i = 0; i < this.triggers.Count; i++)
		{
			IHandEffectsTrigger handEffectsTrigger = this.triggers[i];
			int num2 = i * 30;
			for (int j = 0; j < this.triggers.Count; j++)
			{
				if (i != j && Time.time - this.triggerTimes[i] > 0.5f && Time.time - this.triggerTimes[j] > 0.5f)
				{
					IHandEffectsTrigger handEffectsTrigger2 = this.triggers[j];
					if (handEffectsTrigger.InTriggerZone(handEffectsTrigger2))
					{
						int num3 = 1 << num2 + j;
						num |= num3;
						if ((this.existingCollisionBits & num3) == 0)
						{
							handEffectsTrigger.OnTriggerEntered(handEffectsTrigger2);
							handEffectsTrigger2.OnTriggerEntered(handEffectsTrigger);
							this.triggerTimes[i] = (this.triggerTimes[j] = Time.time);
						}
					}
				}
			}
		}
		this.existingCollisionBits = num;
	}

	// Token: 0x0400102A RID: 4138
	private const int MAX_TRIGGERS = 30;

	// Token: 0x0400102B RID: 4139
	private const float COOLDOWN_TIME = 0.5f;

	// Token: 0x0400102C RID: 4140
	private List<IHandEffectsTrigger> triggers = new List<IHandEffectsTrigger>();

	// Token: 0x0400102D RID: 4141
	private float[] triggerTimes = new float[30];

	// Token: 0x0400102E RID: 4142
	private int existingCollisionBits;
}
