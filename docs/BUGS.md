## Rastreamento de Bugs (Bugs a Consertar)

Seja específico aqui. Descreva como reproduzir o bug.

### Bugs Ativos


### Bugs Corrigidos
- **ID:** 000
  - **Descrição:** Tiles sendo renderizados na diagonal.
  - **Solução:** Corrigido o cálculo da posição Y no `TilemapManager`, que usava `_buildCoordX` em vez de `_buildCoordY`.
