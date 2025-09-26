using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slotmachine : MonoBehaviour
{
    [SerializeField] private Wheel wheel1;
    [SerializeField] private Wheel wheel2;
    [SerializeField] private Wheel wheel3;

    private int level = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (level == -1)
        {
            wheel1.Stop();
            wheel2.Stop();
            wheel3.Stop();

            Debug.Log("Press space to start the slot machine!");
            level = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (level == 0)
            {
                //start all the wheels spinning again
                wheel1.StartSpin();
                wheel2.StartSpin();
                wheel3.StartSpin();
                level = 1;
            }
            else if (level == 1)
            {
                bool pass = wheel1.Stop();
                if (pass)
                {
                    level++;
                    Debug.Log("Level up! Now at level " + level);
                }
                else
                {
                    Debug.Log("Try again!");
                    level = 0;
                }
            }
            else if (level == 2)
            {
                bool pass2 = wheel2.Stop();
                if (pass2)
                {
                    level++;
                    Debug.Log("Level up! Now at level " + level);
                }
                else
                {
                    Debug.Log("Try again!");
                    level = 0;
                }
            }
            else if (level == 3)
            {
                bool pass3 = wheel3.Stop();
                if (pass3)
                {
                    Debug.Log("You win the jackpot!");
                    level = 0;
                }
                else
                {
                    Debug.Log("Try again!");
                    level = 0;
                }
            }
        }
    }
}
