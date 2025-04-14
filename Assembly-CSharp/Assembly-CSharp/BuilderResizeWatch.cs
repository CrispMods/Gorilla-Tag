﻿using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004E1 RID: 1249
public class BuilderResizeWatch : MonoBehaviour
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001E63 RID: 7779 RVA: 0x00098D84 File Offset: 0x00096F84
	public int SizeLayerMaskGrow
	{
		get
		{
			int num = 0;
			if (this.growSettings.affectLayerA)
			{
				num |= 1;
			}
			if (this.growSettings.affectLayerB)
			{
				num |= 2;
			}
			if (this.growSettings.affectLayerC)
			{
				num |= 4;
			}
			if (this.growSettings.affectLayerD)
			{
				num |= 8;
			}
			return num;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001E64 RID: 7780 RVA: 0x00098DD8 File Offset: 0x00096FD8
	public int SizeLayerMaskShrink
	{
		get
		{
			int num = 0;
			if (this.shrinkSettings.affectLayerA)
			{
				num |= 1;
			}
			if (this.shrinkSettings.affectLayerB)
			{
				num |= 2;
			}
			if (this.shrinkSettings.affectLayerC)
			{
				num |= 4;
			}
			if (this.shrinkSettings.affectLayerD)
			{
				num |= 8;
			}
			return num;
		}
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x00098E2C File Offset: 0x0009702C
	private void Start()
	{
		if (this.enlargeButton != null)
		{
			this.enlargeButton.onPressButton.AddListener(new UnityAction(this.OnEnlargeButtonPressed));
		}
		if (this.shrinkButton != null)
		{
			this.shrinkButton.onPressButton.AddListener(new UnityAction(this.OnShrinkButtonPressed));
		}
		this.ownerRig = base.GetComponentInParent<VRRig>();
		this.enableDist = GTPlayer.Instance.bodyCollider.height;
		this.enableDistSq = this.enableDist * this.enableDist;
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x00098EC4 File Offset: 0x000970C4
	private void OnDestroy()
	{
		if (this.enlargeButton != null)
		{
			this.enlargeButton.onPressButton.RemoveListener(new UnityAction(this.OnEnlargeButtonPressed));
		}
		if (this.shrinkButton != null)
		{
			this.shrinkButton.onPressButton.RemoveListener(new UnityAction(this.OnShrinkButtonPressed));
		}
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x00098F28 File Offset: 0x00097128
	private void OnEnlargeButtonPressed()
	{
		if (this.sizeManager == null)
		{
			if (this.ownerRig == null)
			{
				Debug.LogWarning("Builder resize watch has no owner rig");
				return;
			}
			this.sizeManager = this.ownerRig.sizeManager;
		}
		if (this.sizeManager != null && this.sizeManager.currentSizeLayerMaskValue != this.SizeLayerMaskGrow && !this.updateCollision)
		{
			this.DisableCollisionWithPieces();
			this.sizeManager.currentSizeLayerMaskValue = this.SizeLayerMaskGrow;
			if (this.fxForLayerChange != null)
			{
				ObjectPools.instance.Instantiate(this.fxForLayerChange, this.ownerRig.transform.position);
			}
			this.timeToCheckCollision = (double)(Time.time + this.growDelay);
			this.updateCollision = true;
		}
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x00098FF8 File Offset: 0x000971F8
	private void DisableCollisionWithPieces()
	{
		int num = Physics.OverlapSphereNonAlloc(GTPlayer.Instance.headCollider.transform.position, 1f, this.tempDisableColliders, BuilderTable.instance.allPiecesMask);
		for (int i = 0; i < num; i++)
		{
			BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(this.tempDisableColliders[i]);
			if (builderPieceFromCollider != null && builderPieceFromCollider.state == BuilderPiece.State.AttachedAndPlaced && !builderPieceFromCollider.isBuiltIntoTable && !this.collisionDisabledPieces.Contains(builderPieceFromCollider))
			{
				foreach (Collider collider in builderPieceFromCollider.colliders)
				{
					collider.enabled = false;
				}
				foreach (Collider collider2 in builderPieceFromCollider.placedOnlyColliders)
				{
					collider2.enabled = false;
				}
				this.collisionDisabledPieces.Add(builderPieceFromCollider);
			}
		}
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x0009911C File Offset: 0x0009731C
	private void EnableCollisionWithPieces()
	{
		for (int i = this.collisionDisabledPieces.Count - 1; i >= 0; i--)
		{
			BuilderPiece builderPiece = this.collisionDisabledPieces[i];
			if (builderPiece == null)
			{
				this.collisionDisabledPieces.RemoveAt(i);
			}
			else if (Vector3.SqrMagnitude(GTPlayer.Instance.bodyCollider.transform.position - builderPiece.transform.position) >= this.enableDistSq)
			{
				this.EnableCollisionWithPiece(builderPiece);
				this.collisionDisabledPieces.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x000991AC File Offset: 0x000973AC
	private void EnableCollisionWithPiece(BuilderPiece piece)
	{
		foreach (Collider collider in piece.colliders)
		{
			collider.enabled = (piece.state != BuilderPiece.State.None && piece.state != BuilderPiece.State.Displayed);
		}
		foreach (Collider collider2 in piece.placedOnlyColliders)
		{
			collider2.enabled = (piece.state == BuilderPiece.State.AttachedAndPlaced);
		}
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x0009925C File Offset: 0x0009745C
	private void Update()
	{
		if (this.updateCollision && (double)Time.time >= this.timeToCheckCollision)
		{
			this.EnableCollisionWithPieces();
			if (this.collisionDisabledPieces.Count <= 0)
			{
				this.updateCollision = false;
			}
		}
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x00099290 File Offset: 0x00097490
	private void OnShrinkButtonPressed()
	{
		if (this.sizeManager == null)
		{
			if (this.ownerRig == null)
			{
				Debug.LogWarning("Builder resize watch has no owner rig");
			}
			this.sizeManager = this.ownerRig.sizeManager;
		}
		if (this.sizeManager != null && this.sizeManager.currentSizeLayerMaskValue != this.SizeLayerMaskShrink)
		{
			this.sizeManager.currentSizeLayerMaskValue = this.SizeLayerMaskShrink;
		}
	}

	// Token: 0x040021ED RID: 8685
	[SerializeField]
	private HeldButton enlargeButton;

	// Token: 0x040021EE RID: 8686
	[SerializeField]
	private HeldButton shrinkButton;

	// Token: 0x040021EF RID: 8687
	[SerializeField]
	private GameObject fxForLayerChange;

	// Token: 0x040021F0 RID: 8688
	private VRRig ownerRig;

	// Token: 0x040021F1 RID: 8689
	private SizeManager sizeManager;

	// Token: 0x040021F2 RID: 8690
	[HideInInspector]
	public Collider[] tempDisableColliders = new Collider[128];

	// Token: 0x040021F3 RID: 8691
	[HideInInspector]
	public List<BuilderPiece> collisionDisabledPieces = new List<BuilderPiece>();

	// Token: 0x040021F4 RID: 8692
	private float enableDist = 1f;

	// Token: 0x040021F5 RID: 8693
	private float enableDistSq = 1f;

	// Token: 0x040021F6 RID: 8694
	private bool updateCollision;

	// Token: 0x040021F7 RID: 8695
	private float growDelay = 1f;

	// Token: 0x040021F8 RID: 8696
	private double timeToCheckCollision;

	// Token: 0x040021F9 RID: 8697
	public BuilderResizeWatch.BuilderSizeChangeSettings growSettings;

	// Token: 0x040021FA RID: 8698
	public BuilderResizeWatch.BuilderSizeChangeSettings shrinkSettings;

	// Token: 0x020004E2 RID: 1250
	[Serializable]
	public struct BuilderSizeChangeSettings
	{
		// Token: 0x040021FB RID: 8699
		public bool affectLayerA;

		// Token: 0x040021FC RID: 8700
		public bool affectLayerB;

		// Token: 0x040021FD RID: 8701
		public bool affectLayerC;

		// Token: 0x040021FE RID: 8702
		public bool affectLayerD;
	}
}
