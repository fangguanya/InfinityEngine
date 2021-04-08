using InfinityEngine.Core.Mathmatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityEngine.Rendering.TerrainPipeline
{
    /*[Serializable]
    public class FTerrainSector
    {
        public int[] MaxLODs;
        public FBound BoundBox;
        public FTerrainSection[] Sections;
        public NativeArray<FTerrainSection> NativeSections;

        public FTerrainSector(in int SectorSize, in int NumSection, in int NumQuad, in float3 SectorPivotPosition, FAABB SectorBound)
        {
            int SectorSize_Half = SectorSize / 2;
            int SectionSize_Half = NumQuad / 2;

            MaxLODs = new int[NumSection * NumSection];
            Sections = new FTerrainSection[NumSection * NumSection];
            BoundBox = new FBound(new float3(SectorPivotPosition.x + SectorSize_Half, SectorPivotPosition.y + (SectorBound.size.y / 2), SectorPivotPosition.z + SectorSize_Half), SectorBound.size * 0.5f);

            for (int SectorSizeX = 0; SectorSizeX <= NumSection - 1; SectorSizeX++)
            {
                for (int SectorSizeY = 0; SectorSizeY <= NumSection - 1; SectorSizeY++)
                {
                    int SectionIndex = (SectorSizeX * NumSection) + SectorSizeY;
                    float3 SectionPivotPosition = SectorPivotPosition + new float3(NumQuad * SectorSizeX, 0, NumQuad * SectorSizeY);
                    float3 SectionCenterPosition = SectionPivotPosition + new float3(SectionSize_Half, 0, SectionSize_Half);

                    Sections[SectionIndex] = new FTerrainSection();
                    Sections[SectionIndex].PivotPosition = SectionPivotPosition;
                    Sections[SectionIndex].CenterPosition = SectionCenterPosition;
                    Sections[SectionIndex].BoundingBox = new FAABB(SectionCenterPosition, new float3(NumQuad, 1, NumQuad));
                }
            }

            InitializLOD(7);
        }

        public void Initializ()
        {
            if (NativeSections.IsCreated == false)
            {
                NativeSections = new NativeArray<FTerrainSection>(Sections.Length, Allocator.Persistent);
            }
        }

        public void Release()
        {
            if (NativeSections.IsCreated == true)
            {
                NativeSections.Dispose();
            }
        }

        public void BuildNativeCollection()
        {
            if (NativeSections.IsCreated == true)
            {
                for (int i = 0; i < Sections.Length; i++)
                {
                    NativeSections[i] = Sections[i];
                }
            }
        }

        private void InitializLOD(in int MaxLOD)
        {
            for (int i = 0; i < MaxLODs.Length; i++)
            {
                MaxLODs[i] = MaxLOD;
            }
        }

        public void BuildLODData(in float LOD0ScreenSize, in float LOD0Distribution, in float LODDistribution)
        {
            for (int i = 0; i < Sections.Length; i++)
            {
                ref int MaxLOD = ref MaxLODs[i];
                ref FSectionLODData LODSetting = ref Sections[i].LODSetting;

                float CurrentScreenSizeRatio = LOD0ScreenSize;
                float[] LODScreenRatioSquared = new float[MaxLOD];
                float ScreenSizeRatioDivider = math.max(LOD0Distribution, 1.01f);
                LODScreenRatioSquared[0] = CurrentScreenSizeRatio * CurrentScreenSizeRatio;

                // LOD 0 handling
                LODSetting.LOD0ScreenSizeSquared = CurrentScreenSizeRatio * CurrentScreenSizeRatio;
                CurrentScreenSizeRatio /= ScreenSizeRatioDivider;
                LODSetting.LOD1ScreenSizeSquared = CurrentScreenSizeRatio * CurrentScreenSizeRatio;
                ScreenSizeRatioDivider = math.max(LODDistribution, 1.01f);
                LODSetting.LODOnePlusDistributionScalarSquared = ScreenSizeRatioDivider * ScreenSizeRatioDivider;

                // Other LODs
                for (int j = 1; j < MaxLOD; ++j) // This should ALWAYS be calculated from the section size, not user MaxLOD override
                {
                    LODScreenRatioSquared[j] = CurrentScreenSizeRatio * CurrentScreenSizeRatio;
                    CurrentScreenSizeRatio /= ScreenSizeRatioDivider;
                }

                // Clamp ForcedLOD to the valid range and then apply
                LODSetting.LastLODIndex = MaxLOD;
                LODSetting.LastLODScreenSizeSquared = LODScreenRatioSquared[MaxLOD - 1];
            }
        }

        public void UpdateLODData(in int NumQuad, in float3 ViewOringin, in float4x4 Matrix_Proj)
        {
            if (NativeSections.IsCreated == false) { return; }

            for (int i = 0; i < NativeSections.Length; ++i)
            {
                FTerrainSection Section = NativeSections[i];
                float ScreenSize = TerrainUtility.ComputeBoundsScreenRadiusSquared(TerrainUtility.GetBoundRadius(Section.BoundingBox), Section.BoundingBox.center, ViewOringin, Matrix_Proj);
                Section.LODIndex = math.min(6, TerrainUtility.GetLODFromScreenSize(Section.LODSetting, ScreenSize, 1, out Section.FractionLOD));
                Section.FractionLOD = math.min(5, Section.FractionLOD);
                Section.NumQuad = math.clamp(NumQuad >> Section.LODIndex, 1, NumQuad);

                NativeSections[i] = Section;
            }
        }
    }*/
}
