namespace SA.Application.View {
    using System.Windows.Input;

    public partial class WindowMain {

        void AddCommandBindings() {

            CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Close,
                (_, _) => Close(),
                (_, eventArgs) => { eventArgs.CanExecute = true; }));
            
            CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Help,
                (_, _) => about.ShowAbout(this),
                (_, eventArgs) => { eventArgs.CanExecute = true; }));

        } //AddCommandBindings

} //class WindowMain

}
