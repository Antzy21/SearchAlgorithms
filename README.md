# SearchAlgorithms

Generic implimentations of search algorithms to be used in other projects

## Documentation

`minMax (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int)
        (isMaxing: bool)
        (previousMove: 'Move option)
        (parentNode: 'Node)
        : 'Move option * 'EvalValue when IMinMaxValue<'EvalValue> =`
        
`let minMaxAbPruning
        (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int)
        (isMaxing: bool)
        (previousMove: 'Move option)
        (parentNode: 'Node)
        : 'Move option * 'EvalValue when IMinMaxValue<'EvalValue> =`
        
Both minMax and minMaxAbPruning take the same parameters.
They will also both return the same result for given inputs.
Alpha Beta pruning is likely to provide significant performance improvements.
        
Parameters:
- getNodesFromParent
  This function takes in a node and returns a list of moves and nodes.
  Nodes might be GameStates and moves are how the new gameState arrose from the previous gameState.
  Multiple moves might be playable from a given gamestate, hence this function returns a list of moves and corresponding new gameStates.
- evaluationFunction
  This function takes a move and node, and returns an evaluation.
  This will be a heurstical evaluation of a static gameState, but can also take into account the move being made, to enrich the heristic calculation.
  The move parameter must be optional, because there may not be a previous move, for example the start of a game.
- depth
  How deep to perform the search
- isMaxing
  Should the algorithm try to maximise the evaluation from the start?
  This is relevant to the evaluation function between two players.
  If it is player1's turn and the evaluation function is set to be larger if player1 is winning, then isMaxing should be true.
- previousMove
  This is required in the event that the node has no children, and so is evaluated immediately.
  The evaluation takes in an option move, which is provided here.
  It is possible to default pass None here, but this may not evaluate the node correctly.
- parentNode
  This is the current node, or gameState that is getting evaluated.
