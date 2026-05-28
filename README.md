# Museu Orbital VR — Primeira Experiência VR Interativa

**Aluno:** Anderson Santos da Silva

---

## Apresentando o Projeto

Este projeto consiste em um **Museu Orbital Virtual** desenvolvido em Unity com suporte a Realidade Virtual via Meta XR SDK e OpenXR. O ambiente representa uma estação museal flutuando em órbita da Terra, visível pela skybox ao sair pela porta de entrada. O interior apresenta quadros interativos, esculturas levitantes em pedestais e iluminação azulada de estação espacial, compondo uma experiência cultural futurista navegável em primeira pessoa.

O visitante pode explorar o museu e interagir com três tipos de objetos:
- **Quadros**: pressione **E** olhando para um quadro — ele é destacado, um som de chime é reproduzido e um painel exibe o título e o artista da obra
- **Porta**: pressione **E** olhando para a porta — ela abre ou fecha suavemente, revelando a vista do espaço exterior
- **Esculturas levitantes**: pressione **E** olhando para uma escultura — a oscilação e rotação aceleram ao máximo com o som de propulsão intensificado, desacelerando gradualmente até o idle

---

## Contexto e Objetivos

O ambiente representa um **museu cultural orbital no contexto do Metaverso**, com foco em educação, cultura e entretenimento futurista. A proposta é um espaço museológico que transcende a limitação física e geográfica — um museu acessível a qualquer pessoa com um headset VR, localizado simbolicamente em órbita da Terra como metáfora da universalidade do conhecimento e da arte no Metaverso.

As esculturas levitantes com propulsão reforçam a ambientação de baixa gravidade do espaço, enquanto os quadros interativos reproduzem a função educacional de um museu tradicional em formato digital imersivo.

---

## Processo de Criação e Dificuldades

### Como o projeto foi desenvolvido

1. **Configuração técnica:** O projeto foi criado com Unity 6 (6000.4.6f1) com Universal Render Pipeline (URP). O Meta XR SDK foi instalado via Scoped Registry e configurado com OpenXR para Android (Meta Quest), incluindo o perfil Oculus Touch Controller. A plataforma de build foi configurada para Android.

2. **Movimentação no PC:** Para atender ao requisito de movimentação no Editor sem depender do headset, foi utilizado o pacote **Starter Assets - FirstPerson (URP)** da Unity Technologies, que fornece um controlador em primeira pessoa com WASD e mouse.

3. **Ambiente:** A sala do museu foi construída com primitivos do Unity (Plane, Cube, Sphere, Capsule), organizados em grupos lógicos na Hierarchy (`Sala` e `Exposicao`). A identidade visual de estação espacial foi criada com materiais em tons metálicos azul-acinzentados e iluminação interna fria. A skybox orbital (Terra vista do espaço) é visível ao sair pela porta, reforçando o conceito.

4. **Interações:** Foram implementadas em C# com quatro scripts:
   - `InteracaoQuadro.cs`: alterna cor de destaque, reproduz som de chime procedural e exibe informações da obra.
   - `InteracaoPorta.cs`: abre e fecha suavemente a porta via rotação animada em torno da dobradiça.
   - `EsculturaFlutuante.cs`: oscila e gira as esculturas continuamente; ao interagir, salta para intensidade máxima e desacelera sozinho com o tempo. O som de propulsão é gerado proceduralmente e acompanha a intensidade da animação.
   - `PlayerInteractor.cs`: lança raycast do centro da câmera ao pressionar E, detectando e acionando qualquer objeto interagível.

5. **UI:** Um painel Canvas (Screen Space Overlay) exibe o título e o artista da obra ao interagir com quadros, com crosshair central para auxiliar a mira.

### Principais dificuldades

- **Conflito entre Input System e OnMouseDown:** A interação inicial usava `OnMouseDown()`, que entrava em conflito com o novo Input System do projeto e com o cursor lock do FPS controller. A solução foi migrar para raycast via tecla E no `Update()`.

- **Configuração do OpenXR no Editor:** A ativação do OpenXR na aba PC causava renderização estéreo (tela dividida) no Editor. A solução foi desativar OpenXR e Mock HMD na aba PC para testes, mantendo a configuração apenas para Android.

- **Câmera Cinemachine:** Foi necessário vincular manualmente o `PlayerCameraRoot` (filho do `PlayerCapsule`) aos campos Follow e Look At do `PlayerFollowCamera` para a câmera funcionar corretamente.

- **Outros objetos se movendo junto com as esculturas:** Ao usar `transform.position` para animar a oscilação, valores de X e Z eram lidos do estado atual a cada frame, causando interferência visual. A solução foi migrar para `transform.localPosition` com X e Z fixos na posição inicial.

- **Vazamento de luz nas junções das paredes:** A Directional Light padrão do Unity atravessa geometria fina, causando luz visível nas junções das paredes. A solução foi remover a Directional Light e depender exclusivamente do Point Light interno e da luz externa para iluminar a cena.

- **Z-fighting nas junções das paredes:** Paredes com faces coplanares nos cantos causavam artefatos visuais (chanfro). A solução foi fazer as paredes Leste e Oeste com Scale Z=9.2, posicionando suas extremidades rentes às faces externas das paredes Norte e Sul, eliminando qualquer sobreposição de faces.

---

## Configuração Técnica

- **Engine:** Unity 6 (6000.4.6f1)
- **Render Pipeline:** Universal Render Pipeline (URP)
- **XR SDK:** Meta XR Core SDK via OpenXR
- **Plataforma de Build:** Android (Meta Quest)
- **Input:** Unity Input System + StarterAssets FirstPerson Controller

## Como Executar no Editor

1. Abra o projeto no Unity 6
2. Carregue a cena `Assets/Scenes/SampleScene.unity`
3. Clique em **Play**
4. Use **WASD** para mover e **mouse** para olhar
5. Pressione **E** olhando para quadros, porta ou esculturas para interagir
6. Saia pela porta para ver o ambiente orbital externo
