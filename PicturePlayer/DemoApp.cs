using Xilium.CefGlue;

using PicturePlayer.JsCall;

namespace PicturePlayer
{
    internal sealed class DemoApp : CefApp
    {
        private CefRenderProcessHandler _renderProcessHandler = new DemoRenderProcessHandler();

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }
    }
}
