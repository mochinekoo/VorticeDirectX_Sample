using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Vortice.D3DCompiler;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Vortice.Wpf;

namespace VorticeDirectX_Sample {
    public class Render {

        /// DirectX
        public ID3D11Device DXDevice { get; private set; }
        public ID3D11DepthStencilState DepthStencilState { get; private set; }
        public ID3D11Buffer VertexBuffer { get; private set; }
        public ID3D11RasterizerState RasterizerState { get; private set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex {
            public float X;
            public float Y;
            public float Z;
            public Color4 Color;

            public Vertex(float x, float y, float z, Color4 color)
            {
                X = x;
                Y = y;
                Z = z;
                Color = color;
            }

            public const uint SizeInBytes = 28;
        }

        /// シェーダー
        public ID3D11VertexShader VetexShader { get; private set; }
        public ID3D11PixelShader PixelShader { get; private set; }
        public ID3D11InputLayout InputLayout { get; private set; }

        public Render() { }

        public void Init(DrawingSurfaceEventArgs e) {
            DXDevice = e.Device;
            InitShader();
            InitRasterizerState();
            InitVertexBuffer();
        }

        public void InitShader() {

            string shaderPath = Path.Combine(AppContext.BaseDirectory, "Assets", "TestShader.hlsl");

            Debug.Print($"Shader Path: {shaderPath}");
            Debug.Print($"Exists: {File.Exists(shaderPath)}");

            ReadOnlyMemory<byte> vertexShaderData = Compiler.CompileFromFile(
                Path.Combine(AppContext.BaseDirectory, "Assets", "TestShader.hlsl"),
                "VSMain",
                "vs_4_0");

            ReadOnlyMemory<byte> pixelShaderData = Compiler.CompileFromFile(
                Path.Combine(AppContext.BaseDirectory, "Assets", "TestShader.hlsl"),
                "PSMain",
                "ps_4_0");

            VetexShader = DXDevice.CreateVertexShader(vertexShaderData.Span);
            PixelShader = DXDevice.CreatePixelShader(pixelShaderData.Span);

            InputElementDescription[] inputElements = [
                    new InputElementDescription("POSTION", 0, Vortice.DXGI.Format.R32G32B32_Float, 0, 0),
                    new InputElementDescription("COLOR", 0, Vortice.DXGI.Format.R32G32B32A32_Float, 12, 0)
                ];
            InputLayout = DXDevice.CreateInputLayout(inputElements, vertexShaderData.Span);
        }

        public void InitRasterizerState() {
            RasterizerDescription rasterizerDescription = new RasterizerDescription
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                FrontCounterClockwise = false,
                DepthClipEnable = true,
                ScissorEnable = false,
                MultisampleEnable = false,
                AntialiasedLineEnable = false
            };
            RasterizerState = DXDevice.CreateRasterizerState(rasterizerDescription);
        }

        public void InitVertexBuffer() {
            Vertex[] vertices = [
                new Vertex(0.0f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.0f, 0.5f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.5f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),

                new Vertex(0.5f, 0.0f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.0f, 0.5f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new Vertex(0.5f, 0.5f, 0.0f, new Color4(1.0f, 0.0f, 0.0f, 1.0f)) 
                ];
            int size = Marshal.SizeOf<Vertex>() * vertices.Length;

            BufferDescription bufferDescription =
                new BufferDescription
                {
                    Usage = ResourceUsage.Default,
                    ByteWidth = (uint) size,
                    BindFlags = BindFlags.VertexBuffer,
                    CPUAccessFlags = CpuAccessFlags.None,
                    MiscFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };


            GCHandle handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);

            try {
                IntPtr pointer = handle.AddrOfPinnedObject();

                unsafe {
                    SubresourceData subresourceData = new SubresourceData(pointer.ToPointer());

                    VertexBuffer = DXDevice.CreateBuffer(bufferDescription, subresourceData);
                }
            }
            finally
            {
                handle.Free();
            }
        }

        public void Draw(DrawEventArgs e) {
            e.Context.OMSetRenderTargets(e.Surface.ColorTextureView, null);
            e.Context.ClearRenderTargetView(e.Surface.ColorTextureView, new Vortice.Mathematics.Color4(0.0f, 0.0f, 0.0f, 1.0f));
            e.Context.OMSetBlendState(null);
            e.Context.OMSetDepthStencilState(DepthStencilState, 0);
            e.Context.RSSetState(RasterizerState);

            e.Context.IASetPrimitiveTopology(Vortice.Direct3D.PrimitiveTopology.TriangleList);
            e.Context.IASetInputLayout(InputLayout);
            e.Context.VSSetShader(VetexShader);
            e.Context.IASetVertexBuffer(0, VertexBuffer, Vertex.SizeInBytes);
            e.Context.PSSetShader(PixelShader);
            using var resource = e.Surface.ColorTextureView.Resource;
            using var texture = resource.QueryInterface<ID3D11Texture2D>();
            var desc = texture.Description;

            e.Context.RSSetViewport(0, 0, desc.Width, desc.Height);
            e.Context.Draw(6, 0);

            e.Context.RSSetState(null);
        }

        public void Release(DrawingSurfaceEventArgs e) {

        }
    }
}
