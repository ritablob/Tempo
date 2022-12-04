using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    // this is a test script for testing push in Sourcetree :) - Rita
    //Okay - Xavi
    //I'm using this now :) - Xavi

    [SerializeField] RhythmKeeper keeper;

    private void Update()
    {
        float scaleConst = gameObject.transform.localScale.x - Time.deltaTime;
        scaleConst = Mathf.Clamp(scaleConst, 1, 999);

        gameObject.transform.localScale = new Vector3(scaleConst, scaleConst, scaleConst);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.M))
        {
            switch (keeper.timingKey)
            {
                case "Miss":
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case "Early":
                    gameObject.transform.localScale = new Vector3(2, 2, 2);
                    break;
                case "Late":
                    gameObject.transform.localScale = new Vector3(2, 2, 2);
                    break;
                case "Perfect":
                    gameObject.transform.localScale = new Vector3(5, 5, 5);
                    break;
            }
        }
    }
}
