namespace App

open System

module Types =
    type Lane = Chord | Lyric

    type Beats = Beats of int with
        static member (+) (Beats a, Beats b) = Beats (a + b)
        static member (-) (Beats a, Beats b) = Beats (a - b)
        member this.v =
            let (Beats v) = this
            v
    type Bars = Bars of int with
        member this.v =
            let (Bars v) = this
            v
    

    type Cursor = {
        beatIndex: Beats
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
    
    type SectionEvent = {
        id: Guid
        name: string
        padding: Bars
    }

    type Msg =
    | ClickBeat of beatIndex: Beats * lane: Lane
    | InputText of string
    | InsertSectionLabel
    | RemoveSectionLabel
    | Commit
    | Cancel        
            
    type Model = {
        cursor: Cursor option
        lyrics: Map<Beats, LyricEvent>
        chords: Map<Beats, ChordEvent>
        sections: Map<Bars, SectionEvent>
        draft: string option
    }

    type RenderContext = {
        beatsPerMeasure: Beats
        barsPerRow: Bars
        padBars: Bars
        maxRows: int
        dispatch: Msg -> unit
    }
