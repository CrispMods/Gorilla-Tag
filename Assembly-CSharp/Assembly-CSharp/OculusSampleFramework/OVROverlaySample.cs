using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OculusSampleFramework
{
	// Token: 0x02000A7F RID: 2687
	public class OVROverlaySample : MonoBehaviour
	{
		// Token: 0x060042F0 RID: 17136 RVA: 0x0013B780 File Offset: 0x00139980
		private void Start()
		{
			DebugUIBuilder.instance.AddLabel("OVROverlay Sample", 0);
			DebugUIBuilder.instance.AddDivider(0);
			DebugUIBuilder.instance.AddLabel("Level Loading Example", 0);
			DebugUIBuilder.instance.AddButton("Simulate Level Load", new DebugUIBuilder.OnClick(this.TriggerLoad), -1, 0, false);
			DebugUIBuilder.instance.AddButton("Destroy Cubes", new DebugUIBuilder.OnClick(this.TriggerUnload), -1, 0, false);
			DebugUIBuilder.instance.AddDivider(0);
			DebugUIBuilder.instance.AddLabel("OVROverlay vs. Application Render Comparison", 0);
			DebugUIBuilder.instance.AddRadio("OVROverlay", "group", delegate(Toggle t)
			{
				this.RadioPressed("OVROverlayID", "group", t);
			}, 0).GetComponentInChildren<Toggle>();
			this.applicationRadioButton = DebugUIBuilder.instance.AddRadio("Application", "group", delegate(Toggle t)
			{
				this.RadioPressed("ApplicationID", "group", t);
			}, 0).GetComponentInChildren<Toggle>();
			this.noneRadioButton = DebugUIBuilder.instance.AddRadio("None", "group", delegate(Toggle t)
			{
				this.RadioPressed("NoneID", "group", t);
			}, 0).GetComponentInChildren<Toggle>();
			DebugUIBuilder.instance.Show();
			this.CameraAndRenderTargetSetup();
			this.cameraRenderOverlay.enabled = true;
			this.cameraRenderOverlay.currentOverlayShape = OVROverlay.OverlayShape.Quad;
			this.spawnedCubes.Capacity = this.numObjectsPerLevel * this.numLevels;
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x0013B8D8 File Offset: 0x00139AD8
		private void Update()
		{
			if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Active) || OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Active))
			{
				if (this.inMenu)
				{
					DebugUIBuilder.instance.Hide();
				}
				else
				{
					DebugUIBuilder.instance.Show();
				}
				this.inMenu = !this.inMenu;
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				this.TriggerLoad();
			}
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x0013B940 File Offset: 0x00139B40
		private void ActivateWorldGeo()
		{
			this.worldspaceGeoParent.SetActive(true);
			this.uiGeoParent.SetActive(false);
			this.uiCamera.SetActive(false);
			this.cameraRenderOverlay.enabled = false;
			this.renderingLabelOverlay.enabled = true;
			this.renderingLabelOverlay.textures[0] = this.applicationLabelTexture;
			Debug.Log("Switched to ActivateWorldGeo");
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x0013B9A8 File Offset: 0x00139BA8
		private void ActivateOVROverlay()
		{
			this.worldspaceGeoParent.SetActive(false);
			this.uiCamera.SetActive(true);
			this.cameraRenderOverlay.enabled = true;
			this.uiGeoParent.SetActive(true);
			this.renderingLabelOverlay.enabled = true;
			this.renderingLabelOverlay.textures[0] = this.compositorLabelTexture;
			Debug.Log("Switched to ActivateOVROVerlay");
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x0013BA10 File Offset: 0x00139C10
		private void ActivateNone()
		{
			this.worldspaceGeoParent.SetActive(false);
			this.uiCamera.SetActive(false);
			this.cameraRenderOverlay.enabled = false;
			this.uiGeoParent.SetActive(false);
			this.renderingLabelOverlay.enabled = false;
			Debug.Log("Switched to ActivateNone");
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x0013BA63 File Offset: 0x00139C63
		private void TriggerLoad()
		{
			base.StartCoroutine(this.WaitforOVROverlay());
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x0013BA72 File Offset: 0x00139C72
		private IEnumerator WaitforOVROverlay()
		{
			Transform transform = this.mainCamera.transform;
			Transform transform2 = this.loadingTextQuadOverlay.transform;
			Vector3 position = transform.position + transform.forward * this.distanceFromCamToLoadText;
			position.y = transform.position.y;
			transform2.position = position;
			this.cubemapOverlay.enabled = true;
			this.loadingTextQuadOverlay.enabled = true;
			this.noneRadioButton.isOn = true;
			yield return new WaitForSeconds(0.1f);
			this.ClearObjects();
			this.SimulateLevelLoad();
			this.cubemapOverlay.enabled = false;
			this.loadingTextQuadOverlay.enabled = false;
			yield return null;
			yield break;
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x0013BA81 File Offset: 0x00139C81
		private void TriggerUnload()
		{
			this.ClearObjects();
			this.applicationRadioButton.isOn = true;
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x0013BA98 File Offset: 0x00139C98
		private void CameraAndRenderTargetSetup()
		{
			float x = this.cameraRenderOverlay.transform.localScale.x;
			float y = this.cameraRenderOverlay.transform.localScale.y;
			float z = this.cameraRenderOverlay.transform.localScale.z;
			float num = 2160f;
			float num2 = 1200f;
			float num3 = num * 0.5f;
			float num4 = num2;
			float num5 = this.mainCamera.GetComponent<Camera>().fieldOfView / 2f;
			float num6 = 2f * z * Mathf.Tan(0.017453292f * num5);
			float num7 = num4 / num6 * x;
			float num8 = num6 * this.mainCamera.GetComponent<Camera>().aspect;
			float num9 = num3 / num8 * x;
			float orthographicSize = y / 2f;
			float aspect = x / y;
			this.uiCamera.GetComponent<Camera>().orthographicSize = orthographicSize;
			this.uiCamera.GetComponent<Camera>().aspect = aspect;
			if (this.uiCamera.GetComponent<Camera>().targetTexture != null)
			{
				this.uiCamera.GetComponent<Camera>().targetTexture.Release();
			}
			RenderTexture renderTexture = new RenderTexture((int)num9 * 2, (int)num7 * 2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			Debug.Log("Created RT of resolution w: " + num9.ToString() + " and h: " + num7.ToString());
			renderTexture.hideFlags = HideFlags.DontSave;
			renderTexture.useMipMap = true;
			renderTexture.filterMode = FilterMode.Trilinear;
			renderTexture.anisoLevel = 4;
			renderTexture.autoGenerateMips = true;
			this.uiCamera.GetComponent<Camera>().targetTexture = renderTexture;
			this.cameraRenderOverlay.textures[0] = renderTexture;
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x0013BC34 File Offset: 0x00139E34
		private void SimulateLevelLoad()
		{
			int num = 0;
			for (int i = 0; i < this.numLoopsTrigger; i++)
			{
				num++;
			}
			Debug.Log("Finished " + num.ToString() + " Loops");
			Vector3 position = this.mainCamera.transform.position;
			position.y = 0.5f;
			for (int j = 0; j < this.numLevels; j++)
			{
				for (int k = 0; k < this.numObjectsPerLevel; k++)
				{
					float f = (float)k * 3.1415927f * 2f / (float)this.numObjectsPerLevel;
					float d = (k % 2 == 0) ? 1.5f : 1f;
					Vector3 a = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * this.cubeSpawnRadius * d;
					a.y = (float)j * this.heightBetweenItems;
					GameObject gameObject = Object.Instantiate<GameObject>(this.prefabForLevelLoadSim, a + position, Quaternion.identity);
					Transform transform = gameObject.transform;
					transform.LookAt(position);
					Vector3 eulerAngles = transform.rotation.eulerAngles;
					eulerAngles.x = 0f;
					transform.rotation = Quaternion.Euler(eulerAngles);
					this.spawnedCubes.Add(gameObject);
				}
			}
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x0013BD8C File Offset: 0x00139F8C
		private void ClearObjects()
		{
			for (int i = 0; i < this.spawnedCubes.Count; i++)
			{
				Object.DestroyImmediate(this.spawnedCubes[i]);
			}
			this.spawnedCubes.Clear();
			GC.Collect();
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0013BDD0 File Offset: 0x00139FD0
		public void RadioPressed(string radioLabel, string group, Toggle t)
		{
			if (string.Compare(radioLabel, "OVROverlayID") == 0)
			{
				this.ActivateOVROverlay();
				return;
			}
			if (string.Compare(radioLabel, "ApplicationID") == 0)
			{
				this.ActivateWorldGeo();
				return;
			}
			if (string.Compare(radioLabel, "NoneID") == 0)
			{
				this.ActivateNone();
			}
		}

		// Token: 0x04004414 RID: 17428
		private bool inMenu;

		// Token: 0x04004415 RID: 17429
		private const string ovrOverlayID = "OVROverlayID";

		// Token: 0x04004416 RID: 17430
		private const string applicationID = "ApplicationID";

		// Token: 0x04004417 RID: 17431
		private const string noneID = "NoneID";

		// Token: 0x04004418 RID: 17432
		private Toggle applicationRadioButton;

		// Token: 0x04004419 RID: 17433
		private Toggle noneRadioButton;

		// Token: 0x0400441A RID: 17434
		[Header("App vs Compositor Comparison Settings")]
		public GameObject mainCamera;

		// Token: 0x0400441B RID: 17435
		public GameObject uiCamera;

		// Token: 0x0400441C RID: 17436
		public GameObject uiGeoParent;

		// Token: 0x0400441D RID: 17437
		public GameObject worldspaceGeoParent;

		// Token: 0x0400441E RID: 17438
		public OVROverlay cameraRenderOverlay;

		// Token: 0x0400441F RID: 17439
		public OVROverlay renderingLabelOverlay;

		// Token: 0x04004420 RID: 17440
		public Texture applicationLabelTexture;

		// Token: 0x04004421 RID: 17441
		public Texture compositorLabelTexture;

		// Token: 0x04004422 RID: 17442
		[Header("Level Loading Sim Settings")]
		public GameObject prefabForLevelLoadSim;

		// Token: 0x04004423 RID: 17443
		public OVROverlay cubemapOverlay;

		// Token: 0x04004424 RID: 17444
		public OVROverlay loadingTextQuadOverlay;

		// Token: 0x04004425 RID: 17445
		public float distanceFromCamToLoadText;

		// Token: 0x04004426 RID: 17446
		public float cubeSpawnRadius;

		// Token: 0x04004427 RID: 17447
		public float heightBetweenItems;

		// Token: 0x04004428 RID: 17448
		public int numObjectsPerLevel;

		// Token: 0x04004429 RID: 17449
		public int numLevels;

		// Token: 0x0400442A RID: 17450
		public int numLoopsTrigger = 500000000;

		// Token: 0x0400442B RID: 17451
		private List<GameObject> spawnedCubes = new List<GameObject>();
	}
}
