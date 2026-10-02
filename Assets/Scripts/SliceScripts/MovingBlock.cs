using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveSpeed = 3f;
    private bool moveOnX = true;
    private bool isMoving = true;

    // Define em qual eixo o bloco vai se mover
    public void Initialize(bool moveOnXAxis)
    {
        moveOnX = moveOnXAxis;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void Update()
    {
        if (!isMoving) return;

        // Cria o movimento de vai e vem usando Mathf.PingPong
        float pingPongValue = Mathf.PingPong(Time.time * moveSpeed, 6f) - 3f;

        if (moveOnX)
        {
            transform.position = new Vector3(pingPongValue, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, pingPongValue);
        }
    }
}