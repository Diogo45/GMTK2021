using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturableObjective : MonoBehaviour
{

    public Material CaptureMat;


    public delegate void OnChargeChange(float charge);
    public delegate void OnChargeMaxed(CapturableObjective obj);

    public event OnChargeChange ChargeChanged;
    public event OnChargeMaxed ChargeMaxed;

    public bool Captured = false;
    public float CaptureCharge = 0;
    public float MaxCharge = 100.0f;

    public float ChargeStrength = 1.01f;
    public float ChargeCap = 8.0f;
    public float ChargePow = 1.2f;
    public float ChargeRate = 1.4f;
    public float check_timer = 1.0f;

    public HashSet<GameObject> neighbours;


    public IEnumerator CheckCharge()
    {
        while (!Captured)
        {
            Charge();

            yield return new WaitForSeconds(check_timer);
        }
    }

    public void Charge() 
    {
        int quantity = neighbours.Count;

        float charge = Mathf.Clamp(ChargeRate *  Mathf.Pow(ChargeStrength, ChargePow * quantity), 0f, ChargeCap);

        CaptureCharge += charge;

        ChargeChanged.Invoke(CaptureCharge);

        if (CaptureCharge >= MaxCharge)
        {
            Captured = true;
            ChargeMaxed.Invoke(this);
        }

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

        StartCoroutine(CheckCharge());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Riot"))
        {
            neighbours.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Riot"))
        {
            neighbours.Remove(other.gameObject);
        }
    }

}
