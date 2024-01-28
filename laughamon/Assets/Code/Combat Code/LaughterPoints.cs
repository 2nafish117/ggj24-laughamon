using System;
using UnityEngine;
using UnityEngine.Assertions;

public class LaughterPoints : MonoBehaviour
{
    [Range(0, 500)]
    [SerializeField]
    private float maxLaughPoints = 100;

    [Range(0, 500)]
    [SerializeField]
    private float laughPoints = 100;

    public float LaughPoints => laughPoints;

    public bool IsDead => LaughPoints <= 0;

    public float MaxLaughPoints => maxLaughPoints;

    public event Action<float, float> OnLaughPointsChanged;

    public void Laugh(float amount)
    {
        //Assert.IsTrue(amount >= 0);
        laughPoints -= amount;
        laughPoints = Mathf.Clamp(laughPoints, 0, maxLaughPoints);
        OnLaughPointsChanged?.Invoke(laughPoints, -amount);
    }

    public void Heal(float amount)
    {
        Assert.IsTrue(amount >= 0);
        laughPoints = Mathf.Min(MaxLaughPoints, Mathf.Max(0, laughPoints) + amount); ;
        OnLaughPointsChanged?.Invoke(laughPoints, amount);
    }

    public void Init(float maxHP)
    {
        maxLaughPoints = maxHP;
        laughPoints = maxHP;
    }
}
