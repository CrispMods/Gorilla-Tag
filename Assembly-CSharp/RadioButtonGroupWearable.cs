using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000182 RID: 386
public class RadioButtonGroupWearable : MonoBehaviour, ISpawnable
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00036D1F File Offset: 0x00034F1F
	// (set) Token: 0x060009AA RID: 2474 RVA: 0x00036D27 File Offset: 0x00034F27
	public bool IsSpawned { get; set; }

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060009AB RID: 2475 RVA: 0x00036D30 File Offset: 0x00034F30
	// (set) Token: 0x060009AC RID: 2476 RVA: 0x00036D38 File Offset: 0x00034F38
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060009AD RID: 2477 RVA: 0x000926B4 File Offset: 0x000908B4
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

	// Token: 0x060009AE RID: 2478 RVA: 0x00036D41 File Offset: 0x00034F41
	private void OnEnable()
	{
		this.SharedRefreshState();
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00036D49 File Offset: 0x00034F49
	private int GetCurrentState()
	{
		return GTBitOps.ReadBits(this.ownerRig.WearablePackedStates, this.stateBitsWriteInfo.index, this.stateBitsWriteInfo.valueMask);
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00036D71 File Offset: 0x00034F71
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

	// Token: 0x060009B1 RID: 2481 RVA: 0x00092714 File Offset: 0x00090914
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

	// Token: 0x060009B2 RID: 2482 RVA: 0x00092784 File Offset: 0x00090984
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

	// Token: 0x060009B3 RID: 2483 RVA: 0x00036D95 File Offset: 0x00034F95
	public void OnSpawn(VRRig rig)
	{
		this.ownerRig = rig;
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x04000B97 RID: 2967
	[SerializeField]
	private bool AllowSelectNone = true;

	// Token: 0x04000B98 RID: 2968
	[SerializeField]
	private GorillaPressableButton[] buttons;

	// Token: 0x04000B99 RID: 2969
	[SerializeField]
	private UnityEvent<int> OnSelectionChanged;

	// Token: 0x04000B9A RID: 2970
	[Tooltip("This is to determine what bit to change in VRRig.WearablesPackedStates.")]
	[SerializeField]
	private VRRig.WearablePackedStateSlots assignedSlot = VRRig.WearablePackedStateSlots.Pants1;

	// Token: 0x04000B9B RID: 2971
	private int lastReportedState;

	// Token: 0x04000B9C RID: 2972
	private VRRig ownerRig;

	// Token: 0x04000B9D RID: 2973
	private GTBitOps.BitWriteInfo stateBitsWriteInfo;
}
