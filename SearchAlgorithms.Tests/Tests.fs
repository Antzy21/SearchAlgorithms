module Tests

open SearchAlgorithms
open Xunit

type node = int list

type move = int

let game = 
    [
        [
            [-1;3];
            [5;1]
        ];
        [
            [-6;-4];
            [0;9]
        ]
    ]

// Expected:
//               +3
//            /      \
//          /          \
//       +3              -4
//     /    \          /    \
//   +3      +5      -4      +9
//  /  \    /  \    /  \    /  \
// -1  +3  +5  +1  -6  -4  +0  +9

let getNodesFromParent (node: node) : (move * node) list =
    [0;1]
    |> List.map (fun i -> i, (i :: node))

let evaluationFunction (move: move option) (node: node) : int =
    match node with
    | [i1; i2; i3] ->
        game.[i3].[i2].[i1]
    | _ -> failwith "Only check 3 levels deep"

[<Fact>]
let ``Example for minMax`` () =
    let move, eval = Algorithms.minMax getNodesFromParent evaluationFunction 3 true None []
    Assert.Equal(3, eval) // Minmax evaluation is 3
    Assert.Equal(Some 0, move) // Best branch for active player is the first.
    
[<Fact>]
let ``Example for minMax with ab pruning`` () =
    let move, eval = Algorithms.minMaxAbPruning getNodesFromParent evaluationFunction 3 true None []
    Assert.Equal(3, eval) // Minmax evaluation is 3
    Assert.Equal(Some 0, move) // Best branch for active player is the first.
