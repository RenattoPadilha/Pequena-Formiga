# 🐜 Pequena Formiga

> O que para nós é só um jardim tranquilo, para ela é um campo de batalha.

**Pequena Formiga** é um jogo de plataforma e aventura em 2.5D, para um jogador. Você controla uma formiga operária que sai da segurança do formigueiro para buscar comida e garantir a sobrevivência da colônia.

Folhas, gravetos, poças d'água e tampas de garrafa viram obstáculos gigantes; aranhas, besouros e pássaros viram predadores colossais. Sem magia nem superpoderes, a formiga conta apenas com agilidade e instinto para chegar à comida e voltar com ela para casa.

## Como se joga

1. **Explore** a fase em busca da comida.
2. **Sobreviva**: corra, escale e fuja dos perigos. A formiga aguenta só **2 golpes**.
3. **Volte** ao formigueiro carregando a comida.
4. **Entregue** a comida para desbloquear a próxima fase no mapa.

Se a formiga morrer, ela perde a comida e recomeça a fase. As vidas são infinitas, e o reinício é rápido para incentivar novas tentativas.

## Pilares

- **Desafio justo e ágil:** exige reflexos, mas nunca parece injusto.
- **A perspectiva do minúsculo:** tudo reforça que você é pequeno num mundo gigante.
- **Adaptação constante:** o ambiente muda e obriga o jogador a mudar a forma de jogar.
- **Foco no objetivo:** pegar a comida e levá-la para casa.

## Rodando o projeto

**Requisitos**
- [Unity **6000.4.2f1**](https://unity.com/releases/editor/archive) (Unity 6), com Universal Render Pipeline
- [Blender](https://www.blender.org/download/) instalado. O Unity precisa dele para importar o modelo da formiga (`Ant.blend`).

**Passos**
1. Clone o repositório e adicione a pasta no Unity Hub.
2. Abra o projeto. A primeira importação leva alguns minutos.
3. Abra a cena `Assets/_PequenaFormiga/Scenes/Map` e aperte **Play**.

**Controles do mapa de seleção**

| Ação | Teclado |
|---|---|
| Mover entre as fases | WASD ou setas |
| Entrar na fase | Espaço ou E |

**Controles da formiga 3D** (cena `TesteFormiga`)

| Ação | Teclado |
|---|---|
| Andar | WASD |
| Correr | Shift |

## Estrutura do projeto

```
Assets/_PequenaFormiga/
├── Art/          Modelos, sprites, tilesets (inclui os pacotes KayKit)
├── Editor/       Ferramentas de editor (ex.: gerador de prefabs KayKit)
├── Input/        Mapeamento de controles (Input System)
├── LevelData/    Dados de cada fase (nome, número e cena)
├── Materials/    Materiais URP compartilhados
├── Prefabs/      Player, HUD, sistemas de fase, mapa e cenário
├── Scenes/       Map (seleção de fases), fases e cenas de teste
└── Scripts/      Código do jogo (vida, movimento, fase, mapa)
```

## Documentação

O design completo do jogo (mecânicas, audiovisual, roadmap e arquitetura do código) está no **[GDD](GDD.md)**.

## Equipe

- Anselmo Junior
- Murilo Ventura
- Pedro Stelzer
- Renatto Padilha
- Sara

## Créditos

- Modelos 3D: [KayKit](https://kaylousberg.itch.io/) (Forest Nature Pack e Dungeon Pack), por Kay Lousberg, licença CC0.
- Projeto desenvolvido para a disciplina de Jogos Digitais, UFPE.
