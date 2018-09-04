using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentar : MonoBehaviour
{
    public float velocidade = 5f;

	void Update()
    {
        float h = Input.GetAxis("Horizontal") * velocidade * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * velocidade * Time.deltaTime;

        transform.Translate(h, 0, v);
    }
}
