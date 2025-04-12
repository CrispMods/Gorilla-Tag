using System;
using System.Collections.Generic;
using System.Reflection;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Animations.Rigging;

// Token: 0x020003AB RID: 939
public class CalibrationCube : MonoBehaviour
{
	// Token: 0x060015F0 RID: 5616 RVA: 0x0003DDB0 File Offset: 0x0003BFB0
	private void Awake()
	{
		this.calibratedLength = this.baseLength;
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x000BF61C File Offset: 0x000BD81C
	private void Start()
	{
		try
		{
			this.OnCollisionExit(null);
		}
		catch
		{
		}
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnTriggerEnter(Collider other)
	{
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnTriggerExit(Collider other)
	{
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x000BF648 File Offset: 0x000BD848
	public void RecalibrateSize(bool pressed)
	{
		this.lastCalibratedLength = this.calibratedLength;
		this.calibratedLength = (this.rightController.transform.position - this.leftController.transform.position).magnitude;
		this.calibratedLength = ((this.calibratedLength > this.maxLength) ? this.maxLength : ((this.calibratedLength < this.minLength) ? this.minLength : this.calibratedLength));
		float d = this.calibratedLength / this.lastCalibratedLength;
		Vector3 localScale = this.playerBody.transform.localScale;
		this.playerBody.GetComponentInChildren<RigBuilder>().Clear();
		this.playerBody.transform.localScale = new Vector3(1f, 1f, 1f);
		this.playerBody.GetComponentInChildren<TransformReset>().ResetTransforms();
		this.playerBody.transform.localScale = d * localScale;
		this.playerBody.GetComponentInChildren<RigBuilder>().Build();
		this.playerBody.GetComponentInChildren<VRRig>().SetHeadBodyOffset();
		GorillaPlaySpace.Instance.bodyColliderOffset *= d;
		GorillaPlaySpace.Instance.bodyCollider.gameObject.transform.localScale *= d;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x000BF7A4 File Offset: 0x000BD9A4
	private void OnCollisionExit(Collision collision)
	{
		try
		{
			bool flag = false;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				AssemblyName name = assemblies[i].GetName();
				if (!this.calibrationPresetsTest3[0].Contains(name.Name))
				{
					flag = true;
				}
			}
			if (!flag || Application.platform == RuntimePlatform.Android)
			{
				GorillaComputer.instance.includeUpdatedServerSynchTest = 0;
			}
		}
		catch
		{
		}
	}

	// Token: 0x04001822 RID: 6178
	public PrimaryButtonWatcher watcher;

	// Token: 0x04001823 RID: 6179
	public GameObject rightController;

	// Token: 0x04001824 RID: 6180
	public GameObject leftController;

	// Token: 0x04001825 RID: 6181
	public GameObject playerBody;

	// Token: 0x04001826 RID: 6182
	private float calibratedLength;

	// Token: 0x04001827 RID: 6183
	private float lastCalibratedLength;

	// Token: 0x04001828 RID: 6184
	public float minLength = 1f;

	// Token: 0x04001829 RID: 6185
	public float maxLength = 2.5f;

	// Token: 0x0400182A RID: 6186
	public float baseLength = 1.61f;

	// Token: 0x0400182B RID: 6187
	public string[] calibrationPresets;

	// Token: 0x0400182C RID: 6188
	public string[] calibrationPresetsTest;

	// Token: 0x0400182D RID: 6189
	public string[] calibrationPresetsTest2;

	// Token: 0x0400182E RID: 6190
	public string[] calibrationPresetsTest3;

	// Token: 0x0400182F RID: 6191
	public string[] calibrationPresetsTest4;

	// Token: 0x04001830 RID: 6192
	public string outputstring;

	// Token: 0x04001831 RID: 6193
	private List<string> stringList = new List<string>();
}
