namespace TabTest

/// This is the main module of your application
/// here you handle all of your child pages as well as their
/// messages and their updates, useful to update multiple parts
/// of your application, Please refer to the `view` function
/// to see how to handle different kinds of "*child*" controls
module Shell =
    open Elmish
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish

    type State =
        { devicePages : Page.State list }

    type Msg =
        | Msg of System.Guid * Page.Msg

    let init =
        { devicePages = List.map (fun _ -> {uid = System.Guid.NewGuid()} ) [0..3] },
        Cmd.none

    let update (msg: Msg) (state: State): State * Cmd<_> =
        printfn "Message: %A" msg
        printfn "State: %A\n" state
        match msg with
        | Msg (uid,Page.Msg.Close) ->
            {state with devicePages = List.filter (fun st -> st.uid <> uid) state.devicePages},
            Cmd.none
        | Msg (_,_) ->
            state, Cmd.none

    let view (state: State) dispatch =
        let deviceDispatch uid msg =
            Msg(uid,msg) |> dispatch

        TabControl.create
                [ TabControl.classes ["shell"]
                  TabControl.tabStripPlacement Dock.Top
                  TabControl.viewItems
                      [ yield! List.map (fun st -> Page.view st (deviceDispatch st.uid)) state.devicePages ]
                ]

    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Full App"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0

            this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
//            this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

            Elmish.Program.mkProgram (fun () -> init ) update view
            |> Program.withHost this

            |> Program.run
