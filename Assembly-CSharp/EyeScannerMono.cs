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

// Token: 0x02000093 RID: 147
public class EyeScannerMono : MonoBehaviour, ISpawnable, IGorillaSliceableSimple
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060003B4 RID: 948 RVA: 0x00032D17 File Offset: 0x00030F17
	// (set) Token: 0x060003B5 RID: 949 RVA: 0x00079E38 File Offset: 0x00078038
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

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x00032D1F File Offset: 0x00030F1F
	private List<IEyeScannable> registeredScannables
	{
		get
		{
			return EyeScannerMono._registeredScannables;
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00032D26 File Offset: 0x00030F26
	public static void Register(IEyeScannable scannable)
	{
		if (EyeScannerMono._registeredScannableIds.Add(scannable.scannableId))
		{
			EyeScannerMono._registeredScannables.Add(scannable);
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00032D45 File Offset: 0x00030F45
	public static void Unregister(IEyeScannable scannable)
	{
		if (EyeScannerMono._registeredScannableIds.Remove(scannable.scannableId))
		{
			EyeScannerMono._registeredScannables.Remove(scannable);
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00079E88 File Offset: 0x00078088
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

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060003BA RID: 954 RVA: 0x00032D65 File Offset: 0x00030F65
	// (set) Token: 0x060003BB RID: 955 RVA: 0x00032D6D File Offset: 0x00030F6D
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060003BC RID: 956 RVA: 0x00032D76 File Offset: 0x00030F76
	// (set) Token: 0x060003BD RID: 957 RVA: 0x00032D7E File Offset: 0x00030F7E
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060003BE RID: 958 RVA: 0x00032D87 File Offset: 0x00030F87
	// (set) Token: 0x060003BF RID: 959 RVA: 0x00032D8F File Offset: 0x00030F8F
	public string DebugData { get; private set; }

	// Token: 0x060003C0 RID: 960 RVA: 0x00079F24 File Offset: 0x00078124
	public void OnSpawn(VRRig rig)
	{
		if (rig != null && !rig.isOfflineVRRig)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (GTPlayer.hasInstance)
		{
			GTPlayer instance = GTPlayer.Instance;
			this._firstPersonCamera = instance.GetComponentInChildren<Camera>();
			this._has_firstPersonCamera = (this._firstPersonCamera != null);
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00079F78 File Offset: 0x00078178
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

	// Token: 0x060003C5 RID: 965 RVA: 0x00032D98 File Offset: 0x00030F98
	private void Scannable_OnDataChange()
	{
		this._OnScannableChanged(this._oldClosestScannable, false);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0007A0B4 File Offset: 0x000782B4
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

	// Token: 0x060003C7 RID: 967 RVA: 0x0007A1E0 File Offset: 0x000783E0
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

	// Token: 0x060003CA RID: 970 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000438 RID: 1080
	[FormerlySerializedAs("_scanDistance")]
	[Tooltip("Any scannables with transforms beyond this distance will be automatically ignored.")]
	[SerializeField]
	private float m_scanDistanceMax = 10f;

	// Token: 0x04000439 RID: 1081
	[SerializeField]
	private float m_scanDistanceMin = 0.5f;

	// Token: 0x0400043A RID: 1082
	[FormerlySerializedAs("_textTyper")]
	[Tooltip("The component that handles setting text in the TextMeshPro and animates the text typing.")]
	[SerializeField]
	private TextTyperAnimatorMono m_textTyper;

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	private Transform m_reticle;

	// Token: 0x0400043C RID: 1084
	[SerializeField]
	private Transform m_overlay;

	// Token: 0x0400043D RID: 1085
	[SerializeField]
	private GameObject m_overlayBg;

	// Token: 0x0400043E RID: 1086
	[SerializeField]
	private float m_reticleScale = 1f;

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private float m_textScale = 1f;

	// Token: 0x04000440 RID: 1088
	[SerializeField]
	private float m_overlayScale = 1f;

	// Token: 0x04000441 RID: 1089
	[SerializeField]
	private Vector3 m_pointerOffset;

	// Token: 0x04000442 RID: 1090
	[SerializeField]
	private Vector2 m_position;

	// Token: 0x04000443 RID: 1091
	[HideInInspector]
	[SerializeField]
	private Color32 m_keyTextColor = new Color32(byte.MaxValue, 34, 0, byte.MaxValue);

	// Token: 0x04000444 RID: 1092
	private string _keyRichTextColorTagString = "";

	// Token: 0x04000445 RID: 1093
	private static readonly List<IEyeScannable> _registeredScannables = new List<IEyeScannable>(128);

	// Token: 0x04000446 RID: 1094
	private static readonly HashSet<int> _registeredScannableIds = new HashSet<int>(128);

	// Token: 0x04000447 RID: 1095
	private IEyeScannable _oldClosestScannable;

	// Token: 0x04000448 RID: 1096
	private Utf16ValueStringBuilder _sb;

	// Token: 0x04000449 RID: 1097
	private readonly int[] _entryIndexes = new int[16];

	// Token: 0x0400044A RID: 1098
	[SerializeField]
	private LayerMask _layerMask;

	// Token: 0x0400044B RID: 1099
	private Camera _firstPersonCamera;

	// Token: 0x0400044C RID: 1100
	private bool _has_firstPersonCamera;

	// Token: 0x04000450 RID: 1104
	[SerializeField]
	private float m_LookPrecision = 0.65f;

	// Token: 0x04000451 RID: 1105
	[SerializeField]
	private bool m_xrayVision;

	// Token: 0x04000452 RID: 1106
	private LineRenderer _line;
}
