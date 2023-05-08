namespace RayCasting.Core.Structures;

public class Face
{
    public List<Vertex> Vertices = new List<Vertex>();
    
    public override string ToString()
    {
        return string.Format("Face (Vertices: {0})", Vertices.Count);
    }
}