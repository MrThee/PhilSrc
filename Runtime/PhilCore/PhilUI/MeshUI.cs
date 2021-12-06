// https://bitbucket.org/Unity-Technologies/ui/src/31cbc456efd5ed74cba398ec1a101a31f66716db/UnityEngine.UI/UI/Core/Image.cs#lines-825

// For custom meshes, it looked like I have to override:
// protected virtual void OnPopulateMesh(VertexHelper toFill);

// Other neat TMPro debugging tools: 
// https://forum.unity.com/threads/get-the-actual-width-of-the-visible-text.521457/#post-3427734

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Phil.Core {

// https://www.hallgrimgames.com/blog/2018/11/25/custom-unity-ui-meshes
public class MeshUI : Image {

    public bool softSlice = false;
    public SoftSlicing softSlicing = new SoftSlicing( 64, 64 );

    public Mesh mesh {
        get {
            return m_mesh;
        }
        set {
            if(m_mesh == value){
                return;
            }
            m_mesh = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    [SerializeField]
    private Mesh m_mesh;

    private static List<Vector3> s_vertBuffer;
    private static List<int> s_triBuffer;
    private static List<Vector2> s_uv0Buffer;
    private static List<Color32> s_colorBuffer;
    static MeshUI(){
        s_vertBuffer = new List<Vector3>(256);
        s_triBuffer = new List<int>(1024);
        s_colorBuffer = new List<Color32>(256);
        s_uv0Buffer = new List<Vector2>(256);
    }

    protected override void OnPopulateMesh(VertexHelper vh){
        if(m_mesh == null){
            base.OnPopulateMesh(vh);
            return;
        }
        vh.Clear();
        s_vertBuffer.Clear();
        s_triBuffer.Clear();
        s_colorBuffer.Clear();
        s_uv0Buffer.Clear();

        // Calc Bounds
        mesh.GetVertices(s_vertBuffer);
        mesh.GetTriangles(s_triBuffer, 0);
        mesh.GetColors(s_colorBuffer);
        mesh.GetUVs(0, s_uv0Buffer);
        Bounds bounds = new Bounds(s_vertBuffer[0], Vector3.zero);
        foreach(var vert in s_vertBuffer){
            bounds.Encapsulate(vert);
        }

        Vector2 dims = rectTransform.rect.size;

        // Add the same verts, but now normalized
        int meshVertCount = s_vertBuffer.Count;
        for(int vi = 0; vi < meshVertCount; vi ++) {
            Vector3 meshVert = s_vertBuffer[vi];
            UIVertex uiVert = new UIVertex();
            
            bool hasColors = mesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color);
            Color geoFactor = Color.white; 
            if(hasColors && vi < s_colorBuffer.Count){
                geoFactor = s_colorBuffer[vi];
            }
            uiVert.color = this.color * geoFactor;
            Vector2 normPoint = bounds.CalcNormalizedPoint(meshVert).XY();
            normPoint = softSlice ? softSlicing.Remap( normPoint, dims ) : normPoint;
            normPoint -= 0.5f*Vector2.one;
            Vector3 scaledPos = new Vector3(-normPoint.x * dims.x, normPoint.y * dims.y, 0f);
            uiVert.position = scaledPos;

            uiVert.uv0 = s_uv0Buffer[vi];
            
            // if( mesh.uv2 != null && vi < mesh.uv2.Length){
            //     uiVert.uv1 = mesh.uv2[vi];
            // }
            // if( mesh.uv3 != null && vi < mesh.uv3.Length){
            //     uiVert.uv2 = mesh.uv3[vi];
            // }
            // if( mesh.uv4 != null && vi < mesh.uv4.Length){
            //     uiVert.uv3 = mesh.uv4[vi];
            // }

            vh.AddVert(uiVert);
        }

        // Same tri indices
        int triCount = s_triBuffer.Count;
        for(int ti = 0; ti < triCount; ti+=3){
            vh.AddTriangle( s_triBuffer[ti+0], s_triBuffer[ti+1], s_triBuffer[ti+2] );
        }
    }

    protected override void OnRectTransformDimensionsChange(){
        base.OnRectTransformDimensionsChange();
        SetVerticesDirty();
        SetMaterialDirty();
    }

    // Data
    [System.Serializable]
    public struct SoftSlicing {
        public Vector2 pixelSize;
        public float top;
        public float bottom;
        public float left;
        public float right;

        public SoftSlicing(float pixWidth, float pixHeight){
            this.pixelSize = new Vector2(pixWidth, pixHeight);
            this.top = 0f;
            this.bottom = 0f;
            this.left = 0f;
            this.right = 0f;
        }

        public Vector2 Remap(Vector2 normInput, Vector2 rectDims){
            Vector2 atResolution = Utils.Multiply(normInput, pixelSize);

            Vector2 bl = new Vector2(left, bottom);
            Vector2 tr = rectDims - new Vector2(right, top);
            var innerRect = Rect.MinMaxRect(bl.x, bl.y, tr.x, tr.y);

            normInput.x = ThreeSectionRemap( normInput.x, left, right, rectDims.x, pixelSize.x);
            normInput.y = ThreeSectionRemap( normInput.y, bottom, top, rectDims.y, pixelSize.y);
            return normInput;
        }

        float ThreeSectionRemap(float t, float leftCutoffPix, float rightCutoffPix, float forcedPixDim, float idealDim){
            float normA = leftCutoffPix / forcedPixDim;
            float rPix = forcedPixDim - rightCutoffPix;
            float normB = rPix / forcedPixDim;
            float destinyPix = t * idealDim;
            if(destinyPix < leftCutoffPix){
                // Figure out how far you are in normA
                float inNormA = destinyPix / leftCutoffPix;
                return Mathf.Lerp(0f, normA, inNormA);
            } else if(destinyPix > idealDim - rightCutoffPix){
                // Figure out how far you are from normB
                float fromNormB = (destinyPix-(idealDim - rightCutoffPix)) / rightCutoffPix;
                return Mathf.Lerp( normB, 1f, fromNormB );
            } else {
                // In the mid-section
                float fromLeftCutoff = destinyPix - leftCutoffPix;
                float midSectionLength = idealDim - leftCutoffPix - rightCutoffPix;
                float tt = fromLeftCutoff / midSectionLength;
                return Mathf.Lerp( normA, normB, tt );
            }
        }
    }

}

}