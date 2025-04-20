using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200085A RID: 2138
public class CompositeTriggerEvents : MonoBehaviour
{
	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x0600342D RID: 13357 RVA: 0x0005250C File Offset: 0x0005070C
	private Dictionary<Collider, int> CollderMasks
	{
		get
		{
			return this.overlapMask;
		}
	}

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x0600342E RID: 13358 RVA: 0x0013D520 File Offset: 0x0013B720
	// (remove) Token: 0x0600342F RID: 13359 RVA: 0x0013D558 File Offset: 0x0013B758
	public event CompositeTriggerEvents.TriggerEvent CompositeTriggerEnter;

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06003430 RID: 13360 RVA: 0x0013D590 File Offset: 0x0013B790
	// (remove) Token: 0x06003431 RID: 13361 RVA: 0x0013D5C8 File Offset: 0x0013B7C8
	public event CompositeTriggerEvents.TriggerEvent CompositeTriggerExit;

	// Token: 0x06003432 RID: 13362 RVA: 0x0013D600 File Offset: 0x0013B800
	private void Awake()
	{
		if (this.individualTriggerColliders.Count > 32)
		{
			Debug.LogError("The max number of triggers was exceeded in this composite trigger event sender on GameObject: " + base.gameObject.name + ".");
		}
		for (int i = 0; i < this.individualTriggerColliders.Count; i++)
		{
			TriggerEventNotifier triggerEventNotifier = this.individualTriggerColliders[i].gameObject.AddComponent<TriggerEventNotifier>();
			triggerEventNotifier.maskIndex = i;
			triggerEventNotifier.TriggerEnterEvent += this.TriggerEnterReceiver;
			triggerEventNotifier.TriggerExitEvent += this.TriggerExitReceiver;
			this.triggerEventNotifiers.Add(triggerEventNotifier);
		}
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x0013D6A0 File Offset: 0x0013B8A0
	private void OnDestroy()
	{
		for (int i = 0; i < this.triggerEventNotifiers.Count; i++)
		{
			if (this.triggerEventNotifiers[i] != null)
			{
				this.triggerEventNotifiers[i].TriggerEnterEvent -= this.TriggerEnterReceiver;
				this.triggerEventNotifiers[i].TriggerExitEvent -= this.TriggerExitReceiver;
			}
		}
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x0013D714 File Offset: 0x0013B914
	public void TriggerEnterReceiver(TriggerEventNotifier notifier, Collider other)
	{
		int num;
		if (this.overlapMask.TryGetValue(other, out num))
		{
			num = this.SetMaskIndexTrue(num, notifier.maskIndex);
			this.overlapMask[other] = num;
			return;
		}
		int value = this.SetMaskIndexTrue(0, notifier.maskIndex);
		this.overlapMask.Add(other, value);
		CompositeTriggerEvents.TriggerEvent compositeTriggerEnter = this.CompositeTriggerEnter;
		if (compositeTriggerEnter == null)
		{
			return;
		}
		compositeTriggerEnter(other);
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x0013D77C File Offset: 0x0013B97C
	public void TriggerExitReceiver(TriggerEventNotifier notifier, Collider other)
	{
		int num;
		if (this.overlapMask.TryGetValue(other, out num))
		{
			num = this.SetMaskIndexFalse(num, notifier.maskIndex);
			if (num == 0)
			{
				this.overlapMask.Remove(other);
				CompositeTriggerEvents.TriggerEvent compositeTriggerExit = this.CompositeTriggerExit;
				if (compositeTriggerExit == null)
				{
					return;
				}
				compositeTriggerExit(other);
				return;
			}
			else
			{
				this.overlapMask[other] = num;
			}
		}
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x0013D7D8 File Offset: 0x0013B9D8
	public void ResetColliderMask(Collider other)
	{
		int num;
		if (this.overlapMask.TryGetValue(other, out num))
		{
			if (num != 0)
			{
				CompositeTriggerEvents.TriggerEvent compositeTriggerExit = this.CompositeTriggerExit;
				if (compositeTriggerExit != null)
				{
					compositeTriggerExit(other);
				}
			}
			this.overlapMask.Remove(other);
		}
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x00052514 File Offset: 0x00050714
	public void CompositeTriggerEnterReceiver(Collider other)
	{
		CompositeTriggerEvents.TriggerEvent compositeTriggerEnter = this.CompositeTriggerEnter;
		if (compositeTriggerEnter == null)
		{
			return;
		}
		compositeTriggerEnter(other);
	}

	// Token: 0x06003438 RID: 13368 RVA: 0x00052527 File Offset: 0x00050727
	public void CompositeTriggerExitReceiver(Collider other)
	{
		CompositeTriggerEvents.TriggerEvent compositeTriggerExit = this.CompositeTriggerExit;
		if (compositeTriggerExit == null)
		{
			return;
		}
		compositeTriggerExit(other);
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x0005253A File Offset: 0x0005073A
	private bool TestMaskIndex(int mask, int index)
	{
		return (mask & 1 << index) != 0;
	}

	// Token: 0x0600343A RID: 13370 RVA: 0x00052547 File Offset: 0x00050747
	private int SetMaskIndexTrue(int mask, int index)
	{
		return mask | 1 << index;
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x00052551 File Offset: 0x00050751
	private int SetMaskIndexFalse(int mask, int index)
	{
		return mask & ~(1 << index);
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x0013D818 File Offset: 0x0013BA18
	private string MaskToString(int mask)
	{
		string text = "";
		for (int i = 31; i >= 0; i--)
		{
			text += (this.TestMaskIndex(mask, i) ? "1" : "0");
		}
		return text;
	}

	// Token: 0x0400377A RID: 14202
	[SerializeField]
	private List<Collider> individualTriggerColliders = new List<Collider>();

	// Token: 0x0400377B RID: 14203
	private List<TriggerEventNotifier> triggerEventNotifiers = new List<TriggerEventNotifier>();

	// Token: 0x0400377C RID: 14204
	private Dictionary<Collider, int> overlapMask = new Dictionary<Collider, int>();

	// Token: 0x0200085B RID: 2139
	// (Invoke) Token: 0x0600343F RID: 13375
	public delegate void TriggerEvent(Collider collider);
}
