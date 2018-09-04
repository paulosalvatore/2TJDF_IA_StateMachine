using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing : MonoBehaviour
{
    public Vector3 sentidoMovimento;
    public float velocidade = 2f;
    public float delayDirecao = 2f;
    private int direcao = 1;

	void Awake()
    {
        InvokeRepeating("InverterDirecao", delayDirecao, delayDirecao);
	}

	void Update()
    {
        transform.Translate(
            sentidoMovimento * direcao * velocidade * Time.deltaTime
        );
	}

    void InverterDirecao()
    {
        direcao *= -1;
    }
}
