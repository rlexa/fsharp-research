# Tutorial

## REPL in `MyFirstScript.fs`

- create file, add:
```fsharp
let toPigLatin (word: string) =
    let isVowel (c: char) =
        match c with
        | 'a' | 'e' | 'i' |'o' |'u'
        | 'A' | 'E' | 'I' | 'O' | 'U' -> true
        |_ -> false
    
    if isVowel word.[0] then
        word + "yay"
    else
        word.[1..] + string(word.[0]) + "ay"
```
- highlight the whole function with a mouse and hit `ALT+ENTER`, look for a terminal
  - FSI process is started
  - the highlighted code is sent to in FSI
  - the function is evaluated in FSI
- now the function is available in FSI interactive terminal, try
  - `toPigLatin "banana";;`
  - `toPigLatin "apple";;`
- _(i) `;;` means it's done and should be evaluated, just `;` allows multilines_

## Move to a module

- open `Program.fs`
- add the code from above as a module above the `main` function
```fsharp
module PigLatin =
    let toPigLatin (word: string) =
        let isVowel (c: char) =
            match c with
            | 'a' | 'e' | 'i' |'o' |'u'
            | 'A' | 'E' | 'I' | 'O' | 'U' -> true
            |_ -> false
        
        if isVowel word.[0] then
            word + "yay"
        else
            word.[1..] + string(word.[0]) + "ay"
```
- change the `main` function to use the module
```fsharp
[<EntryPoint>]
let main argv =
    for name in argv do
        let newName = PigLatin.toPigLatin name
        printfn "%s in Pig Latin is: %s" name newName
    0
```
- now it can be run as an executable (in the directory of the project)
  - `dotnet run apple banana`
```
apple in Pig Latin is: appleyay
banana in Pig Latin is: ananabay
```