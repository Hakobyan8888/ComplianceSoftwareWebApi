using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

class RenderModeInteractiveServer : RenderModeAttribute
{
    public override IComponentRenderMode Mode => RenderMode.InteractiveServer;
}
