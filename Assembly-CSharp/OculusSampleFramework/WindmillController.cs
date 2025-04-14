using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A7A RID: 2682
	public class WindmillController : MonoBehaviour
	{
		// Token: 0x060042DE RID: 17118 RVA: 0x0013B077 File Offset: 0x00139277
		private void Awake()
		{
			this._bladesRotation = base.GetComponentInChildren<WindmillBladesController>();
			this._bladesRotation.SetMoveState(true, this._maxSpeed);
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x0013B097 File Offset: 0x00139297
		private void OnEnable()
		{
			this._startStopButton.GetComponent<Interactable>().InteractableStateChanged.AddListener(new UnityAction<InteractableStateArgs>(this.StartStopStateChanged));
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x0013B0BA File Offset: 0x001392BA
		private void OnDisable()
		{
			if (this._startStopButton != null)
			{
				this._startStopButton.GetComponent<Interactable>().InteractableStateChanged.RemoveListener(new UnityAction<InteractableStateArgs>(this.StartStopStateChanged));
			}
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x0013B0EC File Offset: 0x001392EC
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

		// Token: 0x060042E2 RID: 17122 RVA: 0x0013B150 File Offset: 0x00139350
		private void Update()
		{
			if (this._toolInteractingWithMe == null)
			{
				this._selectionCylinder.CurrSelectionState = SelectionCylinder.SelectionState.Off;
				return;
			}
			this._selectionCylinder.CurrSelectionState = ((this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown || this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay) ? SelectionCylinder.SelectionState.Highlighted : SelectionCylinder.SelectionState.Selected);
		}

		// Token: 0x040043F8 RID: 17400
		[SerializeField]
		private GameObject _startStopButton;

		// Token: 0x040043F9 RID: 17401
		[SerializeField]
		private float _maxSpeed = 10f;

		// Token: 0x040043FA RID: 17402
		[SerializeField]
		private SelectionCylinder _selectionCylinder;

		// Token: 0x040043FB RID: 17403
		private WindmillBladesController _bladesRotation;

		// Token: 0x040043FC RID: 17404
		private InteractableTool _toolInteractingWithMe;
	}
}
