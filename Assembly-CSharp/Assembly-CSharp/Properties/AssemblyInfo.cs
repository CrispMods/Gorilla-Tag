﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Fusion;
using Unity.Burst;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: NetworkAssemblyWeaved]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.New_00002D29$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Project_00002D33$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Length_00002D34$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.New_00002D3C$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(BurstClassInfo.Index_00002D46$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(BurstClassInfo.NewIndex_00002D47$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(BurstClassInfo.NameCall_00002D48$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
