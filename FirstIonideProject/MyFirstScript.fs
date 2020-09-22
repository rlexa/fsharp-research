// If starts with a vowel append "yay" else move first char to end and append "ay"
let toPigLatin (word: string) =
    let isVowel (char: char) =
        match char with
        | 'a' | 'e' | 'i' |'o' |'u'
        | 'A' | 'E' | 'I' | 'O' | 'U' -> true
        |_ -> false
    
    if isVowel word.[0] then
        word + "yay"
    else
        word.[1..] + string(word.[0]) + "ay"