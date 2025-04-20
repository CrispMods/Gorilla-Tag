using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OculusSampleFramework
{
	// Token: 0x02000AA9 RID: 2729
	public class OVROverlaySample : MonoBehaviour
	{
		// Token: 0x06004429 RID: 17449 RVA: 0x0017A264 File Offset: 0x00178464
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

		// Token: 0x0600442A RID: 17450 RVA: 0x0017A3BC File Offset: 0x001785BC
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

		// Token: 0x0600442B RID: 17451 RVA: 0x0017A424 File Offset: 0x00178624
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

		// Token: 0x0600442C RID: 17452 RVA: 0x0017A48C File Offset: 0x0017868C
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

		// Token: 0x0600442D RID: 17453 RVA: 0x0017A4F4 File Offset: 0x001786F4
		private void ActivateNone()
		{
			this.worldspaceGeoParent.SetActive(false);
			this.uiCamera.SetActive(false);
			this.cameraRenderOverlay.enabled = false;
			this.uiGeoParent.SetActive(false);
			this.renderingLabelOverlay.enabled = false;
			Debug.Log("Switched to ActivateNone");
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0005C792 File Offset: 0x0005A992
		private void TriggerLoad()
		{
			base.StartCoroutine(this.WaitforOVROverlay());
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0005C7A1 File Offset: 0x0005A9A1
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

		// Token: 0x06004430 RID: 17456 RVA: 0x0005C7B0 File Offset: 0x0005A9B0
		private void TriggerUnload()
		{
			this.ClearObjects();
			this.applicationRadioButton.isOn = true;
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0017A548 File Offset: 0x00178748
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

		// Token: 0x06004432 RID: 17458 RVA: 0x0017A6E4 File Offset: 0x001788E4
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
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefabForLevelLoadSim, a + position, Quaternion.identity);
					Transform transform = gameObject.transform;
					transform.LookAt(position);
					Vector3 eulerAngles = transform.rotation.eulerAngles;
					eulerAngles.x = 0f;
					transform.rotation = Quaternion.Euler(eulerAngles);
					this.spawnedCubes.Add(gameObject);
				}
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0017A83C File Offset: 0x00178A3C
		private void ClearObjects()
		{
			for (int i = 0; i < this.spawnedCubes.Count; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.spawnedCubes[i]);
			}
			this.spawnedCubes.Clear();
			GC.Collect();
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0005C7C4 File Offset: 0x0005A9C4
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

		// Token: 0x040044FC RID: 17660
		private bool inMenu;

		// Token: 0x040044FD RID: 17661
		private const string ovrOverlayID = "OVROverlayID";

		// Token: 0x040044FE RID: 17662
		private const string applicationID = "ApplicationID";

		// Token: 0x040044FF RID: 17663
		private const string noneID = "NoneID";

		// Token: 0x04004500 RID: 17664
		private Toggle applicationRadioButton;

		// Token: 0x04004501 RID: 17665
		private Toggle noneRadioButton;

		// Token: 0x04004502 RID: 17666
		[Header("App vs Compositor Comparison Settings")]
		public GameObject mainCamera;

		// Token: 0x04004503 RID: 17667
		public GameObject uiCamera;

		// Token: 0x04004504 RID: 17668
		public GameObject uiGeoParent;

		// Token: 0x04004505 RID: 17669
		public GameObject worldspaceGeoParent;

		// Token: 0x04004506 RID: 17670
		public OVROverlay cameraRenderOverlay;

		// Token: 0x04004507 RID: 17671
		public OVROverlay renderingLabelOverlay;

		// Token: 0x04004508 RID: 17672
		public Texture applicationLabelTexture;

		// Token: 0x04004509 RID: 17673
		public Texture compositorLabelTexture;

		// Token: 0x0400450A RID: 17674
		[Header("Level Loading Sim Settings")]
		public GameObject prefabForLevelLoadSim;

		// Token: 0x0400450B RID: 17675
		public OVROverlay cubemapOverlay;

		// Token: 0x0400450C RID: 17676
		public OVROverlay loadingTextQuadOverlay;

		// Token: 0x0400450D RID: 17677
		public float distanceFromCamToLoadText;

		// Token: 0x0400450E RID: 17678
		public float cubeSpawnRadius;

		// Token: 0x0400450F RID: 17679
		public float heightBetweenItems;

		// Token: 0x04004510 RID: 17680
		public int numObjectsPerLevel;

		// Token: 0x04004511 RID: 17681
		public int numLevels;

		// Token: 0x04004512 RID: 17682
		public int numLoopsTrigger = 500000000;

		// Token: 0x04004513 RID: 17683
		private List<GameObject> spawnedCubes = new List<GameObject>();
	}
}
