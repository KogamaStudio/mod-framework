using Il2CppSystem;
using Il2Cpp;
using Il2CppMV.WorldObject;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KogamaModFramework.Conversion;
using Il2CppMV.Common;

namespace KogamaModFramework.Generation;

public static class WorldObjectOperations
{
    public static void LinkObjects(int outputId, int inputId)
    {
        var wocm = MVGameControllerBase.WOCM;
        if (wocm == null) return;

        var outputWo = wocm?.GetWorldObject(outputId);
        var inputWo = wocm?.GetWorldObject(inputId);

        if (outputWo == null || inputWo == null) return;

        Link link = new Link();
        link.outputWOID = outputId;
        link.inputWOID = inputId;

        MVGameControllerBase.OperationRequests.AddLink(link);
    }

    public static List<int> GetAllWorldObjectIds()
    {
        var wocm = MVGameControllerBase.WOCM;
        if (wocm == null) return new List<int>();

        var ids = new Il2CppSystem.Collections.Generic.HashSet<int>();
        wocm.GetAllWoIds(75578, ids);

        var result = new List<int>();

        foreach (var id in ids)
        {
            result.Add(id);
        }

        return result;
    }

    public static MVWorldObject GetObject(int id)
    {
        var wo = MVGameControllerBase.WOCM?.GetWorldObjectClient(id);
        return wo;
    }

    public static void SetPosition(int id, Vector3 position)
    {
        var wo = GetObject(id);
        if (wo == null) return;

        MVGameControllerBase.OperationRequests.UpdateWorldObject(id, position, RotationConverter.QuaternionToBytes(wo.Rotation), TransformPackageType.Teleport);
    }

    public static void SetRotation(int id, Quaternion rotation)
    {
        var wo = GetObject(id);
        if (wo == null) return;

        MVGameControllerBase.OperationRequests.UpdateWorldObject(id, wo.Position, RotationConverter.QuaternionToBytes(rotation), TransformPackageType.Teleport);
    }
}

