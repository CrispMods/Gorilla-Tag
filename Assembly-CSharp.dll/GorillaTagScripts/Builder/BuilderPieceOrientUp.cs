﻿using System;
using BoingKit;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A03 RID: 2563
	public class BuilderPieceOrientUp : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x0600401A RID: 16410 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPieceCreate(int pieceType, int pieceId)
		{
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPieceDestroy()
		{
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x0016A920 File Offset: 0x00168B20
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

		// Token: 0x0600401D RID: 16413 RVA: 0x0016A920 File Offset: 0x00168B20
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

		// Token: 0x0600401E RID: 16414 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPieceDeactivate()
		{
		}

		// Token: 0x0400414C RID: 16716
		[SerializeField]
		private Transform alwaysFaceUp;
	}
}
