using System;
using System.Collections;
using GorillaTag;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class HitTargetScoreDisplay : MonoBehaviour
{
	// Token: 0x060014D4 RID: 5332 RVA: 0x000663B4 File Offset: 0x000645B4
	protected void Awake()
	{
		this.rotateTimeTotal = 180f / (float)this.rotateSpeed;
		this.matPropBlock = new MaterialPropertyBlock();
		this.networkedScore.AddCallback(new Action<int>(this.OnScoreChanged), true);
		this.ResetRotation();
		this.tensOld = 0;
		this.hundredsOld = 0;
		this.matPropBlock.SetVector(this.shaderPropID_MainTex_ST, this.numberSheet[0]);
		this.singlesRend.SetPropertyBlock(this.matPropBlock);
		this.tensRend.SetPropertyBlock(this.matPropBlock);
		this.hundredsRend.SetPropertyBlock(this.matPropBlock);
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x0006645B File Offset: 0x0006465B
	private void OnDestroy()
	{
		this.networkedScore.RemoveCallback(new Action<int>(this.OnScoreChanged));
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x00066474 File Offset: 0x00064674
	private void ResetRotation()
	{
		Quaternion rotation = base.transform.rotation;
		this.singlesCard.rotation = rotation;
		this.tensCard.rotation = rotation;
		this.hundredsCard.rotation = rotation;
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x000664B1 File Offset: 0x000646B1
	private IEnumerator RotatingCo()
	{
		float timeElapsedSinceHit = 0f;
		int singlesPlace = this.currentScore % 10;
		int tensPlace = this.currentScore / 10 % 10;
		bool tensChange = this.tensOld != tensPlace;
		this.tensOld = tensPlace;
		int hundredsPlace = this.currentScore / 100 % 10;
		bool hundredsChange = this.hundredsOld != hundredsPlace;
		this.hundredsOld = hundredsPlace;
		bool digitsChange = true;
		while (timeElapsedSinceHit < this.rotateTimeTotal)
		{
			this.singlesCard.Rotate((float)this.rotateSpeed * Time.deltaTime, 0f, 0f, Space.Self);
			Vector3 localEulerAngles = this.singlesCard.localEulerAngles;
			localEulerAngles.x = Mathf.Clamp(localEulerAngles.x, 0f, 180f);
			this.singlesCard.localEulerAngles = localEulerAngles;
			if (tensChange)
			{
				this.tensCard.Rotate((float)this.rotateSpeed * Time.deltaTime, 0f, 0f, Space.Self);
				Vector3 localEulerAngles2 = this.tensCard.localEulerAngles;
				localEulerAngles2.x = Mathf.Clamp(localEulerAngles2.x, 0f, 180f);
				this.tensCard.localEulerAngles = localEulerAngles2;
			}
			if (hundredsChange)
			{
				this.hundredsCard.Rotate((float)this.rotateSpeed * Time.deltaTime, 0f, 0f, Space.Self);
				Vector3 localEulerAngles3 = this.hundredsCard.localEulerAngles;
				localEulerAngles3.x = Mathf.Clamp(localEulerAngles3.x, 0f, 180f);
				this.hundredsCard.localEulerAngles = localEulerAngles3;
			}
			if (digitsChange && timeElapsedSinceHit >= this.rotateTimeTotal / 2f)
			{
				this.matPropBlock.SetVector(this.shaderPropID_MainTex_ST, this.numberSheet[singlesPlace]);
				this.singlesRend.SetPropertyBlock(this.matPropBlock);
				if (tensChange)
				{
					this.matPropBlock.SetVector(this.shaderPropID_MainTex_ST, this.numberSheet[tensPlace]);
					this.tensRend.SetPropertyBlock(this.matPropBlock);
				}
				if (hundredsChange)
				{
					this.matPropBlock.SetVector(this.shaderPropID_MainTex_ST, this.numberSheet[hundredsPlace]);
					this.hundredsRend.SetPropertyBlock(this.matPropBlock);
				}
				digitsChange = false;
			}
			yield return null;
			timeElapsedSinceHit += Time.deltaTime;
		}
		this.ResetRotation();
		yield break;
		yield break;
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x000664C0 File Offset: 0x000646C0
	private void OnScoreChanged(int newScore)
	{
		if (newScore == this.currentScore)
		{
			return;
		}
		if (this.currentRotationCoroutine != null)
		{
			base.StopCoroutine(this.currentRotationCoroutine);
		}
		this.currentScore = newScore;
		this.currentRotationCoroutine = base.StartCoroutine(this.RotatingCo());
	}

	// Token: 0x0400170D RID: 5901
	[SerializeField]
	private WatchableIntSO networkedScore;

	// Token: 0x0400170E RID: 5902
	private int currentScore;

	// Token: 0x0400170F RID: 5903
	private int tensOld;

	// Token: 0x04001710 RID: 5904
	private int hundredsOld;

	// Token: 0x04001711 RID: 5905
	private float rotateTimeTotal;

	// Token: 0x04001712 RID: 5906
	private int shaderPropID_MainTex_ST = Shader.PropertyToID("_BaseMap_ST");

	// Token: 0x04001713 RID: 5907
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x04001714 RID: 5908
	private readonly Vector4[] numberSheet = new Vector4[]
	{
		new Vector4(1f, 1f, 0.8f, -0.5f),
		new Vector4(1f, 1f, 0f, 0f),
		new Vector4(1f, 1f, 0.2f, 0f),
		new Vector4(1f, 1f, 0.4f, 0f),
		new Vector4(1f, 1f, 0.6f, 0f),
		new Vector4(1f, 1f, 0.8f, 0f),
		new Vector4(1f, 1f, 0f, -0.5f),
		new Vector4(1f, 1f, 0.2f, -0.5f),
		new Vector4(1f, 1f, 0.4f, -0.5f),
		new Vector4(1f, 1f, 0.6f, -0.5f)
	};

	// Token: 0x04001715 RID: 5909
	public int rotateSpeed = 180;

	// Token: 0x04001716 RID: 5910
	public Transform singlesCard;

	// Token: 0x04001717 RID: 5911
	public Transform tensCard;

	// Token: 0x04001718 RID: 5912
	public Transform hundredsCard;

	// Token: 0x04001719 RID: 5913
	public Renderer singlesRend;

	// Token: 0x0400171A RID: 5914
	public Renderer tensRend;

	// Token: 0x0400171B RID: 5915
	public Renderer hundredsRend;

	// Token: 0x0400171C RID: 5916
	private Coroutine currentRotationCoroutine;
}
