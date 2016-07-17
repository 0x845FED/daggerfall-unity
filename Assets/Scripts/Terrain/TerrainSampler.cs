﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2016 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Collections;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility;

namespace DaggerfallWorkshop
{
    /// <summary>
    /// Interface to TerrainSampler.
    /// </summary>
    public interface ITerrainSampler
    {
        /// <summary>
        /// Version of terrain sampler implementation.
        /// This is serialized with save games to ensure player is being loaded
        /// back into the same world on deserialization.
        /// If you make a change to how heightmaps are generated, then you MUST tick up the version number.
        /// Failed to do this can make player fall through world on load.
        /// </summary>
        int Version { get; }

        // Terrain heightmap dimension (+1 extra point for end vertex)
        // Example settings are 129, 257, 513, 1023, etc.
        // Do not set to a value less than MapsFile.WorldMapTileDim
        int HeightmapDimension { get; set; }

        /// <summary>
        /// Maximum height of terrain. Used for clamping max height and setting Unity's TerrainData.size Y axis.
        /// </summary>
        float MaxTerrainHeight { get; set; }

        /// <summary>
        /// Sea level. Use for clamping min height and texturing with ocean.
        /// </summary>
        float OceanElevation { get; set; }

        /// <summary>
        /// Beach line elevation. How far above sea level the beach line extends.
        /// </summary>
        float BeachElevation { get; set; }

        /// <summary>
        /// Populates a MapPixelData struct using custom height sample generator.
        /// </summary>
        /// <param name="mapPixel">MapPixelData struct.</param>
        void GenerateSamples(ref MapPixelData mapPixel);
    }

    /// <summary>
    /// Base TerrainSampler for transforming heightmap samples.
    /// This class and interface will be expanded on later.
    /// </summary>
    public abstract class TerrainSampler : ITerrainSampler
    {
        protected int defaultHeightmapDimension = 129;

        public abstract int Version { get; }
        public virtual int HeightmapDimension { get; set; }
        public virtual float MaxTerrainHeight { get; set; }
        public virtual float OceanElevation { get; set; }
        public virtual float BeachElevation { get; set; }

        public abstract void GenerateSamples(ref MapPixelData mapPixel);
    }
}