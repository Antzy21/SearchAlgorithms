module Tests.Performance

open SearchAlgorithms
open System
open Xunit
open FsCheck.Xunit
open Helpers

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
