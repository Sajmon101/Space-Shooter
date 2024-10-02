using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;

    private float timer = 0.0f;
    private Vector3 originalCameraPosition;

    void Start()
    {
        // Zapisz oryginaln¹ pozycjê kamery przy starcie
        originalCameraPosition = transform.localPosition;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            DoBobbing();
        }
        else
        {
            // Resetuj timer i pozycjê kamery do oryginalnych wartoœci, gdy gracz nie porusza siê
            timer = 0.0f;
            transform.localPosition = originalCameraPosition;
        }
    }

    void DoBobbing()
    {
        float waveslice = 0.0f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            // Resetuj timer, gdy gracz nie porusza siê
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer += bobbingSpeed;

            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            // Dodaj efekt bujania do oryginalnej pozycji kamery
            Vector3 newPosition = originalCameraPosition + new Vector3(0f, translateChange, 0f);
            transform.localPosition = newPosition;
        }
        else
        {
            // Jeœli nie zachodzi bujanie, przywróæ kamery jej oryginaln¹ pozycjê
            transform.localPosition = originalCameraPosition;
        }
    }
}
