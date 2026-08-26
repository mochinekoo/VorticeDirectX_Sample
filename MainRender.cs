using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Effects;
using Vortice.D3DCompiler;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Vortice.Wpf;
using VorticeDirectX_Sample.Manager;
using static VorticeDirectX_Sample.Manager.DX3D;

namespace VorticeDirectX_Sample {
    public class MainRender {

        public static DX3D DX3D {
            get;
        } = new DX3D();

        public static SceneManager SceneManager {
            get;
        } = new SceneManager();

        public void Init(DrawingSurfaceEventArgs e) {
            DX3D.Init(e);
            SceneManager.Init();
        }

        public void Draw(DrawEventArgs e) {
            e.Context.OMSetRenderTargets(e.Surface.ColorTextureView, null);
            e.Context.ClearRenderTargetView(e.Surface.ColorTextureView, new Vortice.Mathematics.Color4(0.0f, 0.0f, 0.0f, 1.0f));
            e.Context.OMSetBlendState(null);
            e.Context.OMSetDepthStencilState(DX3D.DepthStencilState, 0);
            e.Context.RSSetState(DX3D.RasterizerState);

            using var resource = e.Surface.ColorTextureView.Resource;
            using var texture = resource.QueryInterface<ID3D11Texture2D>();
            var desc = texture.Description;
            e.Context.RSSetViewport(0, 0, desc.Width, desc.Height);

            SceneManager.Update();

            e.Context.RSSetState(null);
        }

    }
}
