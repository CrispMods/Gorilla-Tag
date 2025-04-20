using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200061B RID: 1563
public class StopwatchFace : MonoBehaviour
{
	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x060026CB RID: 9931 RVA: 0x0004A7C0 File Offset: 0x000489C0
	public bool watchActive
	{
		get
		{
			return this._watchActive;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x060026CC RID: 9932 RVA: 0x0004A7C8 File Offset: 0x000489C8
	public int millisElapsed
	{
		get
		{
			return this._millisElapsed;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x060026CD RID: 9933 RVA: 0x0004A7D0 File Offset: 0x000489D0
	public Vector3Int digitsMmSsMs
	{
		get
		{
			return StopwatchFace.ParseDigits(TimeSpan.FromMilliseconds((double)this._millisElapsed));
		}
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x0004A7E3 File Offset: 0x000489E3
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

	// Token: 0x060026CF RID: 9935 RVA: 0x0004A7FC File Offset: 0x000489FC
	private void Awake()
	{
		this._lerpToZero = new LerpTask<int>();
		this._lerpToZero.onLerp = new Action<int, int, float>(this.OnLerpToZero);
		this._lerpToZero.onLerpEnd = new Action(this.OnLerpEnd);
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x0004A837 File Offset: 0x00048A37
	private void OnLerpToZero(int a, int b, float t)
	{
		this._millisElapsed = Mathf.FloorToInt(Mathf.Lerp((float)a, (float)b, t * t));
		this.UpdateText();
		this.UpdateHand();
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x0004A85C File Offset: 0x00048A5C
	private void OnLerpEnd()
	{
		this.WatchReset(false);
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x0004A85C File Offset: 0x00048A5C
	private void OnEnable()
	{
		this.WatchReset(false);
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x0004A85C File Offset: 0x00048A5C
	private void OnDisable()
	{
		this.WatchReset(false);
	}

	// Token: 0x060026D4 RID: 9940 RVA: 0x001093A8 File Offset: 0x001075A8
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

	// Token: 0x060026D5 RID: 9941 RVA: 0x00109400 File Offset: 0x00107600
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

	// Token: 0x060026D6 RID: 9942 RVA: 0x00109468 File Offset: 0x00107668
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

	// Token: 0x060026D7 RID: 9943 RVA: 0x001094FC File Offset: 0x001076FC
	private void UpdateHand()
	{
		float z = (float)(this._millisElapsed % 60000) / 60000f * 360f;
		this._hand.localEulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x060026D8 RID: 9944 RVA: 0x0004A865 File Offset: 0x00048A65
	public void WatchToggle()
	{
		if (!this._watchActive)
		{
			this.WatchStart();
			return;
		}
		this.WatchStop();
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x0004A87C File Offset: 0x00048A7C
	public void WatchStart()
	{
		if (this._lerpToZero.active)
		{
			return;
		}
		this._watchActive = true;
	}

	// Token: 0x060026DA RID: 9946 RVA: 0x0004A893 File Offset: 0x00048A93
	public void WatchStop()
	{
		if (this._lerpToZero.active)
		{
			return;
		}
		this._watchActive = false;
	}

	// Token: 0x060026DB RID: 9947 RVA: 0x0004A8AA File Offset: 0x00048AAA
	public void WatchReset()
	{
		this.WatchReset(true);
	}

	// Token: 0x060026DC RID: 9948 RVA: 0x00109540 File Offset: 0x00107740
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

	// Token: 0x04002AD2 RID: 10962
	[SerializeField]
	private Transform _hand;

	// Token: 0x04002AD3 RID: 10963
	[SerializeField]
	private Text _text;

	// Token: 0x04002AD4 RID: 10964
	[Space]
	[SerializeField]
	private StopwatchCosmetic _cosmetic;

	// Token: 0x04002AD5 RID: 10965
	[Space]
	[SerializeField]
	private AudioClip _audioClick;

	// Token: 0x04002AD6 RID: 10966
	[SerializeField]
	private AudioClip _audioReset;

	// Token: 0x04002AD7 RID: 10967
	[SerializeField]
	private AudioClip _audioTick;

	// Token: 0x04002AD8 RID: 10968
	[Space]
	[NonSerialized]
	private int _millisElapsed;

	// Token: 0x04002AD9 RID: 10969
	[NonSerialized]
	private bool _watchActive;

	// Token: 0x04002ADA RID: 10970
	[NonSerialized]
	private LerpTask<int> _lerpToZero;
}
