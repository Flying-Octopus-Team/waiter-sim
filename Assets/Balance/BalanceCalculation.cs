using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceCalculation : MonoBehaviour
{
    const float MIN_BALANCE_ALLOWED = -100;
    const float MAX_BALANCE_ALLOWED = 100;

    private float _balanceValue;
    public float BalanceValue
    {
        get => _balanceValue;
        set
        {
            _balanceValue = value;
            balanceSlider.value = _balanceValue;
        }
    }
    [SerializeField] private Slider balanceSlider;
    [SerializeField] private float losingBalanceBase = 1.02f;
    [SerializeField] private float losingBalanceMultiplier = 2f;
    [SerializeField] private float balanceControlSpeed = 5f;
    [SerializeField] private NewScoreChecker scoreChecker;

    private bool isGameEnded = false;

    void Start()
    {
        StartCoroutine(nameof(UpdateBalanceValue));
        StartCoroutine(nameof(SetRandomBalanceInterrupt));
        StartCoroutine(nameof(ShowCurrentBalance));
    }

    void Update() 
    {
        if (IsPlayingCondition())
        {
            if (Input.GetKey("a"))
            {
                BalanceValue -= balanceControlSpeed * Time.deltaTime;
            }
            else if (Input.GetKey("d"))
            {
                BalanceValue += balanceControlSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (!isGameEnded)
            {
                isGameEnded = true;
                scoreChecker.OnGameEnded();
            }
        }

        BalanceValue = Mathf.Clamp(BalanceValue, MIN_BALANCE_ALLOWED, MAX_BALANCE_ALLOWED);
    }

    private IEnumerator UpdateBalanceValue()
    {
        while(IsPlayingCondition())
        {
            int sign = BalanceValue > 0 ? 1 : -1; 
            BalanceValue += 
                sign * 
                (Mathf.Pow(losingBalanceBase, Mathf.Abs(BalanceValue)) - 1) * 
                losingBalanceMultiplier * 
                Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
    private IEnumerator SetRandomBalanceInterrupt()
    {
        while (IsPlayingCondition())
        {
            yield return new WaitForSeconds(Random.Range(4,8));

            float elapsedTime = 0;
            float debalancingTime = Random.Range(1f, 2f);

            float startBalanceValue = BalanceValue;
            float targetBalance = Random.Range(-70f, 70f);

            while (elapsedTime < debalancingTime &&
                IsPlayingCondition())
            {
                elapsedTime += Time.deltaTime;

                BalanceValue = Mathf.Lerp(
                    startBalanceValue, targetBalance, elapsedTime / debalancingTime);

                yield return null;
            }
        }
    }

    private IEnumerator ShowCurrentBalance()
    {
        while (IsPlayingCondition())
        {
            Debug.Log(BalanceValue.ToString());
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log(BalanceValue.ToString());
        yield return null;
    }

    private bool IsPlayingCondition()
    {
        return BalanceValue > MIN_BALANCE_ALLOWED &&
            BalanceValue < MAX_BALANCE_ALLOWED;
    }
}
