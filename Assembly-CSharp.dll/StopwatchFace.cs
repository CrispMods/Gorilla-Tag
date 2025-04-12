using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200063D RID: 1597
public class StopwatchFace : MonoBehaviour
{
	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x060027A8 RID: 10152 RVA: 0x0004A22B File Offset: 0x0004842B
	public bool watchActive
	{
		get
		{
			return this._watchActive;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x060027A9 RID: 10153 RVA: 0x0004A233 File Offset: 0x00048433
	public int millisElapsed
	{
		get
		{
			return this._millisElapsed;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x060027AA RID: 10154 RVA: 0x0004A23B File Offset: 0x0004843B
	public Vector3Int digitsMmSsMs
	{
		get
		{
			return StopwatchFace.ParseDigits(TimeSpan.FromMilliseconds((double)this._millisElapsed));
		}
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x0004A24E File Offset: 0x0004844E
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

	// Token: 0x060027AC RID: 10156 RVA: 0x0004A267 File Offset: 0x00048467
	private void Awake()
	{
		this._lerpToZero = new LerpTask<int>();
		this._lerpToZero.onLerp = new Action<int, int, float>(this.OnLerpToZero);
		this._lerpToZero.onLerpEnd = new Action(this.OnLerpEnd);
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x0004A2A2 File Offset: 0x000484A2
	private void OnLerpToZero(int a, int b, float t)
	{
		this._millisElapsed = Mathf.FloorToInt(Mathf.Lerp((float)a, (float)b, t * t));
		this.UpdateText();
		this.UpdateHand();
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x0004A2C7 File Offset: 0x000484C7
	private void OnLerpEnd()
	{
		this.WatchReset(false);
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x0004A2C7 File Offset: 0x000484C7
	private void OnEnable()
	{
		this.WatchReset(false);
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x0004A2C7 File Offset: 0x000484C7
	private void OnDisable()
	{
		this.WatchReset(false);
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x0010AF80 File Offset: 0x00109180
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

	// Token: 0x060027B2 RID: 10162 RVA: 0x0010AFD8 File Offset: 0x001091D8
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

	// Token: 0x060027B3 RID: 10163 RVA: 0x0010B040 File Offset: 0x00109240
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

	// Token: 0x060027B4 RID: 10164 RVA: 0x0010B0D4 File Offset: 0x001092D4
	private void UpdateHand()
	{
		float z = (float)(this._millisElapsed % 60000) / 60000f * 360f;
		this._hand.localEulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x0004A2D0 File Offset: 0x000484D0
	public void WatchToggle()
	{
		if (!this._watchActive)
		{
			this.WatchStart();
			return;
		}
		this.WatchStop();
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x0004A2E7 File Offset: 0x000484E7
	public void WatchStart()
	{
		if (this._lerpToZero.active)
		{
			return;
		}
		this._watchActive = true;
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x0004A2FE File Offset: 0x000484FE
	public void WatchStop()
	{
		if (this._lerpToZero.active)
		{
			return;
		}
		this._watchActive = false;
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x0004A315 File Offset: 0x00048515
	public void WatchReset()
	{
		this.WatchReset(true);
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x0010B118 File Offset: 0x00109318
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

	// Token: 0x04002B72 RID: 11122
	[SerializeField]
	private Transform _hand;

	// Token: 0x04002B73 RID: 11123
	[SerializeField]
	private Text _text;

	// Token: 0x04002B74 RID: 11124
	[Space]
	[SerializeField]
	private StopwatchCosmetic _cosmetic;

	// Token: 0x04002B75 RID: 11125
	[Space]
	[SerializeField]
	private AudioClip _audioClick;

	// Token: 0x04002B76 RID: 11126
	[SerializeField]
	private AudioClip _audioReset;

	// Token: 0x04002B77 RID: 11127
	[SerializeField]
	private AudioClip _audioTick;

	// Token: 0x04002B78 RID: 11128
	[Space]
	[NonSerialized]
	private int _millisElapsed;

	// Token: 0x04002B79 RID: 11129
	[NonSerialized]
	private bool _watchActive;

	// Token: 0x04002B7A RID: 11130
	[NonSerialized]
	private LerpTask<int> _lerpToZero;
}
