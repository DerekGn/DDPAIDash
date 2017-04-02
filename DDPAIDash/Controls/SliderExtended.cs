
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace DDPAIDash.Controls
{
    public class SliderExtended : Slider
    {
        private Binding SupressedBinding { get; set; }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            
            var expression = GetBindingExpression(ValueProperty);

            if (expression != null)
            {
                SupressedBinding = expression.ParentBinding;
                SetBinding(ValueProperty, new Binding());
                SetValue(ValueProperty, Value);
            }
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (SupressedBinding != null)
            {
                var value = Value;
                SetBinding(ValueProperty, SupressedBinding);
                SetValue(ValueProperty, value);
                SupressedBinding = null;
            }

            base.OnPointerExited(e);
        }
    }
}
