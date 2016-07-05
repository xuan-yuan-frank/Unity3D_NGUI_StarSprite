using System;
using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Star Sprite")]
public class UIStarSprite : UISprite
{

    public int PointsCount;
    public float[] NormalizedValue;


    public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture tex = mainTexture;

		if (tex != null)
		{
			if (mSprite == null) mSprite = atlas.GetSprite(spriteName);

		    if (mSprite == null)
		        return;

            // Calculate only OuterUV because we only use Simple mode
			mOuterUV.Set(mSprite.x, mSprite.y, mSprite.width, mSprite.height);
			mOuterUV = NGUIMath.ConvertToTexCoords(mOuterUV, tex.width, tex.height);
		}

		if (mSprite == null || PointsCount < 3 || PointsCount > NormalizedValue.Length) return;

		Vector4 v = drawingDimensions;
		Vector4 u = drawingUVs;

        Vector2 normalizedCenter = new Vector2(0.5f, 0.5f);

        // We have to draw the center with every three vertex together 
        // because NGUI makes every four vertex into two triangles
        int index = 0;
	    while (index + 2  < PointsCount)
	    {
	        verts.Add(MapFromNormalizedVector(v, normalizedCenter));
            uvs.Add(MapFromNormalizedVector(u, normalizedCenter));

	        for (int i = 0; i < 3; ++i)
	        {
                AddVertex(index + i, u, v, verts, uvs);
	        }
	        index += 2;
	    }

        // Handle the vertex left, should be one or two

        // Center again
        verts.Add(MapFromNormalizedVector(v, normalizedCenter));
        uvs.Add(MapFromNormalizedVector(u, normalizedCenter));

        // First left vertex
        AddVertex(index, u, v, verts, uvs);

        // Second one may exist, or we use the one with index 0
        if (++index < PointsCount)
        {
            AddVertex(index, u, v, verts, uvs);
        }
        else
        {
            AddVertex(0, u, v, verts, uvs);
        }

        // Last one, draw first point again
        AddVertex(0, u, v, verts, uvs);

        Color colF = color;
		colF.a = finalAlpha;
		Color32 col = atlas.premultipliedAlpha ? NGUITools.ApplyPMA(colF) : colF;
		
	    for (int i = 0; i < verts.size; ++i)
		    cols.Add(col);
	}

    private void AddVertex(int index, Vector4 u, Vector4 v, BetterList<Vector3> verts, BetterList<Vector2> uvs)
    {
	    float angleInc = 2 * Mathf.PI / PointsCount;
        // If the PointsCount is even, we move a half radIncreament
	    float startAngle = Mathf.PI + (PointsCount % 2 == 0 ? (angleInc / 2) : 0);
        float radius = 0.5f;
        float pointAngle = startAngle + index*angleInc;

        float clamped1DValue = Mathf.Clamp(NormalizedValue[index], 0, 1);
        Vector2 normalizedValue = new Vector2( radius - clamped1DValue*radius*Mathf.Sin(pointAngle), radius - clamped1DValue*radius*Mathf.Cos(pointAngle));

        verts.Add(MapFromNormalizedVector(v, normalizedValue));
        uvs.Add(MapFromNormalizedVector(u, normalizedValue));
    }
    private Vector2 MapFromNormalizedVector(Vector4 vec, Vector2 normalized)
    {
        float x = vec.x + (vec.z - vec.x)*normalized.x;
        float y = vec.y + (vec.w - vec.y)*normalized.y;
        return new Vector2(x, y);
    }
}
