using Il2CppMV.WorldObject;
using KogamaModFramework.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;

namespace KogamaModFramework.Operations;

public class WorldObject
{
    public readonly MVWorldObjectClient wo;
    public int Id => wo.Id;

    public WorldObject(int Id)
    {
        wo = WorldObjectOperations.GetObject(Id) as MVWorldObjectClient;
    }

    public Vector3 Position
    {
        get => wo.Position;
        set { WorldObjectOperations.SetPosition(Id, value); }
    }

    public Quaternion Rotation
    {
        get => wo.Rotation;
        set { WorldObjectOperations.SetRotation(Id, value); }
    }

    public List<int> InputLinks
    {
        get => WorldObjectOperations.GetAllInputLinks(Id);
        set
        {
            var current = WorldObjectOperations.GetAllInputLinks(Id);

            foreach (var id in value.Except(current))
                WorldObjectOperations.AddLink(id, Id);

            foreach (var id in current.Except(value))
                WorldObjectOperations.RemoveLink(id, Id);
        }
    }

    public List<int> OutputLinks
    {
        get => WorldObjectOperations.GetAllOutputLinks(Id);
        set
        {
            var current = WorldObjectOperations.GetAllOutputLinks(Id);

            foreach (var id in value.Except(current))
                WorldObjectOperations.AddLink(Id, id);

            foreach (var id in current.Except(value))
                WorldObjectOperations.RemoveLink(Id, id);
        }
    }

    public void AddInputLink(int fromId)
    {
        WorldObjectOperations.AddLink(fromId, Id);
    }

    public void RemoveInputLink(int fromId)
    {
        WorldObjectOperations.RemoveLink(fromId, Id);
    }

    public void AddOutputLink(int toId)
    {
        WorldObjectOperations.AddLink(Id, toId);
    }

    public void RemoveOutputLink(int toId)
    {
        WorldObjectOperations.RemoveLink(Id, toId);
    }
}
