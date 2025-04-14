using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000840 RID: 2112
public class CompositeTriggerEvents : MonoBehaviour
{
	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06003372 RID: 13170 RVA: 0x000F5AE2 File Offset: 0x000F3CE2
	private Dictionary<Collider, int> CollderMasks
	{
		get
		{
			return this.overlapMask;
		}
	}

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06003373 RID: 13171 RVA: 0x000F5AEC File Offset: 0x000F3CEC
	// (remove) Token: 0x06003374 RID: 13172 RVA: 0x000F5B24 File Offset: 0x000F3D24
	public event CompositeTriggerEvents.TriggerEvent CompositeTriggerEnter;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06003375 RID: 13173 RVA: 0x000F5B5C File Offset: 0x000F3D5C
	// (remove) Token: 0x06003376 RID: 13174 RVA: 0x000F5B94 File Offset: 0x000F3D94
	public event CompositeTriggerEvents.TriggerEvent CompositeTriggerExit;

	// Token: 0x06003377 RID: 13175 RVA: 0x000F5BCC File Offset: 0x000F3DCC
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

	// Token: 0x06003378 RID: 13176 RVA: 0x000F5C6C File Offset: 0x000F3E6C
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

	// Token: 0x06003379 RID: 13177 RVA: 0x000F5CE0 File Offset: 0x000F3EE0
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

	// Token: 0x0600337A RID: 13178 RVA: 0x000F5D48 File Offset: 0x000F3F48
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

	// Token: 0x0600337B RID: 13179 RVA: 0x000F5DA4 File Offset: 0x000F3FA4
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

	// Token: 0x0600337C RID: 13180 RVA: 0x000F5DE3 File Offset: 0x000F3FE3
	public void CompositeTriggerEnterReceiver(Collider other)
	{
		CompositeTriggerEvents.TriggerEvent compositeTriggerEnter = this.CompositeTriggerEnter;
		if (compositeTriggerEnter == null)
		{
			return;
		}
		compositeTriggerEnter(other);
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x000F5DF6 File Offset: 0x000F3FF6
	public void CompositeTriggerExitReceiver(Collider other)
	{
		CompositeTriggerEvents.TriggerEvent compositeTriggerExit = this.CompositeTriggerExit;
		if (compositeTriggerExit == null)
		{
			return;
		}
		compositeTriggerExit(other);
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x000F5E09 File Offset: 0x000F4009
	private bool TestMaskIndex(int mask, int index)
	{
		return (mask & 1 << index) != 0;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x000F5E16 File Offset: 0x000F4016
	private int SetMaskIndexTrue(int mask, int index)
	{
		return mask | 1 << index;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x000F5E20 File Offset: 0x000F4020
	private int SetMaskIndexFalse(int mask, int index)
	{
		return mask & ~(1 << index);
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x000F5E2C File Offset: 0x000F402C
	private string MaskToString(int mask)
	{
		string text = "";
		for (int i = 31; i >= 0; i--)
		{
			text += (this.TestMaskIndex(mask, i) ? "1" : "0");
		}
		return text;
	}

	// Token: 0x040036BE RID: 14014
	[SerializeField]
	private List<Collider> individualTriggerColliders = new List<Collider>();

	// Token: 0x040036BF RID: 14015
	private List<TriggerEventNotifier> triggerEventNotifiers = new List<TriggerEventNotifier>();

	// Token: 0x040036C0 RID: 14016
	private Dictionary<Collider, int> overlapMask = new Dictionary<Collider, int>();

	// Token: 0x02000841 RID: 2113
	// (Invoke) Token: 0x06003384 RID: 13188
	public delegate void TriggerEvent(Collider collider);
}
