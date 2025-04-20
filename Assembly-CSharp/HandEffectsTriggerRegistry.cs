using System;
using System.Collections.Generic;
using TagEffects;
using UnityEngine;

// Token: 0x0200023A RID: 570
[DefaultExecutionOrder(10000)]
public class HandEffectsTriggerRegistry : MonoBehaviour
{
	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000D0D RID: 3341 RVA: 0x000392FF File Offset: 0x000374FF
	// (set) Token: 0x06000D0E RID: 3342 RVA: 0x00039306 File Offset: 0x00037506
	public static HandEffectsTriggerRegistry Instance { get; private set; }

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0003930E File Offset: 0x0003750E
	// (set) Token: 0x06000D10 RID: 3344 RVA: 0x00039315 File Offset: 0x00037515
	public static bool HasInstance { get; private set; }

	// Token: 0x06000D11 RID: 3345 RVA: 0x0003931D File Offset: 0x0003751D
	public static void FindInstance()
	{
		HandEffectsTriggerRegistry.Instance = UnityEngine.Object.FindAnyObjectByType<HandEffectsTriggerRegistry>();
		HandEffectsTriggerRegistry.HasInstance = true;
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0003932F File Offset: 0x0003752F
	private void Awake()
	{
		HandEffectsTriggerRegistry.Instance = this;
		HandEffectsTriggerRegistry.HasInstance = true;
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x0003933D File Offset: 0x0003753D
	public void Register(IHandEffectsTrigger trigger)
	{
		if (this.triggers.Count < 30)
		{
			this.triggers.Add(trigger);
		}
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x0003935A File Offset: 0x0003755A
	public void Unregister(IHandEffectsTrigger trigger)
	{
		this.triggers.Remove(trigger);
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x000A0EF0 File Offset: 0x0009F0F0
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

	// Token: 0x04001070 RID: 4208
	private const int MAX_TRIGGERS = 30;

	// Token: 0x04001071 RID: 4209
	private const float COOLDOWN_TIME = 0.5f;

	// Token: 0x04001072 RID: 4210
	private List<IHandEffectsTrigger> triggers = new List<IHandEffectsTrigger>();

	// Token: 0x04001073 RID: 4211
	private float[] triggerTimes = new float[30];

	// Token: 0x04001074 RID: 4212
	private int existingCollisionBits;
}
