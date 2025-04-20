using System;
using BoingKit;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A2D RID: 2605
	public class BuilderPieceOrientUp : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x06004153 RID: 16723 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDestroy()
		{
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x001717A4 File Offset: 0x0016F9A4
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

		// Token: 0x06004156 RID: 16726 RVA: 0x001717A4 File Offset: 0x0016F9A4
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

		// Token: 0x06004157 RID: 16727 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPieceDeactivate()
		{
		}

		// Token: 0x04004234 RID: 16948
		[SerializeField]
		private Transform alwaysFaceUp;
	}
}
