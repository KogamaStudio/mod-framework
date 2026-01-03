using Il2CppMV.WorldObject;
using KogamaModFramework.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace KogamaModFramework.Generation;

public class WorldObject
{
    public readonly MVWorldObject _wo;
    public int Id => _wo.Id;

    public WorldObject(int Id)
    {
        _wo = WorldObjectOperations.GetObject(Id);
    }

    public Vector3 Position
    {
        get => _wo.Position;
        set { WorldObjectOperations.SetPosition(Id, value); }
    }

    public Quaternion Rotation
    {
        get => _wo.Rotation;
        set { WorldObjectOperations.SetRotation(Id, value); }
    }

}
