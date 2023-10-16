using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private int maxLife; 
    private int currentLife;

    private void Start(){
        currentLife = maxLife;
    }

    public void AddLife(int points){
        currentLife += points;
        if (currentLife > maxLife){
            currentLife = maxLife;
        }
    }

    public void SubtractLife(int points){
        currentLife -= points;
        if (currentLife < 0){
            currentLife = 0;
        }
    }

    public int GetCurrentLife(){
        return currentLife;
    }

    public void SetMaxLife(int value){
        maxLife = value;
        if (currentLife > maxLife) currentLife = maxLife;      
    }

    public void SetCurrentLife(int value){
        currentLife = value;
    }

    public int GetMaxLife(){
        return maxLife;
    }
}
