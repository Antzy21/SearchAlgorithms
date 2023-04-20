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
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int) (isMaxing: bool) (previousMove: 'Move option) (parentNode: 'Node)
        : 'Move option * 'EvalValue when IMinMaxValue<'EvalValue> =
        if depth = 0 then
            previousMove, evaulationFunction previousMove parentNode
        else
            let movesAndNodeList = getNodesFromParent parentNode
            match movesAndNodeList with
            | [] ->
                previousMove, evaulationFunction previousMove parentNode
            | movesAndNodeList ->

                let initEval = if isMaxing then 'EvalValue.MaxValue else 'EvalValue.MaxValue
                let initMove = None

                List.fold (fun optimalMoveAndEval (move, node) ->
                    let _, eval =
                        minMax getNodesFromParent evaulationFunction (depth-1) (not isMaxing) (Some move) node
                    getMinOrMaxOfMoveAndEval isMaxing optimalMoveAndEval (Some move, eval)                     
                ) (initMove, initEval) movesAndNodeList
        