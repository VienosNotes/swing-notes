namespace App

open App.components
open App.components.ScoreParts
open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Main =
    let defaultCtx = {
            beatsPerMeasure = 4
            barsPerRow = 4
            maxRows = 20
            padBars = 0
        }
    
    [<ReactComponent>]
    let El() =
        importSideEffects "./App.css"
        
        Html.div [            
            sectionView defaultCtx
        ]