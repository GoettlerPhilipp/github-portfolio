using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GegnerText2 : MonoBehaviour
{
    public static GegnerText2 Instance { get; private set; }
    [Header("TextField")]
    Button btn;
    [SerializeField] public string textToDisplayOnClick;       //In Inspector: FGFAA...
    [SerializeField] public TextMeshProUGUI textField;

    [Header("InputField")]
    public string textToCompare;                        //In Inspector: helloworld
    [SerializeField] public string ifTextIsRight;      //In Inspector: Hello World
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

        EingabeFeld2.Instance.getText = this.textToCompare;
        EingabeFeld2.Instance.ifGuessWasRight = this.ifTextIsRight;
        EingabeFeld2.Instance.picture = this.unitPicture;
        EingabeFeld2.Instance.gegnerText = this;

        //eingabefeld.TextInputField();
    }


}
