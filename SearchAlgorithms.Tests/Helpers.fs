module Tests.Helpers

open System

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

let getNodesFromParentWithBranchCount (branchCount: int) (node: node) : (move * node) list =
    [0..branchCount-1]
    |> List.map (fun i -> 
        i,
        List.append node [i]
    )

let evaluationFunctionForGame (game: GameTree) (move: move option) (node: node) : int =
    getToNode node game