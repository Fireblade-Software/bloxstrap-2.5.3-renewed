using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Bloxstrap.UI.ViewModels.Bootstrapper
{
    public class ProgressFluentDialogViewModel : BootstrapperDialogViewModel
    {
        public BackgroundType WindowBackdropType { get; set; } = BackgroundType.Mica;
        public SolidColorBrush BackgroundColourBrush { get; set; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        [Obsolete("Do not use this! This is for the designer only.", true)]
        public ProgressFluentDialogViewModel() : base()
        { }

        public ProgressFluentDialogViewModel(IBootstrapperDialog dialog, bool aero) : base(dialog)
        {
            WindowBackdropType = aero ? BackgroundType.Aero : BackgroundType.Mica;

            if (aero)
                BackgroundColourBrush = App.Settings.Prop.Theme.GetFinal() == Enums.Theme.Light ?
                    new SolidColorBrush(Color.FromArgb(32, 255, 255, 255)) :
                    new SolidColorBrush(Color.FromArgb(32, 0, 0, 0));
        }
    }
}
