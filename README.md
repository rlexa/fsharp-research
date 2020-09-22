# fsharp-research

Playground for F#.

# First Steps

- install [.NET Core SDK](https://dotnet.microsoft.com/download)
- in VS Code install `ionide-fsharp`, `ionide-fake`
- `dotnet new console -lang "F#" -o FirstIonideProject`
- in VS code settings activate 'FSharp: use SDK scripts'
- see rest in [FirstIonideProject readme](./FirstIonideProject/README.md)

# F#

## REPL

REPL-driven development basically means there is an interactive terminal (FSI here) where any code can be evaluated and used and experimented with. The working code is then moved to a file for compilation into an assembly.

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

## Record and Discriminated Union Types

[todo](https://docs.microsoft.com/de-de/dotnet/fsharp/tour#record-and-discriminated-union-types)