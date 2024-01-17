using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RessourceTypes{
    wood,
    stone,
    food,
    gold,
    faith,
    residenceCapacity,
    currentResidence
}

public sealed class Ressource : MonoBehaviour
{
    //Ressource types
    [SerializeField] private RessourceTypes ressourceTypes;

    public RessourceTypes RessourceTypes
    {
        get { return ressourceTypes; }
        set { ressourceTypes = value; }
    }

    [SerializeField] private int initialAmount;

    public int InitialAmount
    {
        get { return initialAmount; }
        set { initialAmount = value; }
    }

    [SerializeField] private int amountAvailable;

    public int AmountAvailable
    {
        get { return amountAvailable; }
        set { amountAvailable = value; }
    }

    private void Start()
    {
        AmountAvailable = InitialAmount;
    }

    public delegate void RessorceExhausted();
    public event RessorceExhausted OnResourceExhausted;
}