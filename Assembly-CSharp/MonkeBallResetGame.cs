using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004A3 RID: 1187
public class MonkeBallResetGame : MonoBehaviour
{
	// Token: 0x06001CD9 RID: 7385 RVA: 0x0008CC78 File Offset: 0x0008AE78
	private void Awake()
	{
		this._resetButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
		if (this._resetButton == null)
		{
			this._buttonOrigin = this._resetButton.transform.position;
		}
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x0008CCC5 File Offset: 0x0008AEC5
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

	// Token: 0x06001CDB RID: 7387 RVA: 0x0008CD00 File Offset: 0x0008AF00
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

	// Token: 0x06001CDC RID: 7388 RVA: 0x0008CD50 File Offset: 0x0008AF50
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

	// Token: 0x06001CDD RID: 7389 RVA: 0x0008CD9C File Offset: 0x0008AF9C
	private void OnSelect()
	{
		MonkeBallGame.Instance.RequestResetGame();
	}

	// Token: 0x04001FC2 RID: 8130
	[SerializeField]
	private GorillaPressableButton _resetButton;

	// Token: 0x04001FC3 RID: 8131
	public Renderer button;

	// Token: 0x04001FC4 RID: 8132
	public Vector3 buttonPressOffset;

	// Token: 0x04001FC5 RID: 8133
	private Vector3 _buttonOrigin = Vector3.zero;

	// Token: 0x04001FC6 RID: 8134
	[Space]
	public Material[] teamMaterials;

	// Token: 0x04001FC7 RID: 8135
	public Material neutralMaterial;

	// Token: 0x04001FC8 RID: 8136
	public int allowedTeamId = -1;

	// Token: 0x04001FC9 RID: 8137
	[SerializeField]
	private TextMeshPro _resetLabel;

	// Token: 0x04001FCA RID: 8138
	private bool _cooldown;

	// Token: 0x04001FCB RID: 8139
	private float _cooldownTimer;
}
