using UnityEngine;

// Escultura flutuante do museu virtual
// Idle: oscila e gira suavemente com som de propulsão suave
// Ao interagir (E): salta para oscilação e rotação máximas, depois desacelera sozinho
public class EsculturaFlutuante : MonoBehaviour
{
    // Parâmetros idle (sem toque)
    private const float AMP_BASE    = 0.04f;
    private const float FREQ_BASE   = 1.5f;
    private const float ROT_BASE    = 20f;
    private const float PITCH_BASE  = 0.8f;
    private const float VOLUME_BASE = 0.15f;

    // Parâmetros máximos (no toque)
    private const float AMP_MAX    = 0.15f;
    private const float FREQ_MAX   = 4f;
    private const float ROT_MAX    = 150f;
    private const float PITCH_MAX  = 2.5f;
    private const float VOLUME_MAX = 0.4f;

    // Tempo em segundos para desacelerar do máximo até o idle
    private const float DECAY_DURATION = 5f;

    // Distância máxima que a escultura desce da posição inicial (evita atravessar o pedestal)
    [SerializeField] private float limiteInferior = 0.08f;

    private Vector3 posicaoInicial;
    private AudioSource audioSource;
    private float decayTimer = 0f;

    private void Start()
    {
        // Usa localPosition para não interferir em outros objetos da hierarquia
        posicaoInicial = transform.localPosition;

        // Configura o som de propulsão em loop 3D
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = CriarSomPropulsao();
        audioSource.loop = true;
        audioSource.volume = VOLUME_BASE;
        audioSource.pitch = PITCH_BASE;
        audioSource.spatialBlend = 1f;
        audioSource.Play();
    }

    private void Update()
    {
        // Decai o timer suavemente até zero (idle)
        decayTimer = Mathf.Max(0f, decayTimer - Time.deltaTime);
        float t = decayTimer / DECAY_DURATION; // 1 = máximo, 0 = idle

        float amplitude  = Mathf.Lerp(AMP_BASE,  AMP_MAX,  t);
        float frequencia = Mathf.Lerp(FREQ_BASE,  FREQ_MAX, t);
        float rotSpeed   = Mathf.Lerp(ROT_BASE,   ROT_MAX,  t);

        // Oscilação vertical com clamp — X e Z fixos na posição inicial
        float rawY  = posicaoInicial.y + Mathf.Sin(Time.time * frequencia) * amplitude;
        float novoY = Mathf.Max(posicaoInicial.y - limiteInferior, rawY);
        transform.localPosition = new Vector3(posicaoInicial.x, novoY, posicaoInicial.z);

        // Rotação horizontal (Y) e vertical (X) acompanhando a intensidade da oscilação
        transform.Rotate(rotSpeed * 0.5f * Time.deltaTime, rotSpeed * Time.deltaTime, 0f, Space.World);

        // Transição suave do áudio
        float targetPitch  = Mathf.Lerp(PITCH_BASE,  PITCH_MAX,  t);
        float targetVolume = Mathf.Lerp(VOLUME_BASE, VOLUME_MAX, t);
        audioSource.pitch  = Mathf.Lerp(audioSource.pitch,  targetPitch,  Time.deltaTime * 2f);
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * 2f);
    }

    // Ao pressionar E: salta para máximo e reseta o timer de decaimento
    public void Interagir()
    {
        decayTimer = DECAY_DURATION;
    }

    // Gera som de turbina/propulsão em loop de 1 segundo sem clique de transição
    private AudioClip CriarSomPropulsao()
    {
        int sampleRate = 44100;
        int amostras   = sampleRate;

        AudioClip clip = AudioClip.Create("SomPropulsao", amostras, 1, sampleRate, false);
        float[] dados  = new float[amostras];

        for (int i = 0; i < amostras; i++)
        {
            float t = (float)i / sampleRate;
            dados[i] = (Mathf.Sin(2f * Mathf.PI * 80f  * t) * 0.5f +
                        Mathf.Sin(2f * Mathf.PI * 120f * t) * 0.3f +
                        Mathf.Sin(2f * Mathf.PI * 160f * t) * 0.2f) * 0.3f;
        }

        clip.SetData(dados, 0);
        return clip;
    }
}
