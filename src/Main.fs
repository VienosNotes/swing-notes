namespace App

open App.components
open App.components.ScoreParts
open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Main =

    type Lane = Chord | Lyric
    
    type Beats = Beats of int
    
    type Cursor = {
        beatIndex: int
        lane: Lane
    }
        
    type LyricEventMode =
        | Auto
        | FixedLength of Beats    
    
    type LyricEvent = {
        text: string
        mode: LyricEventMode
    }
    
    type ChordEvent = {
        text: string
    }    
    
    type Msg =
        | ClickBeat of beatIndex: int * lane: Lane
        | InputText of string
        | Commit
        | Cancel        
                
    type Model = {
        cursor: Cursor option
        lyrics: Map<int, LyricEvent>
        chords: Map<int, ChordEvent>
        dispatch: Msg -> unit
        draft: string option
    }
    
    let defaultCtx = {
            beatsPerMeasure = 4
            barsPerRow = 4
            maxRows = 20
            padBars = 0
        }
    
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
        
        Html.div [            
            sectionView defaultCtx
        ]