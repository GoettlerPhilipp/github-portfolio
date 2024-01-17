using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRessources : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI faithText;
    public TextMeshProUGUI residenceText;
    public void UpdateText(int wood, int stone, float food, float gold, float faith, float residence, float maxResidence)
    {
        woodText.text = $" {wood}";
        stoneText.text = $" {stone}";
        foodText.text = food.ToString("F0");
        goldText.text = gold.ToString("F0");
        faithText.text = faith.ToString("F0");
        residenceText.text = $" {residence} / {maxResidence}";
    }
}
