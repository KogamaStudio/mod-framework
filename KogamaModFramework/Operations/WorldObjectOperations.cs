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
using System.Collections;
using HarmonyLib;
using System;
using Il2CppInterop.Runtime;

namespace KogamaModFramework.Operations;

public static class WorldObjectOperations
{
    public static int RootGroupId => MVGameControllerBase.WOCM?.RootGroup?.Id ?? -1;

    public static void AddLink(int outputWoId, int inputWoId)
    {
        var outputWo = GetObject(outputWoId);
        var inputWo = GetObject(inputWoId);

        if (outputWo == null || inputWo == null) return;

        Link link = new Link();
        link.outputWOID = outputWoId;
        link.inputWOID = inputWoId;

        MVGameControllerBase.OperationRequests.AddLink(link);
    }

    public static void RemoveLink(int outputWoId, int inputWoId)
    {
        var outputWo = GetObject(outputWoId);
        if (outputWo == null) return;

        foreach (var link in outputWo.OutputLinkRefs)
        {
            if (link.outputWOID == outputWoId && link.inputWOID == inputWoId)
            {
                MVGameControllerBase.OperationRequests.RemoveLink(link.id);
                return;
            }
        }
    }

    public static List<int> GetAllInputLinks(int woId)
    {
        var wo = GetObject(woId) as MVWorldObjectClient;
        if (wo == null) return new List<int>();

        var result = new List<int>();
        foreach (var link in wo.InputLinkRefs)
        {
            result.Add(link.outputWOID);
        }
        return result;
    }

    public static List<int> GetAllOutputLinks(int woId)
    {
        var wo = GetObject(woId) as MVWorldObjectClient;
        if (wo == null) return new List<int>();

        var result = new List<int>();
        foreach (var link in wo.OutputLinkRefs)
        {
            result.Add(link.inputWOID);
        }
        return result;
    }

    public static List<int> GetAllWorldObjectIds()
    {
        var wocm = MVGameControllerBase.WOCM;
        if (wocm == null) return new List<int>();

        var ids = new Il2CppSystem.Collections.Generic.HashSet<int>();
        wocm.GetAllWoIds(RootGroupId, ids);

        var result = new List<int>();

        foreach (var id in ids)
        {
            result.Add(id);
        }

        return result;
    }

    public static MVWorldObject GetObject(int woId)
    {
        var wo = MVGameControllerBase.WOCM?.GetWorldObjectClient(woId);
        return wo;
    }

    public static void SetPosition(int woId, Vector3 position)
    {
        var wo = GetObject(woId) as MVWorldObjectClient;
        if (wo == null) return;

        wo.Position = position;
        MVGameControllerBase.OperationRequests.TransferOwnership(woId, 0, wo.Transform);
    }

    public static void SetRotation(int woId, Quaternion rotation)
    {
        var wo = GetObject(woId) as MVWorldObjectClient;
        if (wo == null) return;

        wo.Rotation = rotation;
        MVGameControllerBase.OperationRequests.TransferOwnership(woId, 0, wo.Transform);
    }

    public static void SetVisible(int woId, bool visible)
    {
        var wo = GetObject(woId) as MVWorldObjectClient;
        if (wo == null) return;

        wo.GetType().GetProperty("Visible")?.SetValue(wo, visible);
    }

    public static IEnumerator AddItemToWorld(int itemId, Vector3 position, Quaternion rotation, System.Action<int> callback)
    {
        Vector3 tempPos = new Vector3(UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(-1000f, 1000f));
        Quaternion tempRot = UnityEngine.Random.rotation;
        int createdId = -1;

        System.Action<int, MVWorldObjectClient> handler = (id, wo) =>
        {
            if (wo.itemId == itemId &&
                Vector3.Distance(wo.Position, tempPos) < 0.1f &&
                Quaternion.Angle(wo.Rotation, tempRot) < 1f)
                createdId = id;
        };

        WorldObjectCreatedPatch.OnWorldObjectCreated += handler;
        MVGameControllerBase.OperationRequests.AddItemToWorld(itemId, RootGroupId, tempPos, tempRot, true, true, false);

        while (createdId == -1)
            yield return null;

        WorldObjectCreatedPatch.OnWorldObjectCreated -= handler;

        SetPosition(createdId, position);
        SetRotation(createdId, rotation);

        callback(createdId);
    }
}

