using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeiNachricht : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    string[] nachrichtArray;
    

    private void Start()
    {
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        Nachricht();
    }


    public void Nachricht()
    {
        string[] msg = new string[] {
            "Im Grenzgebiet im Osten hat der Feind auf Grund eines Lieferengpasses keine Flak Munition mehr.",
            "Gestern Nacht sind die Munitionslager für die Panzer durch einen Blitzeinschlag vernichtet worden.Der Feind hat dort nur noch Infanterie.",
            "Der Feind hat momentan nur Flaks an seiner Grenze errichtet. Zudem befindet sich dort eine Fabrik für Panzer.",
            "Der Feind hat verstärkte Luftpatrouillen im Süden.",
            "Die Grenze wird nur durch eine Panzerdivision beschützt."};
        string randomMsg = msg[Random.Range(0, msg.Length)];
        Debug.Log(randomMsg);
        textMesh.text = randomMsg.ToString();

    }
}

