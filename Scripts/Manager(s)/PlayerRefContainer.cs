using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRefContainer : MonoBehaviour
{
    private CharacterController cc;
    public CharacterController CC
    {//read only
        get
        {
            if (cc == null)
                cc = GetComponent<CharacterController>();

            return cc;
        }
    }

    private PlayerMovement movement;
    public PlayerMovement Movement
    {//read only
        get
        {
            if (movement == null)
                movement = GetComponent<PlayerMovement>();

            return movement;
        }
    }

    private PlayerInputMap input;
    public PlayerInputMap Input
    {//read only
        get
        {
            if (input == null)
                input = GetComponent<PlayerInputMap>();

            return input;
        }
    }

    private PlayerInteraction interaction;
    public PlayerInteraction Interaction
    {//read only
        get
        {
            if (interaction == null)
                interaction = GetComponent<PlayerInteraction>();

            return interaction;
        }
    }

    private SimulatedSlimes simulator;
    public SimulatedSlimes Simulator
    {//read only
        get
        {
            if (simulator == null)
                simulator = GetComponent<SimulatedSlimes>();

            return simulator;
        }
    }
}
