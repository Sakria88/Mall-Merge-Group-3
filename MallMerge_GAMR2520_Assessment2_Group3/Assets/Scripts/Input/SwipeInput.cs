using UnityEngine;

/// <summary>
/// Detects swipe direction and tells board to move
/// </summary>
public class SwipeInput : MonoBehaviour
{
    public MergeBoardController board;
    public float minSwipeDistance = 60f;

    private Vector2 startPos;
    private bool swiping;

    void Update()
    {
        // Touch
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                startPos = t.position;
                swiping = true;
            }
            else if (t.phase == TouchPhase.Ended && swiping)
            {
                HandleSwipe(startPos, t.position);
                swiping = false;
            }
        }

        // Mouse for editor testing
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            swiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && swiping)
        {
            HandleSwipe(startPos, (Vector2)Input.mousePosition);
            swiping = false;
        }
    }

    private void HandleSwipe(Vector2 start, Vector2 end)
    {
        Vector2 delta = end - start;

        if (delta.magnitude < minSwipeDistance) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            board.Move(delta.x > 0 ? SwipeDir.Right : SwipeDir.Left);
        else
            board.Move(delta.y > 0 ? SwipeDir.Up : SwipeDir.Down);
    }
}
