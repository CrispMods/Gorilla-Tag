using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000494 RID: 1172
public class GamePlayerLocal : MonoBehaviour
{
	// Token: 0x06001C66 RID: 7270 RVA: 0x000DADB4 File Offset: 0x000D8FB4
	private void Awake()
	{
		GamePlayerLocal.instance = this;
		this.hands = new GamePlayerLocal.HandData[2];
		this.inputData = new GamePlayerLocal.InputData[2];
		for (int i = 0; i < this.inputData.Length; i++)
		{
			this.inputData[i] = new GamePlayerLocal.InputData(32);
		}
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x0004289B File Offset: 0x00040A9B
	private void OnApplicationQuit()
	{
		MonkeBallGame.Instance.OnPlayerDestroy();
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x000428A7 File Offset: 0x00040AA7
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			MonkeBallGame.Instance.OnPlayerDestroy();
		}
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x0004289B File Offset: 0x00040A9B
	private void OnDestroy()
	{
		MonkeBallGame.Instance.OnPlayerDestroy();
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x000DAE04 File Offset: 0x000D9004
	public void OnUpdateInteract()
	{
		for (int i = 0; i < this.inputData.Length; i++)
		{
			this.UpdateInput(i);
		}
		for (int j = 0; j < this.hands.Length; j++)
		{
			this.UpdateHand(j);
		}
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x000DAE48 File Offset: 0x000D9048
	private void UpdateInput(int handIndex)
	{
		XRNode xrnode = this.GetXRNode(handIndex);
		GamePlayerLocal.InputDataMotion data = default(GamePlayerLocal.InputDataMotion);
		InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(xrnode);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.devicePosition, out data.position);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceRotation, out data.rotation);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceVelocity, out data.velocity);
		deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out data.angVelocity);
		data.time = Time.timeAsDouble;
		this.inputData[handIndex].AddInput(data);
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x000DAED4 File Offset: 0x000D90D4
	private void UpdateHand(int handIndex)
	{
		if (GameBallManager.Instance == null)
		{
			return;
		}
		if (!this.gamePlayer.GetGameBallId(handIndex).IsValid())
		{
			this.UpdateHandEmpty(handIndex);
			return;
		}
		this.UpdateHandHolding(handIndex);
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x000DAF18 File Offset: 0x000D9118
	public void SetGrabbed(GameBallId gameBallId, int handIndex)
	{
		GamePlayerLocal.HandData handData = this.hands[handIndex];
		handData.gripPressedTime = 0.0;
		this.hands[handIndex] = handData;
		this.UpdateStuckState();
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000428B6 File Offset: 0x00040AB6
	public void ClearGrabbed(int handIndex)
	{
		this.SetGrabbed(GameBallId.Invalid, handIndex);
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x000DAF58 File Offset: 0x000D9158
	public void ClearAllGrabbed()
	{
		for (int i = 0; i < this.hands.Length; i++)
		{
			this.ClearGrabbed(i);
		}
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x000DAF80 File Offset: 0x000D9180
	private void UpdateStuckState()
	{
		bool disableMovement = false;
		for (int i = 0; i < this.hands.Length; i++)
		{
			if (this.gamePlayer.GetGameBallId(i).IsValid())
			{
				disableMovement = true;
				break;
			}
		}
		GTPlayer.Instance.disableMovement = disableMovement;
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x000DAFC8 File Offset: 0x000D91C8
	private void UpdateHandEmpty(int handIndex)
	{
		GamePlayerLocal.HandData handData = this.hands[handIndex];
		bool flag = ControllerInputPoller.GripFloat(this.GetXRNode(handIndex)) > 0.7f;
		double timeAsDouble = Time.timeAsDouble;
		if (flag && !handData.gripWasHeld)
		{
			handData.gripPressedTime = timeAsDouble;
		}
		double num = timeAsDouble - handData.gripPressedTime;
		handData.gripWasHeld = flag;
		this.hands[handIndex] = handData;
		if (flag && num < 0.15000000596046448)
		{
			Vector3 position = this.GetHandTransform(handIndex).position;
			GameBallId gameBallId = GameBallManager.Instance.TryGrabLocal(position, this.gamePlayer.teamId);
			float num2 = 0.15f;
			if (gameBallId.IsValid())
			{
				bool flag2 = GamePlayerLocal.IsLeftHand(handIndex);
				BodyDockPositions myBodyDockPositions = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions;
				object obj = flag2 ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform;
				GameBall gameBall = GameBallManager.Instance.GetGameBall(gameBallId);
				Vector3 position2 = gameBall.transform.position;
				Vector3 vector = gameBall.transform.position - position;
				if (vector.sqrMagnitude > num2 * num2)
				{
					position2 = position + vector.normalized * num2;
				}
				object obj2 = obj;
				Vector3 localPosition = obj2.InverseTransformPoint(position2);
				Quaternion localRotation = Quaternion.Inverse(obj2.rotation) * gameBall.transform.rotation;
				obj2.InverseTransformPoint(gameBall.transform.position);
				GameBallManager.Instance.RequestGrabBall(gameBallId, flag2, localPosition, localRotation);
			}
		}
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x000DB154 File Offset: 0x000D9354
	private void UpdateHandHolding(int handIndex)
	{
		XRNode xrnode = this.GetXRNode(handIndex);
		if (ControllerInputPoller.GripFloat(xrnode) <= 0.7f)
		{
			InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(xrnode);
			Vector3 vector;
			deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out vector);
			Quaternion rotation;
			deviceAtXRNode.TryGetFeatureValue(CommonUsages.deviceRotation, out rotation);
			Transform transform = GorillaTagger.Instance.offlineVRRig.transform;
			Quaternion rotation2 = GTPlayer.Instance.turnParent.transform.rotation;
			GamePlayerLocal.InputData inputData = this.inputData[handIndex];
			Vector3 vector2 = inputData.GetMaxSpeed(0f, 0.05f) * inputData.GetAvgVel(0f, 0.05f).normalized;
			vector2 = rotation2 * vector2;
			vector2 *= transform.localScale.x;
			vector = rotation2 * -(Quaternion.Inverse(rotation) * vector);
			GameBallId gameBallId = this.gamePlayer.GetGameBallId(handIndex);
			GameBall gameBall = GameBallManager.Instance.GetGameBall(gameBallId);
			if (gameBall == null)
			{
				return;
			}
			if (gameBall.IsLaunched)
			{
				return;
			}
			if (gameBall.disc)
			{
				Vector3 vector3 = gameBall.transform.rotation * gameBall.localDiscUp;
				vector3.Normalize();
				float d = Vector3.Dot(vector3, vector);
				vector = vector3 * d;
				vector *= 1.25f;
				vector2 *= 1.25f;
			}
			else
			{
				vector2 *= 1.5f;
			}
			GorillaVelocityTracker bodyVelocityTracker = GTPlayer.Instance.bodyVelocityTracker;
			vector2 += bodyVelocityTracker.GetAverageVelocity(true, 0.05f, false);
			GameBallManager.Instance.RequestThrowBall(gameBallId, GamePlayerLocal.IsLeftHand(handIndex), vector2, vector);
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x000428C4 File Offset: 0x00040AC4
	private XRNode GetXRNode(int handIndex)
	{
		if (handIndex != 0)
		{
			return XRNode.RightHand;
		}
		return XRNode.LeftHand;
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x000DB30C File Offset: 0x000D950C
	private Transform GetHandTransform(int handIndex)
	{
		BodyDockPositions myBodyDockPositions = GorillaTagger.Instance.offlineVRRig.myBodyDockPositions;
		return ((handIndex == 0) ? myBodyDockPositions.leftHandTransform : myBodyDockPositions.rightHandTransform).parent;
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x0004288D File Offset: 0x00040A8D
	public static bool IsLeftHand(int handIndex)
	{
		return handIndex == 0;
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x00042893 File Offset: 0x00040A93
	public static int GetHandIndex(bool leftHand)
	{
		if (!leftHand)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000428CC File Offset: 0x00040ACC
	public void PlayCatchFx(bool isLeftHand)
	{
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength, 0.1f);
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000428E8 File Offset: 0x00040AE8
	public void PlayThrowFx(bool isLeftHand)
	{
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength * 0.15f, 0.1f);
	}

	// Token: 0x04001F5F RID: 8031
	public GamePlayer gamePlayer;

	// Token: 0x04001F60 RID: 8032
	private const int MAX_INPUT_HISTORY = 32;

	// Token: 0x04001F61 RID: 8033
	private GamePlayerLocal.HandData[] hands;

	// Token: 0x04001F62 RID: 8034
	private GamePlayerLocal.InputData[] inputData;

	// Token: 0x04001F63 RID: 8035
	[OnEnterPlay_SetNull]
	public static volatile GamePlayerLocal instance;

	// Token: 0x02000495 RID: 1173
	private enum HandGrabState
	{
		// Token: 0x04001F65 RID: 8037
		Empty,
		// Token: 0x04001F66 RID: 8038
		Holding
	}

	// Token: 0x02000496 RID: 1174
	private struct HandData
	{
		// Token: 0x04001F67 RID: 8039
		public GamePlayerLocal.HandGrabState grabState;

		// Token: 0x04001F68 RID: 8040
		public bool gripWasHeld;

		// Token: 0x04001F69 RID: 8041
		public double gripPressedTime;

		// Token: 0x04001F6A RID: 8042
		public GameBallId grabbedGameBallId;
	}

	// Token: 0x02000497 RID: 1175
	public struct InputDataMotion
	{
		// Token: 0x04001F6B RID: 8043
		public double time;

		// Token: 0x04001F6C RID: 8044
		public Vector3 position;

		// Token: 0x04001F6D RID: 8045
		public Quaternion rotation;

		// Token: 0x04001F6E RID: 8046
		public Vector3 velocity;

		// Token: 0x04001F6F RID: 8047
		public Vector3 angVelocity;
	}

	// Token: 0x02000498 RID: 1176
	public class InputData
	{
		// Token: 0x06001C7A RID: 7290 RVA: 0x0004290A File Offset: 0x00040B0A
		public InputData(int maxInputs)
		{
			this.maxInputs = maxInputs;
			this.inputMotionHistory = new List<GamePlayerLocal.InputDataMotion>(maxInputs);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00042925 File Offset: 0x00040B25
		public void AddInput(GamePlayerLocal.InputDataMotion data)
		{
			if (this.inputMotionHistory.Count >= this.maxInputs)
			{
				this.inputMotionHistory.RemoveAt(0);
			}
			this.inputMotionHistory.Add(data);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000DB340 File Offset: 0x000D9540
		public float GetMaxSpeed(float ignoreRecent, float window)
		{
			double timeAsDouble = Time.timeAsDouble;
			double num = timeAsDouble - (double)ignoreRecent - (double)window;
			double num2 = timeAsDouble - (double)ignoreRecent;
			float num3 = 0f;
			for (int i = this.inputMotionHistory.Count - 1; i >= 0; i--)
			{
				GamePlayerLocal.InputDataMotion inputDataMotion = this.inputMotionHistory[i];
				if (inputDataMotion.time <= num2)
				{
					if (inputDataMotion.time < num)
					{
						break;
					}
					float sqrMagnitude = inputDataMotion.velocity.sqrMagnitude;
					if (sqrMagnitude > num3)
					{
						num3 = sqrMagnitude;
					}
				}
			}
			return Mathf.Sqrt(num3);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000DB3BC File Offset: 0x000D95BC
		public Vector3 GetAvgVel(float ignoreRecent, float window)
		{
			double timeAsDouble = Time.timeAsDouble;
			double num = timeAsDouble - (double)ignoreRecent - (double)window;
			double num2 = timeAsDouble - (double)ignoreRecent;
			Vector3 a = Vector3.zero;
			int num3 = 0;
			for (int i = this.inputMotionHistory.Count - 1; i >= 0; i--)
			{
				GamePlayerLocal.InputDataMotion inputDataMotion = this.inputMotionHistory[i];
				if (inputDataMotion.time <= num2)
				{
					if (inputDataMotion.time < num)
					{
						break;
					}
					a += inputDataMotion.velocity;
					num3++;
				}
			}
			if (num3 == 0)
			{
				return Vector3.zero;
			}
			return a / (float)num3;
		}

		// Token: 0x04001F70 RID: 8048
		public int maxInputs;

		// Token: 0x04001F71 RID: 8049
		public List<GamePlayerLocal.InputDataMotion> inputMotionHistory;
	}
}
