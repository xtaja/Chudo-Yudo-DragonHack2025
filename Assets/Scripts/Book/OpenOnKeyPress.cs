using UnityEngine;

public class OpenOnKeyPress : MonoBehaviour
{
    public Animator animator;

    public Vector3 currentPos;
    public Vector3 targetPos;

    public bool opening = false;
    float elapsedTime = 0f;
    public float speed;
    
    public void openBook()
    {
        animator.SetBool("open", true);
        animator.speed = 100000;
    }
    private float EaseInOutQuint(float x)
    {
        return x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    public void clickStart()
    {
        animator.SetBool("open", true);
        currentPos = transform.position;
        targetPos = new Vector3(currentPos.x, currentPos.y, 0);
        speed = 2f;
        opening = true;
    }
    
    void Update()
    {
        if (opening)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / speed);
            float easedT = EaseInOutQuint(t);
            transform.position = Vector3.Lerp(currentPos, targetPos, easedT);
        }
    }
}
