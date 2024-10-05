using Microsoft.AspNetCore.Components;

namespace StandardComponents.InputComponents
{
    public partial class InputSelectComponent<TItem> : ComponentBase
    {
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Label { get; set; }
        private TItem _value;
        [Parameter]
        public TItem Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged.InvokeAsync(_value);
            }
        }
        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }
        [Parameter]
        public List<TItem> Options { get; set; }
        [Parameter]
        public string PropertyName { get; set; }

        private async Task OnValueChanged(object e)
        {
            Value = (TItem)e;
            await ValueChanged.InvokeAsync(Value);
        }
    }
}