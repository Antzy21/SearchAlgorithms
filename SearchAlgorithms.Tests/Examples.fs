module Tests.Examples

open SearchAlgorithms
open Xunit
open Helpers

let gameExample1 = 
    Branches [
        Branches [
            Branches [
                Node -1;
                Node 3
            ];
            Branches [
                Node 5;
                Node 1
            ]
        ];
        Branches [
            Branches [
                Node -6;
                Node -4
            ];
            Branches [
                Node 0;
                Node 9
            ]
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

[<Fact>]
let ``Example1 for minMax`` () =
    let evaluationFunction = evaluationFunctionForGame gameExample1
    let getNodesFromParent = getNodesFromParentWithBranchCount 2
    let move, eval = Algorithms.minMax getNodesFromParent evaluationFunction 3 true None []
    Assert.Equal(3, eval) // Minmax evaluation is 3
    Assert.Equal(Some 0, move) // Best branch for active player is the first.
    
[<Fact>]
let ``Example1 for ab pruning`` () =
    let evaluationFunction = evaluationFunctionForGame gameExample1
    let getNodesFromParent = getNodesFromParentWithBranchCount 2
    let move, eval = Algorithms.minMaxAbPruning getNodesFromParent evaluationFunction 3 true None []
    Assert.Equal(3, eval) // Minmax evaluation is 3
    Assert.Equal(Some 0, move) // Best branch for active player is the first.
