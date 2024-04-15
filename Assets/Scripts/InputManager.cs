using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public SphereController sphereController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sphereController != null)
            {
                sphereController.StopMovement();
            }
        }
    }
}
