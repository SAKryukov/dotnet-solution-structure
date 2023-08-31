namespace SA.Application.View {
    using System.Windows.Input;

    public partial class WindowMain {

        void AddCommandBindings() {

            CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Open,
                (_, _) => { },
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
