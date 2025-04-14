using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000177 RID: 375
public class RadioButtonGroupWearable : MonoBehaviour, ISpawnable
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600095D RID: 2397 RVA: 0x00032208 File Offset: 0x00030408
	// (set) Token: 0x0600095E RID: 2398 RVA: 0x00032210 File Offset: 0x00030410
	public bool IsSpawned { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x00032219 File Offset: 0x00030419
	// (set) Token: 0x06000960 RID: 2400 RVA: 0x00032221 File Offset: 0x00030421
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000961 RID: 2401 RVA: 0x0003222C File Offset: 0x0003042C
	private void Start()
	{
		this.stateBitsWriteInfo = VRRig.WearablePackedStatesBitWriteInfos[(int)this.assignedSlot];
		if (!this.ownerRig.isLocal)
		{
			GorillaPressableButton[] array = this.buttons;
			for (int i = 0; i < array.Length; i++)
			{
				Collider component = array[i].GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0003228A File Offset: 0x0003048A
	private void OnEnable()
	{
		this.SharedRefreshState();
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x00032292 File Offset: 0x00030492
	private int GetCurrentState()
	{
		return GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x000322BA File Offset: 0x000304BA
	private void Update()
	{
		if (this.ownerRig.isLocal)
		{
			return;
		}
		if (this.lastReportedState != this.GetCurrentState())
		{
			this.SharedRefreshState();
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x000322E0 File Offset: 0x000304E0
	public void SharedRefreshState()
	{
		int currentState = this.GetCurrentState();
		int num = this.AllowSelectNone ? (currentState - 1) : currentState;
		for (int i = 0; i < this.buttons.Length; i++)
		{
			this.buttons[i].isOn = (num == i);
			this.buttons[i].UpdateColor();
		}
		if (this.lastReportedState != currentState)
		{
			this.lastReportedState = currentState;
			this.OnSelectionChanged.Invoke(currentState);
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00032350 File Offset: 0x00030550
	public void OnPress(GorillaPressableButton button)
	{
		int currentState = this.GetCurrentState();
		int num = Array.IndexOf<GorillaPressableButton>(this.buttons, button);
		if (this.AllowSelectNone)
		{
			num++;
		}
		int value = num;
		if (this.AllowSelectNone && num == currentState)
		{
			value = 0;
		}
		this.ownerRig.WearablePackedStates = GTBitOps.WriteBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo, value);
		this.SharedRefreshState();
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x000323B5 File Offset: 0x000305B5
	public void OnSpawn(VRRig rig)
	{
		this.ownerRig = rig;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private bool AllowSelectNone = true;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private GorillaPressableButton[] buttons;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	private UnityEvent<int> OnSelectionChanged;

	// Token: 0x04000B54 RID: 2900
	[Tooltip("This is to determine what bit to change in VRRig.WearablesPackedStates.")]
	[SerializeField]
	private VRRig.WearablePackedStateSlots assignedSlot = VRRig.WearablePackedStateSlots.Pants1;

	// Token: 0x04000B55 RID: 2901
	private int lastReportedState;

	// Token: 0x04000B56 RID: 2902
	private VRRig ownerRig;

	// Token: 0x04000B57 RID: 2903
	private GTBitOps.BitWriteInfo stateBitsWriteInfo;
}
