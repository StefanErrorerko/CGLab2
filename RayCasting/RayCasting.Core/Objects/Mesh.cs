using RayCasting.Core.ObjLoader;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public class Mesh
{
    public List<Vector3> Vertices;
    public List<Vector3> Normals;
    public List<Vector2> TexCoords;
    public List<Face> Faces;

    public Mesh(List<Vector3> vertices, List<Vector3> normals, List<Vector2> texCoords, List<Face> faces)
    {
        Vertices = vertices;
        Normals = normals;
        TexCoords = texCoords;
        Faces = faces;
    }

    public float? Intersects(Ray ray)
    {
        throw new NotImplementedException();
    }

    public Vector3 Normal(Vector3 point)
    {
        throw new NotImplementedException();
    }

    public void Print()
    {
        Console.WriteLine("Vertices:");
        foreach (Vector3 vertex in Vertices)
        {
            Console.WriteLine($"v {vertex.X} {vertex.Y} {vertex.Z}");
        }

        // Console.WriteLine("\nTexture Coordinates:");
        // foreach (Vector2 texCoord in TexCoords)
        // {
        //     Console.WriteLine($"vt {texCoord.X} {texCoord.Y}");
        // }
        //
        // Console.WriteLine("\nNormals:");
        // foreach (Vector3 normal in Normals)
        // {
        //     Console.WriteLine($"vn {normal.X} {normal.Y} {normal.Z}");
        // }
        //
        // Console.WriteLine("\nFaces:");
        // foreach (Face face in Faces)
        // {
        //     Console.Write("f ");
        //     foreach (Vertex vertex in face.Vertices)
        //     {
        //         Console.Write($"{vertex.VertexIndex}/{vertex.TexCoordIndex}/{vertex.NormalIndex} ");
        //     }
        //     Console.WriteLine();
        // }

    }
}