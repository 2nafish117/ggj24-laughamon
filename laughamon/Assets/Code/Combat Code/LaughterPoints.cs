using System;
using UnityEngine;
using UnityEngine.Assertions;

public class LaughterPoints : MonoBehaviour
{
    [Range(0, 500)]
    private float maxLaughPoints = 100;

    [Range(0, 500)]
    private float laughPoints = 100;

    public float LaughPoints => laughPoints;

    public bool IsDead => LaughPoints <= 0;

    public float MaxLaughPoints => maxLaughPoints;

    public event Action<float, float> OnLaughPointsChanged;

    public void Laugh(int amount)
    {
        Assert.IsTrue(amount >= 0);
        laughPoints -= amount;
        OnLaughPointsChanged?.Invoke(laughPoints, -amount);
    }

    public void Heal(int amount)
    {
        Assert.IsTrue(amount >= 0);
        laughPoints = Mathf.Min(MaxLaughPoints, Mathf.Max(0, laughPoints) + amount); ;
        OnLaughPointsChanged?.Invoke(laughPoints, amount);
    }
}
