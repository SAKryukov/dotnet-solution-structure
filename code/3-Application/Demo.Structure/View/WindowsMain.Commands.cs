namespace SA.Application.View {
    using System.Windows.Input;

    public partial class WindowMain {

        void AddCommandBindings() {

            CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Open,
                (_, _) => LoadPlugin(doAct: true),
                (_, eventArgs) => { eventArgs.CanExecute = true; }));

            CommandBindings.Add(new CommandBinding(
                CommandSet.ExecuteUiPlugin,
                (_, _) => ExecuteUiPlugin(doAct: true),
                (_, eventArgs) => { eventArgs.CanExecute = ExecuteUiPlugin(doAct: false); }));

            CommandBindings.Add(new CommandBinding(
                CommandSet.ExecutePropertyPluginWithEntryAssembly,
                (_, _) => ExecutePropertyPlugin(PropertyPluginAction.ProcessEntryAssembly, doAct: true),
                (_, eventArgs) => { eventArgs.CanExecute = ExecutePropertyPlugin(PropertyPluginAction.ProcessEntryAssembly, doAct: false); }));

            CommandBindings.Add(new CommandBinding(
                CommandSet.ExecutePropertyPluginWithPluginAssembly,
                (_, _) => ExecutePropertyPlugin(PropertyPluginAction.ProcessPluginAssembly, doAct: true),
                (_, eventArgs) => { eventArgs.CanExecute = ExecutePropertyPlugin(PropertyPluginAction.ProcessPluginAssembly, doAct: false); }));

            CommandBindings.Add(new CommandBinding(
                CommandSet.ExecutePropertyPluginWithAssembly,
                (_, _) => ExecutePropertyPlugin(PropertyPluginAction.LoadAndProcessAssembly, doAct: true),
                (_, eventArgs) => { eventArgs.CanExecute = ExecutePropertyPlugin(PropertyPluginAction.LoadAndProcessAssembly, doAct: false); }));

            CommandBindings.Add(new CommandBinding(
                CommandSet.UnloadPlugin,
                (_, _) => UnloadPlugin(doAct: true),
                (_, eventArgs) => { eventArgs.CanExecute = UnloadPlugin(doAct: false); }));

            CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Help,
                (_, _) => about.ShowAbout(this),
                (_, eventArgs) => { eventArgs.CanExecute = true; }));

            CommandBindings.Add(new CommandBinding(
                CommandSet.TestException,
                (_, _) => {
                    int x = 0;
                    int y = 1;
                    y /= x;
                },
                (_, eventArgs) => { eventArgs.CanExecute = true; }));
        } //AddCommandBindings

    } //class WindowMain

}
