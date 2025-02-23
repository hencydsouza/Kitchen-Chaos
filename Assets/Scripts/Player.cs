using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;


    private bool isWalking;

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move on the X axis only
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X axis, try only Z movement

                // Attempt only z movement
                Vector3 moveDirz = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirz, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirz;
                }
                else
                {
                    // Cannot move in any direction
                }
            }

        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSped = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSped);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
