using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

namespace Extensions.Unity
{
    public class GetMeshPixels
    {
        private readonly GameObject _myCamGO;
        private readonly Camera _myCamera;

        private static readonly int depthTextureMask = Shader.PropertyToID
            (
                "DepthTextureMask"
            );

        private readonly Shader _depthShader;
        private readonly Shader _textureMaskShader;

        public GetMeshPixels(Vector3 worldPos, Vector3 renderDir, int renderLayer)
        {
            _myCamGO = new GameObject("tempDepthCam");
            _myCamera = _myCamGO.AddComponent<Camera>();
            _myCamera.clearFlags = CameraClearFlags.Color;
            _myCamera.backgroundColor = Color.black;
            Transform myCamTrans = _myCamera.transform;
            _myCamera.nearClipPlane = 0.01f;
            _myCamera.farClipPlane = 250f;
            myCamTrans.position = worldPos;
            myCamTrans.forward = renderDir;
            _myCamera.cullingMask = renderLayer;
            _myCamera.fieldOfView = 20f;
            _myCamera.enabled = false;

            _depthShader = Resources.Load<Shader>("DepthShader");
            _textureMaskShader = Resources.Load<Shader>("TextureMask");
        }

        private float DecodeRGB(Color color)
        {
            uint ex = (uint)(color.r * 255);
            uint ey = (uint)(color.g * 255);
            uint ez = (uint)(color.b * 255);
            uint ew = (uint)(color.a * 255);
            var v = ew;
            return ew / (256.0f * 256.0f * 256.0f * 256.0f);
        }
        
        private float DecodeR(float r)
        {
            uint ew = (uint)(r * 255);
            return ew / 256.0f;
        }

        private float DecodeRG(Vector2 rg)
        {
            uint ez = (uint)(rg.x * 255);
            uint ew = (uint)(rg.y * 255);
            uint v = (ez << 8) + ew;
            return v / (256.0f * 256.0f);
        }
        
        public List<Vector3> GetPixelPositions(Texture2D textureMask, int renderW, int renderH, out Texture2D depthTextureDebug)
        {
            RenderTexture activeRenderTexture = RenderTexture.active;
            
            RenderTexture tempRenderTexture = RenderTexture.GetTemporary
                (
                    renderW,
                    renderH,
                    8,
                    GraphicsFormat.R16G16_SFloat
                );
            
            RenderTexture.active = tempRenderTexture;
            _myCamera.targetTexture = tempRenderTexture;
            
            Shader.SetGlobalTexture(depthTextureMask, textureMask);
            
            _myCamera.RenderWithShader(_depthShader, "RenderType");
            
            Texture2D depthTexture = new Texture2D
                (
                    tempRenderTexture.width,
                    tempRenderTexture.height,
                    TextureFormat.RGHalf,
                    false
                );
            
            depthTexture.ReadPixels
                (
                    new Rect
                        (
                            0f,
                            0f,
                            tempRenderTexture.width,
                            tempRenderTexture.height
                        ),
                    0,
                    0
                );
            
            depthTexture.Apply();
            
            Color[] pixels = depthTexture.GetPixels();
            
            List<Vector3> pixelWorldPoses = new List<Vector3>();

            int px = 0;
        
            for (int y = 0; y < depthTexture.height; y ++)
            {
                for (int x = 0; x < depthTexture.width; x ++)
                {
                    float pixelColR = pixels[px].DepthChannelTot();
                    px ++;

                    if (pixelColR < 0.01f) continue;
        
                    Vector3 pix3dPos = new Vector3(x, y, pixelColR);
            
                    pix3dPos = _myCamera.ScreenToWorldPoint(pix3dPos);
            
                    pixelWorldPoses.Add(pix3dPos);
                }
            }
            
            _myCamera.targetTexture = null;
            RenderTexture.ReleaseTemporary(tempRenderTexture);
            RenderTexture.active = activeRenderTexture;
            Object.Destroy(_myCamGO);
            
            depthTextureDebug = depthTexture;
            return pixelWorldPoses;
        }
        
        /// <summary>
        /// [Experimental]
        /// Low performance
        /// </summary>
        /// <param name="textureMask"></param>
        /// <param name="renderW"></param>
        /// <param name="renderH"></param>
        /// <param name="depthTextureDebug"></param>
        /// <returns></returns>
        public List<PixelData> GetPixelPosColor(Texture2D textureMask, int renderW, int renderH, out Texture2D depthTextureDebug)
        {
            RenderTexture activeRenderTexture = RenderTexture.active;
            
            RenderTexture tempRenderTexture = RenderTexture.GetTemporary
                (
                    renderW,
                    renderH,
                    8,
                    GraphicsFormat.R16G16_SFloat
                );
            
            RenderTexture.active = tempRenderTexture;
            _myCamera.targetTexture = tempRenderTexture;
            
            Shader.SetGlobalTexture(depthTextureMask, textureMask);
            
            _myCamera.RenderWithShader(_depthShader, "RenderType");
            
            Texture2D depthTexture = new Texture2D
                (
                    tempRenderTexture.width,
                    tempRenderTexture.height,
                    TextureFormat.RGHalf,
                    false
                );
            
            depthTexture.ReadPixels
                (
                    new Rect
                        (
                            0f,
                            0f,
                            tempRenderTexture.width,
                            tempRenderTexture.height
                        ),
                    0,
                    0
                );
            
            depthTexture.Apply();
            
            _myCamera.ResetReplacementShader();
            
            RenderTexture.ReleaseTemporary(tempRenderTexture);
            
            tempRenderTexture = RenderTexture.GetTemporary
            (
                renderW,
                renderH,
                8,
                GraphicsFormat.R8G8B8A8_UNorm
            );
            
            RenderTexture.active = tempRenderTexture;
            _myCamera.targetTexture = tempRenderTexture;
            
            _myCamera.RenderWithShader(_textureMaskShader, "RenderType");
            
            Texture2D colorText = new Texture2D
            (
                tempRenderTexture.width,
                tempRenderTexture.height,
                TextureFormat.RGBAFloat,
                false
            );
            
            colorText.ReadPixels
            (
                new Rect
                (
                    0f,
                    0f,
                    tempRenderTexture.width,
                    tempRenderTexture.height
                ),
                0,
                0
            );
            
            colorText.Apply();
            
            Color[] depthPixels = depthTexture.GetPixels();
            Color[] colorPixels = colorText.GetPixels();
            
            List<PixelData> pixelData = new List<PixelData>();
        
            int px = 0;
        
            for (int y = 0; y < depthTexture.height; y ++)
            {
                for (int x = 0; x < depthTexture.width; x ++)
                {
                    float pixelColR = depthPixels[px].DepthChannelTot();
                    px ++;
        
                    if (pixelColR < 0.01f) continue;
        
                    Vector3 pix3dPos = new Vector3(x, y, pixelColR);
            
                    pix3dPos = _myCamera.ScreenToWorldPoint(pix3dPos);
            
                    pixelData.Add(new PixelData(pix3dPos, colorPixels[px], px));
                }
            }
            
            _myCamera.targetTexture = null;
            RenderTexture.ReleaseTemporary(tempRenderTexture);
            RenderTexture.active = activeRenderTexture;
            Object.Destroy(_myCamGO);
            
            depthTextureDebug = colorText;
            return pixelData;
        }
                
                
        public struct PixelData
        {
            public readonly Vector3 P;
            public readonly Color C;
            public readonly int Tp;
    
            public PixelData(Vector3 p, Color c, int tp)
            {
                P = p;
                C = c;
                Tp = tp;// * tp * tp;
            }
        }
    }
}