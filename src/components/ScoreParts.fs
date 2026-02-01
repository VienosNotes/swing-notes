module App.components.ScoreParts


open System
open App.Types
open App.components
open App.components.Primitives
open App.components.Themes
open Feliz

let lyricInputOverlay ctx model beatIndex =
    Html.input [
        prop.value (model.draft |> Option.defaultValue String.Empty)
        prop.autoFocus true
        
        prop.onChange (fun (ev:Browser.Types.Event) ->
            let value = (ev.currentTarget :?> Browser.Types.HTMLInputElement).value |> string
            ctx.dispatch (InputText value)
        )        
        
        prop.onKeyDown (fun ev ->
            match ev.key with
            | "Enter" ->
                ev.preventDefault()
                ctx.dispatch Commit
            | "Escape" ->
                ev.preventDefault()
                ctx.dispatch Cancel
            | _ -> ()
        )

        prop.onBlur (fun _ ->
            ctx.dispatch Commit
        )

        prop.style [
            style.position.absolute
            style.top 0
            style.left 0
            style.width 200            
            style.height (length.percent 100)

            style.boxSizing.borderBox
            style.padding 4

            style.fontSize 14
            style.border (1, borderStyle.solid, "#66f")
            style.backgroundColor "dimgray"
            style.color "white"
            style.zIndex 10
        ]
    ]
    
let beatCell ctx model (beatIndex: int) =
    let isEditing i lane =
        model.cursor = Some { beatIndex = i; lane = lane }
        
    let chord =
        model.chords
        |> Map.tryFind beatIndex

    let text =
        chord
        |> Option.map _.text
        
    Html.div [
        prop.style ([
            style.position.relative
            style.width beatWidthPx
            style.height beatHeightPx
            style.display.flex
            style.alignItems.center
            style.justifyContent.center
            style.cursor.pointer
            style.backgroundColor defaultBackground
            style.color "gray"
        ] @ (buildStyle text))
        
        prop.onClick (fun _ -> ctx.dispatch (ClickBeat(beatIndex, Chord)))
        
        prop.children [
            Html.div [
                prop.text (text |> Option.defaultValue (beatIndex |> string))
            ]
            if isEditing beatIndex Chord then
                lyricInputOverlay ctx model beatIndex
        ]
    ]    


let lyricCell ctx (model:Model) beatIndex =
    let isEditing i lane =
        model.cursor = Some { beatIndex = i; lane = lane }
    let lyric =
        model.lyrics
        |> Map.tryFind beatIndex

    let text =
        lyric
        |> Option.map _.text
        |> Option.defaultValue String.Empty            
        
    Html.div [
        prop.style [
            style.position.absolute
            style.whitespace.nowrap
            style.textAlign.justify
            style.letterSpacing 20
            style.textJustify.interCharacter
            style.wordBreak.breakAll
            style.bottom 0
            style.left ((beatIndex % ctx.beatsPerMeasure) * (beatWidthPx + 1))
            style.minWidth 500
            style.minHeight 24
            style.zIndex beatIndex
        ]
        
        prop.onClick (fun _ -> ctx.dispatch (ClickBeat(beatIndex, Lyric)))
        
        prop.children [
            Html.div [
                prop.text text
            ]
            if isEditing beatIndex Lyric then
                lyricInputOverlay ctx model beatIndex
        ]
    ]
    

let bar (ctx: RenderContext) model (barIndex: int) =
    vStack
        [
            style.gap 0           
            style.padding 0
        ]
        [
            hStack
                [
                    style.position.relative
                    style.alignItems.center
                    style.overflow.visible
                    style.fontSize 12
                    style.height 24
                ]
                [
                    for i in 0..(ctx.beatsPerMeasure-1) -> lyricCell ctx model (barIndex*ctx.beatsPerMeasure + i)
                ]
            hStack
                [
                    style.backgroundColor "gray"
                    style.gap 1
                    style.padding 1
            
                ]
                [
                    for i in 0..(ctx.beatsPerMeasure-1) -> beatCell ctx model (barIndex*ctx.beatsPerMeasure + i)
                ]
        ]

let rowView ctx model  rowIndex =
    hStack
        [
            style.gap 0           // StackPanel の Margin 相当
            style.padding 6
        ]
        [
            Html.div [
                prop.style [
                    style.width 30
                    style.textAlign.left
                    style.color "darkgray"
                    style.fontSize 12
                    style.fontStyle.italic
                    style.fontFamily "Times New Roman, serif"
                ]
                prop.text $"#{rowIndex * ctx.barsPerRow + 1}"
            ]
            (hStack []
            [
                for i in 0..(ctx.barsPerRow-1) -> bar ctx model (rowIndex*ctx.barsPerRow + i)        
            ])
        ]
        
let sectionView ctx model  =
    vStack
        [

        ]
        [
            for i in 0..(ctx.maxRows-1) -> rowView ctx model i
        ]