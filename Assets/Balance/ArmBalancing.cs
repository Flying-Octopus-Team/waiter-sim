using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBalancing : MonoBehaviour
{
    public float maxAngleDifference = 45;
    public BalanceCalculation balanceCalculation;
    public Transform spine;

    private void Update() 
    {
        BalanceArms();
    }

    private void BalanceArms()
    {
        int sign = balanceCalculation.BalanceValue > 0 ? 1 : -1;
        float normalizedBalance = Mathf.Abs(balanceCalculation.BalanceValue) / 100f;
        float targetAngle = Mathf.Lerp(
            0,
            (sign * maxAngleDifference), 
            normalizedBalance);
        spine.transform.eulerAngles = new Vector3(0, 0, 90 -targetAngle);
    }
}

