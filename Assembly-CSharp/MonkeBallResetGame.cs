using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004AF RID: 1199
public class MonkeBallResetGame : MonoBehaviour
{
	// Token: 0x06001D2D RID: 7469 RVA: 0x000E01D4 File Offset: 0x000DE3D4
	private void Awake()
	{
		this._resetButton.onPressButton.AddListener(new UnityAction(this.OnSelect));
		if (this._resetButton == null)
		{
			this._buttonOrigin = this._resetButton.transform.position;
		}
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x00043F40 File Offset: 0x00042140
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

	// Token: 0x06001D2F RID: 7471 RVA: 0x000E0224 File Offset: 0x000DE424
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

	// Token: 0x06001D30 RID: 7472 RVA: 0x000E0274 File Offset: 0x000DE474
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

	// Token: 0x06001D31 RID: 7473 RVA: 0x00043F78 File Offset: 0x00042178
	private void OnSelect()
	{
		MonkeBallGame.Instance.RequestResetGame();
	}

	// Token: 0x04002011 RID: 8209
	[SerializeField]
	private GorillaPressableButton _resetButton;

	// Token: 0x04002012 RID: 8210
	public Renderer button;

	// Token: 0x04002013 RID: 8211
	public Vector3 buttonPressOffset;

	// Token: 0x04002014 RID: 8212
	private Vector3 _buttonOrigin = Vector3.zero;

	// Token: 0x04002015 RID: 8213
	[Space]
	public Material[] teamMaterials;

	// Token: 0x04002016 RID: 8214
	public Material neutralMaterial;

	// Token: 0x04002017 RID: 8215
	public int allowedTeamId = -1;

	// Token: 0x04002018 RID: 8216
	[SerializeField]
	private TextMeshPro _resetLabel;

	// Token: 0x04002019 RID: 8217
	private bool _cooldown;

	// Token: 0x0400201A RID: 8218
	private float _cooldownTimer;
}
