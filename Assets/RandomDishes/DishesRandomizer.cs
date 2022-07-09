using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishesRandomizer : MonoBehaviour
{
    public Sprite[] dishes;
    public SpriteRenderer[] dishesOnHands;
    public SpriteRenderer targetTableDishes;

    private void Start() 
    {
        targetTableDishes.enabled = false;
    }

    public void RandomizeDishes()
    {
        foreach (SpriteRenderer renderer in dishesOnHands)
        {
            renderer.enabled = true;
            renderer.sprite = dishes[Random.Range(0, dishes.Length)];
        }
        targetTableDishes.enabled = false;
    }

    public void PutDownDishes()
    {
        foreach (SpriteRenderer renderer in dishesOnHands)
        {
            renderer.enabled = false;
        }
        targetTableDishes.enabled = true;
    }
}
