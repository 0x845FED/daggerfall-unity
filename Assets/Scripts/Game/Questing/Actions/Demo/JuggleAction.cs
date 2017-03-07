﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2017 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Text.RegularExpressions;
using FullSerializer;

// Place actions in this namespace
namespace DaggerfallWorkshop.Game.Questing.Actions
{
    /// <summary>
    /// Example action to give contributors a starting point.
    /// This action simulates the player juggling some number of objects with a chance to drop each frame.
    /// Inherits from ActionTemplate which implements basics of IQuestAction.
    /// </summary>
    public class JuggleAction : ActionTemplate
    {
        // These fields represent state for this actions
        string thingName;
        int thingsRemaining;
        int interval;
        int dropPercent;

        // These fields are for internal use
        float timer = 0;

        /// <summary>
        /// Data to serialize action state can be placed in a struct.
        /// It's a good idea to version struct in case you need to migrate between very different state setups in future.
        /// For basic changes (e.g. just adding a new field with a sensible default) no migration should be needed.
        /// See FullSerializer versioning docs for more: https://github.com/jacobdufault/fullserializer/wiki/Versioning
        /// </summary>
        [fsObject("v1")]
        public struct MySaveData
        {
            public string thingName;
            public int thingsRemaining;
            public int interval;
            public int dropPercent;
        }

        /// <summary>
        /// Signature is used to match action source and retrieve parameter values.
        /// You need to provide this so action can be tested and factoried as required.
        /// See Regex.Match docs for more: https://msdn.microsoft.com/en-us/library/twcw2f1c(v=vs.110).aspx
        /// Example match: "juggle 5 apples every 2 seconds drop 40%"
        /// </summary>
        public override string Pattern
        {
            get { return @"juggle (?<numberOfThings>\d+) (?<thingName>\w+) every (?<interval>\d+) seconds drop (?<dropPercent>\d+)%"; }
        }

        /// <summary>
        /// Create is called when action is factoried during parse time.
        /// Any setup required should be checked and instantiated from here.
        /// If anything prevents action from starting, please throw or log descriptive information.
        /// </summary>
        /// <param name="source">Source line.</param>
        /// <returns>New quest action from this template or null if not created.</returns>
        public override IQuestAction Create(string source)
        {
            // Source must match pattern
            Match match = Test(source);
            if (!match.Success)
                return null;

            // Factory new action and set default data as needed
            JuggleAction action = new JuggleAction();
            action.thingName = match.Groups["thingName"].Value;
            action.thingsRemaining = Parser.ParseInt(match.Groups["numberOfThings"].Value);
            action.interval = Parser.ParseInt(match.Groups["interval"].Value);
            action.dropPercent = Parser.ParseInt(match.Groups["dropPercent"].Value);

            return action;
        }

        /// <summary>
        /// Gets save data for action serialization.
        /// </summary>
        /// <returns>Data packet with action state to save.</returns>
        public override object GetSaveData()
        {
            MySaveData data = new MySaveData();
            data.thingName = thingName;
            data.thingsRemaining = thingsRemaining;
            data.interval = interval;
            data.dropPercent = dropPercent;

            return data;
        }

        /// <summary>
        /// Restores deserialized state back to action.
        /// </summary>
        /// <param name="dataIn">Data packet with action state to load.</param>
        public override void RestoreSaveData(object dataIn)
        {
            MySaveData data = (MySaveData)dataIn;
            thingName = data.thingName;
            thingsRemaining = data.thingsRemaining;
            interval = data.interval;
            dropPercent = data.dropPercent;
        }

        /// <summary>
        /// Update is called by owning task once per frame as part of quest machine tick.
        /// Update is only called by task if active conditions are met.
        /// Perform any updates required here.
        /// </summary>
        /// <param name="caller">Task hosting this action.</param>
        public override void Update(Task caller)
        {
            // Increment timer
            timer += Time.deltaTime;
            if (timer < interval)
                return;

            // Juggle 'em if you got 'em
            if (thingsRemaining > 0)
            {
                Juggle();
            }

            // Reset timer
            timer = 0;
        }

        /// <summary>
        /// Our work is done here.
        /// </summary>
        private void Juggle()
        {
            // Juggle current things
            DaggerfallUI.AddHUDText(string.Format("Juggling {0} {1}...", thingsRemaining, thingName));

            // We might drop something!
            int roll = Random.Range(1, 101);
            if (roll < dropPercent)
            {
                thingsRemaining--;
                DaggerfallUI.AddHUDText("Oops, I dropped one!");
            }

            // Give up if we've dropped everything
            if (thingsRemaining == 0)
            {
                DaggerfallUI.AddHUDText(string.Format("Dropped all the {0}. I give up!", thingName));
                return;
            }
        }
    }
}