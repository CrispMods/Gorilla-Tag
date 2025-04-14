using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A7D RID: 2685
	public class WindmillController : MonoBehaviour
	{
		// Token: 0x060042EA RID: 17130 RVA: 0x0013B63F File Offset: 0x0013983F
		private void Awake()
		{
			this._bladesRotation = base.GetComponentInChildren<WindmillBladesController>();
			this._bladesRotation.SetMoveState(true, this._maxSpeed);
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x0013B65F File Offset: 0x0013985F
		private void OnEnable()
		{
			this._startStopButton.GetComponent<Interactable>().InteractableStateChanged.AddListener(new UnityAction<InteractableStateArgs>(this.StartStopStateChanged));
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x0013B682 File Offset: 0x00139882
		private void OnDisable()
		{
			if (this._startStopButton != null)
			{
				this._startStopButton.GetComponent<Interactable>().InteractableStateChanged.RemoveListener(new UnityAction<InteractableStateArgs>(this.StartStopStateChanged));
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x0013B6B4 File Offset: 0x001398B4
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

		// Token: 0x060042EE RID: 17134 RVA: 0x0013B718 File Offset: 0x00139918
		private void Update()
		{
			if (this._toolInteractingWithMe == null)
			{
				this._selectionCylinder.CurrSelectionState = SelectionCylinder.SelectionState.Off;
				return;
			}
			this._selectionCylinder.CurrSelectionState = ((this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown || this._toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay) ? SelectionCylinder.SelectionState.Highlighted : SelectionCylinder.SelectionState.Selected);
		}

		// Token: 0x0400440A RID: 17418
		[SerializeField]
		private GameObject _startStopButton;

		// Token: 0x0400440B RID: 17419
		[SerializeField]
		private float _maxSpeed = 10f;

		// Token: 0x0400440C RID: 17420
		[SerializeField]
		private SelectionCylinder _selectionCylinder;

		// Token: 0x0400440D RID: 17421
		private WindmillBladesController _bladesRotation;

		// Token: 0x0400440E RID: 17422
		private InteractableTool _toolInteractingWithMe;
	}
}
