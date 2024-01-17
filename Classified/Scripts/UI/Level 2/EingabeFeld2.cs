using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EingabeFeld2 : MonoBehaviour
{
    public static EingabeFeld2 Instance { get; private set; }
    [Header("Inputs")]
    [SerializeField] public TMP_InputField guessField;


    string test;
    public string getText;
    public string ifGuessWasRight;
    public GegnerText2 gegnerText;
    public GameObject picture;

    private void Awake()
    {
        Instance = this;
        getText = GegnerText2.Instance.textToCompare;
        ifGuessWasRight = GegnerText2.Instance.ifTextIsRight;
        picture = GegnerText2.Instance.unitPicture;

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
                test = guessField.text.ToLower().Trim();
                Debug.Log(test);
                if (gegnerText != null)
                    Debug.Log(gegnerText.textToCompare);

                if (test.Equals(getText, System.StringComparison.InvariantCultureIgnoreCase))
                {

                    //GegnerText.Instance.textField.text = ifGuessWasRight;
                    gegnerText.textField.text = ifGuessWasRight;
                    picture.SetActive(true);
                    Debug.Log("Right");
                    RoundManager.Instance.roundPerTurn -= 1;
                    guessField.text = null;
                }
                else
                {

                    Debug.Log("Wrong");
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
        //EingabeFeld.Instance.gegnerText = this;
    }
}
