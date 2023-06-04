using System.Globalization;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.ObjLoader;

public class ObjReader
{
    /// <summary>
    /// Parses string lines of .obj structure to mesh
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Mesh ReadToMesh(string[] data)
    {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var texCoords = new List<Vector2>();
        var faces = new List<Face>();

        foreach (var line in data)
        {
            var parts = line.Split(' ');
            if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
                continue;

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

        return new Mesh(vertices, normals, texCoords, faces);
    }

    /// <summary>
    /// Parses string lines of .obj structure to triangles
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static List<Triangle> ReadToTriangles(string[] data)
    {
        var triangles = new List<Triangle>();

        var vertices = new List<Point3>();
        var faceIndices = new List<string>();

        foreach (var line in data)
        {
            var parts = line.Split(' ');

            if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
                continue;

            switch (parts[0])
            {
                case "v":
                    if (parts.Length < 4)
                        throw new InvalidDataException("Invalid vertex line in OBJ file.");
                    var x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                    var y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                    var z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    vertices.Add(new Point3(x, y, z));
                    break;

                case "f":
                    if (parts.Length < 4)
                    throw new InvalidDataException("Invalid face line in OBJ file.");var pointIndexes = new List<int>();
                    for (int i = 1; i < parts.Length; i++)
                    {
                        var indices = parts[i].Split('/');
                        int pointIndex = int.Parse(indices[0]) - 1;
                        int uvIndex = indices.Length > 1 && !string.IsNullOrEmpty(indices[1]) ? int.Parse(indices[1]) - 1 : -1;
                        int normalIndex = indices.Length > 2 ? int.Parse(indices[2]) - 1 : -1;
                        pointIndexes.Add(pointIndex);

                        // if this is the third or later vertex in the face, create a triangle from it and the two previous vertices
                        if (i >= 3)
                        {
                            triangles.Add(new Triangle(
                                    vertices[pointIndexes[0]], 
                                    vertices[pointIndexes[i - 2]], 
                                    vertices[pointIndexes[i - 1]]));
                        }
                    }

                    //faceIndices.Add(parts[1]);
                    //faceIndices.Add(parts[2]);
                    //faceIndices.Add(parts[3]);
                    break;
            }
        }

        //if (faceIndices.Count % 3 != 0)
        //    throw new InvalidDataException("Invalid face indices count in OBJ file.");

        //for (var i = 0; i < faceIndices.Count; i += 3)
        //{
        //    var index1 = int.Parse(faceIndices[i]) - 1;
        //    var index2 = int.Parse(faceIndices[i + 1]) - 1;
        //    var index3 = int.Parse(faceIndices[i + 2]) - 1;

        //    triangles.Add(new Triangle(vertices[index1], vertices[index2], vertices[index3]));
        //}

        return triangles;
    }
}