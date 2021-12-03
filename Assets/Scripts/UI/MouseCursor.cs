using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D cursorTex;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);
    }
}
