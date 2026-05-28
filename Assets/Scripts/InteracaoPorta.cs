using UnityEngine;

// Controla a abertura e fechamento da porta do museu ao pressionar E
public class InteracaoPorta : MonoBehaviour
{
    [SerializeField] private float velocidadeRotacao = 90f;

    private bool aberta = false;
    private bool animando = false;
    private float anguloAtual = 0f;
    private float anguloAlvo = 0f;

    // Chamado pelo PlayerInteractor ao pressionar E olhando para a porta
    public void Interagir()
    {
        if (animando) return;

        aberta = !aberta;
        anguloAlvo = aberta ? 90f : 0f;
        animando = true;
    }

    private void Update()
    {
        if (!animando) return;

        // Rotaciona suavemente a porta em torno da dobradiça até o ângulo alvo
        anguloAtual = Mathf.MoveTowards(anguloAtual, anguloAlvo, velocidadeRotacao * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0f, anguloAtual, 0f);

        if (Mathf.Approximately(anguloAtual, anguloAlvo))
            animando = false;
    }
}
