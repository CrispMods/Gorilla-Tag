using System;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
public class SizeChangerTrigger : MonoBehaviour, IBuilderPieceComponent
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x0600255F RID: 9567 RVA: 0x000B9004 File Offset: 0x000B7204
	// (remove) Token: 0x06002560 RID: 9568 RVA: 0x000B903C File Offset: 0x000B723C
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnEnter;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06002561 RID: 9569 RVA: 0x000B9074 File Offset: 0x000B7274
	// (remove) Token: 0x06002562 RID: 9570 RVA: 0x000B90AC File Offset: 0x000B72AC
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnExit;

	// Token: 0x06002563 RID: 9571 RVA: 0x000B90E1 File Offset: 0x000B72E1
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000B90EF File Offset: 0x000B72EF
	public void OnTriggerEnter(Collider other)
	{
		if (this.OnEnter != null)
		{
			this.OnEnter(other);
		}
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x000B9105 File Offset: 0x000B7305
	public void OnTriggerExit(Collider other)
	{
		if (this.OnExit != null)
		{
			this.OnExit(other);
		}
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x000B911B File Offset: 0x000B731B
	public Vector3 ClosestPoint(Vector3 position)
	{
		return this.myCollider.ClosestPoint(position);
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPieceCreate(int pieceType, int pieceId)
	{
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPieceDestroy()
	{
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPiecePlacementDeserialized()
	{
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x000B9129 File Offset: 0x000B7329
	public void OnPieceActivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x000B9129 File Offset: 0x000B7329
	public void OnPieceDeactivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x0400299D RID: 10653
	private Collider myCollider;

	// Token: 0x040029A0 RID: 10656
	public bool builderEnterTrigger;

	// Token: 0x040029A1 RID: 10657
	public bool builderExitOnEnterTrigger;

	// Token: 0x020005E1 RID: 1505
	// (Invoke) Token: 0x0600256E RID: 9582
	public delegate void SizeChangerTriggerEvent(Collider other);
}
