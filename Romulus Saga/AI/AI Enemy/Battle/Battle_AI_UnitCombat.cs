using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Battle_AI_UnitCombat : MonoBehaviour
{
    
    // Component and stats for enemys in the battlescene
    
    
    [Header("Enemy Data")] 
    public UnitData unitData;

    [Header("Components")] 
    private Camera battleCamera;
    private BattleMenuController battleMenuController;
    private Battle_AI_Controller unitController;
    [SerializeField] private Battle_AI_Controller thisUnit;
    
    [Header("Current Stats")] 
    public int currentAttackRange;
    public int currentMovementRange;
    public float currentHP;
    public int currentDamage;
    public int numberOfUnits;
    public float maxHP;

    [Header("Combat UI")] 
    public TextMeshProUGUI textHP;
    public Image imageHP;
    public TextMeshProUGUI numberOfUnitsText;
    
    private void Start()
    {
        battleMenuController = FindObjectOfType<BattleMenuController>();
        battleCamera = GameObject.Find("BattleCamera").GetComponent<Camera>();
        unitController = GetComponent<Battle_AI_Controller>();

        currentAttackRange = unitData.baseAttackRange;
        currentMovementRange = unitData.baseMovementRange;
        currentHP = unitData.baseHP;
        currentDamage = unitData.baseDamage;
        maxHP = numberOfUnits * currentHP;


        if(this.gameObject.CompareTag("Enemy"))
            thisUnit = unitController.units[unitController.ID];
    }

    private void Update()
    {
        CheckForDeath();
        UpdateCombatUI();
        numberOfUnitsText.text = numberOfUnits.ToString(); 
    }
    

    #region Health Management

    public void RemoveHealth(UnitCombat _playerUnit, Battle_AI_UnitCombat _enemyUnit)
    {
        float damageToPlayer = _playerUnit.currentHP - _enemyUnit.currentDamage * _enemyUnit.numberOfUnits;
        //_playerUnit.currentHP -= _enemyUnit.currentDamage * _enemyUnit.numberOfUnits;
        if (damageToPlayer <= 0)
        {
            float minusZero = damageToPlayer;
            float loseUnits = +minusZero / _playerUnit.unitData.baseHP;
            int loseOfUnitsWholeNum = Mathf.FloorToInt(loseUnits);
            _playerUnit.numberOfUnits += loseOfUnitsWholeNum;
            float compareDifference = +minusZero - _playerUnit.unitData.baseHP * loseOfUnitsWholeNum;
            _playerUnit.currentHP = _playerUnit.unitData.baseHP;
            _playerUnit.currentHP -= compareDifference;
        }
        else if (damageToPlayer > 0)
        {
            _playerUnit.currentHP -= _enemyUnit.currentDamage * _enemyUnit.numberOfUnits;
        }
    }

    private void CheckForDeath()
    {
        if (this.numberOfUnits <= 0)
        {
            //Vllt Animation für Tod und dann zerstören
            
            DestroyImmediate(this.gameObject);
        }
    }

    #endregion

    private void UpdateCombatUI()
    {
        imageHP.fillAmount = Mathf.Round(currentHP / unitData.baseHP * 10) / 10;
        textHP.text = this.currentHP.ToString("F0");
    }
   
}
