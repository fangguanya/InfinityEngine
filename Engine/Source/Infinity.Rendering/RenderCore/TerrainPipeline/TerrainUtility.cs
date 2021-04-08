﻿using InfinityEngine.Core.Mathematics.Geometry;
using InfinityEngine.Core.Mathmatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityEngine.Rendering.TerrainPipeline
{
    /*public static class HeightmapLoader
    {
        public static void ImportRaw(string path, int m_Depth, int m_Resolution, bool m_FlipVertically, TerrainData terrainData)
        {
            // Read data
            byte[] data;
            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
            {
                data = br.ReadBytes(m_Resolution * m_Resolution * (int)m_Depth);
                br.Close();
            }

            int heightmapRes = terrainData.heightmapResolution;
            float[,] heights = new float[heightmapRes, heightmapRes];

            float normalize = 1.0F / (1 << 16);
            for (int y = 0; y < heightmapRes; ++y)
            {
                for (int x = 0; x < heightmapRes; ++x)
                {
                    int index = Mathf.Clamp(x, 0, m_Resolution - 1) + Mathf.Clamp(y, 0, m_Resolution - 1) * m_Resolution;
                    ushort compressedHeight = System.BitConverter.ToUInt16(data, index * 2);
                    float height = compressedHeight * normalize;
                    int destY = m_FlipVertically ? heightmapRes - 1 - y : y;
                    heights[destY, x] = height;
                }
            }

            terrainData.SetHeights(0, 0, heights);
        }

        public static void ExportRaw(string path, int m_Depth, bool m_FlipVertically, TerrainData terrainData)
        {
            // Write data
            int heightmapRes = terrainData.heightmapResolution;
            float[,] heights = terrainData.GetHeights(0, 0, heightmapRes, heightmapRes);
            byte[] data = new byte[heightmapRes * heightmapRes * (int)m_Depth];

            float normalize = (1 << 16);
            for (int y = 0; y < heightmapRes; ++y)
            {
                for (int x = 0; x < heightmapRes; ++x)
                {
                    int index = x + y * heightmapRes;
                    int srcY = m_FlipVertically ? heightmapRes - 1 - y : y;
                    int height = Mathf.RoundToInt(heights[srcY, x] * normalize);
                    ushort compressedHeight = (ushort)Mathf.Clamp(height, 0, ushort.MaxValue);

                    byte[] byteData = System.BitConverter.GetBytes(compressedHeight);
                    data[index * 2 + 0] = byteData[0];
                    data[index * 2 + 1] = byteData[1];
                }
            }

            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }

        public static void InitTerrainData(int TextureSize, TerrainData DescTerrainData)
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(DescTerrainData, "InitTerrainData");
#endif
            float[,] heights = new float[TextureSize, TextureSize];

            for (int x = 0; x <= TextureSize - 1; x++)
            {
                for (int y = 0; y <= TextureSize - 1; y++)
                {
                    heights[(TextureSize - 1) - x, y] = 0;
                }
            }

            DescTerrainData.SetHeights(0, 0, heights);
        }

        public static void TextureToTerrainData(Texture2D SourceHeightMap, TerrainData DescTerrainData)
        {
            Color[] HeightData = SourceHeightMap.GetPixels(0);
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(DescTerrainData, "CopyTextureToTerrainData");
#endif

            int TextureSize = SourceHeightMap.width;
            float[,] heights = new float[TextureSize, TextureSize];

            for (int x = 0; x <= TextureSize - 1; x++)
            {
                for (int y = 0; y <= TextureSize - 1; y++)
                {
                    heights[(TextureSize - 1) - x, y] = HeightData[x * TextureSize + y].r;
                }
            }

            DescTerrainData.SetHeights(0, 0, heights);
        }

        public static void TerrainDataToTexture(Texture2D SourceHeightMap, TerrainData DescTerrainData)
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(SourceHeightMap, "CopyTerrainDataToTexture");
#endif

            int TextureSize = SourceHeightMap.width;
            Color[] HeightData = new Color[TextureSize * TextureSize];
            for (int x = 0; x <= TextureSize - 1; x++)
            {
                for (int y = 0; y <= TextureSize - 1; y++)
                {
                    HeightData[x * TextureSize + y].r = DescTerrainData.GetHeight(y, (TextureSize - 1) - x) / DescTerrainData.heightmapScale.y;
                    HeightData[x * TextureSize + y].g = DescTerrainData.GetHeight(y, (TextureSize - 1) - x) / DescTerrainData.heightmapScale.y;
                    HeightData[x * TextureSize + y].b = DescTerrainData.GetHeight(y, (TextureSize - 1) - x) / DescTerrainData.heightmapScale.y;
                    HeightData[x * TextureSize + y].a = 1;
                }
            }

            SourceHeightMap.SetPixels(HeightData, 0);
            SourceHeightMap.Apply();
        }
    }

    public static class TerrainUtility
    {
        public static Color[] LODColor = new Color[7] { new Color(1, 1, 1, 1), new Color(1, 0, 0, 1), new Color(0, 1, 0, 1), new Color(0, 0, 1, 1), new Color(1, 1, 0, 1), new Color(1, 0, 1, 1), new Color(0, 1, 1, 1) };

        public static float Squared(in float A)
        {
            return A * A;
        }

        public static float DistSquared(in float3 V1, in float3 V2)
        {
            return Squared(V2.x - V1.x) + Squared(V2.y - V1.y) + Squared(V2.z - V1.z);
        }

        public static float LogX(in float Base, in float Value)
        {
            return math.log(Value) / math.log(Base);
        }

        public static float GetBoundRadius(in FBound BoundBox)
        {
            float3 Extents = BoundBox.extents;
            return math.max(math.max(math.abs(Extents.x), math.abs(Extents.y)), math.abs(Extents.z));
        }

        public static float4x4 GetProjectionMatrix(in float HalfFOV, in float Width, in float Height, in float MinZ, in float MaxZ)
        {
            float4 column0 = new float4(1.0f / math.tan(HalfFOV), 0.0f, 0.0f, 0.0f);
            float4 column1 = new float4(0.0f, Width / math.tan(HalfFOV) / Height, 0.0f, 0.0f);
            float4 column2 = new float4(0.0f, 0.0f, MinZ == MaxZ ? 1.0f : MaxZ / (MaxZ - MinZ), 1.0f);
            float4 column3 = new float4(0.0f, 0.0f, -MinZ * (MinZ == MaxZ ? 1.0f : MaxZ / (MaxZ - MinZ)), 0.0f);

            return new float4x4(column0, column1, column2, column3);
        }

        public static float ComputeBoundsScreenRadiusSquared(in float SphereRadius, in float3 BoundsOrigin, in float3 ViewOrigin, in float4x4 ProjMatrix)
        {
            float DistSqr = DistSquared(BoundsOrigin, ViewOrigin) * ProjMatrix.m23;

            float ScreenMultiple = math.max(0.5f * ProjMatrix., 0.5f * ProjMatrix.m11);
            ScreenMultiple *= SphereRadius;

            return (ScreenMultiple * ScreenMultiple) / math.max(1, DistSqr);
        }

        public static float ComputeBoundsScreenRadiusSquared(in float SphereRadius, in float3 BoundsOrigin, in float3 ViewOrigin, in float4x4 ProjMatrix)
        {
            float DistSqr = DistSquared(BoundsOrigin, ViewOrigin) * ProjMatrix.c2.z;

            float ScreenMultiple = math.max(0.5f * ProjMatrix.c0.x, 0.5f * ProjMatrix.c1.y);
            ScreenMultiple *= SphereRadius;

            return (ScreenMultiple * ScreenMultiple) / math.max(1, DistSqr);
        }

        public static bool IntersectAABBFrustum(FPlane[] plane, in FAABB bound)
        {
            for (int i = 0; i < 6; i++)
            {
                float3 normal = plane[i].normal;
                float distance = plane[i].distance;

                float dist = math.dot(normal, bound.center) + distance;
                float radius = math.dot(bound.extents, math.abs(normal));

                if (dist + radius < 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static int GetLODFromScreenSize(in FSectionLODData LODSetting, in float InScreenSizeSquared, in float InViewLODScale, out float OutFractionalLOD)
        {
            float ScreenSizeSquared = InScreenSizeSquared / InViewLODScale;

            if (ScreenSizeSquared <= LODSetting.LastLODScreenSizeSquared)
            {
                OutFractionalLOD = LODSetting.LastLODIndex;
                return LODSetting.LastLODIndex;
            }
            else if (ScreenSizeSquared > LODSetting.LOD1ScreenSizeSquared)
            {
                OutFractionalLOD = (LODSetting.LOD0ScreenSizeSquared - math.min(ScreenSizeSquared, LODSetting.LOD0ScreenSizeSquared)) / (LODSetting.LOD0ScreenSizeSquared - LODSetting.LOD1ScreenSizeSquared);
                return 0;
            }
            else
            {
                OutFractionalLOD = 1 + LogX(LODSetting.LODOnePlusDistributionScalarSquared, LODSetting.LOD1ScreenSizeSquared / ScreenSizeSquared);
                return (int)OutFractionalLOD;
            }
        }

        public static int GetSectionNumFromTerrainSize(int InTerrainSize)
        {
            int SectionNum = 0;

            switch (InTerrainSize)
            {
                case 1024:
                    {
                        SectionNum = 16;
                        break;
                    }

                case 512:
                    {
                        SectionNum = 8;
                        break;
                    }

                case 256:
                    {
                        SectionNum = 4;
                        break;
                    }

                case 128:
                    {
                        SectionNum = 2;
                        break;
                    }
            }

            return SectionNum;
        }
    }*/
}
