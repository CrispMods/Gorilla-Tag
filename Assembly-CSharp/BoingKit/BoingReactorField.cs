using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CCC RID: 3276
	public class BoingReactorField : BoingBase
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x0600529C RID: 21148 RVA: 0x00194492 File Offset: 0x00192692
		public static BoingReactorField.ShaderPropertyIdSet ShaderPropertyId
		{
			get
			{
				if (BoingReactorField.s_shaderPropertyId == null)
				{
					BoingReactorField.s_shaderPropertyId = new BoingReactorField.ShaderPropertyIdSet();
				}
				return BoingReactorField.s_shaderPropertyId;
			}
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x001944AC File Offset: 0x001926AC
		public bool UpdateShaderConstants(MaterialPropertyBlock props, float positionSampleMultiplier = 1f, float rotationSampleMultiplier = 1f)
		{
			if (this.HardwareMode != BoingReactorField.HardwareModeEnum.GPU)
			{
				return false;
			}
			if (this.m_fieldParamsBuffer == null || this.m_cellsBuffer == null)
			{
				return false;
			}
			props.SetFloat(BoingReactorField.ShaderPropertyId.PositionSampleMultiplier, positionSampleMultiplier);
			props.SetFloat(BoingReactorField.ShaderPropertyId.RotationSampleMultiplier, rotationSampleMultiplier);
			props.SetBuffer(BoingReactorField.ShaderPropertyId.RenderFieldParams, this.m_fieldParamsBuffer);
			props.SetBuffer(BoingReactorField.ShaderPropertyId.RenderCells, this.m_cellsBuffer);
			return true;
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x00194528 File Offset: 0x00192728
		public bool UpdateShaderConstants(Material material, float positionSampleMultiplier = 1f, float rotationSampleMultiplier = 1f)
		{
			if (this.HardwareMode != BoingReactorField.HardwareModeEnum.GPU)
			{
				return false;
			}
			if (this.m_fieldParamsBuffer == null || this.m_cellsBuffer == null)
			{
				return false;
			}
			material.SetFloat(BoingReactorField.ShaderPropertyId.PositionSampleMultiplier, positionSampleMultiplier);
			material.SetFloat(BoingReactorField.ShaderPropertyId.RotationSampleMultiplier, rotationSampleMultiplier);
			material.SetBuffer(BoingReactorField.ShaderPropertyId.RenderFieldParams, this.m_fieldParamsBuffer);
			material.SetBuffer(BoingReactorField.ShaderPropertyId.RenderCells, this.m_cellsBuffer);
			return true;
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x0600529F RID: 21151 RVA: 0x001945A1 File Offset: 0x001927A1
		public int GpuResourceSetId
		{
			get
			{
				return this.m_gpuResourceSetId;
			}
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x001945AC File Offset: 0x001927AC
		public BoingReactorField()
		{
			this.Params.Init();
			this.m_bounds = Aabb.Empty;
			this.m_init = false;
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x0019468C File Offset: 0x0019288C
		public void Reboot()
		{
			this.m_gridCenter = base.transform.position;
			Vector3 vector = this.QuantizeNorm(this.m_gridCenter);
			this.m_qPrevGridCenterNorm = vector;
			BoingReactorField.CellMoveModeEnum cellMoveMode = this.CellMoveMode;
			if (cellMoveMode == BoingReactorField.CellMoveModeEnum.Follow)
			{
				this.m_gridCenter = base.transform.position;
				this.m_iCellBaseX = 0;
				this.m_iCellBaseY = 0;
				this.m_iCellBaseZ = 0;
				this.m_iCellBaseZ = 0;
				this.m_iCellBaseZ = 0;
				return;
			}
			if (cellMoveMode != BoingReactorField.CellMoveModeEnum.WrapAround)
			{
				return;
			}
			this.m_gridCenter = vector * this.CellSize;
			this.m_iCellBaseX = MathUtil.Modulo((int)this.m_qPrevGridCenterNorm.x, this.CellsX);
			this.m_iCellBaseY = MathUtil.Modulo((int)this.m_qPrevGridCenterNorm.y, this.CellsY);
			this.m_iCellBaseZ = MathUtil.Modulo((int)this.m_qPrevGridCenterNorm.z, this.CellsZ);
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x0019476B File Offset: 0x0019296B
		public void OnEnable()
		{
			this.Reboot();
			BoingManager.Register(this);
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x00194779 File Offset: 0x00192979
		public void Start()
		{
			this.Reboot();
			this.m_cellMoveMode = this.CellMoveMode;
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x0019478D File Offset: 0x0019298D
		public void OnDisable()
		{
			BoingManager.Unregister(this);
			this.DisposeCpuResources();
			this.DisposeGpuResources();
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x001947A1 File Offset: 0x001929A1
		public void DisposeCpuResources()
		{
			this.m_aCpuCell = null;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001947AC File Offset: 0x001929AC
		public void DisposeGpuResources()
		{
			if (this.m_effectorIndexBuffer != null)
			{
				this.m_effectorIndexBuffer.Dispose();
				this.m_effectorIndexBuffer = null;
			}
			if (this.m_reactorParamsBuffer != null)
			{
				this.m_reactorParamsBuffer.Dispose();
				this.m_reactorParamsBuffer = null;
			}
			if (this.m_fieldParamsBuffer != null)
			{
				this.m_fieldParamsBuffer.Dispose();
				this.m_fieldParamsBuffer = null;
			}
			if (this.m_cellsBuffer != null)
			{
				this.m_cellsBuffer.Dispose();
				this.m_cellsBuffer = null;
			}
			if (this.m_cellsBuffer != null)
			{
				this.m_cellsBuffer.Dispose();
				this.m_cellsBuffer = null;
			}
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x0019483C File Offset: 0x00192A3C
		public bool SampleCpuGrid(Vector3 p, out Vector3 positionOffset, out Vector4 rotationOffset)
		{
			bool flag = false;
			switch (this.FalloffDimensions)
			{
			case BoingReactorField.FalloffDimensionsEnum.XYZ:
				flag = this.m_bounds.Contains(p);
				break;
			case BoingReactorField.FalloffDimensionsEnum.XY:
				flag = (this.m_bounds.ContainsX(p) && this.m_bounds.ContainsY(p));
				break;
			case BoingReactorField.FalloffDimensionsEnum.XZ:
				flag = (this.m_bounds.ContainsX(p) && this.m_bounds.ContainsZ(p));
				break;
			case BoingReactorField.FalloffDimensionsEnum.YZ:
				flag = (this.m_bounds.ContainsY(p) && this.m_bounds.ContainsZ(p));
				break;
			}
			if (!flag)
			{
				positionOffset = Vector3.zero;
				rotationOffset = QuaternionUtil.ToVector4(Quaternion.identity);
				return false;
			}
			float num = 0.5f * this.CellSize;
			Vector3 a = p - (this.m_gridCenter + this.GetCellCenterOffset(0, 0, 0));
			Vector3 vector = this.QuantizeNorm(a + new Vector3(-num, -num, -num));
			Vector3 b = vector * this.CellSize;
			int num2 = Mathf.Clamp((int)vector.x, 0, this.CellsX - 1);
			int num3 = Mathf.Clamp((int)vector.y, 0, this.CellsY - 1);
			int num4 = Mathf.Clamp((int)vector.z, 0, this.CellsZ - 1);
			int x = Mathf.Min(num2 + 1, this.CellsX - 1);
			int y = Mathf.Min(num3 + 1, this.CellsY - 1);
			int z = Mathf.Min(num4 + 1, this.CellsZ - 1);
			int num5;
			int num6;
			int num7;
			this.ResolveCellIndex(num2, num3, num4, 1, out num5, out num6, out num7);
			int num8;
			int num9;
			int num10;
			this.ResolveCellIndex(x, y, z, 1, out num8, out num9, out num10);
			bool lerpX = num5 != num8;
			bool lerpY = num6 != num9;
			bool lerpZ = num7 != num10;
			Vector3 vector2 = (a - b) / this.CellSize;
			Vector3 vector3 = p - base.transform.position;
			switch (this.FalloffDimensions)
			{
			case BoingReactorField.FalloffDimensionsEnum.XY:
				vector3.z = 0f;
				break;
			case BoingReactorField.FalloffDimensionsEnum.XZ:
				vector3.y = 0f;
				break;
			case BoingReactorField.FalloffDimensionsEnum.YZ:
				vector3.x = 0f;
				break;
			}
			int num11 = Mathf.Max(this.CellsX, Mathf.Max(this.CellsY, this.CellsZ));
			float num12 = 1f;
			BoingReactorField.FalloffModeEnum falloffMode = this.FalloffMode;
			if (falloffMode != BoingReactorField.FalloffModeEnum.Circle)
			{
				if (falloffMode == BoingReactorField.FalloffModeEnum.Square)
				{
					Vector3 a2 = num * new Vector3((float)this.CellsX, (float)this.CellsY, (float)this.CellsZ);
					Vector3 vector4 = this.FalloffRatio * a2 - num * Vector3.one;
					vector4.x = Mathf.Max(0f, vector4.x);
					vector4.y = Mathf.Max(0f, vector4.y);
					vector4.z = Mathf.Max(0f, vector4.z);
					Vector3 vector5 = (1f - this.FalloffRatio) * a2 - num * Vector3.one;
					vector5.x = Mathf.Max(MathUtil.Epsilon, vector5.x);
					vector5.y = Mathf.Max(MathUtil.Epsilon, vector5.y);
					vector5.z = Mathf.Max(MathUtil.Epsilon, vector5.z);
					Vector3 vector6 = new Vector3(1f - Mathf.Clamp01((Mathf.Abs(vector3.x) - vector4.x) / vector5.x), 1f - Mathf.Clamp01((Mathf.Abs(vector3.y) - vector4.y) / vector5.y), 1f - Mathf.Clamp01((Mathf.Abs(vector3.z) - vector4.z) / vector5.z));
					switch (this.FalloffDimensions)
					{
					case BoingReactorField.FalloffDimensionsEnum.XY:
						vector6.x = 1f;
						break;
					case BoingReactorField.FalloffDimensionsEnum.XZ:
						vector6.y = 1f;
						break;
					case BoingReactorField.FalloffDimensionsEnum.YZ:
						vector6.z = 1f;
						break;
					}
					num12 = Mathf.Min(vector6.x, Mathf.Min(vector6.y, vector6.z));
				}
			}
			else
			{
				float num13 = num * (float)num11;
				Vector3 vector7 = new Vector3((float)num11 / (float)this.CellsX, (float)num11 / (float)this.CellsY, (float)num11 / (float)this.CellsZ);
				vector3.x *= vector7.x;
				vector3.y *= vector7.y;
				vector3.z *= vector7.z;
				float magnitude = vector3.magnitude;
				float num14 = Mathf.Max(0f, this.FalloffRatio * num13 - num);
				float num15 = Mathf.Max(MathUtil.Epsilon, (1f - this.FalloffRatio) * num13 - num);
				num12 = 1f - Mathf.Clamp01((magnitude - num14) / num15);
			}
			BoingReactorField.s_aCellOffset[0] = this.m_aCpuCell[num7, num6, num5].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(num2, num3, num4);
			BoingReactorField.s_aCellOffset[1] = this.m_aCpuCell[num7, num6, num8].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(x, num3, num4);
			BoingReactorField.s_aCellOffset[2] = this.m_aCpuCell[num7, num9, num5].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(num2, y, num4);
			BoingReactorField.s_aCellOffset[3] = this.m_aCpuCell[num7, num9, num8].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(x, y, num4);
			BoingReactorField.s_aCellOffset[4] = this.m_aCpuCell[num10, num6, num5].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(num2, num3, z);
			BoingReactorField.s_aCellOffset[5] = this.m_aCpuCell[num10, num6, num8].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(x, num3, z);
			BoingReactorField.s_aCellOffset[6] = this.m_aCpuCell[num10, num9, num5].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(num2, y, z);
			BoingReactorField.s_aCellOffset[7] = this.m_aCpuCell[num10, num9, num8].PositionSpring.Value - this.m_gridCenter - this.GetCellCenterOffset(x, y, z);
			positionOffset = VectorUtil.TriLerp(ref BoingReactorField.s_aCellOffset[0], ref BoingReactorField.s_aCellOffset[1], ref BoingReactorField.s_aCellOffset[2], ref BoingReactorField.s_aCellOffset[3], ref BoingReactorField.s_aCellOffset[4], ref BoingReactorField.s_aCellOffset[5], ref BoingReactorField.s_aCellOffset[6], ref BoingReactorField.s_aCellOffset[7], lerpX, lerpY, lerpZ, vector2.x, vector2.y, vector2.z);
			rotationOffset = VectorUtil.TriLerp(ref this.m_aCpuCell[num7, num6, num5].RotationSpring.ValueVec, ref this.m_aCpuCell[num7, num6, num8].RotationSpring.ValueVec, ref this.m_aCpuCell[num7, num9, num5].RotationSpring.ValueVec, ref this.m_aCpuCell[num7, num9, num8].RotationSpring.ValueVec, ref this.m_aCpuCell[num10, num6, num5].RotationSpring.ValueVec, ref this.m_aCpuCell[num10, num6, num8].RotationSpring.ValueVec, ref this.m_aCpuCell[num10, num9, num5].RotationSpring.ValueVec, ref this.m_aCpuCell[num10, num9, num8].RotationSpring.ValueVec, lerpX, lerpY, lerpZ, vector2.x, vector2.y, vector2.z);
			positionOffset *= num12;
			rotationOffset = QuaternionUtil.ToVector4(QuaternionUtil.Pow(QuaternionUtil.FromVector4(rotationOffset, true), num12));
			return true;
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x0019510C File Offset: 0x0019330C
		private void UpdateFieldParamsGpu()
		{
			this.m_fieldParams.CellsX = this.CellsX;
			this.m_fieldParams.CellsY = this.CellsY;
			this.m_fieldParams.CellsZ = this.CellsZ;
			this.m_fieldParams.NumEffectors = 0;
			if (this.Effectors != null)
			{
				foreach (BoingEffector boingEffector in this.Effectors)
				{
					if (!(boingEffector == null))
					{
						BoingEffector component = boingEffector.GetComponent<BoingEffector>();
						if (!(component == null) && component.isActiveAndEnabled)
						{
							this.m_fieldParams.NumEffectors = this.m_fieldParams.NumEffectors + 1;
						}
					}
				}
			}
			this.m_fieldParams.iCellBaseX = this.m_iCellBaseX;
			this.m_fieldParams.iCellBaseY = this.m_iCellBaseY;
			this.m_fieldParams.iCellBaseZ = this.m_iCellBaseZ;
			this.m_fieldParams.FalloffMode = (int)this.FalloffMode;
			this.m_fieldParams.FalloffDimensions = (int)this.FalloffDimensions;
			this.m_fieldParams.PropagationDepth = this.PropagationDepth;
			this.m_fieldParams.GridCenter = this.m_gridCenter;
			this.m_fieldParams.UpWs = (this.Params.Bits.IsBitSet(6) ? this.Params.RotationReactionUp : (base.transform.rotation * VectorUtil.NormalizeSafe(this.Params.RotationReactionUp, Vector3.up)));
			this.m_fieldParams.FieldPosition = base.transform.position;
			this.m_fieldParams.FalloffRatio = this.FalloffRatio;
			this.m_fieldParams.CellSize = this.CellSize;
			this.m_fieldParams.DeltaTime = Time.deltaTime;
			if (this.m_fieldParamsBuffer != null)
			{
				this.m_fieldParamsBuffer.SetData(new BoingReactorField.FieldParams[]
				{
					this.m_fieldParams
				});
			}
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x001952F0 File Offset: 0x001934F0
		private void UpdateFlags()
		{
			this.Params.Bits.SetBit(0, this.TwoDDistanceCheck);
			this.Params.Bits.SetBit(1, this.TwoDPositionInfluence);
			this.Params.Bits.SetBit(2, this.TwoDRotationInfluence);
			this.Params.Bits.SetBit(3, this.EnablePositionEffect);
			this.Params.Bits.SetBit(4, this.EnableRotationEffect);
			this.Params.Bits.SetBit(6, this.GlobalReactionUpVector);
			this.Params.Bits.SetBit(7, this.EnablePropagation);
			this.Params.Bits.SetBit(8, this.AnchorPropagationAtBorder);
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x001953B8 File Offset: 0x001935B8
		public void UpdateBounds()
		{
			this.m_bounds = new Aabb(this.m_gridCenter + this.GetCellCenterOffset(0, 0, 0), this.m_gridCenter + this.GetCellCenterOffset(this.CellsX - 1, this.CellsY - 1, this.CellsZ - 1));
			this.m_bounds.Expand(this.CellSize);
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x00195420 File Offset: 0x00193620
		public void PrepareExecute()
		{
			this.Init();
			if (this.SharedParams != null)
			{
				BoingWork.Params.Copy(ref this.SharedParams.Params, ref this.Params);
			}
			this.UpdateFlags();
			this.UpdateBounds();
			BoingReactorField.HardwareModeEnum hardwareMode;
			if (this.m_hardwareMode != this.HardwareMode)
			{
				hardwareMode = this.m_hardwareMode;
				if (hardwareMode != BoingReactorField.HardwareModeEnum.CPU)
				{
					if (hardwareMode == BoingReactorField.HardwareModeEnum.GPU)
					{
						this.DisposeGpuResources();
					}
				}
				else
				{
					this.DisposeCpuResources();
				}
				this.m_hardwareMode = this.HardwareMode;
			}
			hardwareMode = this.m_hardwareMode;
			if (hardwareMode != BoingReactorField.HardwareModeEnum.CPU)
			{
				if (hardwareMode == BoingReactorField.HardwareModeEnum.GPU)
				{
					this.ValidateGpuResources();
				}
			}
			else
			{
				this.ValidateCpuResources();
			}
			this.HandleCellMove();
			hardwareMode = this.m_hardwareMode;
			if (hardwareMode == BoingReactorField.HardwareModeEnum.CPU)
			{
				this.FinishPrepareExecuteCpu();
				return;
			}
			if (hardwareMode != BoingReactorField.HardwareModeEnum.GPU)
			{
				return;
			}
			this.FinishPrepareExecuteGpu();
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x001954DC File Offset: 0x001936DC
		private void ValidateCpuResources()
		{
			this.CellsX = Mathf.Max(1, this.CellsX);
			this.CellsY = Mathf.Max(1, this.CellsY);
			this.CellsZ = Mathf.Max(1, this.CellsZ);
			if (this.m_aCpuCell == null || this.m_cellsX != this.CellsX || this.m_cellsY != this.CellsY || this.m_cellsZ != this.CellsZ)
			{
				this.m_aCpuCell = new BoingWork.Params.InstanceData[this.CellsZ, this.CellsY, this.CellsX];
				for (int i = 0; i < this.CellsZ; i++)
				{
					for (int j = 0; j < this.CellsY; j++)
					{
						for (int k = 0; k < this.CellsX; k++)
						{
							int x;
							int y;
							int z;
							this.ResolveCellIndex(k, j, i, -1, out x, out y, out z);
							this.m_aCpuCell[i, j, k].Reset(this.m_gridCenter + this.GetCellCenterOffset(x, y, z), false);
						}
					}
				}
				this.m_cellsX = this.CellsX;
				this.m_cellsY = this.CellsY;
				this.m_cellsZ = this.CellsZ;
			}
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x0019560C File Offset: 0x0019380C
		private void ValidateGpuResources()
		{
			bool flag = false;
			bool flag2 = this.m_shader == null || BoingReactorField.s_computeKernelId == null;
			if (flag2)
			{
				this.m_shader = Resources.Load<ComputeShader>("Boing Kit/BoingReactorFieldCompute");
				flag = true;
				if (BoingReactorField.s_computeKernelId == null)
				{
					BoingReactorField.s_computeKernelId = new BoingReactorField.ComputeKernelId();
					BoingReactorField.s_computeKernelId.InitKernel = this.m_shader.FindKernel("Init");
					BoingReactorField.s_computeKernelId.MoveKernel = this.m_shader.FindKernel("Move");
					BoingReactorField.s_computeKernelId.WrapXKernel = this.m_shader.FindKernel("WrapX");
					BoingReactorField.s_computeKernelId.WrapYKernel = this.m_shader.FindKernel("WrapY");
					BoingReactorField.s_computeKernelId.WrapZKernel = this.m_shader.FindKernel("WrapZ");
					BoingReactorField.s_computeKernelId.ExecuteKernel = this.m_shader.FindKernel("Execute");
				}
			}
			bool flag3 = this.m_effectorIndexBuffer == null || (this.Effectors != null && this.m_numEffectors != this.Effectors.Length);
			if (flag3 && this.Effectors != null)
			{
				if (this.m_effectorIndexBuffer != null)
				{
					this.m_effectorIndexBuffer.Dispose();
				}
				this.m_effectorIndexBuffer = new ComputeBuffer(this.Effectors.Length, 4);
				flag = true;
				this.m_numEffectors = this.Effectors.Length;
			}
			if (flag2 || flag3)
			{
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.ExecuteKernel, BoingReactorField.ShaderPropertyId.EffectorIndices, this.m_effectorIndexBuffer);
			}
			bool flag4 = this.m_reactorParamsBuffer == null;
			if (flag4)
			{
				this.m_reactorParamsBuffer = new ComputeBuffer(1, BoingWork.Params.Stride);
				flag = true;
			}
			if (flag2 || flag4)
			{
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.ExecuteKernel, BoingReactorField.ShaderPropertyId.ReactorParams, this.m_reactorParamsBuffer);
			}
			bool flag5 = this.m_fieldParamsBuffer == null;
			if (flag5)
			{
				this.m_fieldParamsBuffer = new ComputeBuffer(1, BoingReactorField.FieldParams.Stride);
				flag = true;
			}
			if (flag2 || flag5)
			{
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.InitKernel, BoingReactorField.ShaderPropertyId.ComputeFieldParams, this.m_fieldParamsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.MoveKernel, BoingReactorField.ShaderPropertyId.ComputeFieldParams, this.m_fieldParamsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.WrapXKernel, BoingReactorField.ShaderPropertyId.ComputeFieldParams, this.m_fieldParamsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.WrapYKernel, BoingReactorField.ShaderPropertyId.ComputeFieldParams, this.m_fieldParamsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.WrapZKernel, BoingReactorField.ShaderPropertyId.ComputeFieldParams, this.m_fieldParamsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.ExecuteKernel, BoingReactorField.ShaderPropertyId.ComputeFieldParams, this.m_fieldParamsBuffer);
			}
			this.m_cellBufferNeedsReset = (this.m_cellsBuffer == null || this.m_cellsX != this.CellsX || this.m_cellsY != this.CellsY || this.m_cellsZ != this.CellsZ);
			if (this.m_cellBufferNeedsReset)
			{
				if (this.m_cellsBuffer != null)
				{
					this.m_cellsBuffer.Dispose();
				}
				int num = this.CellsX * this.CellsY * this.CellsZ;
				this.m_cellsBuffer = new ComputeBuffer(num, BoingWork.Params.InstanceData.Stride);
				BoingWork.Params.InstanceData[] array = new BoingWork.Params.InstanceData[num];
				for (int i = 0; i < num; i++)
				{
					array[i].PositionSpring.Reset();
					array[i].RotationSpring.Reset();
				}
				this.m_cellsBuffer.SetData(array);
				flag = true;
				this.m_cellsX = this.CellsX;
				this.m_cellsY = this.CellsY;
				this.m_cellsZ = this.CellsZ;
			}
			if (flag2 || this.m_cellBufferNeedsReset)
			{
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.InitKernel, BoingReactorField.ShaderPropertyId.ComputeCells, this.m_cellsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.MoveKernel, BoingReactorField.ShaderPropertyId.ComputeCells, this.m_cellsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.WrapXKernel, BoingReactorField.ShaderPropertyId.ComputeCells, this.m_cellsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.WrapYKernel, BoingReactorField.ShaderPropertyId.ComputeCells, this.m_cellsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.WrapZKernel, BoingReactorField.ShaderPropertyId.ComputeCells, this.m_cellsBuffer);
				this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.ExecuteKernel, BoingReactorField.ShaderPropertyId.ComputeCells, this.m_cellsBuffer);
			}
			if (flag)
			{
				this.m_gpuResourceSetId++;
				if (this.m_gpuResourceSetId < 0)
				{
					this.m_gpuResourceSetId = -1;
				}
			}
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x00195AEC File Offset: 0x00193CEC
		private void FinishPrepareExecuteCpu()
		{
			Quaternion rotation = base.transform.rotation;
			for (int i = 0; i < this.CellsZ; i++)
			{
				for (int j = 0; j < this.CellsY; j++)
				{
					for (int k = 0; k < this.CellsX; k++)
					{
						int x;
						int y;
						int z;
						this.ResolveCellIndex(k, j, i, -1, out x, out y, out z);
						this.m_aCpuCell[i, j, k].PrepareExecute(ref this.Params, this.m_gridCenter, rotation, this.GetCellCenterOffset(x, y, z));
					}
				}
			}
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x00195B74 File Offset: 0x00193D74
		private void FinishPrepareExecuteGpu()
		{
			if (this.m_cellBufferNeedsReset)
			{
				this.UpdateFieldParamsGpu();
				this.m_shader.Dispatch(BoingReactorField.s_computeKernelId.InitKernel, this.CellsX, this.CellsY, this.CellsZ);
			}
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x00195BAB File Offset: 0x00193DAB
		public void Init()
		{
			if (this.m_init)
			{
				return;
			}
			this.m_hardwareMode = this.HardwareMode;
			this.m_init = true;
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x00195BC9 File Offset: 0x00193DC9
		public void Sanitize()
		{
			if (this.PropagationDepth < 0)
			{
				Debug.LogWarning("Propagation iterations must be a positive number.");
			}
			else if (this.PropagationDepth > 3)
			{
				Debug.LogWarning("For performance reasons, propagation is limited to 3 iterations.");
			}
			this.PropagationDepth = Mathf.Clamp(this.PropagationDepth, 1, 3);
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x00195C08 File Offset: 0x00193E08
		public void HandleCellMove()
		{
			if (this.m_cellMoveMode != this.CellMoveMode)
			{
				this.Reboot();
				this.m_cellMoveMode = this.CellMoveMode;
			}
			BoingReactorField.CellMoveModeEnum cellMoveMode = this.CellMoveMode;
			BoingReactorField.HardwareModeEnum hardwareMode;
			if (cellMoveMode == BoingReactorField.CellMoveModeEnum.Follow)
			{
				Vector3 vector = base.transform.position - this.m_gridCenter;
				hardwareMode = this.HardwareMode;
				if (hardwareMode != BoingReactorField.HardwareModeEnum.CPU)
				{
					if (hardwareMode == BoingReactorField.HardwareModeEnum.GPU)
					{
						this.UpdateFieldParamsGpu();
						this.m_shader.SetVector(BoingReactorField.ShaderPropertyId.MoveParams, vector);
						this.m_shader.Dispatch(BoingReactorField.s_computeKernelId.MoveKernel, this.CellsX, this.CellsY, this.CellsZ);
					}
				}
				else
				{
					for (int i = 0; i < this.CellsZ; i++)
					{
						for (int j = 0; j < this.CellsY; j++)
						{
							for (int k = 0; k < this.CellsX; k++)
							{
								ref BoingWork.Params.InstanceData ptr = ref this.m_aCpuCell[i, j, k];
								ptr.PositionSpring.Value = ptr.PositionSpring.Value + vector;
							}
						}
					}
				}
				this.m_gridCenter = base.transform.position;
				this.m_qPrevGridCenterNorm = this.QuantizeNorm(this.m_gridCenter);
				return;
			}
			if (cellMoveMode != BoingReactorField.CellMoveModeEnum.WrapAround)
			{
				return;
			}
			this.m_gridCenter = base.transform.position;
			Vector3 vector2 = this.QuantizeNorm(this.m_gridCenter);
			this.m_gridCenter = vector2 * this.CellSize;
			int num = (int)(vector2.x - this.m_qPrevGridCenterNorm.x);
			int num2 = (int)(vector2.y - this.m_qPrevGridCenterNorm.y);
			int num3 = (int)(vector2.z - this.m_qPrevGridCenterNorm.z);
			this.m_qPrevGridCenterNorm = vector2;
			if (num == 0 && num2 == 0 && num3 == 0)
			{
				return;
			}
			hardwareMode = this.m_hardwareMode;
			if (hardwareMode != BoingReactorField.HardwareModeEnum.CPU)
			{
				if (hardwareMode == BoingReactorField.HardwareModeEnum.GPU)
				{
					this.WrapGpu(num, num2, num3);
				}
			}
			else
			{
				this.WrapCpu(num, num2, num3);
			}
			this.m_iCellBaseX = MathUtil.Modulo(this.m_iCellBaseX + num, this.CellsX);
			this.m_iCellBaseY = MathUtil.Modulo(this.m_iCellBaseY + num2, this.CellsY);
			this.m_iCellBaseZ = MathUtil.Modulo(this.m_iCellBaseZ + num3, this.CellsZ);
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x00195E46 File Offset: 0x00194046
		private void InitPropagationCpu(ref BoingWork.Params.InstanceData data)
		{
			data.PositionPropagationWorkData = Vector3.zero;
			data.RotationPropagationWorkData = Vector3.zero;
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x00195E64 File Offset: 0x00194064
		private void PropagateSpringCpu(ref BoingWork.Params.InstanceData data, float dt)
		{
			data.PositionSpring.Velocity = data.PositionSpring.Velocity + BoingReactorField.kPropagationFactor * this.PositionPropagation * data.PositionPropagationWorkData * dt;
			data.RotationSpring.VelocityVec = data.RotationSpring.VelocityVec + BoingReactorField.kPropagationFactor * this.RotationPropagation * data.RotationPropagationWorkData * dt;
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x00195EE4 File Offset: 0x001940E4
		private void ExtendPropagationBorder(ref BoingWork.Params.InstanceData data, float weight, int adjDeltaX, int adjDeltaY, int adjDeltaZ)
		{
			data.PositionPropagationWorkData += weight * (data.PositionOrigin + new Vector3((float)adjDeltaX, (float)adjDeltaY, (float)adjDeltaZ) * this.CellSize);
			data.RotationPropagationWorkData += weight * data.RotationOrigin;
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x00195F54 File Offset: 0x00194154
		private void AccumulatePropagationWeightedNeighbor(ref BoingWork.Params.InstanceData data, ref BoingWork.Params.InstanceData neighbor, float weight)
		{
			data.PositionPropagationWorkData += weight * (neighbor.PositionSpring.Value - neighbor.PositionOrigin);
			data.RotationPropagationWorkData += weight * (neighbor.RotationSpring.ValueVec - neighbor.RotationOrigin);
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x00195FC8 File Offset: 0x001941C8
		private void GatherPropagation(ref BoingWork.Params.InstanceData data, float weightSum)
		{
			data.PositionPropagationWorkData = data.PositionPropagationWorkData / weightSum - (data.PositionSpring.Value - data.PositionOrigin);
			data.RotationPropagationWorkData = data.RotationPropagationWorkData / weightSum - (data.RotationSpring.ValueVec - data.RotationOrigin);
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x00195E46 File Offset: 0x00194046
		private void AnchorPropagationBorder(ref BoingWork.Params.InstanceData data)
		{
			data.PositionPropagationWorkData = Vector3.zero;
			data.RotationPropagationWorkData = Vector3.zero;
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x00196030 File Offset: 0x00194230
		private void PropagateCpu(float dt)
		{
			int[] array = new int[this.PropagationDepth * 2 + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i - this.PropagationDepth;
			}
			for (int j = 0; j < this.CellsZ; j++)
			{
				for (int k = 0; k < this.CellsY; k++)
				{
					for (int l = 0; l < this.CellsX; l++)
					{
						this.InitPropagationCpu(ref this.m_aCpuCell[j, k, l]);
					}
				}
			}
			for (int m = 0; m < this.CellsZ; m++)
			{
				for (int n = 0; n < this.CellsY; n++)
				{
					for (int num = 0; num < this.CellsX; num++)
					{
						int num2;
						int num3;
						int num4;
						this.ResolveCellIndex(num, n, m, -1, out num2, out num3, out num4);
						float num5 = 0f;
						foreach (int num7 in array)
						{
							foreach (int num9 in array)
							{
								foreach (int num11 in array)
								{
									if (num11 != 0 || num9 != 0 || num7 != 0)
									{
										int num12 = num11 * num11 + num9 * num9 + num7 * num7;
										float num13 = BoingReactorField.s_aSqrtInv[num12];
										num5 += num13;
										if ((this.CellsX <= 2 || ((num2 != 0 || num11 >= 0) && (num2 != this.CellsX - 1 || num11 <= 0))) && (this.CellsY <= 2 || ((num3 != 0 || num9 >= 0) && (num3 != this.CellsY - 1 || num9 <= 0))) && (this.CellsZ <= 2 || ((num4 != 0 || num7 >= 0) && (num4 != this.CellsZ - 1 || num7 <= 0))))
										{
											int num14 = MathUtil.Modulo(num + num11, this.CellsX);
											int num15 = MathUtil.Modulo(n + num9, this.CellsY);
											int num16 = MathUtil.Modulo(m + num7, this.CellsZ);
											this.AccumulatePropagationWeightedNeighbor(ref this.m_aCpuCell[m, n, num], ref this.m_aCpuCell[num16, num15, num14], num13);
										}
									}
								}
							}
						}
						if (num5 > 0f)
						{
							this.GatherPropagation(ref this.m_aCpuCell[m, n, num], num5);
						}
					}
				}
			}
			if (this.AnchorPropagationAtBorder)
			{
				for (int num17 = 0; num17 < this.CellsZ; num17++)
				{
					for (int num18 = 0; num18 < this.CellsY; num18++)
					{
						for (int num19 = 0; num19 < this.CellsX; num19++)
						{
							int num20;
							int num21;
							int num22;
							this.ResolveCellIndex(num19, num18, num17, -1, out num20, out num21, out num22);
							if (((num20 == 0 || num20 == this.CellsX - 1) && this.CellsX > 2) || ((num21 == 0 || num21 == this.CellsY - 1) && this.CellsY > 2) || ((num22 == 0 || num22 == this.CellsZ - 1) && this.CellsZ > 2))
							{
								this.AnchorPropagationBorder(ref this.m_aCpuCell[num17, num18, num19]);
							}
						}
					}
				}
			}
			for (int num23 = 0; num23 < this.CellsZ; num23++)
			{
				for (int num24 = 0; num24 < this.CellsY; num24++)
				{
					for (int num25 = 0; num25 < this.CellsX; num25++)
					{
						this.PropagateSpringCpu(ref this.m_aCpuCell[num23, num24, num25], dt);
					}
				}
			}
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x001963E0 File Offset: 0x001945E0
		private void WrapCpu(int deltaX, int deltaY, int deltaZ)
		{
			if (deltaX != 0)
			{
				int num = (deltaX > 0) ? -1 : 1;
				for (int i = 0; i < this.CellsZ; i++)
				{
					for (int j = 0; j < this.CellsY; j++)
					{
						int num2 = (deltaX > 0) ? (deltaX - 1) : (this.CellsX + deltaX);
						while (num2 >= 0 && num2 < this.CellsX)
						{
							int num3;
							int num4;
							int num5;
							this.ResolveCellIndex(num2, j, i, 1, out num3, out num4, out num5);
							int x;
							int y;
							int z;
							this.ResolveCellIndex(num3 - deltaX, num4 - deltaY, num5 - deltaZ, -1, out x, out y, out z);
							this.m_aCpuCell[num5, num4, num3].Reset(this.m_gridCenter + this.GetCellCenterOffset(x, y, z), true);
							num2 += num;
						}
					}
				}
			}
			if (deltaY != 0)
			{
				int num6 = (deltaY > 0) ? -1 : 1;
				for (int k = 0; k < this.CellsZ; k++)
				{
					int num7 = (deltaY > 0) ? (deltaY - 1) : (this.CellsY + deltaY);
					while (num7 >= 0 && num7 < this.CellsY)
					{
						for (int l = 0; l < this.CellsX; l++)
						{
							int num8;
							int num9;
							int num10;
							this.ResolveCellIndex(l, num7, k, 1, out num8, out num9, out num10);
							int x2;
							int y2;
							int z2;
							this.ResolveCellIndex(num8 - deltaX, num9 - deltaY, num10 - deltaZ, -1, out x2, out y2, out z2);
							this.m_aCpuCell[num10, num9, num8].Reset(this.m_gridCenter + this.GetCellCenterOffset(x2, y2, z2), true);
						}
						num7 += num6;
					}
				}
			}
			if (deltaZ != 0)
			{
				int num11 = (deltaZ > 0) ? -1 : 1;
				int num12 = (deltaZ > 0) ? (deltaZ - 1) : (this.CellsZ + deltaZ);
				while (num12 >= 0 && num12 < this.CellsZ)
				{
					for (int m = 0; m < this.CellsY; m++)
					{
						for (int n = 0; n < this.CellsX; n++)
						{
							int num13;
							int num14;
							int num15;
							this.ResolveCellIndex(n, m, num12, 1, out num13, out num14, out num15);
							int x3;
							int y3;
							int z3;
							this.ResolveCellIndex(num13 - deltaX, num14 - deltaY, num15 - deltaZ, -1, out x3, out y3, out z3);
							this.m_aCpuCell[num15, num14, num13].Reset(this.m_gridCenter + this.GetCellCenterOffset(x3, y3, z3), true);
						}
					}
					num12 += num11;
				}
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x00196634 File Offset: 0x00194834
		private void WrapGpu(int deltaX, int deltaY, int deltaZ)
		{
			this.UpdateFieldParamsGpu();
			this.m_shader.SetInts(BoingReactorField.ShaderPropertyId.WrapParams, new int[]
			{
				deltaX,
				deltaY,
				deltaZ
			});
			if (deltaX != 0)
			{
				this.m_shader.Dispatch(BoingReactorField.s_computeKernelId.WrapXKernel, 1, this.CellsY, this.CellsZ);
			}
			if (deltaY != 0)
			{
				this.m_shader.Dispatch(BoingReactorField.s_computeKernelId.WrapYKernel, this.CellsX, 1, this.CellsZ);
			}
			if (deltaZ != 0)
			{
				this.m_shader.Dispatch(BoingReactorField.s_computeKernelId.WrapZKernel, this.CellsX, this.CellsY, 1);
			}
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x001966E0 File Offset: 0x001948E0
		public void ExecuteCpu(float dt)
		{
			this.PrepareExecute();
			if (this.Effectors == null || this.Effectors.Length == 0)
			{
				return;
			}
			if (this.EnablePropagation)
			{
				this.PropagateCpu(dt);
			}
			foreach (BoingEffector boingEffector in this.Effectors)
			{
				if (!(boingEffector == null))
				{
					BoingEffector.Params @params = default(BoingEffector.Params);
					@params.Fill(boingEffector);
					if (this.m_bounds.Intersects(ref @params))
					{
						for (int j = 0; j < this.CellsZ; j++)
						{
							for (int k = 0; k < this.CellsY; k++)
							{
								for (int l = 0; l < this.CellsX; l++)
								{
									this.m_aCpuCell[j, k, l].AccumulateTarget(ref this.Params, ref @params, dt);
								}
							}
						}
					}
				}
			}
			for (int m = 0; m < this.CellsZ; m++)
			{
				for (int n = 0; n < this.CellsY; n++)
				{
					for (int num = 0; num < this.CellsX; num++)
					{
						this.m_aCpuCell[m, n, num].EndAccumulateTargets(ref this.Params);
						this.m_aCpuCell[m, n, num].Execute(ref this.Params, dt);
					}
				}
			}
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x00196834 File Offset: 0x00194A34
		public void ExecuteGpu(float dt, ComputeBuffer effectorParamsBuffer, Dictionary<int, int> effectorParamsIndexMap)
		{
			this.PrepareExecute();
			this.UpdateFieldParamsGpu();
			this.m_shader.SetBuffer(BoingReactorField.s_computeKernelId.ExecuteKernel, BoingReactorField.ShaderPropertyId.Effectors, effectorParamsBuffer);
			if (this.m_fieldParams.NumEffectors > 0)
			{
				int[] array = new int[this.m_fieldParams.NumEffectors];
				int num = 0;
				foreach (BoingEffector boingEffector in this.Effectors)
				{
					if (!(boingEffector == null))
					{
						BoingEffector component = boingEffector.GetComponent<BoingEffector>();
						int num2;
						if (!(component == null) && component.isActiveAndEnabled && effectorParamsIndexMap.TryGetValue(component.GetInstanceID(), out num2))
						{
							array[num++] = num2;
						}
					}
				}
				this.m_effectorIndexBuffer.SetData(array);
			}
			this.s_aReactorParams[0] = this.Params;
			this.m_reactorParamsBuffer.SetData(this.s_aReactorParams);
			this.m_shader.SetVector(BoingReactorField.ShaderPropertyId.PropagationParams, new Vector4(this.PositionPropagation, this.RotationPropagation, BoingReactorField.kPropagationFactor, 0f));
			this.m_shader.Dispatch(BoingReactorField.s_computeKernelId.ExecuteKernel, this.CellsX, this.CellsY, this.CellsZ);
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x0019696E File Offset: 0x00194B6E
		public void OnDrawGizmosSelected()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.DrawGizmos(true);
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x00196980 File Offset: 0x00194B80
		private void DrawGizmos(bool drawEffectors)
		{
			Vector3 vector = this.GetGridCenter();
			BoingReactorField.CellMoveModeEnum cellMoveMode = this.CellMoveMode;
			if (cellMoveMode != BoingReactorField.CellMoveModeEnum.Follow)
			{
				if (cellMoveMode == BoingReactorField.CellMoveModeEnum.WrapAround)
				{
					vector = new Vector3(Mathf.Round(base.transform.position.x / this.CellSize), Mathf.Round(base.transform.position.y / this.CellSize), Mathf.Round(base.transform.position.z / this.CellSize)) * this.CellSize;
				}
			}
			else
			{
				vector = base.transform.position;
			}
			BoingWork.Params.InstanceData[,,] array = null;
			BoingReactorField.HardwareModeEnum hardwareMode = this.HardwareMode;
			if (hardwareMode != BoingReactorField.HardwareModeEnum.CPU)
			{
				if (hardwareMode == BoingReactorField.HardwareModeEnum.GPU)
				{
					if (this.m_cellsBuffer != null)
					{
						array = new BoingWork.Params.InstanceData[this.CellsZ, this.CellsY, this.CellsX];
						this.m_cellsBuffer.GetData(array);
					}
				}
			}
			else
			{
				array = this.m_aCpuCell;
			}
			int num = 1;
			if (this.CellsX * this.CellsY * this.CellsZ > 1024)
			{
				num = 2;
			}
			if (this.CellsX * this.CellsY * this.CellsZ > 4096)
			{
				num = 3;
			}
			if (this.CellsX * this.CellsY * this.CellsZ > 8192)
			{
				num = 4;
			}
			for (int i = 0; i < this.CellsZ; i++)
			{
				for (int j = 0; j < this.CellsY; j++)
				{
					for (int k = 0; k < this.CellsX; k++)
					{
						int x;
						int y;
						int z;
						this.ResolveCellIndex(k, j, i, -1, out x, out y, out z);
						Vector3 center = vector + this.GetCellCenterOffset(x, y, z);
						if (array != null && k % num == 0 && j % num == 0 && i % num == 0)
						{
							BoingWork.Params.InstanceData instanceData = array[i, j, k];
							Gizmos.color = new Color(1f, 1f, 1f, 1f);
							Gizmos.matrix = Matrix4x4.TRS(instanceData.PositionSpring.Value, instanceData.RotationSpring.ValueQuat, Vector3.one);
							Gizmos.DrawCube(Vector3.zero, Mathf.Min(0.1f, 0.5f * this.CellSize) * Vector3.one);
							Gizmos.matrix = Matrix4x4.identity;
						}
						Gizmos.color = new Color(1f, 0.5f, 0.2f, 1f);
						Gizmos.DrawWireCube(center, this.CellSize * Vector3.one);
					}
				}
			}
			BoingReactorField.FalloffModeEnum falloffMode = this.FalloffMode;
			if (falloffMode != BoingReactorField.FalloffModeEnum.Circle)
			{
				if (falloffMode == BoingReactorField.FalloffModeEnum.Square)
				{
					Vector3 size = this.CellSize * this.FalloffRatio * new Vector3((float)this.CellsX, (float)this.CellsY, (float)this.CellsZ);
					Gizmos.color = new Color(1f, 1f, 0.2f, 0.5f);
					Gizmos.DrawWireCube(vector, size);
				}
			}
			else
			{
				float num2 = (float)Mathf.Max(this.CellsX, Mathf.Max(this.CellsY, this.CellsZ));
				Gizmos.color = new Color(1f, 1f, 0.2f, 0.5f);
				Gizmos.matrix = Matrix4x4.Translate(vector) * Matrix4x4.Scale(new Vector3((float)this.CellsX, (float)this.CellsY, (float)this.CellsZ) / num2);
				Gizmos.DrawWireSphere(Vector3.zero, 0.5f * this.CellSize * num2 * this.FalloffRatio);
				Gizmos.matrix = Matrix4x4.identity;
			}
			if (drawEffectors && this.Effectors != null)
			{
				foreach (BoingEffector boingEffector in this.Effectors)
				{
					if (!(boingEffector == null))
					{
						boingEffector.OnDrawGizmosSelected();
					}
				}
			}
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x00196D58 File Offset: 0x00194F58
		private Vector3 GetGridCenter()
		{
			BoingReactorField.CellMoveModeEnum cellMoveMode = this.CellMoveMode;
			if (cellMoveMode == BoingReactorField.CellMoveModeEnum.Follow)
			{
				return base.transform.position;
			}
			if (cellMoveMode != BoingReactorField.CellMoveModeEnum.WrapAround)
			{
				return base.transform.position;
			}
			return this.QuantizeNorm(base.transform.position) * this.CellSize;
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x00196DA9 File Offset: 0x00194FA9
		private Vector3 QuantizeNorm(Vector3 p)
		{
			return new Vector3(Mathf.Round(p.x / this.CellSize), Mathf.Round(p.y / this.CellSize), Mathf.Round(p.z / this.CellSize));
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x00196DE8 File Offset: 0x00194FE8
		private Vector3 GetCellCenterOffset(int x, int y, int z)
		{
			return this.CellSize * (-0.5f * (new Vector3((float)this.CellsX, (float)this.CellsY, (float)this.CellsZ) - Vector3.one) + new Vector3((float)x, (float)y, (float)z));
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x00196E40 File Offset: 0x00195040
		private void ResolveCellIndex(int x, int y, int z, int baseMult, out int resX, out int resY, out int resZ)
		{
			resX = MathUtil.Modulo(x + baseMult * this.m_iCellBaseX, this.CellsX);
			resY = MathUtil.Modulo(y + baseMult * this.m_iCellBaseY, this.CellsY);
			resZ = MathUtil.Modulo(z + baseMult * this.m_iCellBaseZ, this.CellsZ);
		}

		// Token: 0x040054CF RID: 21711
		private static BoingReactorField.ShaderPropertyIdSet s_shaderPropertyId;

		// Token: 0x040054D0 RID: 21712
		private BoingReactorField.FieldParams m_fieldParams;

		// Token: 0x040054D1 RID: 21713
		public BoingReactorField.HardwareModeEnum HardwareMode = BoingReactorField.HardwareModeEnum.GPU;

		// Token: 0x040054D2 RID: 21714
		private BoingReactorField.HardwareModeEnum m_hardwareMode;

		// Token: 0x040054D3 RID: 21715
		public BoingReactorField.CellMoveModeEnum CellMoveMode = BoingReactorField.CellMoveModeEnum.WrapAround;

		// Token: 0x040054D4 RID: 21716
		private BoingReactorField.CellMoveModeEnum m_cellMoveMode;

		// Token: 0x040054D5 RID: 21717
		[Range(0.1f, 10f)]
		public float CellSize = 1f;

		// Token: 0x040054D6 RID: 21718
		public int CellsX = 8;

		// Token: 0x040054D7 RID: 21719
		public int CellsY = 1;

		// Token: 0x040054D8 RID: 21720
		public int CellsZ = 8;

		// Token: 0x040054D9 RID: 21721
		private int m_cellsX = -1;

		// Token: 0x040054DA RID: 21722
		private int m_cellsY = -1;

		// Token: 0x040054DB RID: 21723
		private int m_cellsZ = -1;

		// Token: 0x040054DC RID: 21724
		private int m_iCellBaseX;

		// Token: 0x040054DD RID: 21725
		private int m_iCellBaseY;

		// Token: 0x040054DE RID: 21726
		private int m_iCellBaseZ;

		// Token: 0x040054DF RID: 21727
		public BoingReactorField.FalloffModeEnum FalloffMode = BoingReactorField.FalloffModeEnum.Square;

		// Token: 0x040054E0 RID: 21728
		[Range(0f, 1f)]
		public float FalloffRatio = 0.7f;

		// Token: 0x040054E1 RID: 21729
		public BoingReactorField.FalloffDimensionsEnum FalloffDimensions = BoingReactorField.FalloffDimensionsEnum.XZ;

		// Token: 0x040054E2 RID: 21730
		public BoingEffector[] Effectors = new BoingEffector[1];

		// Token: 0x040054E3 RID: 21731
		private int m_numEffectors = -1;

		// Token: 0x040054E4 RID: 21732
		private Aabb m_bounds;

		// Token: 0x040054E5 RID: 21733
		public bool TwoDDistanceCheck;

		// Token: 0x040054E6 RID: 21734
		public bool TwoDPositionInfluence;

		// Token: 0x040054E7 RID: 21735
		public bool TwoDRotationInfluence;

		// Token: 0x040054E8 RID: 21736
		public bool EnablePositionEffect = true;

		// Token: 0x040054E9 RID: 21737
		public bool EnableRotationEffect = true;

		// Token: 0x040054EA RID: 21738
		public bool GlobalReactionUpVector;

		// Token: 0x040054EB RID: 21739
		public BoingWork.Params Params;

		// Token: 0x040054EC RID: 21740
		public SharedBoingParams SharedParams;

		// Token: 0x040054ED RID: 21741
		public bool EnablePropagation;

		// Token: 0x040054EE RID: 21742
		[Range(0f, 1f)]
		public float PositionPropagation = 1f;

		// Token: 0x040054EF RID: 21743
		[Range(0f, 1f)]
		public float RotationPropagation = 1f;

		// Token: 0x040054F0 RID: 21744
		[Range(1f, 3f)]
		public int PropagationDepth = 1;

		// Token: 0x040054F1 RID: 21745
		public bool AnchorPropagationAtBorder;

		// Token: 0x040054F2 RID: 21746
		private static readonly float kPropagationFactor = 600f;

		// Token: 0x040054F3 RID: 21747
		private BoingWork.Params.InstanceData[,,] m_aCpuCell;

		// Token: 0x040054F4 RID: 21748
		private ComputeShader m_shader;

		// Token: 0x040054F5 RID: 21749
		private ComputeBuffer m_effectorIndexBuffer;

		// Token: 0x040054F6 RID: 21750
		private ComputeBuffer m_reactorParamsBuffer;

		// Token: 0x040054F7 RID: 21751
		private ComputeBuffer m_fieldParamsBuffer;

		// Token: 0x040054F8 RID: 21752
		private ComputeBuffer m_cellsBuffer;

		// Token: 0x040054F9 RID: 21753
		private int m_gpuResourceSetId = -1;

		// Token: 0x040054FA RID: 21754
		private static BoingReactorField.ComputeKernelId s_computeKernelId;

		// Token: 0x040054FB RID: 21755
		private bool m_init;

		// Token: 0x040054FC RID: 21756
		private Vector3 m_gridCenter;

		// Token: 0x040054FD RID: 21757
		private Vector3 m_qPrevGridCenterNorm;

		// Token: 0x040054FE RID: 21758
		private static Vector3[] s_aCellOffset = new Vector3[8];

		// Token: 0x040054FF RID: 21759
		private bool m_cellBufferNeedsReset;

		// Token: 0x04005500 RID: 21760
		private static float[] s_aSqrtInv = new float[]
		{
			0f,
			1f,
			0.70711f,
			0.57735f,
			0.5f,
			0.44721f,
			0.40825f,
			0.37796f,
			0.35355f,
			0.33333f,
			0.31623f,
			0.30151f,
			0.28868f,
			0.27735f,
			0.26726f,
			0.2582f,
			0.25f,
			0.24254f,
			0.2357f,
			0.22942f,
			0.22361f,
			0.21822f,
			0.2132f,
			0.20851f,
			0.20412f,
			0.2f,
			0.19612f,
			0.19245f
		};

		// Token: 0x04005501 RID: 21761
		private BoingWork.Params[] s_aReactorParams = new BoingWork.Params[1];

		// Token: 0x02000CCD RID: 3277
		public enum HardwareModeEnum
		{
			// Token: 0x04005503 RID: 21763
			CPU,
			// Token: 0x04005504 RID: 21764
			GPU
		}

		// Token: 0x02000CCE RID: 3278
		public enum CellMoveModeEnum
		{
			// Token: 0x04005506 RID: 21766
			Follow,
			// Token: 0x04005507 RID: 21767
			WrapAround
		}

		// Token: 0x02000CCF RID: 3279
		public enum FalloffModeEnum
		{
			// Token: 0x04005509 RID: 21769
			None,
			// Token: 0x0400550A RID: 21770
			Circle,
			// Token: 0x0400550B RID: 21771
			Square
		}

		// Token: 0x02000CD0 RID: 3280
		public enum FalloffDimensionsEnum
		{
			// Token: 0x0400550D RID: 21773
			XYZ,
			// Token: 0x0400550E RID: 21774
			XY,
			// Token: 0x0400550F RID: 21775
			XZ,
			// Token: 0x04005510 RID: 21776
			YZ
		}

		// Token: 0x02000CD1 RID: 3281
		public class ShaderPropertyIdSet
		{
			// Token: 0x060052C5 RID: 21189 RVA: 0x00196EC8 File Offset: 0x001950C8
			public ShaderPropertyIdSet()
			{
				this.MoveParams = Shader.PropertyToID("moveParams");
				this.WrapParams = Shader.PropertyToID("wrapParams");
				this.Effectors = Shader.PropertyToID("aEffector");
				this.EffectorIndices = Shader.PropertyToID("aEffectorIndex");
				this.ReactorParams = Shader.PropertyToID("reactorParams");
				this.ComputeFieldParams = Shader.PropertyToID("fieldParams");
				this.ComputeCells = Shader.PropertyToID("aCell");
				this.RenderFieldParams = Shader.PropertyToID("aBoingFieldParams");
				this.RenderCells = Shader.PropertyToID("aBoingFieldCell");
				this.PositionSampleMultiplier = Shader.PropertyToID("positionSampleMultiplier");
				this.RotationSampleMultiplier = Shader.PropertyToID("rotationSampleMultiplier");
				this.PropagationParams = Shader.PropertyToID("propagationParams");
			}

			// Token: 0x04005511 RID: 21777
			public int MoveParams;

			// Token: 0x04005512 RID: 21778
			public int WrapParams;

			// Token: 0x04005513 RID: 21779
			public int Effectors;

			// Token: 0x04005514 RID: 21780
			public int EffectorIndices;

			// Token: 0x04005515 RID: 21781
			public int ReactorParams;

			// Token: 0x04005516 RID: 21782
			public int ComputeFieldParams;

			// Token: 0x04005517 RID: 21783
			public int ComputeCells;

			// Token: 0x04005518 RID: 21784
			public int RenderFieldParams;

			// Token: 0x04005519 RID: 21785
			public int RenderCells;

			// Token: 0x0400551A RID: 21786
			public int PositionSampleMultiplier;

			// Token: 0x0400551B RID: 21787
			public int RotationSampleMultiplier;

			// Token: 0x0400551C RID: 21788
			public int PropagationParams;
		}

		// Token: 0x02000CD2 RID: 3282
		private struct FieldParams
		{
			// Token: 0x060052C6 RID: 21190 RVA: 0x00196F9C File Offset: 0x0019519C
			private void SuppressWarnings()
			{
				this.m_padding0 = 0;
				this.m_padding1 = 0;
				this.m_padding2 = 0f;
				this.m_padding4 = 0f;
				this.m_padding5 = 0f;
				this.m_padding0 = this.m_padding1;
				this.m_padding1 = (int)this.m_padding2;
				this.m_padding2 = this.m_padding3;
				this.m_padding3 = this.m_padding4;
				this.m_padding4 = this.m_padding5;
			}

			// Token: 0x0400551D RID: 21789
			public static readonly int Stride = 112;

			// Token: 0x0400551E RID: 21790
			public int CellsX;

			// Token: 0x0400551F RID: 21791
			public int CellsY;

			// Token: 0x04005520 RID: 21792
			public int CellsZ;

			// Token: 0x04005521 RID: 21793
			public int NumEffectors;

			// Token: 0x04005522 RID: 21794
			public int iCellBaseX;

			// Token: 0x04005523 RID: 21795
			public int iCellBaseY;

			// Token: 0x04005524 RID: 21796
			public int iCellBaseZ;

			// Token: 0x04005525 RID: 21797
			public int m_padding0;

			// Token: 0x04005526 RID: 21798
			public int FalloffMode;

			// Token: 0x04005527 RID: 21799
			public int FalloffDimensions;

			// Token: 0x04005528 RID: 21800
			public int PropagationDepth;

			// Token: 0x04005529 RID: 21801
			public int m_padding1;

			// Token: 0x0400552A RID: 21802
			public Vector3 GridCenter;

			// Token: 0x0400552B RID: 21803
			private float m_padding3;

			// Token: 0x0400552C RID: 21804
			public Vector3 UpWs;

			// Token: 0x0400552D RID: 21805
			private float m_padding2;

			// Token: 0x0400552E RID: 21806
			public Vector3 FieldPosition;

			// Token: 0x0400552F RID: 21807
			public float m_padding4;

			// Token: 0x04005530 RID: 21808
			public float FalloffRatio;

			// Token: 0x04005531 RID: 21809
			public float CellSize;

			// Token: 0x04005532 RID: 21810
			public float DeltaTime;

			// Token: 0x04005533 RID: 21811
			private float m_padding5;
		}

		// Token: 0x02000CD3 RID: 3283
		private class ComputeKernelId
		{
			// Token: 0x04005534 RID: 21812
			public int InitKernel;

			// Token: 0x04005535 RID: 21813
			public int MoveKernel;

			// Token: 0x04005536 RID: 21814
			public int WrapXKernel;

			// Token: 0x04005537 RID: 21815
			public int WrapYKernel;

			// Token: 0x04005538 RID: 21816
			public int WrapZKernel;

			// Token: 0x04005539 RID: 21817
			public int ExecuteKernel;
		}
	}
}
