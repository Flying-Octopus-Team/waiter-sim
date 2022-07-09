using System.Collections;
using System.Collections.Generic;
using Level;
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
    [SerializeField] private float losingBalanceMinSpeed = 20f;
    [SerializeField] private float balanceControlSpeed = 5f;
    [SerializeField] private float randomDebalanceSpeed = 20f;
    [SerializeField] private float randomDebalanceMinBreak = 2f;
    [SerializeField] private float randomDebalanceMaxBreak = 6f;
    [SerializeField] private NewScoreChecker scoreChecker;
    [SerializeField] private LevelManager levelManager;

    private bool isGameEnded = false;

    void Start()
    {
        StartCoroutine(nameof(UpdateBalanceValue));
        StartCoroutine(nameof(SetRandomBalanceInterrupt));
        StartCoroutine(nameof(ShowCurrentBalance));
        StartCoroutine(nameof(CountScore));
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
                levelManager.GameOver();
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
            BalanceValue += GetImbalanceFactor();
            yield return null;
        }

        yield return null;
    }

    private float GetImbalanceFactor()
    {
        int sign = BalanceValue > 0 ? 1 : -1; 
        float nextimbalanceVal = sign * 
               (Mathf.Pow(losingBalanceBase, Mathf.Abs(BalanceValue)) - 1) * 
               losingBalanceMultiplier;

        if (Mathf.Abs(nextimbalanceVal) < losingBalanceMinSpeed)
        {
            return sign * losingBalanceMinSpeed * 
                Time.deltaTime;
        }
        
        return Mathf.Clamp(
            nextimbalanceVal,
            - 0.8f * balanceControlSpeed,
             0.8f * balanceControlSpeed) * 
            Time.deltaTime;
    }
    private IEnumerator SetRandomBalanceInterrupt()
    {
        while (IsPlayingCondition())
        {
            yield return new WaitForSeconds(
                Random.Range(randomDebalanceMinBreak, randomDebalanceMaxBreak));

            float remainingDebalance = 0;
            float debalancingTime = Random.Range(1f, 2f);

            float startBalanceValue = BalanceValue;
            float targetBalance = Random.Range(20f, 50f);

            int sign = BalanceValue > 0 ? 1 : -1;

            while (remainingDebalance < targetBalance &&
                IsPlayingCondition())
            {
                remainingDebalance += randomDebalanceSpeed * Time.deltaTime;

                BalanceValue += sign * randomDebalanceSpeed * Time.deltaTime;

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

    private IEnumerator CountScore()
    {
        while (IsPlayingCondition())
        {
            scoreChecker.Score++;
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    private bool IsPlayingCondition()
    {
        return BalanceValue > MIN_BALANCE_ALLOWED &&
            BalanceValue < MAX_BALANCE_ALLOWED;
    }
}
