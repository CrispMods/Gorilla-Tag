using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000AA7 RID: 2727
	public class WindmillController : MonoBehaviour
	{
		// Token: 0x06004423 RID: 17443 RVA: 0x0005C70B File Offset: 0x0005A90B
		private void Awake()
		{
			this._bladesRotation = base.GetComponentInChildren<WindmillBladesController>();
			this._bladesRotation.SetMoveState(true, this._maxSpeed);
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0005C72B File Offset: 0x0005A92B
		private void OnEnable()
		{
			this._startStopButton.GetComponent<Interactable>().InteractableStateChanged.AddListener(new UnityAction<InteractableStateArgs>(this.StartStopStateChanged));
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x0005C74E File Offset: 0x0005A94E
		private void OnDisable()
		{
			if (this._startStopButton != null)
			{
				this._startStopButton.GetComponent<Interactable>().InteractableStateChanged.RemoveListener(new UnityAction<InteractableStateArgs>(this.StartStopStateChanged));
			}
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x0017A1AC File Offset: 0x001783AC
		private void StartStopStateChanged(InteractableStateArgs obj)
		{
			if (obj.NewInteractableState == InteractableState.ActionState)
			{
				if (this._bladesRotation.IsMoving)
				{
					this._bladesRotation.SetMoveState(false, 0f);
				}
				else
				{
					this._bladesRotation.SetMoveState(true, this._maxSpeed);
				}
			}
			this._toolInteractingWithMe = ((obj.NewInteractableState > InteractableState.Default) ? obj.Tool : null);
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x0017A210 File Offset: 0x00178410
		private void Update()
		{
			if (this._toolInteractingWithMe == null)
			{
				this._selectionCylinder.CurrSelectionState = SelectionCylinder.SelectionState.Off;
				return;
			}
			this._selectionCylinder.CurrSelectionState = ((this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown || this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay) ? SelectionCylinder.SelectionState.Highlighted : SelectionCylinder.SelectionState.Selected);
		}

		// Token: 0x040044F2 RID: 17650
		[SerializeField]
		private GameObject _startStopButton;

		// Token: 0x040044F3 RID: 17651
		[SerializeField]
		private float _maxSpeed = 10f;

		// Token: 0x040044F4 RID: 17652
		[SerializeField]
		private SelectionCylinder _selectionCylinder;

		// Token: 0x040044F5 RID: 17653
		private WindmillBladesController _bladesRotation;

		// Token: 0x040044F6 RID: 17654
		private InteractableTool _toolInteractingWithMe;
	}
}
