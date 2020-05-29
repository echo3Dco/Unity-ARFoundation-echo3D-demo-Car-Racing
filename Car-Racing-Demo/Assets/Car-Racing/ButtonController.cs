using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * ButtonController is used to add custom behaviors to the 4 buttons: Forward, Backward, Left, Right
 * Based on button presses/hovers, buttonValues field is updated.
 */
public class ButtonController : MonoBehaviour
{
    public GameObject[] toControl; //Array of objects to control using the buttons
    private Dictionary<string, int> buttonValues = new Dictionary<string, int>(); //Map of button name -> value (0,1)


    void Start()
    {       
        foreach (Transform t in this.gameObject.transform)
        {
            buttonValues.Add(t.name, 0); //Initialize all button values to 0

            //Add two triggers: (OnEnter and OnExit)
            EventTrigger trigger = t.gameObject.AddComponent<EventTrigger>();
            
            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerEnter;
            entry1.callback.AddListener((eventData) => { OnEnter(t); });
            trigger.triggers.Add(entry1);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((eventData) => { OnExit(t); });
            trigger.triggers.Add(entry2);
        }
    }
    

    /*
     * Triggered when screen touch enters the button
     * Transform t: Transform of the button
     * When a screen touch enters the button, the corresponding value in buttonValues is updated to 1
     */
    public void OnEnter(Transform t)
    {
        buttonValues[t.name] = 1;
    }


    /*
     * Triggered when screen touch exits the button
     * Transform t: Transform of the button
     * When a screen touch exits the button, the corresponding value in buttonValues is updated to 0
     */
    public void OnExit(Transform t)
    {
        buttonValues[t.name] = 0;
    }

    
    /*
     * For each child of each gameObject in toControl array, verticalInput field in the CustomBehavior script is changed to val
     * float val: the value that verticalInput is changed to.
     */
    private void changeVerticalInput(float val)
    {
        foreach (GameObject obj in toControl)
        {
            foreach (Transform t in obj.transform)
            {
                t.gameObject.GetComponent<CustomBehaviour>().verticalInput = val;
            }
        }
    }


    /*
     * For each child of each gameObject in toControl array, horizontalInput field in the CustomBehavior script is changed to val
     * float val: the value that horizontalInput is changed to.
     */
    private void changeHorizontalInput(float val)
    {
        foreach (GameObject obj in toControl)
        {
            foreach (Transform t in obj.transform)
            {
                t.gameObject.GetComponent<CustomBehaviour>().horizontalInput = val;
            }
        }
    }
    

    /*
     * Called every frame
     */
    void Update()
    {
        toControl = GameObject.FindGameObjectsWithTag("toControl"); //Finds all objects tagged with "toControl" and updates toControl array

        /*
         * If ("Forward" button is pressed) XOR ("Backward" button is pressed), then changeVerticalInput accordingly
         * If neither "Forward" nor "Backward" are pressed, changeVerticalInput to 0 (i.e. makes the car stop moving forward/backward)
         * If both "Forward" and "Backward" are pressed, changeVerticalInput to 0 (i.e. makes the car stop moving forward/backward)
         */
        if ((buttonValues["Forward"] ^ buttonValues["Backward"]) == 1)
        {
            float newVal = (buttonValues["Forward"] == 1) ? -1: 1;
            changeVerticalInput(newVal);
        }
        else
        {
            changeVerticalInput(0f);
        }

        /*
         * If ("Left" button is pressed) XOR ("Right" button is pressed), then changeHorizontalInput accordingly
         * If neither "Left" nor "Right" are pressed, changeHorizontalInput to 0 (i.e. makes the car stop moving left/right)
         * If both "Left" and "Right" are pressed, changeHorizontalInput to 0 (i.e. makes the car stop moving left/right)
         */
        if ((buttonValues["Left"] ^ buttonValues["Right"]) == 1)
        {
            float newVal = (buttonValues["Left"] == 1) ? 1 : -1;
            changeHorizontalInput(newVal);
        }
        else
        {
            changeHorizontalInput(0f);
        }
    }
}
