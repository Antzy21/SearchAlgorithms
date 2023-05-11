module Tests

open SearchAlgorithms
open System
open Xunit
open FsCheck.Xunit

type node = int list

type move = int

type GameTree =
    | Branches of GameTree list
    | Node of int

let rec getToNode (branchDirections: int list) (game: GameTree) : int =
    match game with
    | Node n -> n
    | Branches b ->
        match branchDirections with
        | bHead :: bTail -> getToNode bTail b.[bHead]
        | [] -> failwith "Out of Branch directions"

let rec generateTestGame (r: Random) (depth: int) (branches: int) : GameTree =
    if depth <= 0 then
        Node (r.Next())
    else
        List.init branches (fun _ -> (generateTestGame r (depth-1) branches)) |> Branches

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

let getNodesFromParentWithBranchCount (branchCount: int) (node: node) : (move * node) list =
    [0..branchCount-1]
    |> List.map (fun i -> 
        i,
        List.append node [i]
    )

let evaluationFunctionForGame (game: GameTree) (move: move option) (node: node) : int =
    getToNode node game

[<Fact>]
let ``Example for minMax`` () =
    let evaluationFunction = evaluationFunctionForGame gameExample1
    let getNodesFromParent = getNodesFromParentWithBranchCount 2
    let move, eval = Algorithms.minMax getNodesFromParent evaluationFunction 3 true None []
    Assert.Equal(3, eval) // Minmax evaluation is 3
    Assert.Equal(Some 0, move) // Best branch for active player is the first.
    
[<Fact>]
let ``Example for ab pruning`` () =
    let evaluationFunction = evaluationFunctionForGame gameExample1
    let getNodesFromParent = getNodesFromParentWithBranchCount 2
    let move, eval = Algorithms.minMaxAbPruning getNodesFromParent evaluationFunction 3 true None []
    Assert.Equal(3, eval) // Minmax evaluation is 3
    Assert.Equal(Some 0, move) // Best branch for active player is the first.

[<Property>]
let ``minMax and ab pruning return same result`` (seed: int) =
    let r = new Random(seed)
    let depth = 4
    let branchCount = 4
    let game = generateTestGame r depth branchCount
    let evaluationFunction = evaluationFunctionForGame game
    let getNodesFromParent = getNodesFromParentWithBranchCount branchCount
    let resultFromMinMax = Algorithms.minMax getNodesFromParent evaluationFunction depth true None []
    let resultFromAbPrune = Algorithms.minMaxAbPruning getNodesFromParent evaluationFunction depth true None []
    Assert.Equal(resultFromMinMax, resultFromAbPrune)

// This avoids having to generate the game tree before hand, which can be expensive,
// but it also maintains unique, random and seeded values at each node,
// by using the branch list to seed a Random object.
let evaluationFunctionForProceduralyGeneratedGame (move: move option) (node: node) : int =
    node
    |> List.fold (fun acc i ->
        acc + $"{i}"
    ) ""
    |> int
    |> (fun i -> new Random(i))
    |> fun r -> r.Next()

[<Fact>]
let ``Ab is fast`` () =
    let branchCount = 7
    let depth = 8
    let getNodesFromParent = getNodesFromParentWithBranchCount branchCount
    
    // normal min max takes ~30 seconds
    let resultFromMinMax = Algorithms.minMaxAbPruning getNodesFromParent evaluationFunctionForProceduralyGeneratedGame depth true None []
    
    Assert.True
    
