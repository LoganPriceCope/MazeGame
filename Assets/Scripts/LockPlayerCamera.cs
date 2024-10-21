using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPlayerCamera : MonoBehaviour
{
    public void LockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
