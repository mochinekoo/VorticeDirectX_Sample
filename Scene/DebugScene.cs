using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media.Effects;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using static VorticeDirectX_Sample.Manager.DX3D;

namespace VorticeDirectX_Sample.Scene {
    internal class DebugScene : BaseScene {

        private ID3D11Buffer vertexBuffer;

        public DebugScene() : base("DebugScene") {
            
        }

        public override void Init() {
            InitVertexBuffer();
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
                new BufferDescription {
                    Usage = ResourceUsage.Default,
                    ByteWidth = (uint)size,
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

                    vertexBuffer = MainRender.DX3D.DXDevice.CreateBuffer(bufferDescription, subresourceData);
                }
            } finally {
                handle.Free();
            }
        }

        public override void Update() {
            
        }

        public override void Draw() {

            MainRender.DX3D.DXContext.IASetPrimitiveTopology(Vortice.Direct3D.PrimitiveTopology.TriangleList);
            MainRender.DX3D.DXContext.IASetInputLayout(MainRender.DX3D.InputLayout);
            MainRender.DX3D.DXContext.VSSetShader(MainRender.DX3D.VetexShader);
            MainRender.DX3D.DXContext.IASetVertexBuffer(0, vertexBuffer, Vertex.SizeInBytes);
            MainRender.DX3D.DXContext.PSSetShader(MainRender.DX3D.PixelShader);

            MainRender.DX3D.DXContext.Draw(6, 0);

            MainRender.DX3D.DXContext.RSSetState(null);
        }

    }
}
