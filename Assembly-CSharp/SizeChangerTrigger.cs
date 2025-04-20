using System;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class SizeChangerTrigger : MonoBehaviour, IBuilderPieceComponent
{
	// Token: 0x14000052 RID: 82
	// (add) Token: 0x060025C1 RID: 9665 RVA: 0x00106E60 File Offset: 0x00105060
	// (remove) Token: 0x060025C2 RID: 9666 RVA: 0x00106E98 File Offset: 0x00105098
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnEnter;

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x060025C3 RID: 9667 RVA: 0x00106ED0 File Offset: 0x001050D0
	// (remove) Token: 0x060025C4 RID: 9668 RVA: 0x00106F08 File Offset: 0x00105108
	public event SizeChangerTrigger.SizeChangerTriggerEvent OnExit;

	// Token: 0x060025C5 RID: 9669 RVA: 0x00049A04 File Offset: 0x00047C04
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x060025C6 RID: 9670 RVA: 0x00049A12 File Offset: 0x00047C12
	public void OnTriggerEnter(Collider other)
	{
		if (this.OnEnter != null)
		{
			this.OnEnter(other);
		}
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x00049A28 File Offset: 0x00047C28
	public void OnTriggerExit(Collider other)
	{
		if (this.OnExit != null)
		{
			this.OnExit(other);
		}
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x00049A3E File Offset: 0x00047C3E
	public Vector3 ClosestPoint(Vector3 position)
	{
		return this.myCollider.ClosestPoint(position);
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPieceCreate(int pieceType, int pieceId)
	{
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPieceDestroy()
	{
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPiecePlacementDeserialized()
	{
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x00049A4C File Offset: 0x00047C4C
	public void OnPieceActivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x060025CD RID: 9677 RVA: 0x00049A4C File Offset: 0x00047C4C
	public void OnPieceDeactivate()
	{
		Debug.LogError("Size Trigger Pieces no longer work, need reimplementation");
	}

	// Token: 0x040029FC RID: 10748
	private Collider myCollider;

	// Token: 0x040029FF RID: 10751
	public bool builderEnterTrigger;

	// Token: 0x04002A00 RID: 10752
	public bool builderExitOnEnterTrigger;

	// Token: 0x020005EF RID: 1519
	// (Invoke) Token: 0x060025D0 RID: 9680
	public delegate void SizeChangerTriggerEvent(Collider other);
}
