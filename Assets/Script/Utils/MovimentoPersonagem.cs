using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidade = 5f;

    // Altere esses valores no Inspector para ajustar o limite da pista
    public float limiteEsquerdo = -2.5f;
    public float limiteDireito = 2.5f;

    void Update()
    {
        // 1. Captura a movimentação (Seta para os lados, A/D ou Touch arrastando)
        float movimentoHorizontal = Input.GetAxis("Horizontal");

        // 2. Calcula a nova posição baseada na velocidade
        float novaPosicaoX = transform.position.x + (movimentoHorizontal * velocidade * Time.deltaTime);

        // 3. O SEGREDO: Limita o valor de X entre o mínimo e o máximo permitido
        novaPosicaoX = Mathf.Clamp(novaPosicaoX, limiteEsquerdo, limiteDireito);

        // 4. Aplica a nova posição ao personagem (mantendo o Y e o Z originais)
        transform.position = new Vector3(novaPosicaoX, transform.position.y, transform.position.z);
    }
}
