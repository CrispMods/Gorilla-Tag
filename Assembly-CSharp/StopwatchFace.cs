using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200063C RID: 1596
public class StopwatchFace : MonoBehaviour
{
	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000C1C85 File Offset: 0x000BFE85
	public bool watchActive
	{
		get
		{
			return this._watchActive;
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x060027A1 RID: 10145 RVA: 0x000C1C8D File Offset: 0x000BFE8D
	public int millisElapsed
	{
		get
		{
			return this._millisElapsed;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x060027A2 RID: 10146 RVA: 0x000C1C95 File Offset: 0x000BFE95
	public Vector3Int digitsMmSsMs
	{
		get
		{
			return StopwatchFace.ParseDigits(TimeSpan.FromMilliseconds((double)this._millisElapsed));
		}
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x000C1CA8 File Offset: 0x000BFEA8
	public void SetMillisElapsed(int millis, bool updateFace = true)
	{
		this._millisElapsed = millis;
		if (!updateFace)
		{
			return;
		}
		this.UpdateText();
		this.UpdateHand();
	}

	// Token: 0x060027A4 RID: 10148 RVA: 0x000C1CC1 File Offset: 0x000BFEC1
	private void Awake()
	{
		this._lerpToZero = new LerpTask<int>();
		this._lerpToZero.onLerp = new Action<int, int, float>(this.OnLerpToZero);
		this._lerpToZero.onLerpEnd = new Action(this.OnLerpEnd);
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x000C1CFC File Offset: 0x000BFEFC
	private void OnLerpToZero(int a, int b, float t)
	{
		this._millisElapsed = Mathf.FloorToInt(Mathf.Lerp((float)a, (float)b, t * t));
		this.UpdateText();
		this.UpdateHand();
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x000C1D21 File Offset: 0x000BFF21
	private void OnLerpEnd()
	{
		this.WatchReset(false);
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000C1D21 File Offset: 0x000BFF21
	private void OnEnable()
	{
		this.WatchReset(false);
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000C1D21 File Offset: 0x000BFF21
	private void OnDisable()
	{
		this.WatchReset(false);
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000C1D2C File Offset: 0x000BFF2C
	private void Update()
	{
		if (this._lerpToZero.active)
		{
			this._lerpToZero.Update();
			return;
		}
		if (this._watchActive)
		{
			this._millisElapsed += Mathf.FloorToInt(Time.deltaTime * 1000f);
			this.UpdateText();
			this.UpdateHand();
		}
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000C1D84 File Offset: 0x000BFF84
	private static Vector3Int ParseDigits(TimeSpan time)
	{
		int num = (int)time.TotalMinutes % 100;
		double num2 = 60.0 * (time.TotalMinutes - (double)num);
		int num3 = (int)num2;
		int num4 = (int)(100.0 * (num2 - (double)num3));
		num = Math.Clamp(num, 0, 99);
		num3 = Math.Clamp(num3, 0, 59);
		num4 = Math.Clamp(num4, 0, 99);
		return new Vector3Int(num, num3, num4);
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000C1DEC File Offset: 0x000BFFEC
	private void UpdateText()
	{
		Vector3Int vector3Int = StopwatchFace.ParseDigits(TimeSpan.FromMilliseconds((double)this._millisElapsed));
		string text = vector3Int.x.ToString("D2");
		string text2 = vector3Int.y.ToString("D2");
		string text3 = vector3Int.z.ToString("D2");
		this._text.text = string.Concat(new string[]
		{
			text,
			":",
			text2,
			":",
			text3
		});
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000C1E80 File Offset: 0x000C0080
	private void UpdateHand()
	{
		float z = (float)(this._millisElapsed % 60000) / 60000f * 360f;
		this._hand.localEulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000C1EC2 File Offset: 0x000C00C2
	public void WatchToggle()
	{
		if (!this._watchActive)
		{
			this.WatchStart();
			return;
		}
		this.WatchStop();
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000C1ED9 File Offset: 0x000C00D9
	public void WatchStart()
	{
		if (this._lerpToZero.active)
		{
			return;
		}
		this._watchActive = true;
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000C1EF0 File Offset: 0x000C00F0
	public void WatchStop()
	{
		if (this._lerpToZero.active)
		{
			return;
		}
		this._watchActive = false;
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000C1F07 File Offset: 0x000C0107
	public void WatchReset()
	{
		this.WatchReset(true);
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000C1F10 File Offset: 0x000C0110
	public void WatchReset(bool doLerp)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (doLerp)
		{
			if (!this._lerpToZero.active)
			{
				this._lerpToZero.Start(this._millisElapsed % 60000, 0, 0.36f);
				return;
			}
		}
		else
		{
			this._watchActive = false;
			this._millisElapsed = 0;
			this.UpdateText();
			this.UpdateHand();
		}
	}

	// Token: 0x04002B6C RID: 11116
	[SerializeField]
	private Transform _hand;

	// Token: 0x04002B6D RID: 11117
	[SerializeField]
	private Text _text;

	// Token: 0x04002B6E RID: 11118
	[Space]
	[SerializeField]
	private StopwatchCosmetic _cosmetic;

	// Token: 0x04002B6F RID: 11119
	[Space]
	[SerializeField]
	private AudioClip _audioClick;

	// Token: 0x04002B70 RID: 11120
	[SerializeField]
	private AudioClip _audioReset;

	// Token: 0x04002B71 RID: 11121
	[SerializeField]
	private AudioClip _audioTick;

	// Token: 0x04002B72 RID: 11122
	[Space]
	[NonSerialized]
	private int _millisElapsed;

	// Token: 0x04002B73 RID: 11123
	[NonSerialized]
	private bool _watchActive;

	// Token: 0x04002B74 RID: 11124
	[NonSerialized]
	private LerpTask<int> _lerpToZero;
}
