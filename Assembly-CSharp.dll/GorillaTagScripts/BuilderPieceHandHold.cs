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
		// Token: 0x06003B7A RID: 15226 RVA: 0x00055F8B File Offset: 0x0005418B
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.myCollider = base.GetComponent<Collider>();
			this.initialized = true;
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00055FA9 File Offset: 0x000541A9
		public bool IsHandHoldMoving()
		{
			return this.myPiece.IsPieceMoving();
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x00055FB6 File Offset: 0x000541B6
		public bool MomentaryGrabOnly()
		{
			return this.forceMomentary;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x00055FBE File Offset: 0x000541BE
		public virtual bool CanBeGrabbed(GorillaGrabber grabber)
		{
			return this.myPiece.state == BuilderPiece.State.AttachedAndPlaced && grabber.Player.scale < 0.5f;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0014ECFC File Offset: 0x0014CEFC
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

		// Token: 0x06003B7F RID: 15231 RVA: 0x00055FE1 File Offset: 0x000541E1
		public void OnGrabReleased(GorillaGrabber grabber)
		{
			this.Initialize();
			this.activeGrabbers.Remove(grabber);
			this.isGrabbed = (this.activeGrabbers.Count < 1);
			grabber.Player.RemoveHandHold(grabber, grabber.IsRightHand);
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06003B80 RID: 15232 RVA: 0x0005601C File Offset: 0x0005421C
		// (set) Token: 0x06003B81 RID: 15233 RVA: 0x00056024 File Offset: 0x00054224
		public bool TickRunning { get; set; }

		// Token: 0x06003B82 RID: 15234 RVA: 0x0014ED6C File Offset: 0x0014CF6C
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

		// Token: 0x06003B83 RID: 15235 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0005602D File Offset: 0x0005422D
		public void OnPieceActivate()
		{
			if (!this.TickRunning)
			{
				TickSystem<object>.AddCallbackTarget(this);
			}
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0014EDE4 File Offset: 0x0014CFE4
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

		// Token: 0x06003B89 RID: 15241 RVA: 0x000314EB File Offset: 0x0002F6EB
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
