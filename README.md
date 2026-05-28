# Primeira Experiência VR Interativa — Museu Virtual

**Aluno:** Anderson Santos da Silva

---

## Apresentando o Projeto

Este projeto consiste em um **Museu Virtual** desenvolvido em Unity com suporte a Realidade Virtual via Meta XR SDK e OpenXR. O ambiente apresenta uma sala de exposição com quadros interativos, esculturas em pedestais e iluminação interna, compondo um espaço cultural navegável em primeira pessoa.

O visitante pode explorar o museu livremente e interagir com os quadros expostos: ao pressionar **E** enquanto olha para uma obra, o quadro é destacado com uma cor diferente, um som de chime é reproduzido e um painel com o título e o nome do artista é exibido na tela.

---

## Contexto e Objetivos

O ambiente representa um **museu cultural no contexto do Metaverso**, com foco em educação e entretenimento. No Metaverso, museus virtuais permitem que pessoas de qualquer parte do mundo acessem exposições de arte e cultura sem barreiras geográficas ou físicas.

O objetivo do espaço é simular uma experiência de visitação cultural imersiva, onde o usuário pode se aproximar das obras e consultar informações sobre elas de forma interativa — reproduzindo digitalmente a experiência de um museu presencial, com o potencial de escalar para exposições globais acessíveis a qualquer pessoa com um headset VR ou computador.

---

## Processo de Criação e Dificuldades

### Como o projeto foi desenvolvido

1. **Configuração técnica:** O projeto foi criado com Unity 6 (6000.4.6f1) com Universal Render Pipeline (URP). O Meta XR SDK foi instalado via Scoped Registry e configurado com OpenXR para Android (Meta Quest), incluindo o perfil Oculus Touch Controller. A plataforma de build foi configurada para Android.

2. **Movimentação no PC:** Para atender ao requisito de movimentação no Editor sem depender do headset, foi utilizado o pacote **Starter Assets - FirstPerson (URP)** da Unity Technologies, que fornece um controlador em primeira pessoa com WASD e mouse.

3. **Ambiente:** A sala do museu foi construída com primitivos do Unity (Plane, Cube, Sphere, Capsule), organizados em grupos lógicos na Hierarchy (`Sala` e `Exposicao`). Foram criados materiais com cores distintas para cada elemento, dando identidade visual ao espaço.

4. **Interação:** A interação foi implementada em C# com dois scripts:
   - `InteracaoQuadro.cs`: gerencia o estado de cada quadro — alterna a cor de destaque, reproduz um som de chime gerado proceduralmente (sem arquivo externo) e exibe as informações da obra.
   - `PlayerInteractor.cs`: lança um raycast do centro da câmera ao pressionar E, detecta quadros interagíveis e aciona a interação.

5. **UI:** Um painel Canvas (Screen Space Overlay) exibe o título e o artista da obra ao interagir, com um crosshair central para auxiliar a mira.

### Principais dificuldades

- **Conflito entre Input System e OnMouseDown:** A interação inicial usava `OnMouseDown()`, que entrava em conflito com o novo Input System do projeto e com o cursor lock do FPS controller. A solução foi migrar para um raycast via tecla E no `Update()`, eliminando o conflito.

- **Configuração do OpenXR no Editor:** A ativação do OpenXR na aba PC causava a renderização estéreo (tela dividida) no Editor, dificultando os testes. A solução foi desativar OpenXR e Mock HMD na aba PC para testes no Editor, mantendo a configuração apenas para Android.

- **Câmera Cinemachine:** O Starter Assets utiliza Cinemachine para seguir o jogador. Foi necessário vincular manualmente o `PlayerCameraRoot` (filho do `PlayerCapsule`) aos campos Follow e Look At do `PlayerFollowCamera` para que a câmera funcionasse corretamente.

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
5. Aproxime-se de um quadro, olhe para ele e pressione **E** para interagir
