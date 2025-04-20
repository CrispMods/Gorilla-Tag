using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaTagScripts;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004EE RID: 1262
public class BuilderResizeWatch : MonoBehaviour
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000EB1FC File Offset: 0x000E93FC
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

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06001EBA RID: 7866 RVA: 0x000EB250 File Offset: 0x000E9450
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

	// Token: 0x06001EBB RID: 7867 RVA: 0x000EB2A4 File Offset: 0x000E94A4
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

	// Token: 0x06001EBC RID: 7868 RVA: 0x000EB33C File Offset: 0x000E953C
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

	// Token: 0x06001EBD RID: 7869 RVA: 0x000EB3A0 File Offset: 0x000E95A0
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

	// Token: 0x06001EBE RID: 7870 RVA: 0x000EB470 File Offset: 0x000E9670
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

	// Token: 0x06001EBF RID: 7871 RVA: 0x000EB594 File Offset: 0x000E9794
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

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000EB624 File Offset: 0x000E9824
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

	// Token: 0x06001EC1 RID: 7873 RVA: 0x00044D6F File Offset: 0x00042F6F
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

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000EB6D4 File Offset: 0x000E98D4
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

	// Token: 0x0400223F RID: 8767
	[SerializeField]
	private HeldButton enlargeButton;

	// Token: 0x04002240 RID: 8768
	[SerializeField]
	private HeldButton shrinkButton;

	// Token: 0x04002241 RID: 8769
	[SerializeField]
	private GameObject fxForLayerChange;

	// Token: 0x04002242 RID: 8770
	private VRRig ownerRig;

	// Token: 0x04002243 RID: 8771
	private SizeManager sizeManager;

	// Token: 0x04002244 RID: 8772
	[HideInInspector]
	public Collider[] tempDisableColliders = new Collider[128];

	// Token: 0x04002245 RID: 8773
	[HideInInspector]
	public List<BuilderPiece> collisionDisabledPieces = new List<BuilderPiece>();

	// Token: 0x04002246 RID: 8774
	private float enableDist = 1f;

	// Token: 0x04002247 RID: 8775
	private float enableDistSq = 1f;

	// Token: 0x04002248 RID: 8776
	private bool updateCollision;

	// Token: 0x04002249 RID: 8777
	private float growDelay = 1f;

	// Token: 0x0400224A RID: 8778
	private double timeToCheckCollision;

	// Token: 0x0400224B RID: 8779
	public BuilderResizeWatch.BuilderSizeChangeSettings growSettings;

	// Token: 0x0400224C RID: 8780
	public BuilderResizeWatch.BuilderSizeChangeSettings shrinkSettings;

	// Token: 0x020004EF RID: 1263
	[Serializable]
	public struct BuilderSizeChangeSettings
	{
		// Token: 0x0400224D RID: 8781
		public bool affectLayerA;

		// Token: 0x0400224E RID: 8782
		public bool affectLayerB;

		// Token: 0x0400224F RID: 8783
		public bool affectLayerC;

		// Token: 0x04002250 RID: 8784
		public bool affectLayerD;
	}
}
