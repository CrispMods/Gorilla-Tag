using System;
using System.Collections.Generic;
using Cysharp.Text;
using GorillaExtensions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000090 RID: 144
public class TextTyperAnimatorMono : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060003A6 RID: 934 RVA: 0x00031C7C File Offset: 0x0002FE7C
	public void EdRestartAnimation()
	{
		this.m_textMesh.maxVisibleCharacters = 0;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00077D40 File Offset: 0x00075F40
	protected void Awake()
	{
		this._has_typingSoundBank = (this.m_typingSoundBank != null);
		this._has_beginEntrySoundBank = (this.m_beginEntrySoundBank != null);
		this._waitTime = this._random.NextFloat(this.m_typingSpeedMinMax.x, this.m_typingSpeedMinMax.y);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00030F55 File Offset: 0x0002F155
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00077D98 File Offset: 0x00075F98
	public void SliceUpdate()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		int num = this.m_textMesh.maxVisibleCharacters;
		if (num < 0 || num >= this._charCount || this._timeOfLastTypedChar + this._waitTime > realtimeSinceStartup)
		{
			return;
		}
		num = (this.m_textMesh.maxVisibleCharacters = num + 1);
		this._timeOfLastTypedChar = realtimeSinceStartup;
		if (this._has_beginEntrySoundBank && num == 1)
		{
			this.m_beginEntrySoundBank.Play();
		}
		else if (this._has_typingSoundBank)
		{
			this.m_typingSoundBank.Play();
		}
		this._waitTime = this._random.NextFloat(this.m_typingSpeedMinMax.x, this.m_typingSpeedMinMax.y);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00031C8A File Offset: 0x0002FE8A
	public void SetText(string text, IList<int> entryIndexes, int nonRichTextTagsCharCount)
	{
		this._charCount = nonRichTextTagsCharCount;
		this.m_textMesh.SetText(text, true);
		this.m_textMesh.maxVisibleCharacters = 0;
		this._SetEntryIndexes(entryIndexes);
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00031CB3 File Offset: 0x0002FEB3
	public void SetText(string text, IList<int> entryIndexes)
	{
		this.SetText(text, entryIndexes, text.Length);
		this.m_textMesh.SetText(text, true);
		this.m_textMesh.maxVisibleCharacters = 0;
		this._SetEntryIndexes(entryIndexes);
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00031CE3 File Offset: 0x0002FEE3
	public void SetText(string text)
	{
		this.SetText(text, Array.Empty<int>());
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00031CF1 File Offset: 0x0002FEF1
	public void SetText(Utf16ValueStringBuilder zStringBuilder, IList<int> entryIndexes, int nonRichTextTagsCharCount)
	{
		this._charCount = nonRichTextTagsCharCount;
		this.m_textMesh.SetTextToZString(zStringBuilder);
		this.m_textMesh.maxVisibleCharacters = 0;
		this._SetEntryIndexes(entryIndexes);
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00031D19 File Offset: 0x0002FF19
	public void SetText(Utf16ValueStringBuilder zStringBuilder)
	{
		this.SetText(zStringBuilder, Array.Empty<int>(), zStringBuilder.Length);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x00031D2E File Offset: 0x0002FF2E
	private void _SetEntryIndexes(IList<int> entryIndexes)
	{
		this._entryIndexes.Clear();
		this._entryIndexes.AddRange(entryIndexes);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00077E40 File Offset: 0x00076040
	public void UpdateText(Utf16ValueStringBuilder zStringBuilder, int nonRichTextTagsCharCount)
	{
		TMP_Text textMesh = this.m_textMesh;
		this._charCount = nonRichTextTagsCharCount;
		textMesh.maxVisibleCharacters = nonRichTextTagsCharCount;
		this.m_textMesh.SetTextToZString(zStringBuilder);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000423 RID: 1059
	[FormerlySerializedAs("_textMesh")]
	[Tooltip("Text Mesh Pro component.")]
	[SerializeField]
	private TMP_Text m_textMesh;

	// Token: 0x04000424 RID: 1060
	[Tooltip("Delay between characters in seconds")]
	[SerializeField]
	private Vector2 m_typingSpeedMinMax = new Vector2(0.05f, 0.1f);

	// Token: 0x04000425 RID: 1061
	[Header("Audio")]
	[Tooltip("AudioClips to play while typing.")]
	[SerializeField]
	private SoundBankPlayer m_typingSoundBank;

	// Token: 0x04000426 RID: 1062
	private bool _has_typingSoundBank;

	// Token: 0x04000427 RID: 1063
	[Tooltip("AudioClips to play when a ")]
	[SerializeField]
	private SoundBankPlayer m_beginEntrySoundBank;

	// Token: 0x04000428 RID: 1064
	private bool _has_beginEntrySoundBank;

	// Token: 0x04000429 RID: 1065
	private int _charCount;

	// Token: 0x0400042A RID: 1066
	private readonly List<int> _entryIndexes = new List<int>(16);

	// Token: 0x0400042B RID: 1067
	private float _waitTime;

	// Token: 0x0400042C RID: 1068
	private float _timeOfLastTypedChar = -1f;

	// Token: 0x0400042D RID: 1069
	private Unity.Mathematics.Random _random = new Unity.Mathematics.Random(6746U);
}
