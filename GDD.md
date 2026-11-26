# GDD — Pequena Formiga

> Fonte: GDD da equipe no Notion. Este arquivo é uma cópia organizada para consulta dentro do repositório. Em caso de divergência, o Notion é a referência oficial.

## Sumário

1. [Visão Geral](#1-visão-geral)
2. [Design](#2-design)
3. [Audiovisual](#3-audiovisual)
4. [Mecânicas](#4-mecânicas)
5. [Conteúdo](#5-conteúdo)
6. [Roadmap](#6-roadmap)
7. [Referências](#7-referências)
8. [Outras Possíveis Ideias (fora do escopo atual)](#8-outras-possíveis-ideias-fora-do-escopo-atual)
9. [Código e Arquitetura](#9-código-e-arquitetura)
10. [Notas de Alinhamento com o Repositório](#10-notas-de-alinhamento-com-o-repositório)

---

## 1. Visão Geral

### Elevator Pitch

Imagine a grandiosidade e os perigos do mundo real vistos pelos olhos de um ser minúsculo. Em **Pequena Formiga**, o jogador assume o papel de uma corajosa formiga operária com a missão de deixar a segurança do formigueiro para encontrar comida e garantir a sobrevivência da colônia.

O que para nós é apenas um jardim tranquilo, para ela é um campo de batalha. Ela precisa usar agilidade e inteligência para superar obstáculos, predadores colossais e o clima.

Sem magia ou superpoderes, contando apenas com o instinto num mundo em **2.5D**, cada migalha levada de volta para casa garante a sobrevivência da espécie.

### Fantasia Principal

Sentir na pele a grandiosidade e os perigos da natureza pela perspectiva de uma pequena formiga. O jogador usa agilidade, instinto e inteligência para superar predadores maiores e as intempéries imprevisíveis do clima. A experiência central é **ser minúsculo em um mundo gigante**, onde cada migalha coletada e levada ao formigueiro é uma grande vitória.

### Gêneros

- Um jogador
- Plataforma 2.5D
- Ação e Aventura, com progressão linear baseada em fases

### Público-Alvo e Plataformas

- **Plataformas:** PC e consoles. O jogo é pensado para controle, por ser de plataforma e ação rápida.
- **Público-alvo:** adolescentes e jovens adultos que gostam de jogos de plataforma dinâmicos e mecânicas ágeis, e que buscam um desafio moderado e recompensador, valorizando reflexos e estratégia. Por ser dividido em fases, atende jogadores com pouco tempo livre que ainda querem uma experiência ativa.

### Persona

_(Seção ainda não preenchida no GDD.)_

---

## 2. Design

### Pilares de Design

> Qualquer feature deve reforçar pelo menos um pilar e não contradizer os outros.

| # | Pilar | Descrição |
|---|---|---|
| 1 | **Desafio Justo e Ágil** | O jogo exige reflexos e estratégia, mas nunca pode parecer injusto. O reinício de fase deve ser rápido para manter o fluxo e incentivar novas tentativas, permitindo aprender os padrões dos inimigos e do cenário sem frustração. |
| 2 | **A Perspectiva do Minúsculo** | Level design, arte e áudio reforçam que o jogador é uma formiga num mundo gigante. Obstáculos comuns (poças, garrafas, folhas) viram desafios de plataforma, e predadores naturais são ameaças imponentes. |
| 3 | **Adaptação Constante** | O cenário não é estático. A alternância entre **Sol** e **Chuva** obriga o jogador a mudar a forma de jogar no meio da fase: continuar avançando no Sol ou usar Cavar/Esconder para sobreviver à Chuva e às gotas. |
| 4 | **Foco no Objetivo** | Cada fase tem um objetivo único e claro: encontrar a comida e levá-la ao formigueiro. Todas as mecânicas (Andar, Correr, Escalar, Pegar/Jogar) servem para otimizar essa rota de ida e volta. |

### Core Loop

1. **Entrar na fase:** explorar o cenário e os padrões do clima (Sol/Chuva) para achar o caminho até a comida.
2. **Sobrevivência e coleta:** evitar inimigos (patrulha, ataque), usar Escalar ou Esconder e, por fim, pegar a comida.
3. **O regresso:** voltar ao formigueiro carregando a comida. Os desafios mudam para o jogador nesse trecho.
4. **Conclusão:** entregar a comida em segurança e desbloquear o próximo nível.

### Progressão

- **Desbloqueio de fases:** a comida coletada não é moeda. Ela funciona como "chave" para alimentar a colônia e liberar a próxima fase ou bioma (ex.: do "Jardim" para o "Interior de Casa").
- **Novas ameaças:** transições de clima mais frequentes e inimigos mais agressivos ao longo do jogo.

---

## 3. Audiovisual

### Estética

- **Perspectiva macro:** estilo visual 2.5D em que objetos cotidianos (folhas, latas de refrigerante, galhos, pedras) são gigantescos, reforçando a fragilidade da formiga.
- **Mapa de seleção:** mapa no estilo Mario, em que o personagem anda por uma trilha de fases ambientada num jardim.
  - O início do mapa é o **formigueiro**.
  - Cada fase deve ter algum indicativo de que foi concluída.

### Assets

| Categoria | Conteúdo |
|---|---|
| **Cenário** | Natureza gigante (grama alta, cogumelos para saltar, poças d'água) e lixo humano usado como plataforma (tampas de garrafa, caixas de fósforos). |
| **Inimigos** | Predadores naturais em escala grande: aranhas, besouros, pássaros, sapos. |
| **Recursos (comida)** | Pedaços de fruta, migalhas de pão, doces caídos no chão, que contrastam visualmente com o cenário natural. |
| **Efeitos especiais** | Impacto visual forte para as gotas de chuva gigantes; poeira ao usar Cavar/Esconder; indicadores visuais (ponto de exclamação) quando um inimigo detecta o jogador. |
| **UI** | Design minimalista e orgânico. |

Pacote de efeitos de referência: **Super Pixel Effects Gigapack** (itch.io).

### Interfaces

- **Interface de gameplay:** só o essencial.
  - Os **2 pontos de vida** da formiga (ex.: dois corações ou antenas).
  - Ícone indicando se a comida já foi coletada.
  - Ícone do evento climático atual.
- **Interface de menus e mapa:** tela de título, tela de Game Over (com reinício rápido) e mapa de seleção de fases.

### Sons

- **Efeitos sonoros (SFX):** som satisfatório ao pegar a comida, som rápido de "escavar" ao usar Esconder, sons de impacto pesados para as gotas de chuva.
- **Trilha sonora:** música ambiente ritmada durante o Sol, com transição para tons mais tensos e rápidos durante a Chuva ou perseguições.

---

## 4. Mecânicas

> Descrição funcional das mecânicas principais, sem detalhes de implementação.

### Mecânicas do Jogador (Sobrevivência e Ação)

| Mecânica | Descrição |
|---|---|
| **Vida e Morte (2 hits)** | A formiga aguenta 2 golpes. Ao receber o segundo (de inimigo ou de gota de chuva), ela morre, perde a comida e volta ao início da fase. **Vidas infinitas.** |
| **Andar e Correr** | Movimentação principal pelo cenário 2.5D, permitindo navegação rápida ou cuidadosa. |
| **Escalar** | Subir em elementos escaláveis do cenário (gravetos, folhas) para alcançar caminhos mais altos e fugir do chão. |

---

## 5. Conteúdo

### História Geral

O mundo além dos túneis de terra é vasto, impiedoso e esmagador. Para os gigantes que caminham sobre ele, é só um jardim; para a pequena protagonista, é um universo inteiro. Destemida, a formiga operária deixa o formigueiro com um único propósito: encontrar alimento para garantir a sobrevivência da colônia.

A missão parece simples, mas a vegetação esconde perigos a cada passo. Sob o sol, raízes e folhas abrem caminhos para avançar e carregar o alimento. Quando a chuva chega, cada gota pode virar uma ameaça, e a exploradora precisa se adaptar para sobreviver.

Os perigos não vêm só do céu: entre as folhas, predadores espreitam atentos a qualquer movimento. Com a carga nas costas e o caminho de volta pela frente, ela precisa superar obstáculos, evitar ameaças e encontrar rotas seguras. Não há profecias nem poderes extraordinários, apenas a resistência de um ser minúsculo diante de um mundo imenso. A missão é levar o alimento para casa e garantir que a colônia tenha mais um dia para sobreviver.

### Locais do Mundo e Fases

| Local | Descrição |
|---|---|
| **Formigueiro** | Ponto de partida (spawn) e linha de chegada para a entrega da comida. |
| **Jardim Selvagem** _(fase única)_ | Mistura vegetação densa (cogumelos, grama alta, raízes) com obstáculos que servem de plataforma e esconderijo. O comportamento mecânico muda bastante entre Sol e Chuva. |

### Personagens

- **Formiga Operária (protagonista):** único personagem controlável e único aliado do jogo. Tem agilidade, mas é vulnerável ao ambiente.

### Coletáveis

- **1x Comida (item de missão):** o objetivo central de cada fase.

---

## 6. Roadmap

| Semana | Entregas |
|---|---|
| **1** | Configuração inicial do projeto (git e Unity) · Importação de assets · Mecânica de Andar · Mecânica de Correr · Sistema de vida · Mecânica de morte (volta ao início da fase) |
| **2** | Mecânica de Escalar · Câmera do jogador · Opacidade dos itens · Refinamento da movimentação |
| **3** | Mapa de seleção de fases · Level design do "Jardim Selvagem" · UI de gameplay · _Continuação:_ refinamento da movimentação · _Continuação:_ Escalar |
| **4** | Interfaces de menu (tela principal, pause e game over) · _Continuação:_ mapa de seleção · _Continuação:_ level design do "Jardim Selvagem" · _Continuação:_ refinamento da movimentação |
| **5** | Efeitos sonoros finais · Efeitos especiais (morte e habilidades) · _Continuação:_ level design do "Jardim Selvagem" |
| **6** | Semana extra |
| **7** | Semana extra |
| **8** | Semana extra |

---

## 7. Referências

- **Grounded:** referência de cenário.
- **Super Mario Bros. 3:** referência para a tela de seleção de fases.
- **Cenário isométrico livre:** _Pure 2D Isometric Jumping in Godot [Tech Demo]_, de YesNinjas.

---

## 8. Outras Possíveis Ideias (fora do escopo atual)

> Ideias registradas no GDD que **não** fazem parte do escopo atual. Algumas aparecem nos pilares e no core loop (Clima, Cavar/Esconder, inimigos), então convém a equipe decidir se entram ou não.

### Clima Dinâmico

O jogo alterna entre climas que mudam as regras e limitam as ações da formiga.

- **Sol:** clima estável. Todas as mecânicas liberadas (Andar, Correr, Escalar, Pegar/Jogar Item, Cavar/Esconder).
- **Chuva:** ambiente hostil. Gotas gigantes caem como projéteis e causam dano. Não é possível escalar nem jogar itens, porque a superfície fica escorregadia.

### Habilidades

- **Pegar/Jogar Item:** um botão pega um recurso do chão. O jogador pode carregá-lo nas costas ou atirá-lo num inimigo.
  - Acertar um inimigo deixa-o nocauteado por pouco tempo. **A comida não pode ser jogada.**
- **Cavar/Esconder:** cavar o chão para não ser visto pelos predadores ou para se proteger do clima. Tem tempo de recarga (cooldown).

### Inimigos

Outros insetos que dificultam o caminho até o objetivo.

**Tipo 1**, com IA simples de três comportamentos:

| Estado | Comportamento |
|---|---|
| **Patrulha** | Ronda zonas específicas do mapa, normalmente onde está a comida ou no caminho principal de volta. É visível ao jogador. |
| **Perseguição** | Ao ver o jogador, persegue-o. O jogador deve usar o terreno, correr ou Cavar/Esconder para quebrar a linha de visão. |
| **Ataque** | Ao alcançar a formiga, desfere um golpe que tira 1 ponto de vida. |

---

## 9. Código e Arquitetura

### Visão geral dos componentes

| Arquivo | Onde fica | Responsabilidade resumida |
|---|---|---|
| `HealthComponent` | Prefab `Player` | Vida, dano, cura e eventos de morte |
| `GameSession` | Estático (sem objeto na cena) | Dados que sobrevivem à troca de cena (última fase) |
| `IsometricPlayerController` | Prefab `Player` (2D) | Movimento 2D convencional ou isométrico, mais o Animator |
| `CameraController` | Câmera | Seguir o alvo suavemente |
| `HUDController` | Prefab `HUD_Canvas` | Exibir vida e objetivo |
| `LevelController` | Prefab `LevelSystems` | Registrar a fase atual e voltar ao mapa ao morrer |
| `LevelData` | Asset em `LevelData/` | Dados de cada fase (nome, número, cena) |
| `MapEdge`, `MapDirection`, `MapNode` | Cena `Map` | Grafo do mapa de seleção |
| `LevelTooltipController` | Cena `Map` (Canvas World Space) | Tooltip da fase selecionada |
| `LevelSelectionController` | Cena `Map` | Coordenar o mapa e carregar a fase |
| `AntController3d` (4 arquivos partial) | Formiga 3D | Movimento 3D, detecção de superfícies, escalada e transições |

---

### `HealthComponent.cs`

**Responsabilidade:** armazenar os pontos de vida da entidade e notificar interessados quando a vida muda, quando há dano ou quando a entidade morre. Não cuida de invulnerabilidade, animação, movimento, UI nem troca de cena.

**Onde usar:** genérico; serve para jogador, inimigos ou qualquer objeto destrutível. No jogador, fica sempre dentro do prefab `Player`.

**Inspector**
- `maxHp` (int): vida máxima. Mínimo 1, padrão 2.

**Propriedades**
- `Current` (int): vida atual.
- `Max` (int): vida máxima.
- `IsDead` (bool): `true` quando a vida atual é 0.

**Eventos**
- `OnDamageTaken` (`Action<int>`): a cada dano válido. Passa a quantidade original de dano, antes do limite em 0.
- `OnHealthChanged` (`Action<int, int>`): sempre que a vida muda (dano ou cura). Passa vida atual e máxima.
- `OnDeath` (`Action`): uma única vez, quando a vida chega a 0.

> Para ouvir esses eventos, assine em `OnEnable` com `+=` e cancele em `OnDisable` com `-=`. O C# não desconecta sozinho.

**Métodos públicos**
- `TakeDamage(int damage)`: se `damage > 0` e a entidade estiver viva, reduz a vida (limitada em 0) e dispara `OnDamageTaken` e `OnHealthChanged`. Se chegar a 0, dispara `OnDeath`.
- `Heal(int heal)`: se `heal > 0`, a entidade estiver viva e a vida não estiver cheia, aumenta a vida (limitada em `maxHp`) e dispara `OnHealthChanged`.

---

### `GameSession.cs`

**Responsabilidade:** guardar dados da sessão que precisam sobreviver à troca de cenas. Hoje guarda só a última fase jogada. **Não é sistema de save:** nada é gravado em disco, e por ser `static` não precisa estar na cena.

**Propriedades**
- `LastLevel` (int; get público, set privado): número da última fase jogada. Padrão `-1` ("nenhuma fase jogada ainda").

**Métodos públicos**
- `SetLastLevel(int newLevel)`: atualiza `LastLevel` se o valor for válido.

---

### `IsometricPlayerController.cs`

**Responsabilidade:** controlar o movimento convencional ou isométrico do jogador e atualizar a direção do Animator.

**Onde usar:** no GameObject do jogador, com `Rigidbody2D`, `PlayerController` e um Animator com os parâmetros `X` e `Y`. Fica sempre dentro do prefab `Player`.

**Inspector**
- `moveSpeed` (float): velocidade. Padrão 5.
- `isoMovement` (bool): liga ou desliga o movimento isométrico.
- `isoRight`, `isoUp` (Vector2): direções do movimento isométrico.
- `playerSpriteAnimator` (Animator): Animator do jogador.

**Métodos internos**
- `Awake()`: obtém o `Rigidbody2D` e instancia o `PlayerController`.
- `OnEnable()` / `OnDisable()`: liga e desliga os controles.
- `FixedUpdate()`: processa o movimento e atualiza a direção do sprite.
- `UpdateSpriteDirection(Vector2 direction)`: converte a direção em -1, 0 ou 1 e envia ao Animator.

**Observações**
- O `PlayerController` precisa ter a ação `Player.Move`.
- O `Rigidbody2D` é obtido automaticamente.
- A normalização evita que a diagonal seja mais rápida.
- Os callbacks de entrada não são removidos explicitamente em `OnDisable()`, o que pode exigir ajuste para evitar registros duplicados.

---

### `CameraController.cs`

**Responsabilidade:** fazer a câmera seguir um alvo suavemente, mantendo um deslocamento configurável.

**Onde usar:** no GameObject da câmera, com o jogador atribuído em `target`.

**Inspector**
- `target` (Transform): objeto seguido.
- `smoothSpeed` (float): fator de suavização. Padrão 0,125.
- `offset` (Vector3): deslocamento em relação ao alvo.

**Métodos internos**
- `LateUpdate()`: calcula a posição desejada (alvo + deslocamento) e suaviza com `Vector3.Lerp`. Se `target` for `null`, a câmera fica parada.

**Observações**
- `smoothSpeed` é a intensidade da interpolação, não uma velocidade em unidades por segundo. Use valores entre 0 e 1.
- A suavização não usa `Time.deltaTime`, então pode variar com a taxa de quadros.

---

### `HUDController.cs`

> No Notion aparece como `HUDManager.cs`; no repositório o arquivo é `HUDController.cs`.

**Responsabilidade:** mostrar as informações da partida (vida do jogador e objetivo atual), atualizando os textos quando os eventos disparam. No futuro pode ser dividido em um script por responsabilidade, para não virar um "god object".

**Onde usar:** uma instância por cena de nível, dentro do prefab `HUD_Canvas`, junto com as labels (TextMeshPro).

**Inspector**
- `playerHealth` (HealthComponent): vida do jogador.
- `healthLabel` (TMP_Text): exibe a vida no formato `Health: atual/máxima`.
- `objectiveLabel` (TMP_Text): exibe o objetivo no formato `Objetivo: XYZ`.

---

### `LevelController.cs`

**Responsabilidade:** coordenar a fase em que está inserido: registrar qual fase está sendo jogada e, quando o jogador morre, voltar ao mapa de seleção. Não controla vida, movimento, animação nem UI.

**Onde usar:** uma instância por cena de nível, nunca em menu ou no mapa. Fica sempre dentro do prefab `LevelSystems`.

**Inspector**
- `levelData` (LevelData): dados da fase, de onde vem o número registrado em `GameSession`. Deve ser o asset da própria fase.
  > Não há validação em código de que o `LevelData` pertence à cena atual. Confirme ao montar a cena.
- `playerHealth` (HealthComponent): vida do jogador.
- `mapSceneName` (string): nome da cena do mapa (`"Map"`).
  > A cena precisa estar na lista de cenas do Build Profiles.

---

### `LevelData.cs`

**Responsabilidade:** guardar os dados de uma fase (nome, número e cena) como asset, servindo de fonte única dessas informações. Não tem lógica de jogo e não muda em tempo de execução.

**Onde usar:** um asset por fase (menu *Create > Scriptable Objects > LevelData*), na pasta `LevelData`. É referenciado pelo `MapNode` (mapa) e pelo `LevelController` (fase).

**Inspector**
- `levelName` (string): nome da fase mostrado ao jogador.
- `levelNumber` (int): número da fase. Mínimo 0; `-1` significa "não configurado". **Não repita o número entre fases.**
- `sceneName` (string): nome da cena da fase. A cena precisa estar no Build Profiles.

**Propriedades**
- `LevelName`, `LevelNumber`, `SceneName`: leitura dos campos acima.
- `IsValid` (bool): `true` quando nome e cena estão preenchidos e o número é ≥ 0. Use para checar os dados antes de usá-los e evitar erros.

**Métodos internos**
- `OnValidate()`: avisa no Editor (`LogWarning`) quando algum campo está vazio.

---

### `MapEdge.cs`

**Responsabilidade:** representar uma aresta do grafo do mapa: uma direção e o nó de destino. Não valida nem move nada.

**Onde usar:** só dentro do array `mapEdges` do `MapNode`. É uma struct serializável, então aparece no Inspector.

**Inspector**
- `direction` (MapDirection): direção da ligação a partir do nó dono.
- `target` (MapNode): nó de destino. Vazio significa que não há caminho naquela direção.

**Propriedades:** `Direction` e `Target`, para leitura dos campos.

**Métodos públicos**
- `MapEdge(MapDirection direction, MapNode target)`: construtor, usado no preenchimento padrão do array do `MapNode`.

---

### `MapDirection.cs`

**Responsabilidade:** definir as quatro direções de movimento no mapa (enum) e o cálculo da direção oposta.

**Onde usar:** arquivo compartilhado, usado por `MapEdge`, `MapNode` e pelo script de movimento do jogador no mapa.

**Métodos públicos**
- `Opposite(this MapDirection direction)`: método de extensão que devolve a direção oposta (Norte ↔ Sul, Leste ↔ Oeste). Um valor desconhecido lança `ArgumentOutOfRangeException`, para que direções novas mal configuradas não passem em silêncio.

---

### `MapNode.cs`

**Responsabilidade:** representar um ponto do mapa (um nível ou uma junção) no grafo, guardando onde o jogador para, o nível associado (opcional) e as ligações com os vizinhos. Não controla movimento, input, tooltip nem carregamento de cena.

**Onde usar:** um GameObject por ponto do mapa, no mundo (World Space), fora de Canvas. Nós com `LevelData` são níveis (com arte); nós sem `LevelData` são junções (curvas, bifurcações e o formigueiro).

**Inspector**
- `levelData` (LevelData): dados do nível. Vazio significa junção.
- `playerAnchor` (Transform): onde o jogador para. **Obrigatório em todo nó.**
- `tooltipAnchor` (Transform): onde o tooltip aparece. Obrigatório só em nós com nível.
- `mapEdges` (MapEdge[]): ligações do nó. Já vem com as quatro direções e destino vazio.
  > Não repita a mesma direção no mesmo nó e não aponte o nó para si mesmo. Se A liga a B, B precisa ligar de volta a A.

**Propriedades**
- `Data` (LevelData): dados do nível; `null` em junções.
- `TooltipAnchor` (Transform): ponto do tooltip.
- `StopPosition` (Vector3): posição do `playerAnchor`.
- `HasLevel` (bool): `true` quando o nó tem `LevelData`.
- `IsValid` (bool): `true` quando as arestas são válidas, o `playerAnchor` existe e, se houver nível, o `tooltipAnchor` existe e o `LevelData` é válido.

**Métodos públicos**
- `GetNeighbor(MapDirection direction)`: devolve o vizinho naquela direção, ou `null` se não houver aresta.

**Métodos internos**
- `OnValidate()`: avisa no Editor sobre campos obrigatórios vazios.
- `Awake()`: registra `LogError` para configuração ou arestas inválidas.
- `ValidateEdges(bool log)`: detecta aresta para o próprio nó e direções duplicadas. Com `log = false` não escreve no console (usado pelo `IsValid`).
- `OnDrawGizmos()`: desenha o grafo na Scene view (só no Editor).
  > Esfera amarela = nível; ciano = junção. Linha verde = ligação de ida e volta; vermelha = o vizinho não aponta de volta (aresta errada).

---

### `LevelTooltipController.cs`

**Responsabilidade:** mostrar e esconder o tooltip do nível selecionado (número e nome). Não decide qual nível está selecionado, não lê input e não carrega cenas.

**Onde usar:** no objeto raiz do tooltip, dentro de um Canvas em World Space, com o `CanvasGroup` no mesmo objeto. Uma instância por cena de mapa.

> **Importante:** o Canvas em World Space precisa de escala uniforme (ex.: 0,01, que converte unidades de UI para o mundo). Não altere a Scale sem saber o que está fazendo. Ele também precisa de uma Sorting Layer acima dos sprites do mapa.

**Inspector**
- `tooltip` (CanvasGroup): controla a visibilidade (alpha).
- `levelNameLabel` (TMP_Text): número e nome da fase.
- `actionLabel` (TMP_Text): texto de ação.

**Métodos públicos**
- `Show(LevelData levelData)`: escreve `Fase {número}: {nome}` e "Pressione para Entrar" e mostra o tooltip. `levelData` nulo gera `LogError` e não faz nada.
- `Hide()`: esconde o tooltip.

**Métodos internos**
- `OnValidate()`: avisa no Editor sobre campos vazios.
- `Awake()`: valida os campos (`LogError`) e, se tudo estiver certo, começa escondido. Com configuração inválida, `Show` e `Hide` não fazem nada.

---

### `LevelSelectionController.cs`

**Responsabilidade:** coordenar a tela do mapa: escolher o nó inicial, posicionar o jogador, mostrar o tooltip do nível selecionado e carregar a cena do nível ao confirmar. Não gerencia vida, HUD nem a lógica de cada fase.

**Onde usar:** uma instância na cena `Map`.

**Inspector**
- `player`: jogador no mapa.
- `tooltip` (LevelTooltipController): tooltip dos níveis.
- `allNodes` (MapNode[]): todos os nós do mapa.
  > O primeiro elemento é o nó inicial. Todo nó com nível precisa estar na lista; senão, ao voltar da fase, o jogador cai no nó inicial com um aviso.

**Métodos internos**
- `Awake()`: valida as referências e a lista de nós. Se houver erro, registra `LogError` e se desativa.
- `OnEnable()` / `OnDisable()`: assina e cancela os eventos de input.
- `Start()`: lê `GameSession.LastLevel`, procura o nó com esse número e, se não achar, usa o primeiro da lista.
- Ao chegar a um nó com nível, move o tooltip para o `TooltipAnchor` e o exibe; em junções, esconde o tooltip.
- Ao confirmar, se houver nível selecionado e nada estiver carregando, carrega a cena do nível.
- `ValidateNodeArray()`: reprova lista vazia, nó nulo ou mal configurado, cena fora do Build Profiles e número de nível repetido. Junções não têm cena nem número para validar.

---

### `AntController3d` (partial class em 4 arquivos)

> Os quatro arquivos usam `partial class` para dividir um **único componente** em responsabilidades menores. Não são quatro componentes: todos compartilham os mesmos campos, métodos e estado da classe `AntController3d`. Todos precisam continuar no projeto para a classe compilar.

#### `AntController3D.cs` (principal)

**Responsabilidade:** leitura dos comandos, estado de movimento e movimento convencional no chão. Também guarda as referências e variáveis compartilhadas com os outros arquivos. Não implementa detecção de superfícies nem escalada.

**Onde usar:** como componente da formiga jogável. Apenas uma instância por formiga.

**Inspector**
- `walkSpeed` (float): velocidade andando.
- `runSpeed` (float): velocidade correndo.
- `acceleration` (float): taxa de aceleração ao começar a andar ou ganhar velocidade.
- `deceleration` (float): taxa de desaceleração ao reduzir ou parar.
- `cameraTransform` (Transform): câmera usada para calcular a direção relativa à visão do jogador.
- `visual` (Transform): objeto visual da formiga. Permite girar o visual sem girar o objeto principal (que tem Rigidbody e Collider).

**Métodos**
- `Awake()`: inicializa referências (Rigidbody, CapsuleCollider).
- `Update()`: lógica por frame: atualiza comandos e verifica ações do estado atual, como iniciar escalada ou transição para o topo.
- `ReadInput()`: lê o Input System e atualiza a direção de movimento.
- `FixedUpdate()`: movimento físico; escolhe a lógica de chão ou de escalada.

#### `AntController3D.Detection.cs`

**Responsabilidade:** verificações físicas (principalmente Raycasts) para detectar superfícies, manter a referência da parede escalada e identificar limites de movimento. Não move a formiga.

**Inspector**
- `topProbeForwardOffset` (float): recuo do ponto inicial da verificação do topo, na direção oposta à normal da parede.
- `topProbeUpOffset` (float): altura extra acima da formiga para a verificação do topo.
- `topProbeDistance` (float): alcance do raio que procura a superfície superior.
- `lateralProbeStartOffset` (float): deslocamento do início dos raios das bordas laterais.
- `lateralProbeDistance` (float): alcance dos raios que verificam a parede nas laterais.
- `surfaceCheckDistance` (float): alcance da verificação de contato com a superfície escalada.

**Métodos**
- `HasWallAtPosition()`: verifica se a parede continua nas laterais da posição futura, impedindo a formiga de passar das bordas.
- `CheckWallRay()`: confere por raio se a colisão foi com a parede escalada, ignorando a própria formiga e outras superfícies.
- `TryDetectTopSurface()`: procura uma superfície horizontal acima e analisa a normal para decidir se é um topo válido para a transição.

#### `AntController3D.Climbing.cs`

**Responsabilidade:** a escalada: iniciar, calcular os eixos de movimento na parede, mover e verificar o contato. Durante a escalada a gravidade do Rigidbody fica desligada. Não cuida do movimento no chão nem da transição para o topo.

**Inspector**
- `climbSpeed` (float): velocidade escalando.
- `climbDetectionDistance` (float): alcance da detecção de parede à frente para iniciar a escalada.

**Métodos**
- `TryStartClimbing()`: se houver parede válida à frente, entra no estado de escalada, registra a superfície, calcula os eixos, desliga a gravidade e ajusta a orientação visual.
- `Climb()`: combina os comandos horizontais e verticais com os eixos da parede, aplica `climbSpeed`, respeita as bordas e atualiza a orientação visual.
- `UpdateSurfaceContact()`: confere se a formiga continua encostada na parede e atualiza a normal usada como referência.

#### `AntController3D.Transitions.cs`

**Responsabilidade:** transição da parede para uma superfície superior: encerrar a escalada e ajustar física e visual para a formiga continuar andando no novo plano.

**Métodos**
- `TryTransitionToTop()`: se houver topo válido, posiciona a formiga sobre ele, encerra a escalada, religa a gravidade, zera velocidades e entradas e ajusta a orientação visual para o novo plano.

---

## 10. Notas de Alinhamento com o Repositório

Pontos em que o GDD e o estado do código divergem hoje. Servem de lembrete para a equipe, não fazem parte do design.

- **Cena do Jardim Selvagem:** o GDD define o "Jardim Selvagem" como a fase única, mas não existe cena com esse nome. O `LevelData/JardimSelvagem.asset` aponta para `MovementScene` (teste 2D). O cenário 3D com a formiga está em `TesteFormiga`, que não está no Build Settings nem ligado ao mapa.
- **Fases extras no mapa:** o GDD fala em uma fase única, mas o mapa tem 4 nós de fase (Tutorial, Jardim Selvagem, Campo do Assombro, Mina de Carvão), todos apontando para `MovementScene`.
- **Escalada:** o código dos arquivos `AntController3D.Detection/Climbing/Transitions` está na branch `climbing-system`, ainda não mesclada no `main`.
- **Nomes:** o GDD chama o HUD de `HUDManager.cs`; no repositório é `HUDController.cs`.
- **2D × 3D:** o GDD fala em visual 2.5D. Hoje coexistem um controle 2D isométrico (`IsometricPlayerController`) e um 3D (`AntController3d`), e o mapa de seleção é 2D (tilemap).
