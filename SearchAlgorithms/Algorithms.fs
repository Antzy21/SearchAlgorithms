﻿namespace SearchAlgorithms

open System.Numerics

module Algorithms =
    
    /// Basic min/max function
    let rec minMax
        (getNodesFromParent: 'Node -> moveAndNode<'Move, 'Node> list)
        (evaulationFunction: moveAndNode<'Move, 'Node> -> 'EvalValue)
        (depth: int) (isMaxing: bool) (parentNode: moveAndNode<'Move, 'Node>) : moveAndEvaluation<'Move,'EvalValue> =
        if depth = 0 then
            {move = parentNode.move; eval = evaulationFunction parentNode}
        else
            let movesAndNodeList = getNodesFromParent parentNode.node
            match movesAndNodeList with
            | [] ->
                {move = parentNode.move; eval = evaulationFunction parentNode}
            | movesAndNodeList ->

                let minMaxFunc = if isMaxing then MoveAndEvaluation.max else MoveAndEvaluation.min
                let initValue = if isMaxing then 'EvalValue.MaxValue else 'EvalValue.MaxValue
                // initMove will always be replaced as the eval is set to +-infinity.
                let initMove = movesAndNodeList.Head.move
                let moveAndEval = {move = initMove; eval = initValue}

                List.fold (fun currentOptimalMoveAndEval moveAndNode ->
                    let nodeMoveAndEval =
                        minMax getNodesFromParent evaulationFunction (depth-1) (not isMaxing) moveAndNode
                    minMaxFunc currentOptimalMoveAndEval nodeMoveAndEval
                ) moveAndEval movesAndNodeList
        