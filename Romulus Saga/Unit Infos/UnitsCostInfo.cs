using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitsCostInfo : MonoBehaviour
{
    public TextMeshProUGUI woodCostsText;
    public TextMeshProUGUI stoneCostsText;
    public TextMeshProUGUI foodCostsText;
    public TextMeshProUGUI goldCostsText;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeInfoText(int woodCost, int stoneCost, int foodCost, int goldCost, int timer)
    {
        woodCostsText.text = $"{woodCost}";
        stoneCostsText.text = $"{stoneCost}";
        foodCostsText.text = $"{foodCost}";
        goldCostsText.text = $"{goldCost}";
        timerText.text = $"{timer}";
    }
}
