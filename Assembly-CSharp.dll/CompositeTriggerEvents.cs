using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000843 RID: 2115
public class CompositeTriggerEvents : MonoBehaviour
{
	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x0600337E RID: 13182 RVA: 0x000510FE File Offset: 0x0004F2FE
	private Dictionary<Collider, int> CollderMasks
	{
		get
		{
			return this.overlapMask;
		}
	}

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x0600337F RID: 13183 RVA: 0x00137FC8 File Offset: 0x001361C8
	// (remove) Token: 0x06003380 RID: 13184 RVA: 0x00138000 File Offset: 0x00136200
	public event CompositeTriggerEvents.TriggerEvent CompositeTriggerEnter;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06003381 RID: 13185 RVA: 0x00138038 File Offset: 0x00136238
	// (remove) Token: 0x06003382 RID: 13186 RVA: 0x00138070 File Offset: 0x00136270
	public event CompositeTriggerEvents.TriggerEvent CompositeTriggerExit;

	// Token: 0x06003383 RID: 13187 RVA: 0x001380A8 File Offset: 0x001362A8
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

	// Token: 0x06003384 RID: 13188 RVA: 0x00138148 File Offset: 0x00136348
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

	// Token: 0x06003385 RID: 13189 RVA: 0x001381BC File Offset: 0x001363BC
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

	// Token: 0x06003386 RID: 13190 RVA: 0x00138224 File Offset: 0x00136424
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

	// Token: 0x06003387 RID: 13191 RVA: 0x00138280 File Offset: 0x00136480
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

	// Token: 0x06003388 RID: 13192 RVA: 0x00051106 File Offset: 0x0004F306
	public void CompositeTriggerEnterReceiver(Collider other)
	{
		CompositeTriggerEvents.TriggerEvent compositeTriggerEnter = this.CompositeTriggerEnter;
		if (compositeTriggerEnter == null)
		{
			return;
		}
		compositeTriggerEnter(other);
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x00051119 File Offset: 0x0004F319
	public void CompositeTriggerExitReceiver(Collider other)
	{
		CompositeTriggerEvents.TriggerEvent compositeTriggerExit = this.CompositeTriggerExit;
		if (compositeTriggerExit == null)
		{
			return;
		}
		compositeTriggerExit(other);
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x0005112C File Offset: 0x0004F32C
	private bool TestMaskIndex(int mask, int index)
	{
		return (mask & 1 << index) != 0;
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x00051139 File Offset: 0x0004F339
	private int SetMaskIndexTrue(int mask, int index)
	{
		return mask | 1 << index;
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x00051143 File Offset: 0x0004F343
	private int SetMaskIndexFalse(int mask, int index)
	{
		return mask & ~(1 << index);
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x001382C0 File Offset: 0x001364C0
	private string MaskToString(int mask)
	{
		string text = "";
		for (int i = 31; i >= 0; i--)
		{
			text += (this.TestMaskIndex(mask, i) ? "1" : "0");
		}
		return text;
	}

	// Token: 0x040036D0 RID: 14032
	[SerializeField]
	private List<Collider> individualTriggerColliders = new List<Collider>();

	// Token: 0x040036D1 RID: 14033
	private List<TriggerEventNotifier> triggerEventNotifiers = new List<TriggerEventNotifier>();

	// Token: 0x040036D2 RID: 14034
	private Dictionary<Collider, int> overlapMask = new Dictionary<Collider, int>();

	// Token: 0x02000844 RID: 2116
	// (Invoke) Token: 0x06003390 RID: 13200
	public delegate void TriggerEvent(Collider collider);
}
