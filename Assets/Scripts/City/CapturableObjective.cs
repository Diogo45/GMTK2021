using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturableObjective : MonoBehaviour
{

    public Material CaptureMat;


    public delegate float OnChargeChange();

    public event OnChargeChange ChargeChanged;

    public float CaptureCharge = 0;
    public float MaxCharge = 100.0f;


    public void Charge() 
    {
        UpdateMat();
    }

    public void UpdateMat() 
    {
        CaptureMat.SetFloat("_CapturePercent", Mathf.Clamp01(CaptureCharge / MaxCharge));
    }

    // Start is called before the first frame update
    void Start()
    {
        CaptureMat = new Material(CaptureMat);
    }


}
