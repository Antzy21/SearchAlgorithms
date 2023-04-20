namespace SearchAlgorithms

open System.Numerics

type moveAndEvaluation<'Move, 'EvalValue> when IMinMaxValue<'EvalValue> = {move : 'Move option; eval: 'EvalValue}

module MoveAndEvaluation =

    /// Compares the moveAndEvaluation pairs by evaluation, and returns the maximum
    let max (mae1: moveAndEvaluation<'Move, 'EvalValue>) (mae2: moveAndEvaluation<'Move, 'EvalValue>) : moveAndEvaluation<'Move, 'EvalValue> =
        if mae1.eval > mae2.eval then
            mae1
        else
            mae2
        
    /// Compares the moveAndEvaluation pairs by evaluation, and returns the minimum
    let min (mae1: moveAndEvaluation<'Move, 'EvalValue>) (mae2: moveAndEvaluation<'Move, 'EvalValue>) : moveAndEvaluation<'Move, 'EvalValue> =
        if mae1.eval < mae2.eval then
            mae1
        else
            mae2

