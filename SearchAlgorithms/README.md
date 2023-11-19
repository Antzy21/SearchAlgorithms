# SearchAlgorithms

Generic implimentations of search algorithms to be used in other projects

## Documentation

```
minMax
        (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int)
        (isMaxing: bool)
        (previousMove: 'Move option)
        (parentNode: 'Node)
        : 'Move option * 'EvalValue =
```
        
```
minMaxAbPruning
        (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int)
        (isMaxing: bool)
        (previousMove: 'Move option)
        (parentNode: 'Node)
        : 'Move option * 'EvalValue =
```
        
Both minMax and minMaxAbPruning take the same parameters.<br/>
They will also both return the same result for given inputs.<br/>
Alpha Beta pruning is likely to provide significant performance improvements.
        
Parameters:
- getNodesFromParent<br/>
  This function takes in a node and returns a list of moves and nodes.<br/>
  Nodes might be GameStates and moves are how the new gameState arrose from the previous gameState.<br/>
  Multiple moves might be playable from a given gamestate, hence this function returns a list of moves and corresponding new gameStates.
- evaluationFunction<br/>
  This function takes a move and node, and returns an evaluation.<br/>
  This will be a heurstical evaluation of a static gameState, but can also take into account the move being made, to enrich the heristic calculation.<br/>
  The move parameter must be optional, because there may not be a previous move, for example the start of a game.
- depth<br/>
  How deep to perform the search
- isMaxing<br/>
  Should the algorithm try to maximise the evaluation from the start?<br/>
  This is relevant to the evaluation function between two players.<br/>
  If it is player1's turn and the evaluation function is set to be larger if player1 is winning, then isMaxing should be true.
- previousMove<br/>
  This is required in the event that the node has no children, and so is evaluated immediately.<br/>
  The evaluation takes in an option move, which is provided here.<br/>
  It is possible to default pass None here, but this may not evaluate the node correctly.
- parentNode<br/>
  This is the current node, or gameState that is getting evaluated.
