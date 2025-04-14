using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A3 RID: 1187
public class MonkeBallResetGame : MonoBehaviour
{
	// Token: 0x06001CDC RID: 7388 RVA: 0x0008CFFC File Offset: 0x0008B1FC
	private void Awake()
	{
		this._resetButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
		if (this._resetButton == null)
		{
			this._buttonOrigin = this._resetButton.transform.position;
		}
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x0008D049 File Offset: 0x0008B249
	private void Update()
	{
		if (this._cooldown)
		{
			this._cooldownTimer -= Time.deltaTime;
			if (this._cooldownTimer <= 0f)
			{
				this.ToggleButton(false, -1);
				this._cooldown = false;
			}
		}
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x0008D084 File Offset: 0x0008B284
	public void ToggleReset(bool toggle, int teamId, bool force = false)
	{
		if (teamId < -1 || teamId >= this.teamMaterials.Length)
		{
			return;
		}
		if (toggle)
		{
			this.ToggleButton(true, teamId);
			this._cooldown = false;
			return;
		}
		if (force)
		{
			this.ToggleButton(false, -1);
			return;
		}
		this._cooldown = true;
		this._cooldownTimer = 3f;
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x0008D0D4 File Offset: 0x0008B2D4
	private void ToggleButton(bool toggle, int teamId)
	{
		this._resetButton.enabled = toggle;
		this.allowedTeamId = teamId;
		if (!toggle || teamId == -1)
		{
			this.button.sharedMaterial = this.neutralMaterial;
			return;
		}
		this.button.sharedMaterial = this.teamMaterials[teamId];
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x0008D120 File Offset: 0x0008B320
	private void OnSelect()
	{
		MonkeBallGame.Instance.RequestResetGame();
	}

	// Token: 0x04001FC3 RID: 8131
	[SerializeField]
	private GorillaPressableButton _resetButton;

	// Token: 0x04001FC4 RID: 8132
	public Renderer button;

	// Token: 0x04001FC5 RID: 8133
	public Vector3 buttonPressOffset;

	// Token: 0x04001FC6 RID: 8134
	private Vector3 _buttonOrigin = Vector3.zero;

	// Token: 0x04001FC7 RID: 8135
	[Space]
	public Material[] teamMaterials;

	// Token: 0x04001FC8 RID: 8136
	public Material neutralMaterial;

	// Token: 0x04001FC9 RID: 8137
	public int allowedTeamId = -1;

	// Token: 0x04001FCA RID: 8138
	[SerializeField]
	private TextMeshPro _resetLabel;

	// Token: 0x04001FCB RID: 8139
	private bool _cooldown;

	// Token: 0x04001FCC RID: 8140
	private float _cooldownTimer;
}
