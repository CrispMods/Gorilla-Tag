using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200032E RID: 814
public class SceneSampler : MonoBehaviour
{
	// Token: 0x0600135E RID: 4958 RVA: 0x0003D2C1 File Offset: 0x0003B4C1
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x000B6E10 File Offset: 0x000B5010
	private void Update()
	{
		bool active = OVRInput.GetActiveController() == OVRInput.Controller.Touch || OVRInput.GetActiveController() == OVRInput.Controller.LTouch || OVRInput.GetActiveController() == OVRInput.Controller.RTouch;
		this.displayText.SetActive(active);
		if (OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Active))
		{
			this.currentSceneIndex++;
			if (this.currentSceneIndex >= SceneManager.sceneCountInBuildSettings)
			{
				this.currentSceneIndex = 0;
			}
			SceneManager.LoadScene(this.currentSceneIndex);
		}
		Vector3 vector = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + Vector3.up * 0.09f;
		this.displayText.transform.position = vector;
		this.displayText.transform.rotation = Quaternion.LookRotation(vector - Camera.main.transform.position);
	}

	// Token: 0x0400155C RID: 5468
	private int currentSceneIndex;

	// Token: 0x0400155D RID: 5469
	public GameObject displayText;
}
