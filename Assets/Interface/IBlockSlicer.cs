using UnityEngine;

// DTO (Data Transfer Object) para retornar os dados do corte
public struct SliceResult
{
    public bool IsGameOver;
    public Vector3 NewScale;
    public Vector3 NewPosition;
    public Vector3 DroppedScale;
    public Vector3 DroppedPosition;
}

// Interface que define o contrato para qualquer método de corte (SOLID: OCP, ISP, DIP)
public interface IBlockSlicer
{
    SliceResult Slice(Transform movingBlock, Transform lastBlock, bool movingOnXAxis);
}