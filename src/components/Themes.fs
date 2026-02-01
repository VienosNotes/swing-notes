module App.components.Themes

open System
open Feliz

let defaultBackground = "#242424"
let beatWidthPx = 50
let beatHeightPx = 50
let chordFontName = "Merriweather, serif"

let isValidChord (name:string option) =
    match name with
    | Some s -> not (String.IsNullOrWhiteSpace(s))
    | _ -> false

let buildStyle chord : IStyleAttribute list =
    let style = [
        style.fontFamily chordFontName
        style.fontSize (match chord with
                        | Some s when (String.length s) > 3 -> 20
                        | _ -> 24)
        style.color "white"
        style.fontStyle.italic
        style.fontWeight 700
    ]
    if isValidChord chord then style else []