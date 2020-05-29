using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Defines the behavior of the Reset Button
 * Resets the game by bringing all toControl objects back to their original positions.
 */
public class ResetScript : MonoBehaviour
{
    Vector3 initialPosition = new Vector3(0f, 0.25f, 0f);

    /*
     * Called whenever the Reset Button is clicked
     */
    public void ResetScene()
    {
        GameObject[] toControl = GameObject.FindGameObjectsWithTag("toControl");
        foreach (GameObject obj in toControl)
        {
            obj.transform.localPosition = initialPosition;
            obj.transform.localRotation = Quaternion.identity;
        }
    }
}