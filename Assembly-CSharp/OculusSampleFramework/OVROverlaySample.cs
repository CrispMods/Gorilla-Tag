using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OculusSampleFramework
{
	// Token: 0x02000A7C RID: 2684
	public class OVROverlaySample : MonoBehaviour
	{
		// Token: 0x060042E4 RID: 17124 RVA: 0x0013B1B8 File Offset: 0x001393B8
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

		// Token: 0x060042E5 RID: 17125 RVA: 0x0013B310 File Offset: 0x00139510
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

		// Token: 0x060042E6 RID: 17126 RVA: 0x0013B378 File Offset: 0x00139578
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

		// Token: 0x060042E7 RID: 17127 RVA: 0x0013B3E0 File Offset: 0x001395E0
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

		// Token: 0x060042E8 RID: 17128 RVA: 0x0013B448 File Offset: 0x00139648
		private void ActivateNone()
		{
			this.worldspaceGeoParent.SetActive(false);
			this.uiCamera.SetActive(false);
			this.cameraRenderOverlay.enabled = false;
			this.uiGeoParent.SetActive(false);
			this.renderingLabelOverlay.enabled = false;
			Debug.Log("Switched to ActivateNone");
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x0013B49B File Offset: 0x0013969B
		private void TriggerLoad()
		{
			base.StartCoroutine(this.WaitforOVROverlay());
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x0013B4AA File Offset: 0x001396AA
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

		// Token: 0x060042EB RID: 17131 RVA: 0x0013B4B9 File Offset: 0x001396B9
		private void TriggerUnload()
		{
			this.ClearObjects();
			this.applicationRadioButton.isOn = true;
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x0013B4D0 File Offset: 0x001396D0
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

		// Token: 0x060042ED RID: 17133 RVA: 0x0013B66C File Offset: 0x0013986C
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

		// Token: 0x060042EE RID: 17134 RVA: 0x0013B7C4 File Offset: 0x001399C4
		private void ClearObjects()
		{
			for (int i = 0; i < this.spawnedCubes.Count; i++)
			{
				Object.DestroyImmediate(this.spawnedCubes[i]);
			}
			this.spawnedCubes.Clear();
			GC.Collect();
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0013B808 File Offset: 0x00139A08
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

		// Token: 0x04004402 RID: 17410
		private bool inMenu;

		// Token: 0x04004403 RID: 17411
		private const string ovrOverlayID = "OVROverlayID";

		// Token: 0x04004404 RID: 17412
		private const string applicationID = "ApplicationID";

		// Token: 0x04004405 RID: 17413
		private const string noneID = "NoneID";

		// Token: 0x04004406 RID: 17414
		private Toggle applicationRadioButton;

		// Token: 0x04004407 RID: 17415
		private Toggle noneRadioButton;

		// Token: 0x04004408 RID: 17416
		[Header("App vs Compositor Comparison Settings")]
		public GameObject mainCamera;

		// Token: 0x04004409 RID: 17417
		public GameObject uiCamera;

		// Token: 0x0400440A RID: 17418
		public GameObject uiGeoParent;

		// Token: 0x0400440B RID: 17419
		public GameObject worldspaceGeoParent;

		// Token: 0x0400440C RID: 17420
		public OVROverlay cameraRenderOverlay;

		// Token: 0x0400440D RID: 17421
		public OVROverlay renderingLabelOverlay;

		// Token: 0x0400440E RID: 17422
		public Texture applicationLabelTexture;

		// Token: 0x0400440F RID: 17423
		public Texture compositorLabelTexture;

		// Token: 0x04004410 RID: 17424
		[Header("Level Loading Sim Settings")]
		public GameObject prefabForLevelLoadSim;

		// Token: 0x04004411 RID: 17425
		public OVROverlay cubemapOverlay;

		// Token: 0x04004412 RID: 17426
		public OVROverlay loadingTextQuadOverlay;

		// Token: 0x04004413 RID: 17427
		public float distanceFromCamToLoadText;

		// Token: 0x04004414 RID: 17428
		public float cubeSpawnRadius;

		// Token: 0x04004415 RID: 17429
		public float heightBetweenItems;

		// Token: 0x04004416 RID: 17430
		public int numObjectsPerLevel;

		// Token: 0x04004417 RID: 17431
		public int numLevels;

		// Token: 0x04004418 RID: 17432
		public int numLoopsTrigger = 500000000;

		// Token: 0x04004419 RID: 17433
		private List<GameObject> spawnedCubes = new List<GameObject>();
	}
}
