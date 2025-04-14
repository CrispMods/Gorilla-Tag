using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GorillaTagScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004E9 RID: 1257
public class BuilderScanKiosk : MonoBehaviour
{
	// Token: 0x06001E7E RID: 7806 RVA: 0x000999B0 File Offset: 0x00097BB0
	private void Start()
	{
		if (this.saveButton != null)
		{
			this.saveButton.onPressButton.AddListener(new UnityAction(this.OnSavePressed));
		}
		if (this.targetTable != null)
		{
			this.targetTable.OnSaveDirtyChanged.AddListener(new UnityAction<bool>(this.OnSaveDirtyChanged));
			this.targetTable.OnSaveTimeUpdated.AddListener(new UnityAction(this.OnSaveTimeUpdated));
			this.targetTable.OnSaveFailure.AddListener(new UnityAction<string>(this.OnSaveFail));
		}
		if (this.noneButton != null)
		{
			this.noneButton.onPressButton.AddListener(new UnityAction(this.OnNoneButtonPressed));
		}
		foreach (GorillaPressableButton gorillaPressableButton in this.scanButtons)
		{
			gorillaPressableButton.onPressed += this.OnScanButtonPressed;
		}
		this.scanTriangle = this.scanAnimation.GetComponent<MeshRenderer>();
		this.scanTriangle.enabled = false;
		this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		this.LoadPlayerPrefs();
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x00099AEC File Offset: 0x00097CEC
	private void OnDestroy()
	{
		if (this.saveButton != null)
		{
			this.saveButton.onPressButton.RemoveListener(new UnityAction(this.OnSavePressed));
		}
		if (this.targetTable != null)
		{
			this.targetTable.OnSaveDirtyChanged.RemoveListener(new UnityAction<bool>(this.OnSaveDirtyChanged));
			this.targetTable.OnSaveTimeUpdated.RemoveListener(new UnityAction(this.OnSaveTimeUpdated));
			this.targetTable.OnSaveFailure.RemoveListener(new UnityAction<string>(this.OnSaveFail));
		}
		if (this.noneButton != null)
		{
			this.noneButton.onPressButton.RemoveListener(new UnityAction(this.OnNoneButtonPressed));
		}
		foreach (GorillaPressableButton gorillaPressableButton in this.scanButtons)
		{
			if (!(gorillaPressableButton == null))
			{
				gorillaPressableButton.onPressed -= this.OnScanButtonPressed;
			}
		}
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x00099C0C File Offset: 0x00097E0C
	private void OnNoneButtonPressed()
	{
		if (this.targetTable == null)
		{
			return;
		}
		if (this.scannerState == BuilderScanKiosk.ScannerState.CONFIRMATION)
		{
			this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		}
		if (this.targetTable.CurrentSaveSlot != -1)
		{
			this.targetTable.CurrentSaveSlot = -1;
			this.SavePlayerPrefs();
			this.UpdateUI();
		}
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x00099C60 File Offset: 0x00097E60
	private void OnScanButtonPressed(GorillaPressableButton button, bool isLeft)
	{
		if (this.targetTable == null)
		{
			return;
		}
		if (this.scannerState == BuilderScanKiosk.ScannerState.CONFIRMATION)
		{
			this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		}
		int i = 0;
		while (i < this.scanButtons.Count)
		{
			if (button.Equals(this.scanButtons[i]))
			{
				if (i != this.targetTable.CurrentSaveSlot)
				{
					this.targetTable.CurrentSaveSlot = i;
					this.SavePlayerPrefs();
					this.UpdateUI();
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x00099CE0 File Offset: 0x00097EE0
	private void LoadPlayerPrefs()
	{
		int @int = PlayerPrefs.GetInt(BuilderScanKiosk.playerPrefKey, -1);
		this.targetTable.CurrentSaveSlot = @int;
		this.UpdateUI();
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x00099D0B File Offset: 0x00097F0B
	private void SavePlayerPrefs()
	{
		PlayerPrefs.SetInt(BuilderScanKiosk.playerPrefKey, this.targetTable.CurrentSaveSlot);
		PlayerPrefs.Save();
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x00099D28 File Offset: 0x00097F28
	private void ToggleSaveButton(bool enabled)
	{
		if (enabled)
		{
			this.saveButton.enabled = true;
			this.saveButton.buttonRenderer.material = this.saveButton.unpressedMaterial;
			return;
		}
		this.saveButton.enabled = false;
		this.saveButton.buttonRenderer.material = this.saveButton.pressedMaterial;
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x00099D88 File Offset: 0x00097F88
	private void Update()
	{
		if (this.isAnimating)
		{
			if (this.scanAnimation == null)
			{
				this.isAnimating = false;
			}
			else if ((double)Time.time > this.scanCompleteTime)
			{
				this.scanTriangle.enabled = false;
				this.isAnimating = false;
			}
		}
		if (this.coolingDown && (double)Time.time > this.coolDownCompleteTime)
		{
			this.coolingDown = false;
			this.UpdateUI();
		}
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x00099DF8 File Offset: 0x00097FF8
	private void OnSavePressed()
	{
		if (this.targetTable == null || !this.isDirty || this.coolingDown)
		{
			return;
		}
		switch (this.scannerState)
		{
		case BuilderScanKiosk.ScannerState.IDLE:
			this.scannerState = BuilderScanKiosk.ScannerState.CONFIRMATION;
			this.UpdateUI();
			return;
		case BuilderScanKiosk.ScannerState.CONFIRMATION:
			this.scannerState = BuilderScanKiosk.ScannerState.SAVING;
			if (this.scanAnimation != null)
			{
				this.scanCompleteTime = (double)(Time.time + this.scanAnimation.clip.length);
				this.scanTriangle.enabled = true;
				this.scanAnimation.Rewind();
				this.scanAnimation.Play();
			}
			if (this.soundBank != null)
			{
				this.soundBank.Play();
			}
			this.isAnimating = true;
			this.saveError = false;
			this.errorMsg = string.Empty;
			this.coolDownCompleteTime = (double)(Time.time + this.saveCooldownSeconds);
			this.coolingDown = true;
			this.UpdateUI();
			this.targetTable.SaveTableForPlayer();
			return;
		case BuilderScanKiosk.ScannerState.SAVING:
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x00099F0C File Offset: 0x0009810C
	private string GetSavePath()
	{
		return string.Concat(new string[]
		{
			this.GetSaveFolder(),
			Path.DirectorySeparatorChar.ToString(),
			BuilderScanKiosk.SAVE_FILE,
			"_",
			this.targetTable.CurrentSaveSlot.ToString(),
			".png"
		});
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00099F68 File Offset: 0x00098168
	private string GetSaveFolder()
	{
		return Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + BuilderScanKiosk.SAVE_FOLDER;
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x00099F83 File Offset: 0x00098183
	private void OnSaveDirtyChanged(bool dirty)
	{
		this.isDirty = dirty;
		this.UpdateUI();
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x00099F92 File Offset: 0x00098192
	private void OnSaveTimeUpdated()
	{
		this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		this.saveError = false;
		this.UpdateUI();
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x00099FA8 File Offset: 0x000981A8
	private void OnSaveFail(string errorMsg)
	{
		this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		this.saveError = true;
		this.errorMsg = errorMsg;
		this.UpdateUI();
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x00099FC8 File Offset: 0x000981C8
	private void UpdateUI()
	{
		this.screenText.text = this.GetTextForScreen();
		this.ToggleSaveButton(this.targetTable.CurrentSaveSlot >= 0 && !this.coolingDown);
		this.noneButton.buttonRenderer.material = ((this.targetTable.CurrentSaveSlot < 0) ? this.noneButton.pressedMaterial : this.noneButton.unpressedMaterial);
		for (int i = 0; i < this.scanButtons.Count; i++)
		{
			this.scanButtons[i].buttonRenderer.material = ((this.targetTable.CurrentSaveSlot == i) ? this.scanButtons[i].pressedMaterial : this.scanButtons[i].unpressedMaterial);
		}
		if (this.scannerState == BuilderScanKiosk.ScannerState.CONFIRMATION)
		{
			this.saveButton.myTmpText.text = "YES UPDATE SCAN";
			return;
		}
		this.saveButton.myTmpText.text = "UPDATE SCAN";
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x0009A0D0 File Offset: 0x000982D0
	private string GetTextForScreen()
	{
		if (this.targetTable == null)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (this.targetTable.CurrentSaveSlot < 0)
		{
			stringBuilder.Append("<b><color=red>NONE</color></b>");
		}
		else
		{
			int currentSaveSlot = this.targetTable.CurrentSaveSlot;
			stringBuilder.Append("<b><color=red>");
			stringBuilder.Append("SCAN ");
			stringBuilder.Append(currentSaveSlot + 1);
			stringBuilder.Append("</color></b>");
			stringBuilder.Append(": ");
			DateTime t = DateTime.FromBinary(this.targetTable.GetSaveTimeForSlot(currentSaveSlot));
			if (t > DateTime.MinValue)
			{
				stringBuilder.Append("UPDATED ");
				stringBuilder.Append(t.ToString());
			}
			else
			{
				stringBuilder.Append("EMPTY");
			}
		}
		stringBuilder.Append("\n\n");
		switch (this.scannerState)
		{
		case BuilderScanKiosk.ScannerState.IDLE:
			if (this.saveError)
			{
				stringBuilder.Append("ERROR WHILE SCANNING: ");
				stringBuilder.Append(this.errorMsg);
			}
			else if (this.coolingDown)
			{
				stringBuilder.Append("COOLING DOWN...");
			}
			else if (!this.isDirty)
			{
				stringBuilder.Append("NO UNSAVED CHANGES");
			}
			break;
		case BuilderScanKiosk.ScannerState.CONFIRMATION:
			stringBuilder.Append("YOU ARE ABOUT TO REPLACE ");
			stringBuilder.Append("<b><color=red>SCAN ");
			stringBuilder.Append(this.targetTable.CurrentSaveSlot + 1);
			stringBuilder.Append("</color></b>");
			stringBuilder.Append(" ARE YOU SURE YOU WANT TO SCAN?");
			break;
		case BuilderScanKiosk.ScannerState.SAVING:
			stringBuilder.Append("SCANNING BUILD...");
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		stringBuilder.Append("\n\n\n\n");
		stringBuilder.Append("CREATE A <b><color=red>NEW</color></b> PRIVATE ROOM TO LOAD ");
		if (this.targetTable.CurrentSaveSlot < 0)
		{
			stringBuilder.Append("<b><color=red>AN EMPTY TABLE</color></b>");
		}
		else
		{
			stringBuilder.Append("<b><color=red>");
			stringBuilder.Append("SCAN ");
			stringBuilder.Append(this.targetTable.CurrentSaveSlot + 1);
			stringBuilder.Append("</color></b>");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0400221F RID: 8735
	[SerializeField]
	private GorillaPressableButton saveButton;

	// Token: 0x04002220 RID: 8736
	[SerializeField]
	private GorillaPressableButton noneButton;

	// Token: 0x04002221 RID: 8737
	[SerializeField]
	private List<GorillaPressableButton> scanButtons;

	// Token: 0x04002222 RID: 8738
	[SerializeField]
	private BuilderTable targetTable;

	// Token: 0x04002223 RID: 8739
	[SerializeField]
	private float saveCooldownSeconds = 5f;

	// Token: 0x04002224 RID: 8740
	[SerializeField]
	private TMP_Text screenText;

	// Token: 0x04002225 RID: 8741
	[SerializeField]
	private SoundBankPlayer soundBank;

	// Token: 0x04002226 RID: 8742
	[SerializeField]
	private Animation scanAnimation;

	// Token: 0x04002227 RID: 8743
	private MeshRenderer scanTriangle;

	// Token: 0x04002228 RID: 8744
	private bool isAnimating;

	// Token: 0x04002229 RID: 8745
	private static string playerPrefKey = "BuilderSaveSlot";

	// Token: 0x0400222A RID: 8746
	private static string SAVE_FOLDER = "MonkeBlocks";

	// Token: 0x0400222B RID: 8747
	private static string SAVE_FILE = "MyBuild";

	// Token: 0x0400222C RID: 8748
	public static int NUM_SAVE_SLOTS = 3;

	// Token: 0x0400222D RID: 8749
	private Texture2D buildCaptureTexture;

	// Token: 0x0400222E RID: 8750
	private bool isDirty;

	// Token: 0x0400222F RID: 8751
	private bool saveError;

	// Token: 0x04002230 RID: 8752
	private string errorMsg = string.Empty;

	// Token: 0x04002231 RID: 8753
	private bool coolingDown;

	// Token: 0x04002232 RID: 8754
	private double coolDownCompleteTime;

	// Token: 0x04002233 RID: 8755
	private double scanCompleteTime;

	// Token: 0x04002234 RID: 8756
	private BuilderScanKiosk.ScannerState scannerState;

	// Token: 0x020004EA RID: 1258
	private enum ScannerState
	{
		// Token: 0x04002236 RID: 8758
		IDLE,
		// Token: 0x04002237 RID: 8759
		CONFIRMATION,
		// Token: 0x04002238 RID: 8760
		SAVING
	}
}
