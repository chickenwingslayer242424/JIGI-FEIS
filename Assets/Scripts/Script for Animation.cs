using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptforAnimation : MonoBehaviour

{
    public Animator animator;
    private GameManager gameManager;
    private ClickManager clickManager;
     private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        clickManager = FindObjectOfType<ClickManager>();
    }

    void Update()
    {
        animator.SetFloat("Speed",clickManager.isMoving ? 1.0f : 0.0f); 


    }





}
