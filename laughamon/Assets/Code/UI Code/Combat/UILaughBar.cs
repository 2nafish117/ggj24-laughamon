using UnityEngine;
using UnityEngine.UI;

public class UILaughBar : MonoBehaviour
{
    [SerializeField]
    private Slider scrollbar;

    private LaughterPoints target;

    public void Initialize(LaughterPoints target)
    {
        this.target = target;
        this.target.OnLaughPointsChanged -= UpdateUI;
        this.target.OnLaughPointsChanged += UpdateUI;

        UpdateUI(this.target.LaughPoints, 0);
    }

    public void OnEnable()
    {
        if (target == null)
        {
            return;
        }

        target.OnLaughPointsChanged -= UpdateUI;
        target.OnLaughPointsChanged += UpdateUI;
    }

    public void OnDisable()
    {
        if (target == null)
        {
            return;
        }

        target.OnLaughPointsChanged -= UpdateUI;
    }

    private void UpdateUI(float currentLaughPoints, float changedAmount)
    {
        var normalizedLP = currentLaughPoints / target.MaxLaughPoints;
        scrollbar.value = Mathf.Clamp01(1 - normalizedLP);
    }
}
