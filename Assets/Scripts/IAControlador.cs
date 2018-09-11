using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class IAControlador : MonoBehaviour
{
    #region ENUMS

    public enum Estados
    {
        Esperar,
        Patrulhar,
        Perseguir,
        Procurar
    }

    #endregion ENUMS

    [Header("Waypoints - Patrulha")]
    public Transform waypoint1;

    public Transform waypoint2;
    private Transform waypointAtual;

    // Componentes - AI
    public AICharacterControl aiCharacterControl;
    private NavMeshAgent navMeshAgent;
    private Vector3 posicaoDestino;
    private Transform alvo;

    private Estados estadoAtual;

    [Header("Variáveis - Controle")]
    public float tempoEsperar = 2f;
    private float tempoEsperando;
    public float campoVisao = 5f;
    public float tempoPersistencia = 6f;
    private float tempoSemVisao;
    public bool seguirEnquantoProcura = false;

    [Header("Distâncias")]
    public float distanciaMinimaWaypoint = 0.15f;

    private float distanciaWaypointAtual;
    private float distanciaJogador;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.destination = Vector3.zero;
    }

    private void Start()
    {
        waypointAtual = waypoint1;

        Esperar();
    }

    private void Update()
    {
        //navMeshAgent.destination = Jogador.instancia.transform.position;

        ChecarEstados();
    }

    private void ChecarEstados()
    {
        if (estadoAtual != Estados.Perseguir
            && PossuiVisaoJogador())
        {
            Perseguir();

            return;
        }

        switch (estadoAtual)
        {
            case Estados.Esperar:
                if (EsperouTempoSuficiente())
                {
                    Patrulhar();
                }
                else
                {
                    alvo = transform;
                }

                break;

            case Estados.Patrulhar:
                if (PertoWaypointAtual())
                {
                    Esperar();

                    AlterarWaypoint();
                }
                else
                {
                    alvo = waypointAtual;
                }

                break;

            case Estados.Perseguir:
                if (!PossuiVisaoJogador())
                {
                    Procurar();
                }
                else
                {
                    alvo = Jogador.instancia.transform;
                }

                break;

            case Estados.Procurar:
                if (SemVisaoTempoSuficiente())
                {
                    Esperar();
                    posicaoDestino = Vector3.zero;
                }

                if (seguirEnquantoProcura)
                {
                    alvo = Jogador.instancia.transform;
                }
                else if (alvo != null)
                {
                    posicaoDestino = alvo.position;
                    alvo = null;
                }

                break;

            default:
                break;
        }

        if (alvo != null)
        {
            if (aiCharacterControl != null)
                aiCharacterControl.target = alvo;
            else
                navMeshAgent.destination = alvo.position;
        }
        else if (posicaoDestino != Vector3.zero)
            navMeshAgent.destination = posicaoDestino;
    }

    // Estados

    private void Esperar()
    {
        estadoAtual = Estados.Esperar;

        tempoEsperando = Time.time;
    }

    private void Patrulhar()
    {
        estadoAtual = Estados.Patrulhar;
    }

    private void Perseguir()
    {
        estadoAtual = Estados.Perseguir;
    }

    private void Procurar()
    {
        estadoAtual = Estados.Procurar;

        tempoSemVisao = Time.time;
    }

    // Métodos Comuns

    private bool EsperouTempoSuficiente()
    {
        return tempoEsperando + tempoEsperar < Time.time;
    }

    private bool SemVisaoTempoSuficiente()
    {
        return tempoSemVisao + tempoPersistencia < Time.time;
    }

    private bool PertoWaypointAtual()
    {
        distanciaWaypointAtual = Vector3.Distance(
            transform.position,
            waypointAtual.position
        );
        return distanciaWaypointAtual < distanciaMinimaWaypoint;
    }

    private bool PossuiVisaoJogador()
    {
        distanciaJogador = Vector3.Distance(
            transform.position,
            Jogador.instancia.transform.position
        );
        return distanciaJogador < campoVisao;
    }

    private void AlterarWaypoint()
    {
        waypointAtual = waypointAtual == waypoint1 ? waypoint2 : waypoint1;
    }
}