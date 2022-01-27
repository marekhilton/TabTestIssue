namespace TabTest

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components


module Page =

    let private tabView closeFunc (name:string) =
        let components =
            DockPanel.create [
                DockPanel.children [
                    Button.create [ Button.dock Dock.Right
                                    Button.content "x"
                                    Button.onClick closeFunc ]
                    TextBlock.create [ TextBlock.text name
                                       TextBlock.dock Dock.Left]
                    ]
                ]
        Border.create [ Border.child components ]

    let tabHeader closeFunc =
        tabView closeFunc
        |> DataTemplateView<string>.create

    type State =
        { uid : System.Guid }

    type Msg =
        | Noop
        | Close

    let init uid =
        { uid = uid }

    let update (msg:Msg) (state:State) =
        match msg with
        | Noop -> state
        | _ -> printfn "SHOULD NOT HAPPEN" ; state


    let view (state:State) dispatch : Avalonia.FuncUI.Types.IView =
        let content = TextBox.create [TextBox.text "Hello"]
        let closeFunc (_:'a) =
            dispatch Close
        TabItem.create
            [
                TabItem.header (state.uid.ToString().Substring(0,4))
                TabItem.headerTemplate (tabHeader closeFunc)
                TabItem.content content
//                TabItem.name (state.uid.ToString().Substring(0,4))
            ]
