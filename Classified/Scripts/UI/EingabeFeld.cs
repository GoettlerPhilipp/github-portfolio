using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EingabeFeld : MonoBehaviour
{
    public static EingabeFeld Instance { get; private set; }
    [Header("Inputs")]
    [SerializeField] public TMP_InputField guessField;
    

    string test;
    public string getText;
    public string ifGuessWasRight;
    public GegnerText gegnerText;
    public GameObject picture;

    private void Awake()
    {
        Instance = this;
        getText = GegnerText.Instance.textToCompare;
        ifGuessWasRight = GegnerText.Instance.ifTextIsRight;
        picture = GegnerText.Instance.unitPicture; 
    }
    private void Update()
    {
        TextInputField();
    }

    public void TextFieldNull()
    {
        guessField.text = null;
    }

    public void TextInputField()
    {
        if (RoundManager.Instance.roundPerTurn > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("enter"))
            {
                //Entfernt Leerzeichen und Satzzeichen des InputFields, um die Lösung mit der Eingabe zu vergleichen.
                test = guessField.text.Replace(" ", string.Empty).ToLower().Trim().Replace("." + ",", string.Empty);
                if (gegnerText != null)

                if (test.Equals(getText, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    gegnerText.textField.text = ifGuessWasRight;
                    picture.SetActive(true);
                    RoundManager.Instance.roundPerTurn -= 1;
                    guessField.text = null;
                }
                else
                {
                    RoundManager.Instance.roundPerTurn -= 1;
                }
            }
        }
        else
        {
            return;
        }
    }


    public void SetNull()
    {
        getText = null;
        ifGuessWasRight = null;
        picture = null;
        gegnerText = null;
    }
}
