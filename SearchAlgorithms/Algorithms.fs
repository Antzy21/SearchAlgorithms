namespace SearchAlgorithms

open System.Numerics

module Algorithms =
    
    let private getMinOrMaxOfMoveAndEval (isMaxing: bool) ((move1, eval1): 'Move * 'Eval) ((move2, eval2): 'Move * 'Eval) : 'Move * 'Eval =
        if isMaxing then
            if eval1 > eval2 then
                (move1, eval1)
            else
                (move2, eval2)
        else
            if eval1 < eval2 then
                (move1, eval1)
            else
                (move2, eval2)

    /// Basic min/max function
    let rec minMax
        (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move -> 'Node -> 'EvalValue)
        (depth: int) (isMaxing: bool) (previousMove: 'Move) (parentNode: 'Node)
        : 'Move * 'EvalValue when IMinMaxValue<'EvalValue> =
        if depth = 0 then
            previousMove, evaulationFunction previousMove parentNode
        else
            let movesAndNodeList = getNodesFromParent parentNode
            match movesAndNodeList with
            | [] ->
                previousMove, evaulationFunction previousMove parentNode
            | movesAndNodeList ->

                let initEval = if isMaxing then 'EvalValue.MaxValue else 'EvalValue.MaxValue
                let initMove = fst movesAndNodeList.Head

                List.fold (fun optimalMoveAndEval (move, node) ->
                    let _, eval =
                        minMax getNodesFromParent evaulationFunction (depth-1) (not isMaxing) move node
                    getMinOrMaxOfMoveAndEval isMaxing optimalMoveAndEval (move, eval)                     
                ) (initMove, initEval) movesAndNodeList
        