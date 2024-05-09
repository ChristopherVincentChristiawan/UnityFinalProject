using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Interactable focus;
    public int someRandomNumber;
    public bool attack = false;
    string ReceivedMove;

    void Update()
    {
        // Debug.Log("Enemy Attack Status:" + attack); //buat ngecek status dibawah ini.
        attack = false;
        ReceivedMove = SocketManager.instance.ReceivedMove;
        if (ReceivedMove == "Hook" | ReceivedMove == "Punch" | ReceivedMove == "Block")
        {
            attack = true; //ini untuk eksekusi script enemyAnimator biar random pick moves. Bekerja dengan baik.
            System.Random R = new System.Random();
            someRandomNumber = R.Next(1, 4);
            Interactable interactable = focus.GetComponent<Collider>().GetComponent<Interactable>();
            SetFocus(interactable);
            Invoke("BackToOrigin", 0.5f);
        }
    }

    void BackToOrigin()
    {
        attack = false;
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus) //jika fokusnya tidak sama dengan fokus baru (diawal pastinya kosong)
        {
            if (focus != null) //jika fokus itu berisi (lagi fokus sama suatu karakter)
                focus.OnDeFocused(); //maka ia defocus biar bisa focus ke object lain.
            focus = newFocus; //fokus ke newobject
            // motor.FollowTarget(newFocus); //follow focus itu, tapi motor udah gakepake
        }
        newFocus.OnFocused(transform); //panggil method dari class/script "interactable" yang bernama "OnFocused" yang fungsinya set bool value true, dan ngisi 
                                        //player = enemy dan isi status bool hasInteracted jadi false.
    }
}
