using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUI : MonoBehaviour
{
    //public Texture2D basicCursor;
    public Texture2D changedCursor;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware); 
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(changedCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
