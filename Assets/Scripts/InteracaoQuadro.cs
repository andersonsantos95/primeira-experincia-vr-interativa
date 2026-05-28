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
    private AudioSource audioSource;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        corOriginal = rend.material.color;

        // Adiciona e configura o AudioSource para o som de interação
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = CriarSomInteracao();
    }

    // Chamado pelo PlayerInteractor ao pressionar E olhando para este quadro
    public void Interagir(GameObject painelInfo, TextMeshProUGUI textoInfo)
    {
        destacado = !destacado;

        // Toca o som de interação
        audioSource.Play();

        // Alterna a cor de destaque
        rend.material.color = destacado ? corDestaque : corOriginal;

        if (painelInfo == null) return;

        // Exibe ou oculta o painel com informações da obra
        painelInfo.SetActive(destacado);

        if (destacado && textoInfo != null)
            textoInfo.text = $"Obra: {tituloObra}\nArtista: {nomeArtista}";
    }

    // Gera um som de chime simples sem precisar de arquivo externo
    private AudioClip CriarSomInteracao()
    {
        int sampleRate = 44100;
        float duracao = 0.4f;
        int amostras = (int)(sampleRate * duracao);

        AudioClip clip = AudioClip.Create("SomInteracao", amostras, 1, sampleRate, false);
        float[] dados = new float[amostras];

        for (int i = 0; i < amostras; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = 1f - (t / duracao);
            // Tom em 880Hz (nota A5) com fade out suave
            dados[i] = Mathf.Sin(2f * Mathf.PI * 880f * t) * envelope * 0.5f;
        }

        clip.SetData(dados, 0);
        return clip;
    }
}
