using System.ComponentModel.DataAnnotations.Schema;

namespace CowPress.Models;

public class Embedding
{
    public Embedding()
    {
        Id = 0;
        VectorBytes = [];
    }

    public int Id { get; set; }
    [Column("Vector")]
    public byte[] VectorBytes { get; private set; }
    [NotMapped]
    public float[] Vector
    {
        get
        {
            float[] result = new float[VectorBytes.Length / sizeof(float)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = BitConverter.ToSingle(VectorBytes, i * sizeof(float));
            }
            return result;
        }
        set
        {
            VectorBytes = value.SelectMany(f => BitConverter.GetBytes(f)).ToArray();
        }
    }
}