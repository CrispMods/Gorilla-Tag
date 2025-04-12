using System;
using System.Collections.Generic;
using TagEffects;
using UnityEngine;

// Token: 0x0200022F RID: 559
[DefaultExecutionOrder(10000)]
public class HandEffectsTriggerRegistry : MonoBehaviour
{
	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0003803F File Offset: 0x0003623F
	// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x00038046 File Offset: 0x00036246
	public static HandEffectsTriggerRegistry Instance { get; private set; }

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0003804E File Offset: 0x0003624E
	// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x00038055 File Offset: 0x00036255
	public static bool HasInstance { get; private set; }

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0003805D File Offset: 0x0003625D
	public static void FindInstance()
	{
		HandEffectsTriggerRegistry.Instance = UnityEngine.Object.FindAnyObjectByType<HandEffectsTriggerRegistry>();
		HandEffectsTriggerRegistry.HasInstance = true;
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0003806F File Offset: 0x0003626F
	private void Awake()
	{
		HandEffectsTriggerRegistry.Instance = this;
		HandEffectsTriggerRegistry.HasInstance = true;
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0003807D File Offset: 0x0003627D
	public void Register(IHandEffectsTrigger trigger)
	{
		if (this.triggers.Count < 30)
		{
			this.triggers.Add(trigger);
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0003809A File Offset: 0x0003629A
	public void Unregister(IHandEffectsTrigger trigger)
	{
		this.triggers.Remove(trigger);
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x0009E664 File Offset: 0x0009C864
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

	// Token: 0x0400102B RID: 4139
	private const int MAX_TRIGGERS = 30;

	// Token: 0x0400102C RID: 4140
	private const float COOLDOWN_TIME = 0.5f;

	// Token: 0x0400102D RID: 4141
	private List<IHandEffectsTrigger> triggers = new List<IHandEffectsTrigger>();

	// Token: 0x0400102E RID: 4142
	private float[] triggerTimes = new float[30];

	// Token: 0x0400102F RID: 4143
	private int existingCollisionBits;
}
