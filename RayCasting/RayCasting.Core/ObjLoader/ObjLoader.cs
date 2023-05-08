using System.Globalization;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.ObjLoader;

public class ObjLoader
{
    public static Mesh LoadObj(string filePath)
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var texCoords = new List<Vector2>();
        var faces = new List<Face>();

        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var parts = line.Split(' ');

                switch (parts[0])
                {
                    case "v":
                        var x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        var y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        var z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                        vertices.Add(new Vector3(x, y, z));
                        break;

                    case "vn":
                        var nx = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        var ny = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        var nz = float.Parse(parts[3], CultureInfo.InvariantCulture);
                        normals.Add(new Vector3(nx, ny, nz));
                        break;

                    case "vt":
                        var u = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        var v = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        texCoords.Add(new Vector2(u, v));
                        break;

                    case "f":
                        var face = new Face();
                        for (var i = 1; i < parts.Length; i++)
                        {
                            var faceParts = parts[i].Split('/');
                            var vertexIndex = int.Parse(faceParts[0]) - 1;
                            var texCoordIndex = faceParts.Length > 1 && !string.IsNullOrEmpty(faceParts[1])
                                ? int.Parse(faceParts[1]) - 1
                                : -1;
                            var normalIndex = faceParts.Length > 2 && !string.IsNullOrEmpty(faceParts[2])
                                ? int.Parse(faceParts[2]) - 1
                                : -1;
                            face.Vertices.Add(new Vertex(vertexIndex, texCoordIndex, normalIndex));
                        }

                        faces.Add(face);
                        break;
                }
            }
        }

        return new Mesh(vertices, normals, texCoords, faces);
    }
}