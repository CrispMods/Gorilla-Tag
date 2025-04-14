using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Text;
using GorillaLocomotion;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200008C RID: 140
public class EyeScannerMono : MonoBehaviour, ISpawnable, IGorillaSliceableSimple
{
	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000384 RID: 900 RVA: 0x00015FB6 File Offset: 0x000141B6
	// (set) Token: 0x06000385 RID: 901 RVA: 0x00015FC0 File Offset: 0x000141C0
	private Color32 KeyTextColor
	{
		get
		{
			return this.m_keyTextColor;
		}
		set
		{
			this.m_keyTextColor = value;
			this._keyRichTextColorTagString = string.Format(CultureInfo.InvariantCulture.NumberFormat, "<color=#{0:X2}{1:X2}{2:X2}>", value.r, value.g, value.b);
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000386 RID: 902 RVA: 0x0001600F File Offset: 0x0001420F
	private List<IEyeScannable> registeredScannables
	{
		get
		{
			return EyeScannerMono._registeredScannables;
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00016016 File Offset: 0x00014216
	public static void Register(IEyeScannable scannable)
	{
		if (EyeScannerMono._registeredScannableIds.Add(scannable.scannableId))
		{
			EyeScannerMono._registeredScannables.Add(scannable);
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00016035 File Offset: 0x00014235
	public static void Unregister(IEyeScannable scannable)
	{
		if (EyeScannerMono._registeredScannableIds.Remove(scannable.scannableId))
		{
			EyeScannerMono._registeredScannables.Remove(scannable);
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00016058 File Offset: 0x00014258
	protected void Awake()
	{
		this._sb = ZString.CreateStringBuilder();
		this.KeyTextColor = this.KeyTextColor;
		math.sign(this.m_textTyper.transform.parent.localScale);
		this.m_textTyper.SetText(string.Empty);
		this.m_reticle.gameObject.SetActive(false);
		this.m_textTyper.gameObject.SetActive(false);
		this.m_overlayBg.SetActive(false);
		this._line = base.GetComponent<LineRenderer>();
		this._line.enabled = false;
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600038A RID: 906 RVA: 0x000160F2 File Offset: 0x000142F2
	// (set) Token: 0x0600038B RID: 907 RVA: 0x000160FA File Offset: 0x000142FA
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600038C RID: 908 RVA: 0x00016103 File Offset: 0x00014303
	// (set) Token: 0x0600038D RID: 909 RVA: 0x0001610B File Offset: 0x0001430B
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x0600038E RID: 910 RVA: 0x00016114 File Offset: 0x00014314
	// (set) Token: 0x0600038F RID: 911 RVA: 0x0001611C File Offset: 0x0001431C
	public string DebugData { get; private set; }

	// Token: 0x06000390 RID: 912 RVA: 0x00016128 File Offset: 0x00014328
	public void OnSpawn(VRRig rig)
	{
		if (rig != null && !rig.isOfflineVRRig)
		{
			Object.Destroy(base.gameObject);
		}
		if (GTPlayer.hasInstance)
		{
			GTPlayer instance = GTPlayer.Instance;
			this._firstPersonCamera = instance.GetComponentInChildren<Camera>();
			this._has_firstPersonCamera = (this._firstPersonCamera != null);
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0000FC06 File Offset: 0x0000DE06
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0000FC0F File Offset: 0x0000DE0F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001617C File Offset: 0x0001437C
	void IGorillaSliceableSimple.SliceUpdate()
	{
		IEyeScannable eyeScannable = null;
		Transform transform = base.transform;
		Vector3 position = transform.position;
		Vector3 forward = transform.forward;
		float num = this.m_LookPrecision;
		for (int i = 0; i < EyeScannerMono._registeredScannables.Count; i++)
		{
			IEyeScannable eyeScannable2 = EyeScannerMono._registeredScannables[i];
			Vector3 normalized = (eyeScannable2.Position - position).normalized;
			float num2 = Vector3.Distance(position, eyeScannable2.Position);
			float num3 = Vector3.Dot(forward, normalized);
			if (num2 >= this.m_scanDistanceMin && num2 <= this.m_scanDistanceMax && num3 > num)
			{
				RaycastHit raycastHit;
				if (!this.m_xrayVision && Physics.Raycast(position, normalized, out raycastHit, this.m_scanDistanceMax, this._layerMask.value))
				{
					IEyeScannable componentInParent = raycastHit.collider.GetComponentInParent<IEyeScannable>();
					if (componentInParent == null || componentInParent != eyeScannable2)
					{
						goto IL_BF;
					}
				}
				num = num3;
				eyeScannable = eyeScannable2;
			}
			IL_BF:;
		}
		if (eyeScannable != this._oldClosestScannable)
		{
			if (this._oldClosestScannable != null)
			{
				this._oldClosestScannable.OnDataChange -= this.Scannable_OnDataChange;
			}
			this._OnScannableChanged(eyeScannable, true);
			this._oldClosestScannable = eyeScannable;
			if (this._oldClosestScannable != null)
			{
				this._oldClosestScannable.OnDataChange += this.Scannable_OnDataChange;
			}
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x000162B5 File Offset: 0x000144B5
	private void Scannable_OnDataChange()
	{
		this._OnScannableChanged(this._oldClosestScannable, false);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x000162C4 File Offset: 0x000144C4
	private void LateUpdate()
	{
		if (this._oldClosestScannable != null)
		{
			this.m_reticle.position = this._oldClosestScannable.Position;
			float num = math.distance(base.transform.position, this.m_reticle.position);
			Mathf.Clamp(num * 0.33333f, 0f, 1f);
			float num2 = num * this.m_reticleScale;
			float d = num * this.m_textScale;
			float num3 = num * this.m_overlayScale;
			this.m_reticle.localScale = new Vector3(num2, num2, num2);
			this.m_overlay.localPosition = new Vector3(this.m_position.x * num, this.m_position.y * num, num);
			this.m_overlay.localScale = new Vector3(num3, num3, 1f);
			this._line.SetPosition(0, this.m_reticle.position);
			this._line.SetPosition(1, this.m_textTyper.transform.position + this.m_pointerOffset * d);
			this._line.widthMultiplier = num2;
		}
	}

	// Token: 0x06000397 RID: 919 RVA: 0x000163F0 File Offset: 0x000145F0
	private void _OnScannableChanged(IEyeScannable scannable, bool typeingShow)
	{
		this._sb.Clear();
		if (scannable == null)
		{
			this.m_textTyper.SetText(this._sb);
			this.m_textTyper.gameObject.SetActive(false);
			this.m_reticle.gameObject.SetActive(false);
			this.m_overlayBg.SetActive(false);
			this.m_reticle.parent = base.transform;
			this._line.enabled = false;
			return;
		}
		this.m_reticle.gameObject.SetActive(true);
		this.m_textTyper.gameObject.SetActive(true);
		this.m_overlayBg.SetActive(true);
		this.m_reticle.position = scannable.Position;
		this._line.enabled = true;
		this._sb.AppendLine(this.DebugData);
		this._entryIndexes[0] = 0;
		int i = 1;
		int num = 0;
		for (int j = 0; j < scannable.Entries.Count; j++)
		{
			KeyValueStringPair keyValueStringPair = scannable.Entries[j];
			if (!string.IsNullOrEmpty(keyValueStringPair.Key))
			{
				this._sb.Append(this._keyRichTextColorTagString);
				this._sb.Append(keyValueStringPair.Key);
				this._sb.Append("</color>: ");
				num += keyValueStringPair.Key.Length + 2;
			}
			if (!string.IsNullOrEmpty(keyValueStringPair.Value))
			{
				this._sb.Append(keyValueStringPair.Value);
				num += keyValueStringPair.Value.Length;
			}
			this._sb.AppendLine();
			num += Environment.NewLine.Length;
			if (i < this._entryIndexes.Length)
			{
				this._entryIndexes[i++] = num - 1;
			}
		}
		while (i < this._entryIndexes.Length)
		{
			this._entryIndexes[i] = -1;
			i++;
		}
		if (typeingShow)
		{
			this.m_textTyper.SetText(this._sb, this._entryIndexes, num);
			return;
		}
		this.m_textTyper.UpdateText(this._sb, num);
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0000FD18 File Offset: 0x0000DF18
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000405 RID: 1029
	[FormerlySerializedAs("_scanDistance")]
	[Tooltip("Any scannables with transforms beyond this distance will be automatically ignored.")]
	[SerializeField]
	private float m_scanDistanceMax = 10f;

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	private float m_scanDistanceMin = 0.5f;

	// Token: 0x04000407 RID: 1031
	[FormerlySerializedAs("_textTyper")]
	[Tooltip("The component that handles setting text in the TextMeshPro and animates the text typing.")]
	[SerializeField]
	private TextTyperAnimatorMono m_textTyper;

	// Token: 0x04000408 RID: 1032
	[SerializeField]
	private Transform m_reticle;

	// Token: 0x04000409 RID: 1033
	[SerializeField]
	private Transform m_overlay;

	// Token: 0x0400040A RID: 1034
	[SerializeField]
	private GameObject m_overlayBg;

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	private float m_reticleScale = 1f;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	private float m_textScale = 1f;

	// Token: 0x0400040D RID: 1037
	[SerializeField]
	private float m_overlayScale = 1f;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private Vector3 m_pointerOffset;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private Vector2 m_position;

	// Token: 0x04000410 RID: 1040
	[HideInInspector]
	[SerializeField]
	private Color32 m_keyTextColor = new Color32(byte.MaxValue, 34, 0, byte.MaxValue);

	// Token: 0x04000411 RID: 1041
	private string _keyRichTextColorTagString = "";

	// Token: 0x04000412 RID: 1042
	private static readonly List<IEyeScannable> _registeredScannables = new List<IEyeScannable>(128);

	// Token: 0x04000413 RID: 1043
	private static readonly HashSet<int> _registeredScannableIds = new HashSet<int>(128);

	// Token: 0x04000414 RID: 1044
	private IEyeScannable _oldClosestScannable;

	// Token: 0x04000415 RID: 1045
	private Utf16ValueStringBuilder _sb;

	// Token: 0x04000416 RID: 1046
	private readonly int[] _entryIndexes = new int[16];

	// Token: 0x04000417 RID: 1047
	[SerializeField]
	private LayerMask _layerMask;

	// Token: 0x04000418 RID: 1048
	private Camera _firstPersonCamera;

	// Token: 0x04000419 RID: 1049
	private bool _has_firstPersonCamera;

	// Token: 0x0400041D RID: 1053
	[SerializeField]
	private float m_LookPrecision = 0.65f;

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private bool m_xrayVision;

	// Token: 0x0400041F RID: 1055
	private LineRenderer _line;
}
