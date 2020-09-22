# F# Introduction of Types

## Which Types Below To Use?

- Tuples
  - returning multiple values from a function
  - using an ad-hoc aggregate of values as a value itself
- Records
  - as a "step up" from Tuples, having named labels and support for optional members
  - for a low-ceremony representation of data in-transit
  - with structural equality they are easy to use with comparison
- Discriminated Unions
  - many uses, core benefit is using with Pattern Matching for all possible data "shapes"
- Classes
  - for representing information and also tie that information to functionality
  - preferred data type when interoperating with C# and Visual Basic

## Definitions

- use `let` to define immutable names
- use `mutable` to define mutables
  - use `<-` to assign new value
```fsharp
let val = 3
let mutable otherNumber = 2
otherNumber <- otherNumber + 1
```

## Primitives, Tuples

- convert with e.g. `int 4.1`
- printf `%A` is generic printing as opposed to e.g. `%d` for integer
```fsharp
let boolean1 = true
let sampleInteger = 176
let sampleDouble = 4.1
let sampleInteger2 = (sampleInteger/4 + 5 - 7) * 4 + int sampleDouble
let sampleNumbers = [ 0 .. 99 ]
let sampleTableOfSquares = [ for i in 0 .. 99 -> (i, i*i) ]
printfn "The table of squares from 0 to 99 is:\n%A" sampleTableOfSquares

let string1 = "Hello"
let string2  = "world"
let string3 = @"C:\Program Files\"
let string4 = """The computer said "hello world" when I told it to!"""
let helloWorld = string1 + " " + string2 
printfn "%s" helloWorld
let substringFirstSeven = helloWorld.[0..6]

let tuple1 = (1, 2, 3)
let swapElems (a, b) = (b, a)
printfn "The result of swapping (1, 2) is %A" (swapElems (1,2))
let tuple2 = (1, "fred", 3.1415)
```

## Conditionals

- if/then/elif/else
```fsharp
let sampleFunction3 x = 
    if x < 100.0 then 
        2.0*x*x - x/5.0 + 3.0
    else 
        2.0*x*x + x/5.0 - 37.0

let result3 = sampleFunction3 (6.5 + 4.5)
```

## Functions

- use `(x:int)` parenthesis when explicit type is needed
```fsharp
let sampleFunction1 x = x*x + 3
let result1 = sampleFunction1 4573
printfn "The result of squaring the integer 4573 and adding 3 is %d" result1
let sampleFunction2 (x:int) = 2*x*x - x/5 + 3
let result2 = sampleFunction2 (7 + 4)
```

### Compositions

- use `|>` for piping
```fsharp
let square x = x * x
let addOne x = x + 1
let isOdd x = x % 2 <> 0
let numbers = [ 1; 2; 3; 4; 5 ]

/// imperative, not very fp
let squareOddValuesAndAddOne values = 
    let odds = List.filter isOdd values
    let squares = List.map square odds
    let result = List.map addOne squares
    result

/// nested, order is hard to understand
let squareOddValuesAndAddOneNested values = 
    List.map addOne (List.map square (List.filter isOdd values))

/// declarative
let squareOddValuesAndAddOnePipeline values =
    values
    |> List.filter isOdd
    |> List.map square
    |> List.map addOne

/// with lambda
let squareOddValuesAndAddOneShorterPipeline values =
    values
    |> List.filter isOdd
    |> List.map(fun x -> x |> square |> addOne)
```

### Recursivity

- use `let rec` to define a recursive function
- _(i) recursive fp is preferred in F#_
- _(i) if all recursive calls are tail calls compiler will make it a loop_
  - _(i) "tail recursive" means the last thing executed is the recursive function_
```fsharp
let rec factorial n = 
    if n = 0 then 1 else n * factorial (n-1)
printfn "Factorial of 6 is: %d" (factorial 6)

/// Computes the greatest common factor of two integers.
///
/// Since all of the recursive calls are tail calls,
/// the compiler will turn the function into a loop,
/// which improves performance and reduces memory consumption.
let rec greatestCommonFactor a b =
    if a = 0 then b
    elif a < b then greatestCommonFactor a (b - a)
    else greatestCommonFactor (a - b) b
printfn "The GCFactor of 300 and 620 is %d" (greatestCommonFactor 300 620)

/// sum of a list of integers using recursion
let rec sumList xs =
    match xs with
    | [] -> 0
    | first::rest -> first + sumList rest
printfn "Sum of [1..3] is %d" (sumList [1..3])

/// ...now make it tail recursive using a helper with a result accumulator
let rec private sumListTailRecHelper accumulator xs =
    match xs with
    | [] -> accumulator
    | first::rest -> sumListTailRecHelper (accumulator + first) rest
/// ...seed the tail recursive function with '0'
let sumListTailRecursive xs = sumListTailRecHelper 0 xs
printfn "Sum of [1..3] is %d" (sumListTailRecursive [1..3])
```

## Lists

- use `[ ]`
```fsharp
let list1 = [ ]
let list2 = [ 1; 2; 3 ]
let list3 = [
    1
    2
    3
]
let numberList = [ 1 .. 1000 ]

/// compute
let daysOfTheYearList = 
    [ for month in 1 .. 12 do
            for day in 1 .. System.DateTime.DaysInMonth(2017, month) do 
                yield System.DateTime(2017, month, day) ]
printfn "The first 5 days of 2017 are: %A" (daysOfTheYearList |> List.take 5)

/// compute with conditionals
let blackSquares = 
    [ for i in 0 .. 7 do
            for j in 0 .. 7 do 
                if (i+j) % 2 = 1 then 
                    yield (i, j) ]
let squares = 
    numberList 
    |> List.map (fun x -> x*x) 

let sumOfSquares = 
    numberList
    |> List.filter (fun x -> x % 3 = 0)
    |> List.sumBy (fun x -> x * x)
printfn "Sum of squares of 1..1000 divisible by 3: %d" sumOfSquares
```

## Arrays

- use `[| |]`
- arrays are fixed size but mutable
- _(i) arrays are for same type elements and are faster than lists_
```fsharp
let array1 = [| |]
let array2 = [| "hello"; "world"; "and"; "hello"; "world"; "again" |]
let array3 = [| 1 .. 1000 |]

let arrayWhereSmallL = 
    [| for word in array2 do
        if word.Contains("l") then 
            yield word |]

/// initialized by index and containing even numbers from 0 to 2000
let evenNumbers = Array.init 1001 (fun n -> n * 2) 
let evenNumbersSlice = evenNumbers.[0..500]

for word in arrayWhereSmallL do 
    printfn "word: %s" word

/// mutate by assigning
array2.[1] <- "WORLD!"

let sumOfLengthsOfWords = 
    array2
    |> Array.filter (fun x -> x.StartsWith "h")
    |> Array.sumBy (fun x -> x.Length)
printfn "The sum of the lengths of the words in Array 2 is: %d" sumOfLengthsOfWords
```

## Sequences

- use `seq { }`
- sequences are somewhat like lists but can be lazy
```fsharp
let seq1 = Seq.empty
let seq2 = seq { yield "hello"; yield "world"; yield "and"; yield "hello"; yield "world"; yield "again" }
let numbersSeq = seq { 1 .. 1000 }

let seq3 = 
    seq { for word in seq2 do
            if word.Contains("l") then 
                yield word }

let evenNumbers = Seq.init 1001 (fun n -> n * 2) 

let rnd = System.Random()

/// infinite sequence which is a random walk.
/// This example uses yield! to return each element of a subsequence.
let rec randomWalk x =
    seq { yield x
          yield! randomWalk (x + rnd.NextDouble() - 0.5) }

/// This example shows the first 10 elements of the random walk.
let first10ValuesOfRandomWalk = 
    randomWalk 5.0 
    |> Seq.truncate 10
    |> Seq.toList
printfn "First 10 elements of a random walk: %A" first10ValuesOfRandomWalk
```

## Records

- records are used for data aggregation
- use `type <name> { ... }` to define
- use `member` for OOP members on instance (`static` addition possible)
- _(i) records have structural equality i.e. can be compared directly_
```fsharp
type ContactCard = 
    {   Name : string
        Phone : string
        Verified : bool }
let contact1 = 
    {   Name = "Alf" 
        Phone = "(206) 555-0157" 
        Verified = false }
let contactOnSameLine = { Name = "Alf"; Phone = "(206) 555-0157"; Verified = false }

/// copy and overwrite some fields
let contact2 = 
    {   contact1 with 
            Phone = "(206) 555-0112"
            Verified = true }

let showContactCard (c: ContactCard) = 
    c.Name + " Phone: " + c.Phone + (if not c.Verified then " (unverified)" else "")
printfn "Alf's Contact Card: %s" (showContactCard contact1)

/// record with a member
type ContactCardAlternate =
    {   Name     : string
        Phone    : string
        Address  : string
        Verified : bool }

    /// Members can implement object-oriented members.
    member this.PrintedContactCard =
        this.Name + " Phone: " + this.Phone
        + (if not this.Verified then " (unverified)" else "")
        + this.Address

let contactAlternate = 
    {   Name = "Alf" 
        Phone = "(206) 555-0157" 
        Verified = false 
        Address = "111 Alf Street" }
printfn "Alf's alternate contact card is %s" contactAlternate.PrintedContactCard
```

## Discriminated Unions (DUs)

- can be a number of named forms or cases
- use `type <name> |||` notation
- use `member` for OOP members on instance (`static` addition possible)
```fsharp
type Suit = 
    | Hearts 
    | Clubs 
    | Diamonds 
    | Spades

type Rank = 
    | Value of int
    | Ace
    | King
    | Queen
    | Jack

    static member GetAllRanks() = 
        [   yield Ace
            for i in 2 .. 10 do yield Value i
            yield Jack
            yield Queen
            yield King ]
                                
type Card = { Suit: Suit; Rank: Rank }

let fullDeck = 
    [ for suit in [Hearts; Diamonds; Clubs; Spades] do
            for rank in Rank.GetAllRanks() do 
                yield { Suit=suit; Rank=rank } ]

let showPlayingCard (c: Card) = 
    let rankString = 
        match c.Rank with 
        | Ace -> "Ace"
        | King -> "King"
        | Queen -> "Queen"
        | Jack -> "Jack"
        | Value n -> string n
    let suitString = 
        match c.Suit with 
        | Clubs -> "clubs"
        | Diamonds -> "diamonds"
        | Spades -> "spades"
        | Hearts -> "hearts"
    rankString  + " of " + suitString

let printAllCards() = 
    for card in fullDeck do 
        printfn "%s" (showPlayingCard card)
```

### Single Case DUs

- SCDUs help with domain modeling over primitive types
- SCDUs cannot be implicitly converted to or from the type they wrap
```fsharp
type Address = Address of string
type Name = Name of string
type SSN = SSN of int

let address = Address "111 Alf Way"
let name = Name "Alf"
let ssn = SSN 1234567890

let unwrapAddress (Address a) = a
let unwrapName (Name n) = n
let unwrapSSN (SSN s) = s

// Printing single-case DUs is simple with unwrapping functions.
printfn "Address: %s, Name: %s, and SSN: %d"
    (address |> unwrapAddress) (name |> unwrapName) (ssn |> unwrapSSN)
```
- SCDUs support recursive definitions (for trees and recursive data)
```fsharp
/// Binary Search Tree
/// 1. case: empty tree,
/// 2. case: a Node with a value and two subtrees
type BST<'T> =
    | Empty
    | Node of value:'T * left: BST<'T> * right: BST<'T>

let rec exists item bst =
    match bst with
    | Empty -> false
    | Node (x, left, right) ->
        if item = x then true
        elif item < x then (exists item left)
        else (exists item right)

let rec insert item bst =
    match bst with
    | Empty -> Node(item, Empty, Empty)
    | Node(x, left, right) as node ->
        if item = x then node
        elif item < x then Node(x, insert item left, right)
        else Node(x, left, insert item right)
```

### Optional Types

- represents a value-or-nothing type with `Some` and `None`
- use `option` to mark optional
```fsharp
type ZipCode = ZipCode of string
type Customer = { ZipCode: ZipCode option }

// interface-like with `abstract`
type IShippingCalculator =
    abstract GetState: ZipCode -> string option
    abstract GetShippingZone: string -> int

/// Next, calculate a shipping zone for a customer using a calculator instance.
/// This uses combinators in the Option module to allow a functional pipeline for
/// transforming data with Optionals.
let CustomerShippingZone (calculator: IShippingCalculator, customer: Customer) =
    customer.ZipCode 
    |> Option.bind calculator.GetState 
    |> Option.map calculator.GetShippingZone
```

## Pattern Matching

- use `math x with ...`
```fsharp
type Person = {
    First: string
    Last: string
}

type Employee =
    | Engineer of engineer: Person
    | Manager of manager: Person * reports: List<Employee>
    | Executive of executive: Person * reports: List<Employee> * assistant: Employee

/// count employee management hierarchy including self
let rec countReports(emp : Employee) =
    1 + match emp with
        | Engineer(person) ->
            0
        | Manager(person, reports) ->
            reports |> List.sumBy countReports
        | Executive(person, reports, assistant) ->
            (reports |> List.sumBy countReports) + countReports assistant


/// Find all managers/executives named "Dave" who do not have any reports.
/// This uses the 'function' shorthand as a lambda expression.
let rec findDaveWithOpenPosition(emps : List<Employee>) =
    emps
    |> List.filter(function
                        // [] matches an empty list
                        | Manager({First = "Dave"}, []) -> true
                        // _ matches anything
                        | Executive({First = "Dave"}, [], _) -> true
                        | _ -> false)
```
- use `_` for parse operation fail
```fsharp
open System

/// You can also use the shorthand function construct for pattern matching, 
/// which is useful for Partial Application
let private parseHelper (f: string -> bool * 'T) = f >> function
    | (true, item) -> Some item
    | (false, _) -> None

let parseDateTimeOffset = parseHelper DateTimeOffset.TryParse

let result = parseDateTimeOffset "1970-01-01"
match result with
| Some dto -> printfn "It parsed!"
| None -> printfn "It didn't parse!"

let parseInt = parseHelper Int32.TryParse
let parseDouble = parseHelper Double.TryParse
let parseTimeSpan = parseHelper TimeSpan.TryParse
```
- use active patterns to e.g. partition input data
```fsharp
// partition input data into custom forms decomposing at the match call site
let (|Int|_|) = parseInt
let (|Double|_|) = parseDouble
let (|Date|_|) = parseDateTimeOffset
let (|TimeSpan|_|) = parseTimeSpan

/// Pattern Matching via 'function' and Active Patterns often looks like this
let printParseResult = function
    | Int x -> printfn "%d" x
    | Double x -> printfn "%f" x
    | Date d -> printfn "%s" (d.ToString())
    | TimeSpan t -> printfn "%s" (t.ToString())
    | _ -> printfn "Nothing was parse-able!"

printParseResult "12"
printParseResult "12.045"
printParseResult "12/28/2016"
printParseResult "9:01PM"
printParseResult "banana!"
```

## Units of Measure

- provide context for numeric literals via annotations
```fsharp
/// First, open a collection of common unit names
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

let sampleValue1 = 1600.0<meter>          

/// define a new unit type
[<Measure>]
type mile =
    static member asMeter = 1609.34<meter/mile>

let sampleValue2  = 500.0<mile>          

let sampleValue3 = sampleValue2 * mile.asMeter   

printfn "After a %f race I would walk %f miles which would be %f meters"
    sampleValue1 sampleValue2 sampleValue3
```

## Classes

- classes can have methods, properties, events etc.
```fsharp
/// constructor is on the first line
type Vector2D(dx: double, dy: double) =
    /// internal field, computed on construction
    let length = sqrt (dx*dx + dy*dy)

    /// a property
    member this.DX = dx
    member this.DY = dy
    member this.Length = length

    /// a method
    member this.Scale(k) = Vector2D(k * this.DX, k * this.DY)

let vector1 = Vector2D(3.0, 4.0)
let vector2 = vector1.Scale(10.0)

printfn "Length of vector1: %f\nLength of vector2: %f"
    vector1.Length vector2.Length
```
- generic classes are possible
```fsharp
type StateTracker<'T>(initialElement: 'T) = 
    let mutable states = [ initialElement ]

    member this.UpdateState newState = 
        states <- newState :: states

    member this.History = states
    member this.Current = states.Head

// inferred 'T as int
let tracker = StateTracker 10
tracker.UpdateState 17
printfn "Current state: %d" tracker.Current
```

## Interfaces

- use `abstract` members and then `interface ... with` for extending
```fsharp
/// implement IDisposable in object type
type ReadFile() =
    let file = new System.IO.StreamReader("readme.txt")

    member this.ReadLine() = file.ReadLine()

    interface System.IDisposable with
        member this.Dispose() = file.Close()

/// implement IDisposable in object expression
let interfaceImplementation =
    { new System.IDisposable with
        member this.Dispose() = printfn "disposed" }
```