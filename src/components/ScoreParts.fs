module App.components.ScoreParts

open System.Linq
open Feliz

let beatCell (beatIndex: int) =
    Html.div [
        prop.style [
            style.width 40
            style.height 40
            style.border (1, borderStyle.solid, "gray")         
            style.display.flex
            style.alignItems.center
            style.justifyContent.center
            style.cursor.pointer
        ]
        prop.text (string beatIndex)
        prop.onClick (fun _ -> printfn $"Clicked beat {beatIndex}")        
    ]
    
let bar (beatsPerMeasure: int) =
    Html.div [
        prop.style [
            style.display.flex
            style.gap 2
            style.marginBottom 4
        ]
        
        prop.children [
            Seq.map ()
        ]
    ]
    