using System;
using System.Collections.Generic;
using GorillaLocomotion.Gameplay;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A6 RID: 2470
	[RequireComponent(typeof(Collider))]
	public class BuilderPieceHandHold : MonoBehaviour, IGorillaGrabable, IBuilderPieceComponent, ITickSystemTick
	{
		// Token: 0x06003C86 RID: 15494 RVA: 0x00057822 File Offset: 0x00055A22
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.myCollider = base.GetComponent<Collider>();
			this.initialized = true;
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x00057840 File Offset: 0x00055A40
		public bool IsHandHoldMoving()
		{
			return this.myPiece.IsPieceMoving();
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x0005784D File Offset: 0x00055A4D
		public bool MomentaryGrabOnly()
		{
			return this.forceMomentary;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x00057855 File Offset: 0x00055A55
		public virtual bool CanBeGrabbed(GorillaGrabber grabber)
		{
			return this.myPiece.state == BuilderPiece.State.AttachedAndPlaced && grabber.Player.scale < 0.5f;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x00154CE4 File Offset: 0x00152EE4
		public void OnGrabbed(GorillaGrabber grabber, out Transform grabbedTransform, out Vector3 localGrabbedPosition)
		{
			this.Initialize();
			grabbedTransform = base.transform;
			Vector3 position = grabber.transform.position;
			localGrabbedPosition = base.transform.InverseTransformPoint(position);
			this.activeGrabbers.Add(grabber);
			this.isGrabbed = true;
			Vector3 vector;
			grabber.Player.AddHandHold(base.transform, localGrabbedPosition, grabber, grabber.IsRightHand, false, out vector);
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x00057878 File Offset: 0x00055A78
		public void OnGrabReleased(GorillaGrabber grabber)
		{
			this.Initialize();
			this.activeGrabbers.Remove(grabber);
			this.isGrabbed = (this.activeGrabbers.Count < 1);
			grabber.Player.RemoveHandHold(grabber, grabber.IsRightHand);
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06003C8C RID: 15500 RVA: 0x000578B3 File Offset: 0x00055AB3
		// (set) Token: 0x06003C8D RID: 15501 RVA: 0x000578BB File Offset: 0x00055ABB
		public bool TickRunning { get; set; }

		// Token: 0x06003C8E RID: 15502 RVA: 0x00154D54 File Offset: 0x00152F54
		public void Tick()
		{
			if (!this.isGrabbed)
			{
				return;
			}
			foreach (GorillaGrabber gorillaGrabber in this.activeGrabbers)
			{
				if (gorillaGrabber != null && gorillaGrabber.Player.scale > 0.5f)
				{
					this.OnGrabReleased(gorillaGrabber);
				}
			}
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x000578C4 File Offset: 0x00055AC4
		public void OnPieceActivate()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddCallbackTarget(this);
			}
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00154DCC File Offset: 0x00152FCC
		public void OnPieceDeactivate()
		{
			if (this.TickRunning)
			{
				TickSystem<object>.RemoveCallbackTarget(this);
			}
			foreach (GorillaGrabber grabber in this.activeGrabbers)
			{
				this.OnGrabReleased(grabber);
			}
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x0003261E File Offset: 0x0003081E
		string IGorillaGrabable.get_name()
		{
			return base.name;
		}

		// Token: 0x04003D7A RID: 15738
		private bool initialized;

		// Token: 0x04003D7B RID: 15739
		private Collider myCollider;

		// Token: 0x04003D7C RID: 15740
		[SerializeField]
		private bool forceMomentary = true;

		// Token: 0x04003D7D RID: 15741
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04003D7E RID: 15742
		private List<GorillaGrabber> activeGrabbers = new List<GorillaGrabber>(2);

		// Token: 0x04003D7F RID: 15743
		private bool isGrabbed;
	}
}
