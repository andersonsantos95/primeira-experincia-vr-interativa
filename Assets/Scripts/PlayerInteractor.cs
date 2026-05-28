using UnityEngine;
using TMPro;

// Gerencia a interação do jogador com objetos da cena via raycast
// Pressione E para interagir com um quadro ou porta ao olhar para ele
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float distanciaInteracao = 5f;

    private GameObject painelInfo;
    private TextMeshProUGUI textoInfo;

    private void Start()
    {
        // Busca o painel de UI na cena
        painelInfo = GameObject.Find("PainelInfo");
        if (painelInfo != null)
        {
            textoInfo = painelInfo.GetComponentInChildren<TextMeshProUGUI>();
            painelInfo.SetActive(false);
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        // Lança um raio do centro da câmera para frente
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteracao))
        {
            // Verifica se é um quadro interativo
            InteracaoQuadro quadro = hit.collider.GetComponent<InteracaoQuadro>();
            if (quadro != null)
            {
                quadro.Interagir(painelInfo, textoInfo);
                return;
            }

            // Verifica se é a porta (busca o script no pai para respeitar a dobradiça)
            InteracaoPorta porta = hit.collider.GetComponentInParent<InteracaoPorta>();
            if (porta != null)
                porta.Interagir();
        }
    }
}
