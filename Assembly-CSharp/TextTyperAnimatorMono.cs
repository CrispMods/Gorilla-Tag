using System;
using System.Collections.Generic;
using Cysharp.Text;
using GorillaExtensions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000097 RID: 151
public class TextTyperAnimatorMono : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060003D6 RID: 982 RVA: 0x00032DDF File Offset: 0x00030FDF
	public void EdRestartAnimation()
	{
		this.m_textMesh.maxVisibleCharacters = 0;
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0007A468 File Offset: 0x00078668
	protected void Awake()
	{
		this._has_typingSoundBank = (this.m_typingSoundBank != null);
		this._has_beginEntrySoundBank = (this.m_beginEntrySoundBank != null);
		this._waitTime = this._random.NextFloat(this.m_typingSpeedMinMax.x, this.m_typingSpeedMinMax.y);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0007A4C0 File Offset: 0x000786C0
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

	// Token: 0x060003DB RID: 987 RVA: 0x00032DED File Offset: 0x00030FED
	public void SetText(string text, IList<int> entryIndexes, int nonRichTextTagsCharCount)
	{
		this._charCount = nonRichTextTagsCharCount;
		this.m_textMesh.SetText(text, true);
		this.m_textMesh.maxVisibleCharacters = 0;
		this._SetEntryIndexes(entryIndexes);
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00032E16 File Offset: 0x00031016
	public void SetText(string text, IList<int> entryIndexes)
	{
		this.SetText(text, entryIndexes, text.Length);
		this.m_textMesh.SetText(text, true);
		this.m_textMesh.maxVisibleCharacters = 0;
		this._SetEntryIndexes(entryIndexes);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00032E46 File Offset: 0x00031046
	public void SetText(string text)
	{
		this.SetText(text, Array.Empty<int>());
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00032E54 File Offset: 0x00031054
	public void SetText(Utf16ValueStringBuilder zStringBuilder, IList<int> entryIndexes, int nonRichTextTagsCharCount)
	{
		this._charCount = nonRichTextTagsCharCount;
		this.m_textMesh.SetTextToZString(zStringBuilder);
		this.m_textMesh.maxVisibleCharacters = 0;
		this._SetEntryIndexes(entryIndexes);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00032E7C File Offset: 0x0003107C
	public void SetText(Utf16ValueStringBuilder zStringBuilder)
	{
		this.SetText(zStringBuilder, Array.Empty<int>(), zStringBuilder.Length);
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00032E91 File Offset: 0x00031091
	private void _SetEntryIndexes(IList<int> entryIndexes)
	{
		this._entryIndexes.Clear();
		this._entryIndexes.AddRange(entryIndexes);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0007A568 File Offset: 0x00078768
	public void UpdateText(Utf16ValueStringBuilder zStringBuilder, int nonRichTextTagsCharCount)
	{
		TMP_Text textMesh = this.m_textMesh;
		this._charCount = nonRichTextTagsCharCount;
		textMesh.maxVisibleCharacters = nonRichTextTagsCharCount;
		this.m_textMesh.SetTextToZString(zStringBuilder);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000456 RID: 1110
	[FormerlySerializedAs("_textMesh")]
	[Tooltip("Text Mesh Pro component.")]
	[SerializeField]
	private TMP_Text m_textMesh;

	// Token: 0x04000457 RID: 1111
	[Tooltip("Delay between characters in seconds")]
	[SerializeField]
	private Vector2 m_typingSpeedMinMax = new Vector2(0.05f, 0.1f);

	// Token: 0x04000458 RID: 1112
	[Header("Audio")]
	[Tooltip("AudioClips to play while typing.")]
	[SerializeField]
	private SoundBankPlayer m_typingSoundBank;

	// Token: 0x04000459 RID: 1113
	private bool _has_typingSoundBank;

	// Token: 0x0400045A RID: 1114
	[Tooltip("AudioClips to play when a ")]
	[SerializeField]
	private SoundBankPlayer m_beginEntrySoundBank;

	// Token: 0x0400045B RID: 1115
	private bool _has_beginEntrySoundBank;

	// Token: 0x0400045C RID: 1116
	private int _charCount;

	// Token: 0x0400045D RID: 1117
	private readonly List<int> _entryIndexes = new List<int>(16);

	// Token: 0x0400045E RID: 1118
	private float _waitTime;

	// Token: 0x0400045F RID: 1119
	private float _timeOfLastTypedChar = -1f;

	// Token: 0x04000460 RID: 1120
	private Unity.Mathematics.Random _random = new Unity.Mathematics.Random(6746U);
}
