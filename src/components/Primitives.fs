module App.components.Primitives

open Feliz

let vStack (extraStyles: IStyleAttribute list) (children: ReactElement seq) =
    Html.div [
        prop.style ([
            style.display.flex
            style.flexDirection.column
        ] @ extraStyles)
        
        prop.children children
    ]

let hStack (extraStyles: IStyleAttribute list) (children: ReactElement seq) =
    Html.div [
        prop.style ([
            style.display.flex
            style.flexDirection.row
        ] @ extraStyles)
        
        prop.children children
    ] 