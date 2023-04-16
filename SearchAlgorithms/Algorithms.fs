namespace SearchAlgorithms

open System.Numerics

module Algorithms =
    
    /// Basic min/max function
    let rec minMax
        (getNodesFromParent: 'Node -> 'Node list)
        (evaulationFunction: 'Node -> 'EvalValue)
        (depth: int) (isMaxing: bool) (parentNode: 'Node) : 'EvalValue when IMinMaxValue<'EvalValue> =
        if depth = 0 then
            evaulationFunction parentNode
        else
            let nodes = getNodesFromParent parentNode
            match nodes with
            | [] ->
                evaulationFunction parentNode
            | nodes ->
                let minMaxFunc = if isMaxing then max else min
                let initValue = if isMaxing then 'EvalValue.MaxValue else 'EvalValue.MaxValue
                List.fold (fun currentOptimalEval node ->
                    let nodeEval =
                        minMax getNodesFromParent evaulationFunction (depth-1) (not isMaxing) node
                    minMaxFunc currentOptimalEval nodeEval
                ) initValue nodes   
        