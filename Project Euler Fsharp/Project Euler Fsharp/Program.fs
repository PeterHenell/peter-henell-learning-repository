
// B-tree
//Time complexity
//in big O notation
//          Average	    Worst case
//Space	    O(n)	    O(n)
//Search	O(log n)	O(log n)
//Insert	O(log n)	O(log n)
//Delete	O(log n)	O(log n)


let a = [1; 2; 3]
let b = [2; 1; 3]
let c = [1; 2; 3]

printfn "List [1; 2; 3] = [2; 1; 3]: %b" (a = b)
printfn "List [1; 2; 3] = [1; 2; 3]: %b" (a = c)
printfn ""

let s1 = Seq.ofList a
let s2 = Seq.ofList b
let s3 = Seq.ofList c

printfn "Seq [1; 2; 3] = [2; 1; 3]: %b" (s1 = s2)
printfn "Seq [1; 2; 3] = [1; 2; 3]: %b" (s1 = s3)
printfn ""

let set1 = Set.ofList a
let set2 = Set.ofList b
let set3 = Set.ofList c

printfn "Set [1; 2; 3] = [2; 1; 3]: %b" (set1 = set2)
printfn "Set [1; 2; 3] = [1; 2; 3]: %b" (set1 = set3)
printfn ""

//type 'a Tree = 
//    | Root
//    | Node of 'a * 'a Tree * 'a Tree
//    | Leaf of 'a
//
//
//let rec sumTree tree =
//    match tree with
//    | Root -> 0
//    | Node(value, left, right) ->
//        sumTree(left) + sumTree(right)
//    | Leaf(value) -> value
//
//
//let myTree = Node(0, 
//                Node(1, 
//                    Node(2, Root, Root),
//                    Node(3, Root, Root)
//                ), 
//                Node(4, Root, Root))
//let resultSumTree = sumTree myTree
//
//printfn "%O" resultSumTree

//
//type Node = { Key : int ; Pointer : Node ; }
//
//type Leaf = { data : Node ;}


// Internal node will have keys with ranges counted up from nm where n is the number of internal node and m is the size of the internal node
//type InternalNode =  { Nodes :  Node; }








//
//let f(x) = 
//    printf "%A" x
//
//let rec skrivUt x = 
//    match x with
//    | [] -> () 
//    | _ -> 
//        f(x.Head)
//        skrivUt(x.Tail)
//    
//f 4
//
//