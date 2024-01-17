using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GegnerWin : MonoBehaviour
{
    MeshRenderer meshRenderer;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("plane"))
        {
            FindObjectOfType<ManageAudio>().Play("Win");
            DetachFromParent();
            collision.SendMessage("Destroy");
        }
        if (collision.CompareTag("tank"))
        {
            FindObjectOfType<ManageAudio>().Play("Win");
            DetachFromParent();
            collision.SendMessage("Destroy");
        }
        if (collision.CompareTag("soldier"))
        {
            FindObjectOfType<ManageAudio>().Play("Win");
            DetachFromParent();
            collision.SendMessage("Destroy");
            //transform.GetComponent<Renderer>().material = spielerFarbe;
        }
        if (collision.CompareTag("flak"))
        {
            FindObjectOfType<ManageAudio>().Play("Win");
            DetachFromParent();
            collision.SendMessage("Destroy");
        }
    }


    public void DetachFromParent()
    {
        //gameObject.transform.parent = player.transform;
        //Destroy(GetComponent<TestButton>());
        Destroy(GetComponent<GegnerFlak>());
        Destroy(GetComponent<BoxCollider>());
        Destroy(transform.parent.gameObject);
    }

    private void OnDestroy()
    {
        //transform.parent = null;
    }

}
