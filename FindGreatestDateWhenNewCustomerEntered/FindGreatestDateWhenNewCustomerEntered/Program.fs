
let GetNum x =
    try
        let res = 100 / x
        res
    with
    | :? System.DivideByZeroException -> -1
    | :? System.OverflowException -> -2

let r = GetNum 0

//let bids = [(1,1); 
//            (1,2); 
//            (1,3);
//            (2,1); 
//            (2,2); 
//            (2,3);
//            (3,1); 
//            (3,2); 
//            (3,66);
//            (4,1); 
//            (4,2);
//            (4,5);
//            (4,5)]
//
//
//// from the reverse, find the first day when a customer appear that are not below in the list
//let GetMaxDateOfNewCustomer (theBids : List<int * int>) =
//    let rec custFind (custlist : List<int * int>)  =
//        let rest = custlist.Tail |> List.filter(fun x -> (snd x).Equals(snd custlist.Head)) 
//        match rest with
//        | [] -> custlist.Head
//        | head :: tail -> custFind custlist.Tail 
//    
//    // remove the duplicates then sort it as a list
//    let reverse = theBids |> Set.ofList |> Set.toList |> List.sortBy(fun x -> -(fst x))  
//    custFind reverse
//
//
//let res = GetMaxDateOfNewCustomer bids
//System.Console.WriteLine(res)


//    System.Console.WriteLine(reverse)
//    (cust, found)
    
//given a customer, what is the date when it appears the the first time in the list of unique days 

// find the first day when a new customer appears that is not day one
// find the first day when - a customer that have not appeared before - appeares, that is not day one
// maintain a list of customers, 


//
//let GetMaxDateOfNewCustomer(theBids) = 
//    let getUniqueDates tuples = 
//        Set.map (fun x -> fst x) (Set.ofList tuples)
//    let dates = getUniqueDates theBids
//    0     


//Set.iter (fun x -> printfn "Set.iter: element is %d" x) (getUniqueDates bids)

//let res = GetMaxDateOfNewCustomer bids



//let nub xs = xs |> Set.ofList |> Set.toList
//
//
//
//
//let rec getDates(dates)  =
//    match dates with
//    | head :: tail -> fst dates.Head :: getDates dates.Tail
//    | [] -> []
        

//let dates = getDates bids

//System.Console.WriteLine();
//List.iter (fun x -> printfn "List.iter: element is %d" x) (nub dates)
//System.Console.WriteLine(dates);
//List.iter (fun x -> printfn "List.iter: element is %d" x) dates
//
//
//System.Console.WriteLine(fst bids.Head);
//System.Console.WriteLine(snd bids.Head);
//System.Console.WriteLine(bids.Tail);
//System.Console.WriteLine(bids.Length);
//System.Console.WriteLine(res);