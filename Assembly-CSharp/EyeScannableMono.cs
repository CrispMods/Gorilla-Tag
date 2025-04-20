using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class EyeScannableMono : MonoBehaviour, IEyeScannable
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060003A6 RID: 934 RVA: 0x00079C48 File Offset: 0x00077E48
	// (remove) Token: 0x060003A7 RID: 935 RVA: 0x00079C80 File Offset: 0x00077E80
	public event Action OnDataChange;

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060003A8 RID: 936 RVA: 0x00032CAE File Offset: 0x00030EAE
	int IEyeScannable.scannableId
	{
		get
		{
			return base.GetInstanceID();
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060003A9 RID: 937 RVA: 0x00032CB6 File Offset: 0x00030EB6
	Vector3 IEyeScannable.Position
	{
		get
		{
			return base.transform.position - this._initialPosition + this._bounds.center;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060003AA RID: 938 RVA: 0x00032CDE File Offset: 0x00030EDE
	Bounds IEyeScannable.Bounds
	{
		get
		{
			return this._bounds;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060003AB RID: 939 RVA: 0x00032CE6 File Offset: 0x00030EE6
	IList<KeyValueStringPair> IEyeScannable.Entries
	{
		get
		{
			return this.data.Entries;
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00032CF3 File Offset: 0x00030EF3
	private void Awake()
	{
		this.RecalculateBounds();
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00032CFB File Offset: 0x00030EFB
	public void OnEnable()
	{
		this.RecalculateBoundsLater();
		EyeScannerMono.Register(this);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x000325C8 File Offset: 0x000307C8
	public void OnDisable()
	{
		EyeScannerMono.Unregister(this);
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00079CB8 File Offset: 0x00077EB8
	private void RecalculateBoundsLater()
	{
		EyeScannableMono.<RecalculateBoundsLater>d__17 <RecalculateBoundsLater>d__;
		<RecalculateBoundsLater>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RecalculateBoundsLater>d__.<>4__this = this;
		<RecalculateBoundsLater>d__.<>1__state = -1;
		<RecalculateBoundsLater>d__.<>t__builder.Start<EyeScannableMono.<RecalculateBoundsLater>d__17>(ref <RecalculateBoundsLater>d__);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x00079CF0 File Offset: 0x00077EF0
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

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private KeyValuePairSet data;

	// Token: 0x04000432 RID: 1074
	private Bounds _bounds;

	// Token: 0x04000433 RID: 1075
	private Vector3 _initialPosition;
}
