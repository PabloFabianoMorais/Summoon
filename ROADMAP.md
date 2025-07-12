# Projeto SunMoon - Roadmap e Tarefas

Este documento centraliza as ideias, funcionalidades planejadas e bugs conhecidos para o jogo.

---

## 1. Visão Geral do Jogo (O Norte)

*   **Gênero:** 2D Top-Down Rogue-like com elementos de Tower Defense e Sobrevivência.
*   **Loop Principal:**
    1.  **Dia:** Explorar, coletar recursos, construir defesas, interagir com NPCs.
    2.  **Noite:** Sobreviver a ondas de monstros que aumentam em dificuldade.
*   **Diferencial:** A combinação de exploração e construção de base procedural em um mundo com ciclo dia/noite e IA avançada.
*   **Pilares do Design:** Exploração, Preparação, Defesa, Progressão.

---

## 2. Roadmap de Funcionalidades (O Que Vem a Seguir)

Funcionalidades maiores, em ordem aproximada de implementação.

- [x] Estrutura básica do projeto (ECS, Managers).
- [x] Geração procedural de mundo (baseada em ruído).
- [x] Geração de chunks paralela e incremental.
- [ ] **Ciclo Dia/Noite:**
    - [X] Sistema de tempo global.
    - [ ] Efeitos visuais (mudança de cor/luz).
    - [X] Transição de estado Dia <-> Noite.
- [ ] **Sistema de inventário**
    - [ ] Áreas de gerenciamento de itens.
    - [ ] No jogador, 2 Espaços de item para segura-los na mão esquerda e direita podendo utiliza-los.
    - [ ] No jogador, 3 Espaços para itens de armadura.
    - [ ] No jogador, 1 Espaço para itens de mochila.
    - [ ] Sistema de peso de itens.
- [ ] **Sistema de Combate:**
    - [ ] Componente de Vida (HealthComponent).
    - [ ] Ataque básico do jogador.
    - [ ] Feedback de dano (visual/sonoro).
- [ ] **IA dos Monstros:**
    - [ ] Spawn de monstros à noite.
    - [ ] Comportamento básico (seguir o jogador).
- [ ] **Sistema de Construção:**
    - [ ] Coleta de recursos (árvores, pedras).
    - [ ] Habilidade de colocar estruturas (torres).
- [ ] **IA das Torres:**
    - [ ] Detecção e ataque a alvos.

---

## 3. Backlog de Tarefas (Tarefas Menores e Melhorias)

Use `[ ]` para tarefas a fazer e `[x]` para as concluídas.

### Prioridade Alta (Fazer Agora)
- [ ] Adicionar Origin Ás sprites
- [X] Implementar o sistema de tempo para o ciclo dia/noite.
- [ ] Criar o `HealthComponent` para o jogador.

### Prioridade Média (Fazer em Breve)
- [ ] Criar uma suaviazação da transição de tiles.
- [ ] Adicionar mais tipos de tiles e biomas ao `WorldGenerator`.
- [ ] Criar o primeiro tipo de monstro com prefab e IA básica.
- [ ] Descarregar chunks distantes para liberar memória.

### Prioridade Baixa (Quando Tiver Tempo)
- [ ] Adicionar efeitos sonoros para passos do jogador.
- [ ] Criar um menu principal para o jogo.
- [ ] Refatorar o `DebugPanel` para ser mais customizável.

---

## 4. Rastreamento de Bugs (Bugs a Consertar)

Seja específico aqui. Descreva como reproduzir o bug.

### Bugs Ativos


### Bugs Corrigidos
- **ID:** 000
  - **Descrição:** Tiles sendo renderizados na diagonal.
  - **Solução:** Corrigido o cálculo da posição Y no `TilemapManager`, que usava `_buildCoordX` em vez de `_buildCoordY`.

---

## 5. Caixa de Ideias (Brainstorming)

Um lugar para jogar ideias sem compromisso.

- E se existissem eventos climáticos (chuva, nevasca) que afetassem o gameplay?
- NPCs poderiam dar quests para o jogador?
- Sistema de crafting para criar armas e armaduras melhores.
- Biomas subterrâneos (cavernas).
- Monstros chefes a cada 7 noites?
- Modo cooperativo online?