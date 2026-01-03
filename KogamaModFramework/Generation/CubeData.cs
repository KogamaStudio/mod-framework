namespace KogamaModFramework.Generation;

public class CubeData
{
    private static readonly byte[] DefaultMaterials = { 21, 21, 21, 21, 21, 21 };
    private static readonly byte[] DefaultCorners = { 20, 120, 124, 24, 4, 104, 100, 0 };

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    private byte[] _materials;
    public byte[] Materials {
        get => _materials;
        set
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] < 0 || value[i] > 68)
                        value[i] = 21;
                }
                _materials = value;
            }
            else
            {
                _materials = DefaultMaterials;
            }
        }
    }
    public byte[] Corners { get; set; }

    public CubeData(int x, int y, int z, byte[] materials = null, byte[] corners = null)
    {
        X = x; Y = y; Z = z;
        Materials = materials;
        Corners = corners ?? DefaultCorners;
    }
}

