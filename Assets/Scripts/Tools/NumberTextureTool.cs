using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberTextureTool : MonoBehaviourSingletonTemplate<NumberTextureTool>
{
    [SerializeField]
    private Sprite[] awardNums;

    [SerializeField]
    private Sprite[] checkNums;
    
    public Sprite GetAwardNumSprite(int index) {
        return awardNums[index];
    }

    public Sprite GetCheckNumSprite(int index) {
        return checkNums[index];
    }
}
