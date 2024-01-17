using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GegnerTank : MonoBehaviour
{
    //public GameObject player;
    int randomDigit;
    MeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("plane"))
        {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                FindObjectOfType<ManageAudio>().Play("Win");
                Destroy(transform.parent.gameObject);
                collision.SendMessage("Destroy");
                RoundManager.Instance.roundPerTurn -= 1;
            }
            else
            {
                collision.SendMessage("ResetPos");
            }
        }
        if (collision.CompareTag("tank"))
        {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                randomDigit = Random.Range(0, 5);
                if (randomDigit == 1)
                {
                    FindObjectOfType<ManageAudio>().Play("Win");
                    Destroy(transform.parent.gameObject);
                    collision.SendMessage("Destroy");
                    RoundManager.Instance.roundPerTurn -= 1;
                }
                else
                {
                    FindObjectOfType<ManageAudio>().Play("Lose");
                    collision.SendMessage("Destroy");
                    RoundManager.Instance.roundPerTurn -= 1;
                }
            }
            else
            {
                collision.SendMessage("ResetPos");
            }
        }
        if (collision.CompareTag("soldier"))
        {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                FindObjectOfType<ManageAudio>().Play("Lose");
                collision.SendMessage("Destroy");
                RoundManager.Instance.roundPerTurn -= 1;
            }
            else
            {
                collision.SendMessage("ResetPos");
            }

        }
        if (collision.CompareTag("flak"))
        {
            if (RoundManager.Instance.roundPerTurn > 0)
            {
                randomDigit = Random.Range(0, 5);
                if (randomDigit == 1)
                {
                    FindObjectOfType<ManageAudio>().Play("Win");
                    Destroy(transform.parent.gameObject);
                    collision.SendMessage("Destroy");
                    RoundManager.Instance.roundPerTurn -= 1;
                }
                else
                {
                    FindObjectOfType<ManageAudio>().Play("Lose");
                    collision.SendMessage("Destroy");
                    RoundManager.Instance.roundPerTurn -= 1;
                }
            }
            else
            {
                collision.SendMessage("ResetPos");
            }
        }
    }
}
