using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    protected Animator animator;
    public Interactable focus;
    public TextMeshProUGUI text;
    public bool CrossPunch = false;
    public bool HeavyAttack = false;
    public bool Punch = false;
    public bool Block = false;
    public bool PreserveStats = false;
    string ReceivedMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Punch = false;
        CrossPunch = false;
        Block = false;

        ReceivedMove = SocketManager.instance.ReceivedMove;

        if (ReceivedMove == "Hook")
        {
            text.text = "Hook";
            CrossPunch = true; //ganti state untuk trigger di character animator.
            HeavyAttack = true;
            Invoke("resetHeavyAttack", 2f);
            // Debug.Log("CrossPunch" + CrossPunch);
            Interactable interactable = focus.GetComponent<Collider>().GetComponent<Interactable>(); 
            SetFocus(interactable);
            Invoke("BackToOrigin", 3.0f);
            SocketManager.instance.ResetMove();
        
        }
        else if (ReceivedMove == "Punch") //jalan dan crosspunch
        // else if (Input.GetKeyDown(KeyCode.P)) //jalan dan crosspunch
        {
            text.text = "Punch";
            Punch = true; //ganti state untuk trigger di character animator.
            Interactable interactable = focus.GetComponent<Collider>().GetComponent<Interactable>(); 
            SetFocus(interactable);
            Invoke("BackToOrigin", 3.0f);
            SocketManager.instance.ResetMove();
        }
        else if (ReceivedMove == "Block") //jalan dan crosspunch
        // else if (Input.GetKeyDown(KeyCode.G)) //jalan dan crosspunch
        {
            text.text = "Block";
            Block = true; //ganti state untuk trigger di character animator.
            PreserveStats = true;
            Invoke("resetPreserveStats", 2f);
            // Debug.Log("Punch" + Punch);
            Interactable interactable = focus.GetComponent<Collider>().GetComponent<Interactable>(); 
            SetFocus(interactable);
            Invoke("BackToOrigin", 1.0f);
            SocketManager.instance.ResetMove();
        }
    }

    void resetHeavyAttack()
    {
        HeavyAttack = false;
    }

    void resetPreserveStats()
    {
        PreserveStats = false;
    }

    void BackToOrigin()
    {
        CrossPunch = false; //kembaliin state untuk trigger lagi.
        Punch = false;
        Block = false;
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus) //jika fokusnya tidak sama dengan fokus baru (diawal pastinya kosong)
        {
            if (focus != null) //jika fokus itu berisi (lagi fokus sama suatu karakter)
                focus.OnDeFocused(); //maka ia defocus biar bisa focus ke object lain.
            focus = newFocus; //fokus ke newobject
        }
        newFocus.OnFocused(transform); //panggil method dari class/script "interactable" yang bernama "OnFocused" yang fungsinya set bool value true, dan ngisi 
                                        //player = enemy dan isi status bool hasInteracted jadi false.
    }
}
