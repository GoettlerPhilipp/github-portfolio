using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GegnerText : MonoBehaviour
{
    public static GegnerText Instance { get; private set; }
    [Header("TextField")]
    Button btn;
    [SerializeField] public string textToDisplayOnClick;       //In Inspector: Verschlüsselte Nachricht
    [SerializeField] public TextMeshProUGUI textField;

    [Header("InputField")]
    public string textToCompare;                                //In Inspector Entschlüsselte Nachricht ohne Leerzeichen 
    [SerializeField] public string ifTextIsRight;               //In Inspector Entschlüsselte Nachricht als sauberen Satz
    public GameObject unitPicture;
    EingabeFeld eingabefeld;


    private void Awake()
    {
        Instance = this;
        btn = GetComponent<Button>();
        
    }

    public void HandleClick()
    {
        textField.text = textToDisplayOnClick;
        textField.gameObject.SetActive(true);
    }

    public void GetText()
    {
        //Gibt dem Brief die Nachricht des Landes an, auf dass der Spieler geklickt hat.
        EingabeFeld.Instance.getText = this.textToCompare;
        EingabeFeld.Instance.ifGuessWasRight = this.ifTextIsRight;
        EingabeFeld.Instance.picture = this.unitPicture;
        EingabeFeld.Instance.gegnerText = this;
    }

    
}
