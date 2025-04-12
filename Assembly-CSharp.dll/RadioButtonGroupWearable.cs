using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000177 RID: 375
public class RadioButtonGroupWearable : MonoBehaviour, ISpawnable
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x00035A5F File Offset: 0x00033C5F
	// (set) Token: 0x06000960 RID: 2400 RVA: 0x00035A67 File Offset: 0x00033C67
	public bool IsSpawned { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000961 RID: 2401 RVA: 0x00035A70 File Offset: 0x00033C70
	// (set) Token: 0x06000962 RID: 2402 RVA: 0x00035A78 File Offset: 0x00033C78
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000963 RID: 2403 RVA: 0x0008FDC0 File Offset: 0x0008DFC0
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

	// Token: 0x06000964 RID: 2404 RVA: 0x00035A81 File Offset: 0x00033C81
	private void OnEnable()
	{
		this.SharedRefreshState();
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00035A89 File Offset: 0x00033C89
	private int GetCurrentState()
	{
		return GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00035AB1 File Offset: 0x00033CB1
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

	// Token: 0x06000967 RID: 2407 RVA: 0x0008FE20 File Offset: 0x0008E020
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

	// Token: 0x06000968 RID: 2408 RVA: 0x0008FE90 File Offset: 0x0008E090
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

	// Token: 0x06000969 RID: 2409 RVA: 0x00035AD5 File Offset: 0x00033CD5
	public void OnSpawn(VRRig rig)
	{
		this.ownerRig = rig;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnDespawn()
	{
	}

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private bool AllowSelectNone = true;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	private GorillaPressableButton[] buttons;

	// Token: 0x04000B54 RID: 2900
	[SerializeField]
	private UnityEvent<int> OnSelectionChanged;

	// Token: 0x04000B55 RID: 2901
	[Tooltip("This is to determine what bit to change in VRRig.WearablesPackedStates.")]
	[SerializeField]
	private VRRig.WearablePackedStateSlots assignedSlot = VRRig.WearablePackedStateSlots.Pants1;

	// Token: 0x04000B56 RID: 2902
	private int lastReportedState;

	// Token: 0x04000B57 RID: 2903
	private VRRig ownerRig;

	// Token: 0x04000B58 RID: 2904
	private GTBitOps.BitWriteInfo stateBitsWriteInfo;
}
