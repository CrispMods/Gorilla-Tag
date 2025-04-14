using System;
using UnityEngine;

// Token: 0x020005E1 RID: 1505
public class SizeChangerTrigger : MonoBehaviour, IBuilderPieceComponent
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06002567 RID: 9575 RVA: 0x000B9484 File Offset: 0x000B7684
	// (remove) Token: 0x06002568 RID: 9576 RVA: 0x000B94BC File Offset: 0x000B76BC
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnEnter;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06002569 RID: 9577 RVA: 0x000B94F4 File Offset: 0x000B76F4
	// (remove) Token: 0x0600256A RID: 9578 RVA: 0x000B952C File Offset: 0x000B772C
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnExit;

	// Token: 0x0600256B RID: 9579 RVA: 0x000B9561 File Offset: 0x000B7761
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x0600256C RID: 9580 RVA: 0x000B956F File Offset: 0x000B776F
	public void OnTriggerEnter(Collider other)
	{
		if (this.OnEnter != null)
		{
			this.OnEnter(other);
		}
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x000B9585 File Offset: 0x000B7785
	public void OnTriggerExit(Collider other)
	{
		if (this.OnExit != null)
		{
			this.OnExit(other);
		}
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000B959B File Offset: 0x000B779B
	public Vector3 ClosestPoint(Vector3 position)
	{
		return this.myCollider.ClosestPoint(position);
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPieceCreate(int pieceType, int pieceId)
	{
	}

	// Token: 0x06002570 RID: 9584 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPieceDestroy()
	{
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnPiecePlacementDeserialized()
	{
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x000B95A9 File Offset: 0x000B77A9
	public void OnPieceActivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x000B95A9 File Offset: 0x000B77A9
	public void OnPieceDeactivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x040029A3 RID: 10659
	private Collider myCollider;

	// Token: 0x040029A6 RID: 10662
	public bool builderEnterTrigger;

	// Token: 0x040029A7 RID: 10663
	public bool builderExitOnEnterTrigger;

	// Token: 0x020005E2 RID: 1506
	// (Invoke) Token: 0x06002576 RID: 9590
	public delegate void SizeChangerTriggerEvent(Collider other);
}
