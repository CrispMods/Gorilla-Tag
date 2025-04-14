using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class EyeScannableMono : MonoBehaviour, IEyeScannable
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000376 RID: 886 RVA: 0x00015D60 File Offset: 0x00013F60
	// (remove) Token: 0x06000377 RID: 887 RVA: 0x00015D98 File Offset: 0x00013F98
	public event Action OnDataChange;

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000378 RID: 888 RVA: 0x00015DCD File Offset: 0x00013FCD
	int IEyeScannable.scannableId
	{
		get
		{
			return base.GetInstanceID();
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000379 RID: 889 RVA: 0x00015DD5 File Offset: 0x00013FD5
	Vector3 IEyeScannable.Position
	{
		get
		{
			return base.transform.position - this._initialPosition + this._bounds.center;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x0600037A RID: 890 RVA: 0x00015DFD File Offset: 0x00013FFD
	Bounds IEyeScannable.Bounds
	{
		get
		{
			return this._bounds;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600037B RID: 891 RVA: 0x00015E05 File Offset: 0x00014005
	IList<KeyValueStringPair> IEyeScannable.Entries
	{
		get
		{
			return this.data.Entries;
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00015E12 File Offset: 0x00014012
	private void Awake()
	{
		this.RecalculateBounds();
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00015E1A File Offset: 0x0001401A
	public void OnEnable()
	{
		this.RecalculateBoundsLater();
		EyeScannerMono.Register(this);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x000123DD File Offset: 0x000105DD
	public void OnDisable()
	{
		EyeScannerMono.Unregister(this);
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00015E28 File Offset: 0x00014028
	private void RecalculateBoundsLater()
	{
		EyeScannableMono.<RecalculateBoundsLater>d__17 <RecalculateBoundsLater>d__;
		<RecalculateBoundsLater>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RecalculateBoundsLater>d__.<>4__this = this;
		<RecalculateBoundsLater>d__.<>1__state = -1;
		<RecalculateBoundsLater>d__.<>t__builder.Start<EyeScannableMono.<RecalculateBoundsLater>d__17>(ref <RecalculateBoundsLater>d__);
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00015E60 File Offset: 0x00014060
	private void RecalculateBounds()
	{
		this._initialPosition = base.transform.position;
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
		this._bounds = default(Bounds);
		if (componentsInChildren.Length == 0)
		{
			this._bounds.center = base.transform.position;
			this._bounds.Expand(1f);
			return;
		}
		this._bounds = componentsInChildren[0].bounds;
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			this._bounds.Encapsulate(componentsInChildren[i].bounds);
		}
	}

	// Token: 0x040003FE RID: 1022
	[SerializeField]
	private KeyValuePairSet data;

	// Token: 0x040003FF RID: 1023
	private Bounds _bounds;

	// Token: 0x04000400 RID: 1024
	private Vector3 _initialPosition;
}
