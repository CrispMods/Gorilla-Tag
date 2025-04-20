using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000368 RID: 872
public class StartMenu : MonoBehaviour
{
	// Token: 0x06001456 RID: 5206 RVA: 0x000BBA1C File Offset: 0x000B9C1C
	private void Start()
	{
		DebugUIBuilder.instance.AddLabel("Select Sample Scene", 0);
		int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
		for (int i = 0; i < sceneCountInBuildSettings; i++)
		{
			string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
			int sceneIndex = i;
			DebugUIBuilder.instance.AddButton(Path.GetFileNameWithoutExtension(scenePathByBuildIndex), delegate
			{
				this.LoadScene(sceneIndex);
			}, -1, 0, false);
		}
		DebugUIBuilder.instance.Show();
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x0003DAD2 File Offset: 0x0003BCD2
	private void LoadScene(int idx)
	{
		DebugUIBuilder.instance.Hide();
		Debug.Log("Load scene: " + idx.ToString());
		SceneManager.LoadScene(idx);
	}

	// Token: 0x04001671 RID: 5745
	public OVROverlay overlay;

	// Token: 0x04001672 RID: 5746
	public OVROverlay text;

	// Token: 0x04001673 RID: 5747
	public OVRCameraRig vrRig;
}
