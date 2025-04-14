using System;
using System.Collections.Generic;
using GorillaLocomotion.Gameplay;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000980 RID: 2432
	[RequireComponent(typeof(Collider))]
	public class BuilderPieceHandHold : MonoBehaviour, IGorillaGrabable, IBuilderPieceComponent, ITickSystemTick
	{
		// Token: 0x06003B6E RID: 15214 RVA: 0x00111A6D File Offset: 0x0010FC6D
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.myCollider = base.GetComponent<Collider>();
			this.initialized = true;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x00111A8B File Offset: 0x0010FC8B
		public bool IsHandHoldMoving()
		{
			return this.myPiece.IsPieceMoving();
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00111A98 File Offset: 0x0010FC98
		public bool MomentaryGrabOnly()
		{
			return this.forceMomentary;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00111AA0 File Offset: 0x0010FCA0
		public virtual bool CanBeGrabbed(GorillaGrabber grabber)
		{
			return this.myPiece.state == BuilderPiece.State.AttachedAndPlaced && grabber.Player.scale < 0.5f;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x00111AC4 File Offset: 0x0010FCC4
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

		// Token: 0x06003B73 RID: 15219 RVA: 0x00111B31 File Offset: 0x0010FD31
		public void OnGrabReleased(GorillaGrabber grabber)
		{
			this.Initialize();
			this.activeGrabbers.Remove(grabber);
			this.isGrabbed = (this.activeGrabbers.Count < 1);
			grabber.Player.RemoveHandHold(grabber, grabber.IsRightHand);
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06003B74 RID: 15220 RVA: 0x00111B6C File Offset: 0x0010FD6C
		// (set) Token: 0x06003B75 RID: 15221 RVA: 0x00111B74 File Offset: 0x0010FD74
		public bool TickRunning { get; set; }

		// Token: 0x06003B76 RID: 15222 RVA: 0x00111B80 File Offset: 0x0010FD80
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

		// Token: 0x06003B77 RID: 15223 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x00111BF8 File Offset: 0x0010FDF8
		public void OnPieceActivate()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddCallbackTarget(this);
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00111C08 File Offset: 0x0010FE08
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

		// Token: 0x06003B7D RID: 15229 RVA: 0x0001227B File Offset: 0x0001047B
		string IGorillaGrabable.get_name()
		{
			return base.name;
		}

		// Token: 0x04003CA0 RID: 15520
		private bool initialized;

		// Token: 0x04003CA1 RID: 15521
		private Collider myCollider;

		// Token: 0x04003CA2 RID: 15522
		[SerializeField]
		private bool forceMomentary = true;

		// Token: 0x04003CA3 RID: 15523
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04003CA4 RID: 15524
		private List<GorillaGrabber> activeGrabbers = new List<GorillaGrabber>(2);

		// Token: 0x04003CA5 RID: 15525
		private bool isGrabbed;
	}
}
