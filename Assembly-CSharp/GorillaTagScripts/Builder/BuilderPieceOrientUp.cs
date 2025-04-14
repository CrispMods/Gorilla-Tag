using System;
using BoingKit;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A00 RID: 2560
	public class BuilderPieceOrientUp : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x0600400E RID: 16398 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x00130820 File Offset: 0x0012EA20
		public void OnPiecePlacementDeserialized()
		{
			if (this.alwaysFaceUp != null)
			{
				Quaternion quaternion;
				Quaternion rotation;
				QuaternionUtil.DecomposeSwingTwist(this.alwaysFaceUp.parent.rotation, Vector3.up, out quaternion, out rotation);
				this.alwaysFaceUp.rotation = rotation;
			}
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x00130868 File Offset: 0x0012EA68
		public void OnPieceActivate()
		{
			if (this.alwaysFaceUp != null)
			{
				Quaternion quaternion;
				Quaternion rotation;
				QuaternionUtil.DecomposeSwingTwist(this.alwaysFaceUp.parent.rotation, Vector3.up, out quaternion, out rotation);
				this.alwaysFaceUp.rotation = rotation;
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPieceDeactivate()
		{
		}

		// Token: 0x0400413A RID: 16698
		[SerializeField]
		private Transform alwaysFaceUp;
	}
}
