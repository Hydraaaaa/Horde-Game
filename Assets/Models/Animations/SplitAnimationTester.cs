using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAnimationTester : MonoBehaviour
{
    Animator anim;

    public bool anim1 = false;
    public bool anim2 = false;
    public bool anim3 = false;
    public bool anim4 = false;
    public bool anim5 = false;
    public bool anim6 = false;
    public bool anim7 = false;
    public bool anim8 = false;
    
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        anim1 = Input.GetKey(KeyCode.Alpha1);
        anim.SetBool("1", anim1);

        anim2 = Input.GetKey(KeyCode.Alpha2);
        anim.SetBool("2", anim2);

        anim3 = Input.GetKey(KeyCode.Alpha3);
        anim.SetBool("3", anim3);

        anim4 = Input.GetKey(KeyCode.Alpha4);
        anim.SetBool("4", anim4);

        anim5 = Input.GetKey(KeyCode.Alpha5);
        anim.SetBool("5", anim5);

        anim6 = Input.GetKey(KeyCode.Alpha6);
        anim.SetBool("6", anim6);

        anim7 = Input.GetKey(KeyCode.Alpha7);
        anim.SetBool("7", anim7);

        anim8 = Input.GetKey(KeyCode.Alpha8);
        anim.SetBool("8", anim8);
    }
}
