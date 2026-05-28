# Museu Orbital VR — Primeira Experiência VR Interativa

**Aluno:** Anderson Santos da Silva

---

## Apresentando o Projeto

Este projeto consiste em um **Museu Orbital Virtual** desenvolvido em Unity com suporte a Realidade Virtual via Meta XR SDK e OpenXR. O ambiente representa uma estação museal flutuando em órbita da Terra, visível pela skybox ao sair pela porta de entrada. O interior apresenta quadros interativos, esculturas levitantes em pedestais com som de propulsão, iluminação azulada de estação espacial e revestimento metálico nas superfícies, compondo uma experiência cultural futurista navegável em primeira pessoa.

O visitante pode explorar o museu e interagir com três tipos de objetos:
- **Quadros**: pressione **E** olhando para um quadro — ele é destacado com cor amarela, um som de chime é reproduzido e um painel exibe o título e o artista da obra
- **Porta**: pressione **E** olhando para a porta — ela abre ou fecha suavemente em torno da dobradiça, revelando a vista do espaço exterior e a plataforma orbital
- **Esculturas levitantes**: flutuam e giram continuamente com som de propulsão; pressione **E** para acelerar a oscilação e rotação ao máximo — elas desaceleram sozinhas gradualmente até o estado idle

---

## Contexto e Objetivos

O ambiente representa um **museu cultural orbital no contexto do Metaverso**, situado simbolicamente em órbita da Terra como metáfora da universalidade do conhecimento e da arte. No Metaverso, esse tipo de espaço museológico transcende barreiras físicas e geográficas — qualquer pessoa com um headset VR ou computador pode visitar exposições de arte e cultura de qualquer lugar do mundo, ou até do espaço.

A escolha do cenário orbital reforça a proposta futurista do Metaverso: um museu que não existe no mundo físico, mas que oferece uma experiência cultural imersiva e interativa equivalente ou superior à de um museu presencial. As esculturas levitantes com propulsão reforçam a ambientação de baixa gravidade espacial, enquanto os quadros interativos reproduzem a função educacional do museu em formato digital.

O projeto se enquadra no contexto de **educação e entretenimento** no Metaverso, demonstrando como ambientes virtuais podem ampliar o acesso à cultura.

---

## Processo de Criação e Dificuldades

### Como o projeto foi desenvolvido

1. **Configuração técnica:** O projeto foi criado com Unity 6 (6000.4.6f1) com Universal Render Pipeline (URP). O Meta XR SDK foi instalado via Scoped Registry (`https://npm.xr.meta.com`) e configurado com OpenXR para Android (Meta Quest), incluindo o perfil Oculus Touch Controller e o feature group Meta Quest Support. A plataforma de build foi configurada para Android.

2. **Movimentação no PC:** Para atender ao requisito de movimentação no Editor sem depender do headset, foi utilizado o pacote **Starter Assets - FirstPerson (URP)** da Unity Technologies, com controlador em primeira pessoa WASD + mouse, câmera Cinemachine e CharacterController ajustado (Height: 1.8, Radius: 0.28) para passagem pela porta.

3. **Ambiente:** A sala do museu foi construída inteiramente com primitivos do Unity (Plane, Cube, Sphere, Capsule), organizados em grupos lógicos na Hierarchy (`Sala` e `Exposicao`). Conta com chão, teto, quatro paredes (com abertura para porta), dois pedestais com esculturas, três quadros nas paredes e uma plataforma exterior. A identidade visual de estação espacial foi criada com materiais metálicos do pacote **Yughues Free Metal Materials** (convertidos de Standard para URP) e iluminação interna fria (Point Light azulado). A skybox orbital (Terra vista do espaço) do pacote **Skybox Series Free** é visível ao sair pela porta.

4. **Interações:** Implementadas em C# com quatro scripts:
   - `InteracaoQuadro.cs`: alterna cor de destaque, reproduz som de chime gerado proceduralmente (sem arquivo externo) e exibe informações da obra em painel UI.
   - `InteracaoPorta.cs`: abre e fecha suavemente a porta via rotação animada em torno da dobradiça (Empty Object como pivot).
   - `EsculturaFlutuante.cs`: oscila e gira as esculturas com arranque suave ao iniciar; ao interagir, amplitude e rotação saltam ao máximo e desaceleram gradualmente. Usa fase acumulada no lugar de `Time.time * frequencia` para evitar saltos de posição. Som de propulsão procedural (múltiplas frequências em loop) acompanha a intensidade.
   - `PlayerInteractor.cs`: lança raycast do centro da câmera ao pressionar E, detectando quadros, porta (via `GetComponentInParent`) e esculturas.

5. **UI:** Canvas Screen Space Overlay com crosshair central (`+`) e painel de informações posicionado no centro-baixo da tela, com fundo semitransparente e texto TextMeshPro.

### Assets externos utilizados

| Asset | Fonte | Uso |
|---|---|---|
| Starter Assets - FirstPerson (URP) | Unity Asset Store | Controlador FPS para testes no Editor |
| Skybox Series Free | Unity Asset Store (Avionx) | Skybox orbital da Terra |
| Yughues Free Metal Materials | Unity Asset Store | Texturas metálicas das superfícies |

### Principais dificuldades

- **Conflito entre Input System e OnMouseDown:** A interação inicial usava `OnMouseDown()`, que entrava em conflito com o novo Input System do projeto e com o cursor lock do FPS controller. Solução: raycast via tecla E no `Update()`.

- **Renderização estéreo no Editor:** A ativação do OpenXR na aba PC causava tela dividida (stereo VR) no Editor. Solução: desativar OpenXR e Mock HMD na aba PC para testes, mantendo a configuração apenas para Android.

- **Câmera Cinemachine não seguia o player:** Foi necessário vincular manualmente o `PlayerCameraRoot` aos campos Follow e Look At do `PlayerFollowCamera`.

- **Outros objetos se movendo com as esculturas:** `transform.position` lia X e Z do estado atual a cada frame, causando drift. Solução: migrar para `transform.localPosition` com X e Z fixos na posição inicial.

- **Salto abrupto de posição ao interagir com escultura:** Mudar a frequência do `Mathf.Sin(Time.time * freq)` causava salto de posição por alteração de fase. Solução: substituir por fase acumulada (`fase += frequencia * Time.deltaTime`) e suavizar a amplitude com `Mathf.Lerp`.

- **Vazamento de luz nas paredes:** A Directional Light padrão do Unity atravessa geometria fina. Solução: removê-la e usar exclusivamente o Point Light interno e um Point Light externo.

- **Skybox contribuindo com luz interna:** O skybox ambiente iluminava o interior mesmo com a sala fechada. Solução: definir Environment Lighting Source como Color preta e remover o skybox da câmera principal.

- **Z-fighting nas junções das paredes (chanfro):** Faces coplanares nos cantos criavam artefatos visuais. Solução: Scale Z=9.2 nas paredes Leste e Oeste, posicionando suas extremidades rentes às faces externas de Norte e Sul.

- **Materiais do Yughues incompatíveis com URP:** Os materiais usavam o shader Standard (Built-in). Solução: selecionar todos e usar Edit → Rendering → Materials → Convert Selected Built-in Materials to URP.

- **Player não passava pela porta:** CharacterController com Radius=0.5 tinha diâmetro igual à largura da porta (1m). Solução: ajustar para Radius=0.28, Height=1.8.

---

## Configuração Técnica

| Item | Detalhe |
|---|---|
| Engine | Unity 6 (6000.4.6f1) |
| Render Pipeline | Universal Render Pipeline (URP) |
| XR SDK | Meta XR Core SDK via OpenXR |
| Plataforma de Build | Android (Meta Quest) |
| Input | Unity Input System + StarterAssets FirstPerson |
| Linguagem | C# |

## Como Executar no Editor

1. Abra o projeto no Unity 6
2. Carregue a cena `Assets/Scenes/SampleScene.unity`
3. Clique em **Play**
4. Use **WASD** para mover e **mouse** para olhar
5. Pressione **E** olhando para quadros, porta ou esculturas para interagir
6. Saia pela porta para ver o ambiente orbital externo
