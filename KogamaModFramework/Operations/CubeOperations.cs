using Il2Cpp;
using MelonLoader;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using System.Collections;
using Il2CppMV.WorldObject;

namespace KogamaModFramework.Operations;

public static class CubeOperations
{
    public static int BatchSize = 500;
    public static int FrameDelay = 500;

    public static float Progress = 0.0f;
    public static bool CancelGeneration = false;
    public static bool IsBuilding = false;
    public static IEnumerator Add(MVCubeModelBase targetModel, List<CubeData> cubes)
    {
        if (IsBuilding)
        {
            MelonLogger.Error("[CubeOperations] Generation already in progress!");
            yield break;
        }

        IsBuilding = true;

        try
        {
            if (targetModel == null)
            {
                MelonLogger.Error("[CubeOperations] Target model does not exist!");
                yield break;
            }

            int placedCube = 0;
            foreach (CubeData cubeData in cubes)
            {
                if (CancelGeneration)
                {
                    CancelGeneration = false;
                    MelonLogger.Msg("[CubeOperations] Generation canceled");
                    yield break;
                }

                if (targetModel == null)
                {
                    MelonLogger.Error("[CubeOperations] Target model was deleted!");
                    CancelGeneration = true;
                    yield break;
                }

                if (MVGameControllerBase.IsPlaying)
                {
                    MelonLogger.Error("[CubeOperations] Not in Edit mode!");
                    CancelGeneration = true;
                    yield break;
                }

                var cube = new Cube(
                    new Il2CppStructArray<byte>(cubeData.Corners),
                    new Il2CppStructArray<byte>(cubeData.Materials)
                );

                var position = new Il2CppMV.WorldObject.IntVector(cubeData.X, cubeData.Y, cubeData.Z);

                if (targetModel.ContainsCube(position))
                {
                    targetModel.RemoveCube(position);
                }

                targetModel.AddCube(position, cube);
                placedCube++;
                Progress = (float)placedCube / cubes.Count;

                if (placedCube % BatchSize == 0)
                {
                    targetModel.HandleDelta();
                    yield return new WaitForSecondsRealtime(1f / 60f * FrameDelay);
                }


            }

            targetModel.HandleDelta();
            MelonLogger.Msg($"[CubeOperations] Placed {cubes.Count} cubes");
        }
        finally
        {
            IsBuilding = false;
            Progress = 0.0f;
        }
    }

    public static IEnumerator Remove(MVCubeModelBase targetModel, List<IntVector> positions)
    {
        if (IsBuilding)
        {
            MelonLogger.Error("[CubeOperations] Generation already in progress!");
            yield break;
        }

        IsBuilding = true;

        try
        {
            if (targetModel == null)
            {
                MelonLogger.Error("[CubeOperations] Target model does not exist!");
                yield break;
            }

            int removedCube = 0;

            foreach (IntVector position in positions)
            {
                if (CancelGeneration)
                {
                    CancelGeneration = false;
                    MelonLogger.Msg("[CubeOperations] Generation canceled");
                    yield break;
                }

                if (targetModel == null)
                {
                    MelonLogger.Error("[CubeOperations] Target model was deleted!");
                    CancelGeneration = true;
                    yield break;
                }

                if (MVGameControllerBase.IsPlaying)
                {
                    MelonLogger.Error("[CubeOperations] Not in Edit mode!");
                    CancelGeneration = true;
                    yield break;
                }

                if (targetModel.ContainsCube(position))
                {
                    targetModel.RemoveCube(position);
                    removedCube++;
                }

                Progress = (float)removedCube / positions.Count;

                if (removedCube % BatchSize == 0)
                {
                    targetModel.HandleDelta();
                    yield return new WaitForSecondsRealtime(1f / 60f * FrameDelay);
                }


            }

            targetModel.HandleDelta();
            MelonLogger.Msg($"[CubeOperations] Removed {positions.Count} cubes");
        }
        finally
        {
            IsBuilding = false;
            Progress = 0.0f;
        }
    }

    public static CubeData GetCube(MVCubeModelBase targetModel, IntVector position)
    {
        var cube = targetModel.GetCube(position);
        if (cube != null)
        {
            return new CubeData(position.x, position.y, position.z, cube.FaceMaterials, cube.ByteCorners);
        }

        return null;
    }
}