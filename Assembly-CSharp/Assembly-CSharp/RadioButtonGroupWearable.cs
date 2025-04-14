using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000177 RID: 375
public class RadioButtonGroupWearable : MonoBehaviour, ISpawnable
{
	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x0003252C File Offset: 0x0003072C
	// (set) Token: 0x06000960 RID: 2400 RVA: 0x00032534 File Offset: 0x00030734
	public bool IsSpawned { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000961 RID: 2401 RVA: 0x0003253D File Offset: 0x0003073D
	// (set) Token: 0x06000962 RID: 2402 RVA: 0x00032545 File Offset: 0x00030745
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x06000963 RID: 2403 RVA: 0x00032550 File Offset: 0x00030750
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

	// Token: 0x06000964 RID: 2404 RVA: 0x000325AE File Offset: 0x000307AE
	private void OnEnable()
	{
		this.SharedRefreshState();
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x000325B6 File Offset: 0x000307B6
	private int GetCurrentState()
	{
		return GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x000325DE File Offset: 0x000307DE
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

	// Token: 0x06000967 RID: 2407 RVA: 0x00032604 File Offset: 0x00030804
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

	// Token: 0x06000968 RID: 2408 RVA: 0x00032674 File Offset: 0x00030874
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

	// Token: 0x06000969 RID: 2409 RVA: 0x000326D9 File Offset: 0x000308D9
	public void OnSpawn(VRRig rig)
	{
		this.ownerRig = rig;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x000023F4 File Offset: 0x000005F4
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
