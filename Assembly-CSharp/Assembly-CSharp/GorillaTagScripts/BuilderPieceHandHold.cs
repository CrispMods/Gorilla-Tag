using System;
using System.Collections.Generic;
using GorillaLocomotion.Gameplay;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000983 RID: 2435
	[RequireComponent(typeof(Collider))]
	public class BuilderPieceHandHold : MonoBehaviour, IGorillaGrabable, IBuilderPieceComponent, ITickSystemTick
	{
		// Token: 0x06003B7A RID: 15226 RVA: 0x00112035 File Offset: 0x00110235
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.myCollider = base.GetComponent<Collider>();
			this.initialized = true;
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00112053 File Offset: 0x00110253
		public bool IsHandHoldMoving()
		{
			return this.myPiece.IsPieceMoving();
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x00112060 File Offset: 0x00110260
		public bool MomentaryGrabOnly()
		{
			return this.forceMomentary;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x00112068 File Offset: 0x00110268
		public virtual bool CanBeGrabbed(GorillaGrabber grabber)
		{
			return this.myPiece.state == BuilderPiece.State.AttachedAndPlaced && grabber.Player.scale < 0.5f;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0011208C File Offset: 0x0011028C
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

		// Token: 0x06003B7F RID: 15231 RVA: 0x001120F9 File Offset: 0x001102F9
		public void OnGrabReleased(GorillaGrabber grabber)
		{
			this.Initialize();
			this.activeGrabbers.Remove(grabber);
			this.isGrabbed = (this.activeGrabbers.Count < 1);
			grabber.Player.RemoveHandHold(grabber, grabber.IsRightHand);
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06003B80 RID: 15232 RVA: 0x00112134 File Offset: 0x00110334
		// (set) Token: 0x06003B81 RID: 15233 RVA: 0x0011213C File Offset: 0x0011033C
		public bool TickRunning { get; set; }

		// Token: 0x06003B82 RID: 15234 RVA: 0x00112148 File Offset: 0x00110348
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

		// Token: 0x06003B83 RID: 15235 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x001121C0 File Offset: 0x001103C0
		public void OnPieceActivate()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddCallbackTarget(this);
			}
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x001121D0 File Offset: 0x001103D0
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

		// Token: 0x06003B89 RID: 15241 RVA: 0x0001259F File Offset: 0x0001079F
		string IGorillaGrabable.get_name()
		{
			return base.name;
		}

		// Token: 0x04003CB2 RID: 15538
		private bool initialized;

		// Token: 0x04003CB3 RID: 15539
		private Collider myCollider;

		// Token: 0x04003CB4 RID: 15540
		[SerializeField]
		private bool forceMomentary = true;

		// Token: 0x04003CB5 RID: 15541
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04003CB6 RID: 15542
		private List<GorillaGrabber> activeGrabbers = new List<GorillaGrabber>(2);

		// Token: 0x04003CB7 RID: 15543
		private bool isGrabbed;
	}
}
