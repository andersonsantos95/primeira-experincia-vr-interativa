using UnityEngine;
using TMPro;

// Representa um quadro interativo do museu virtual
public class InteracaoQuadro : MonoBehaviour
{
    [SerializeField] private string tituloObra = "Sem Título";
    [SerializeField] private string nomeArtista = "Artista Desconhecido";
    [SerializeField] private Color corDestaque = Color.yellow;

    private Renderer rend;
    private Color corOriginal;
    private bool destacado = false;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        corOriginal = rend.material.color;
    }

    // Chamado pelo PlayerInteractor ao pressionar E olhando para este quadro
    public void Interagir(GameObject painelInfo, TextMeshProUGUI textoInfo)
    {
        destacado = !destacado;

        // Alterna a cor de destaque
        rend.material.color = destacado ? corDestaque : corOriginal;

        if (painelInfo == null) return;

        // Exibe ou oculta o painel com informações da obra
        painelInfo.SetActive(destacado);

        if (destacado && textoInfo != null)
            textoInfo.text = $"Obra: {tituloObra}\nArtista: {nomeArtista}";
    }
}
