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

                let initEval = if isMaxing then 'EvalValue.MinValue else 'EvalValue.MaxValue
                let initMove = None

                List.fold (fun optimalMoveAndEval (move, node) ->
                    let _, eval =
                        minMax getNodesFromParent evaulationFunction (depth-1) (not isMaxing) (Some move) node
                    getMinOrMaxOfMoveAndEval isMaxing optimalMoveAndEval (Some move, eval)                     
                ) (initMove, initEval) movesAndNodeList
    
    let rec private alphaBetaPruning 
        (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int) (isMaxing: bool) (previousMove: 'Move option) (parentNode: 'Node)
        (alpha: 'EvalValue) (beta: 'EvalValue)
        : 'Move option * 'EvalValue * 'EvalValue * 'EvalValue when IMinMaxValue<'EvalValue> =
        if depth = 0 then
            previousMove, evaulationFunction previousMove parentNode, alpha, beta
        else
            let movesAndNodeList = getNodesFromParent parentNode
            match movesAndNodeList with
            | [] ->
                previousMove, evaulationFunction previousMove parentNode, alpha, beta
            | movesAndNodeList ->

                let initEval = if isMaxing then 'EvalValue.MinValue else 'EvalValue.MaxValue
                let initMove = None

                List.fold (fun (optimalMove, optimalEval, skip, alpha, beta) (move, node) ->
                    if skip then
                        (optimalMove, optimalEval, skip, alpha, beta)
                    else
                        let _, eval, alpha, beta =
                            alphaBetaPruning getNodesFromParent evaulationFunction (depth-1) (not isMaxing) (Some move) node alpha beta
                        let (newOptimalEval, newOptimalMove) = getMinOrMaxOfMoveAndEval isMaxing (optimalMove, optimalEval) (Some move, eval)                     
                        
                        if isMaxing then
                            let newAlpha = max alpha eval
                            let skip = beta <= alpha
                            (newOptimalEval, newOptimalMove, skip, newAlpha, beta)
                        else
                            let newBeta = min beta eval
                            let skip = beta <= alpha
                            (newOptimalEval, newOptimalMove, skip, alpha, newBeta)
                        
                ) (initMove, initEval, false, alpha, beta) movesAndNodeList
                |> fun (optimalMove, optimalEval, skip, alpha, beta) -> (optimalMove, optimalEval, alpha, beta)

    let minMaxAbPruning
        (getNodesFromParent: 'Node -> ('Move * 'Node) list)
        (evaulationFunction: 'Move option -> 'Node -> 'EvalValue)
        (depth: int) (isMaxing: bool) (previousMove: 'Move option) (parentNode: 'Node)
        : 'Move option * 'EvalValue when IMinMaxValue<'EvalValue> =
            alphaBetaPruning getNodesFromParent evaulationFunction depth isMaxing previousMove parentNode 'EvalValue.MinValue 'EvalValue.MaxValue
            |> fun (optimalMove, optimalEval, alpha, beta) -> (optimalMove, optimalEval)
