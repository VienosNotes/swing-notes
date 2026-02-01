namespace App

open App.Types
open App.components
open App.components.ScoreParts
open Elmish.Sub
open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Main =

    
    let update msg model =
        match msg with
        | ClickBeat(beatIndex, lane) -> {
            model with cursor = Some { beatIndex = beatIndex; lane = lane }
                       draft = match lane with
                               | Chord -> (Map.tryFind beatIndex model.chords) |> Option.map _.text
                               | Lyric -> (Map.tryFind beatIndex model.lyrics) |> Option.map _.text
            }
        | InputText text ->
            match model.cursor with
            | Some _ -> { model with draft = Some text }
            | _ -> model
        | Commit ->
            match (model.cursor, model.draft) with
            | Some { lane = Chord; beatIndex = i }, Some draft -> { model with chords = Map.add i { text = draft } model.chords; cursor = None; draft = None }
            | Some { lane = Chord; beatIndex = i}, None -> { model with chords = Map.remove i model.chords; cursor = None; draft = None }
            | Some { lane = Lyric; beatIndex = i }, Some draft -> { model with lyrics = Map.add i { text = draft; mode = Auto } model.lyrics; cursor = None; draft = None }
            | Some { lane = Lyric; beatIndex = i}, None -> { model with lyrics = Map.remove i model.lyrics; cursor = None; draft = None }
            | _ -> { model with cursor = None; draft = None }
        | Cancel -> { model with cursor = None; draft = None }

    [<ReactComponent>]
    let El() =
        importSideEffects "./App.css"
        let initModel = {
            cursor = None
            draft = None
            lyrics = Map.empty
            chords = Map.empty
        }
        let model, dispatch =
            React.useReducer(
                (fun (model: Model) (msg: Msg) -> update msg model),
                initModel
            )
            
        let defaultCtx = {
            beatsPerMeasure = 4
            barsPerRow = 4
            maxRows = 20
            padBars = 0
            dispatch = dispatch
        }
        
        Html.div [            
            sectionView defaultCtx model
        ]