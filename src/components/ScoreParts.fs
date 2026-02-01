module App.components.ScoreParts


open App.components
open App.components.Primitives
open App.components.Themes
open Feliz

type RenderContext = {
    beatsPerMeasure: int
    barsPerRow: int
    padBars: int
    maxRows: int
}


let beatCell (beatIndex: int) =
    Html.div [
        prop.style [
            style.width 50
            style.height 50
            style.display.flex
            style.alignItems.center
            style.justifyContent.center
            style.cursor.pointer
            style.backgroundColor defaultBackground
            style.color "gray"
        ]
        prop.text (string beatIndex)
        prop.onClick (fun _ -> printfn $"Clicked beat {beatIndex}")        
    ]
    
let bar (ctx: RenderContext) (barIndex: int) =
    vStack
        [
            style.gap 0           // StackPanel の Margin 相当
            style.padding 0
        ]
        [
            hStack
                [
                    style.alignItems.center
                    style.fontSize 12
                ]
                [
                    Html.span [ prop.text $"lyrics..." ]
                ]
            hStack
                [
                    style.backgroundColor "gray"
                    style.gap 1
                    style.padding 1
            
                ]
                [
                    for i in 0..(ctx.beatsPerMeasure-1) -> beatCell (barIndex*ctx.beatsPerMeasure + i)
                ]
        ]

let rowView ctx rowIndex =
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
                for i in 0..(ctx.barsPerRow-1) -> bar ctx (rowIndex*ctx.barsPerRow + i)        
            ])
        ]
        
let sectionView ctx  =
    vStack
        [

        ]
        [
            for i in 0..(ctx.maxRows-1) -> rowView ctx i
        ]