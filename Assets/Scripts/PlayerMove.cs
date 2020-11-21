using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove playerMove;
    private CharacterController controller;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        if(playerMove == null)
            playerMove = this.gameObject.GetComponent<PlayerMove>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        gameObject.transform.Translate(move * Time.deltaTime * playerSpeed);
    }
}
