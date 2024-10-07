using Microsoft.AspNetCore.Components;

namespace StandardComponents.UI_Components.Buttons
{
    public partial class PrimaryButtonComponent
    {
        [Parameter] public string ButtonClass { get; set; } = "btn btn-primary"; // Default Bootstrap classes
        [Parameter] public string ButtonType { get; set; } = "button"; // Default button type
        [Parameter] public bool IsDisabled { get; set; } = false; // To disable the button
        [Parameter] public EventCallback OnClick { get; set; } // Click event callback
        [Parameter] public RenderFragment ChildContent { get; set; } // Content inside the button
    }
}