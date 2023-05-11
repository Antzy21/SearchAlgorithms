module Tests.Helpers

open System

type node = int list

type move = int

type GameTree =
    | Branches of GameTree list
    | Node of int

let rec private getToNode (branchDirections: int list) (game: GameTree) : int =
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

let rec private getToSubGameTree (branchDirections: int list) (game: GameTree) : GameTree =
    match branchDirections with
    | bHead :: bTail -> 
        match game with
        | Node node -> Node node
        | Branches b -> getToSubGameTree bTail b.[bHead]
    | [] -> game

let getNodesFromParentForGame (game: GameTree) (node: node) : (move * node) list =
    match getToSubGameTree node game with
    | Node node -> []
    | Branches b ->
        [0..b.Length-1]
        |> List.map (fun i -> 
            i,
            List.append node [i]
        )

let evaluationFunctionForGame (game: GameTree) (move: move option) (node: node) : int =
    getToNode node game