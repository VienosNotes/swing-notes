namespace App

module Types =
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
        draft: string option
    }

    type RenderContext = {
        beatsPerMeasure: int
        barsPerRow: int
        padBars: int
        maxRows: int
        dispatch: Msg -> unit
    }
