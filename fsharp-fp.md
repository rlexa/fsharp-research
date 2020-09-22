# F# Functional Programming

## Terminology

- Function
  - will produce an output when given an input
    - more exactly: maps an item from one set to another set
  - most basic and important concept in functional programming
- Expression
  - code construct producing a value
  - in F# result must be bound or explicitly ignored
  - Referential Transparency
    - can be trivially replaced by a function call
- Purity
  - always same return value for same arguments
  - evaluation has no side effects
- Immutability
  - a value can not be changed in-place

## Functions

### Definition

```fsharp
let addOne x = x + 1
```

### Signature

```fsharp
val addOne: x:int -> int
```
- `addOne` accepts an int `x` and produces an int
- `addOne` is mapping a value from set of int to set of int
- in typed fp signature is more important than runtime behavior of the definition
  - example reason: when the function is used correctly due to it's signature types any problems that arise can only be in the body of that function

### Purity

While input-to-same-output is easy to grasp, it can still become an issue when mutability is introduced and also side effects can creep up accidentally.

```fsharp
let mutable value = 1
/// not pure, depends on mutable value
let addOneToValue x = x + value

/// not pure, has a console side effect
let addOneToValue x =
    printfn "x is %d" x
    x + 1
```

### First-Class

A function can be bound to a name just like a value.
```fsharp
/// int
let num = 10
/// string
let str = "F#"
/// function with lambda
let squareIt = fun n -> n * n
/// function with F#'s `concise` syntax
let squareIt2 n = n * n

let doubleIt = fun n -> 2 * n

// functions can be stored together in a list if signature is the same
let funList = [ squareIt; doubleIt ]

let BMICalculator = fun ht wt ->
    (float wt / float (squareIt ht)) * 703.0

// any function can be stored in tuples like anything else
let funTuple = ( squareIt, BMICalculator )

// same as literals, lambdas can be stored directly
let funAndArgTuple = ((fun n -> n * n), 10)
// expression squares 10, returns 100, then displays 100
System.Console.WriteLine((fst funAndArgTuple)(snd funAndArgTuple))
```

#### Passing As Values

```fsharp
let num = 10
let squareIt = fun x -> x * x
let applyIt = fun op arg -> op arg
// the result returned and displayed is 100
System.Console.WriteLine(applyIt squareIt num)

let integerList = [ 1; 2; 3; 4; 5; 6; 7 ]
let squareAll = List.map squareIt integerList
printfn "%A" squareAll

let evenOrNot = List.map (fun n -> n % 2 = 0) integerList
printfn "%A" evenOrNot
```

#### Return as Value

- using functions as building blocks of functions
```fsharp
let str = "F#"
let doubleIt = fun n -> 2 * n
let squareIt = fun x -> x * x

// BTW lambda and param call inline, returns true
System.Console.WriteLine((fun n -> n % 2 = 1) 15)

let checkFor item =
    let functionToReturn = fun lst ->
        List.exists (fun a -> a = item) lst
    functionToReturn

let integerList = [ 1; 2; 3; 4; 5; 6; 7 ]
let stringList = [ "one"; "two"; "three" ]

let checkFor7 = checkFor 7
let checkForSeven = checkFor "seven"

// return true
System.Console.WriteLine(checkFor7 integerList)
// return false
System.Console.WriteLine(checkForSeven stringList)
```
- example with custom compose implementation
  - _(i) syntax `<<` and `>>` can be used to compose functions_
```fsharp
let compose =
    fun op1 op2 ->
        fun n ->
            op1 (op2 n)

let composeWithConciseSyntax op1 op2 ->
    fun n ->
        op1 (op2 n)

let doubleIt = fun n -> 2 * n
let squareIt = fun x -> x * x
let doubleAndSquare = compose squareIt doubleIt
// => 36
System.Console.WriteLine(doubleAndSquare 3)
// with native syntax, => 36
System.Console.WriteLine((squareIt << doubleIt) 3)
// => 18
System.Console.WriteLine((squareIt >> doubleIt) 3)
```
- example: guessing game
```fsharp
let makeGame target =
    fun guess ->
        if guess = target then
            System.Console.WriteLine("You win!")
        else
            System.Console.WriteLine("Wrong. Try again.")

let playGame = makeGame 7
playGame 2
playGame 9
playGame 7

let alphaGame = makeGame 'q'
alphaGame 'c'
alphaGame 'r'
alphaGame 'j'
alphaGame 'q'
```
  - ...or the same but with implicit currying, making it shorter (partial application)
```fsharp
let makeGame target guess =
    if guess = target then
       System.Console.WriteLine("You win!")
    else
       System.Console.WriteLine("Wrong. Try again.")
```

## Expressions

As opposed to statements (which do not return anything) expressions always returns a value.

```fsharp
// 'x + 1' is an expression
let addOne x = x + 1

// expression can be changed
let addOne x = x.ToString() + "1"
// which changes signature to generic x (anything can have toString) and string return value
val addOne: x:'a -> string
```

### `unit` type for empty result

```fsharp
let printString (str: string) =
    printfn "String is: %s" str
/// has signature:
val printString: str:string -> unit
```