using System;
using UnityEngine;

// Token: 0x020005E1 RID: 1505
public class SizeChangerTrigger : MonoBehaviour, IBuilderPieceComponent
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06002567 RID: 9575 RVA: 0x00103F24 File Offset: 0x00102124
	// (remove) Token: 0x06002568 RID: 9576 RVA: 0x00103F5C File Offset: 0x0010215C
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnEnter;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06002569 RID: 9577 RVA: 0x00103F94 File Offset: 0x00102194
	// (remove) Token: 0x0600256A RID: 9578 RVA: 0x00103FCC File Offset: 0x001021CC
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnExit;

	// Token: 0x0600256B RID: 9579 RVA: 0x0004862D File Offset: 0x0004682D
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x0600256C RID: 9580 RVA: 0x0004863B File Offset: 0x0004683B
	public void OnTriggerEnter(Collider other)
	{
		if (this.OnEnter != null)
		{
			this.OnEnter(other);
		}
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x00048651 File Offset: 0x00046851
	public void OnTriggerExit(Collider other)
	{
		if (this.OnExit != null)
		{
			this.OnExit(other);
		}
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x00048667 File Offset: 0x00046867
	public Vector3 ClosestPoint(Vector3 position)
	{
		return this.myCollider.ClosestPoint(position);
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPieceCreate(int pieceType, int pieceId)
	{
	}

	// Token: 0x06002570 RID: 9584 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPieceDestroy()
	{
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPiecePlacementDeserialized()
	{
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x00048675 File Offset: 0x00046875
	public void OnPieceActivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x00048675 File Offset: 0x00046875
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
