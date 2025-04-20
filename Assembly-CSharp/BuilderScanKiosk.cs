using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GorillaTagScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004F6 RID: 1270
public class BuilderScanKiosk : MonoBehaviour
{
	// Token: 0x06001ED4 RID: 7892 RVA: 0x000EBD10 File Offset: 0x000E9F10
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

	// Token: 0x06001ED5 RID: 7893 RVA: 0x000EBE4C File Offset: 0x000EA04C
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

	// Token: 0x06001ED6 RID: 7894 RVA: 0x000EBF6C File Offset: 0x000EA16C
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

	// Token: 0x06001ED7 RID: 7895 RVA: 0x000EBFC0 File Offset: 0x000EA1C0
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

	// Token: 0x06001ED8 RID: 7896 RVA: 0x000EC040 File Offset: 0x000EA240
	private void LoadPlayerPrefs()
	{
		int @int = PlayerPrefs.GetInt(BuilderScanKiosk.playerPrefKey, -1);
		this.targetTable.CurrentSaveSlot = @int;
		this.UpdateUI();
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x00044E84 File Offset: 0x00043084
	private void SavePlayerPrefs()
	{
		PlayerPrefs.SetInt(BuilderScanKiosk.playerPrefKey, this.targetTable.CurrentSaveSlot);
		PlayerPrefs.Save();
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x000EC06C File Offset: 0x000EA26C
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

	// Token: 0x06001EDB RID: 7899 RVA: 0x000EC0CC File Offset: 0x000EA2CC
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

	// Token: 0x06001EDC RID: 7900 RVA: 0x000EC13C File Offset: 0x000EA33C
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

	// Token: 0x06001EDD RID: 7901 RVA: 0x000EC250 File Offset: 0x000EA450
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

	// Token: 0x06001EDE RID: 7902 RVA: 0x00044EA0 File Offset: 0x000430A0
	private string GetSaveFolder()
	{
		return Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + BuilderScanKiosk.SAVE_FOLDER;
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x00044EBB File Offset: 0x000430BB
	private void OnSaveDirtyChanged(bool dirty)
	{
		this.isDirty = dirty;
		this.UpdateUI();
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x00044ECA File Offset: 0x000430CA
	private void OnSaveTimeUpdated()
	{
		this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		this.saveError = false;
		this.UpdateUI();
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x00044EE0 File Offset: 0x000430E0
	private void OnSaveFail(string errorMsg)
	{
		this.scannerState = BuilderScanKiosk.ScannerState.IDLE;
		this.saveError = true;
		this.errorMsg = errorMsg;
		this.UpdateUI();
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x000EC2AC File Offset: 0x000EA4AC
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

	// Token: 0x06001EE3 RID: 7907 RVA: 0x000EC3B4 File Offset: 0x000EA5B4
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

	// Token: 0x04002271 RID: 8817
	[SerializeField]
	private GorillaPressableButton saveButton;

	// Token: 0x04002272 RID: 8818
	[SerializeField]
	private GorillaPressableButton noneButton;

	// Token: 0x04002273 RID: 8819
	[SerializeField]
	private List<GorillaPressableButton> scanButtons;

	// Token: 0x04002274 RID: 8820
	[SerializeField]
	private BuilderTable targetTable;

	// Token: 0x04002275 RID: 8821
	[SerializeField]
	private float saveCooldownSeconds = 5f;

	// Token: 0x04002276 RID: 8822
	[SerializeField]
	private TMP_Text screenText;

	// Token: 0x04002277 RID: 8823
	[SerializeField]
	private SoundBankPlayer soundBank;

	// Token: 0x04002278 RID: 8824
	[SerializeField]
	private Animation scanAnimation;

	// Token: 0x04002279 RID: 8825
	private MeshRenderer scanTriangle;

	// Token: 0x0400227A RID: 8826
	private bool isAnimating;

	// Token: 0x0400227B RID: 8827
	private static string playerPrefKey = "BuilderSaveSlot";

	// Token: 0x0400227C RID: 8828
	private static string SAVE_FOLDER = "MonkeBlocks";

	// Token: 0x0400227D RID: 8829
	private static string SAVE_FILE = "MyBuild";

	// Token: 0x0400227E RID: 8830
	public static int NUM_SAVE_SLOTS = 3;

	// Token: 0x0400227F RID: 8831
	private Texture2D buildCaptureTexture;

	// Token: 0x04002280 RID: 8832
	private bool isDirty;

	// Token: 0x04002281 RID: 8833
	private bool saveError;

	// Token: 0x04002282 RID: 8834
	private string errorMsg = string.Empty;

	// Token: 0x04002283 RID: 8835
	private bool coolingDown;

	// Token: 0x04002284 RID: 8836
	private double coolDownCompleteTime;

	// Token: 0x04002285 RID: 8837
	private double scanCompleteTime;

	// Token: 0x04002286 RID: 8838
	private BuilderScanKiosk.ScannerState scannerState;

	// Token: 0x020004F7 RID: 1271
	private enum ScannerState
	{
		// Token: 0x04002288 RID: 8840
		IDLE,
		// Token: 0x04002289 RID: 8841
		CONFIRMATION,
		// Token: 0x0400228A RID: 8842
		SAVING
	}
}
