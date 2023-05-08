namespace RayCasting.Core.Structures;

public class Vertex
{
    public int VertexIndex;
    public int TexCoordIndex;
    public int NormalIndex;

    public Vertex(int vertexIndex, int texCoordIndex, int normalIndex)
    {
        VertexIndex = vertexIndex;
        TexCoordIndex = texCoordIndex;
        NormalIndex = normalIndex;
    }
    
    public override string ToString()
    {
        return string.Format("Vertex (Index: {0}, TexCoordIndex: {1}, NormalIndex: {2})", VertexIndex, TexCoordIndex, NormalIndex);
    }

}