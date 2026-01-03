using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace KogamaModFramework.Conversion;

public class RotationConverter
{
    public static Quaternion BytesToQuaternion(byte[] rotBytes)
    {
        float x = ByteToRotation(rotBytes[0]);
        float y = ByteToRotation(rotBytes[1]);
        float z = ByteToRotation(rotBytes[2]);
        return Quaternion.Euler(x, y, z);
    }

    public static byte[] QuaternionToBytes(Quaternion rot)
    {
        Vector3 euler = rot.eulerAngles;
        byte[] result = new byte[3];
        result[0] = RotationToByte(euler.x);
        result[1] = RotationToByte(euler.y);
        result[2] = RotationToByte(euler.z);
        return result;
    }

    public static byte RotationToByte(float angle)
    {
        return (byte)((angle % 360f) / 360f * 255f);
    }

    public static float ByteToRotation(byte b)
    {
        return (b / 255f) * 360f;
    }
}

